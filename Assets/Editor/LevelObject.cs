using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class LevelObject : ScriptableObject {
    public string levelName = "noname";
    public string levelAuthor = "no one";
    public SceneAsset scene = null;
}
