using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class LevelObject : ScriptableObject {
    public SceneReference scene = null;
    public string levelName = "noname";
    public string levelAuthor = "no one";
}
