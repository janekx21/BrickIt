using System.IO;
using System.Linq;
using GamePlay;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Util;

namespace Level {
	public class Level : MonoBehaviour {
		public static Level Own => instance;
		private static Level instance = null;

		private LevelState state = LevelState.begin;
		public LevelState State => state;

        public class OnLevelStateChanged : UnityEvent<LevelState>{}
		public OnLevelStateChanged onStateChanged = new OnLevelStateChanged();

		[MenuItem("Custom/Make Level")]
		public static void MakeLevel() {
			var guid = AssetDatabase.CreateFolder("Assets/Levels", "Level01");
			var path = AssetDatabase.GUIDToAssetPath(guid);
		
			var levelName = Path.GetFileName(path);

			var scenePath = Path.Combine(path, $"{levelName}.unity");
			var worked = AssetDatabase.CopyAsset("Assets/Scenes/Empty.unity", scenePath);
		
			Assert.IsTrue(worked);

			var obj = ScriptableObject.CreateInstance<LevelObject>();
			obj.scene = new SceneReference {ScenePath = scenePath};
			AssetDatabase.CreateAsset(obj, Path.Combine(path, $"{levelName}.asset"));
		}
	
		private void Awake() {
			Assert.IsNull(instance);
			instance = this;
		}

		void Start() {
			Begin();
		}

		void Update() {
			switch (state) {
				case LevelState.play:
					if (Input.GetKeyDown(KeyCode.Escape)) {
						Pause();
					}

					break;
				case LevelState.pause:
					if (Input.GetKeyDown(KeyCode.Escape)) {
						Play();
					}

					break;
			}
		}

		void ChangeState(LevelState newState) {
			state = newState;
			onStateChanged?.Invoke(state);
		}

		void Begin() {
			ChangeState(LevelState.begin);

			Play(); // TODO this is debug for starting the level right away
		}

		public void Play() {
			ChangeState(LevelState.play);
			PlayAll();
		}

		public void Pause() {
			ChangeState(LevelState.pause);
			PauseAll();
		}

		public void Win() {
			Debug.Log("you won :>");
			PauseAll();
			ChangeState(LevelState.win);
		}

		public void Lose() {
			Debug.Log("you lost :(");
			PauseAll();
			ChangeState(LevelState.lost);
		}

		void PlayAll() {
			var objs = FindObjectsOfType<Entity>().OfType<IPausable>();
			foreach (var obj in objs) {
				if (obj.isPaused()) {
					obj.play();
				}
			}
		}

		void PauseAll() {
			var objs = FindObjectsOfType<Entity>().OfType<IPausable>();
			foreach (var obj in objs) {
				if (!obj.isPaused()) {
					obj.pause();
				}
			}
		}

		public void ToMenu() {
			SceneManager.LoadScene("Menu");
		}
	}
}