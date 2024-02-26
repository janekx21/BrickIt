use anyhow::Result;
use serde::{Deserialize, Serialize};
use spin_sdk::http::IntoResponse;
use spin_sdk::http::Params;
use spin_sdk::http::Router;
use spin_sdk::{
    http::{Request, Response},
    http_component,
    key_value::Store,
};
use std::collections::HashMap;

// Define a serializable User type
#[derive(Serialize, Deserialize, Default)]
#[serde(rename_all = "camelCase")]
struct Level {
    highscores: HashMap<String, u32>, // tracks only the highes score of each user
}

impl Level {
    fn id_to_key(id: &str) -> String {
        format!("level.{id}")
    }

    fn key_is_level(id: &str) -> bool {
        id.starts_with("level.")
    }
}

#[derive(Deserialize)]
#[serde(rename_all = "camelCase")]
struct Highscore {
    user_id: String,
    score: u32,
}

#[derive(Serialize)]
struct UserDeleteResponse {
    count: u32,
}

#[http_component]
fn handle_request(req: Request) -> anyhow::Result<Response> {
    let mut router = Router::new();
    router.post("/highscore/level/:level", api::create_highscore);
    router.get("/highscore/level/:level", api::get_level);
    router.delete("/highscore/user/:user", api::remove_user);
    Ok(router.handle(req))
}

mod api {
    use super::*;

    pub fn create_highscore(req: Request, params: Params) -> Result<impl IntoResponse> {
        let level_id = params.get("level").expect("level param in route");
        let store = Store::open_default()?;
        let mut level = store
            .get_json::<Level>(Level::id_to_key(level_id))?
            .unwrap_or(Default::default());
        let body = req.body();
        let highscore = serde_json::from_slice::<Highscore>(body)?;
        level.highscores.insert(highscore.user_id, highscore.score);
        store.set_json(Level::id_to_key(level_id), &level)?;
        Ok(Response::new(200, serde_json::to_string(&level)?))
    }

    pub fn get_level(_req: Request, params: Params) -> Result<impl IntoResponse> {
        let level_id = params.get("level").expect("level param in route");
        let store = Store::open_default()?;
        let level = store
            .get_json::<Level>(Level::id_to_key(level_id))?
            .unwrap_or(Default::default());

        Ok(Response::new(200, serde_json::to_string(&level)?))
    }

    pub fn remove_user(_req: Request, params: Params) -> Result<impl IntoResponse> {
        let user_id = params.get("user").expect("user param in route");
        let store = Store::open_default()?;
        let level_keys = store
            .get_keys()?
            .into_iter()
            .filter(|s| Level::key_is_level(s));

        let mut count = 0u32;
        for level_key in level_keys {
            let mut level = store
                .get_json::<Level>(&level_key)?
                .unwrap_or(Default::default());
            if level.highscores.remove(user_id).is_some() {
                count += 1;
            }
            store.set_json(&level_key, &level)?;
        }
        Ok(Response::new(
            200,
            serde_json::to_string(&UserDeleteResponse { count })?,
        ))
    }
}
