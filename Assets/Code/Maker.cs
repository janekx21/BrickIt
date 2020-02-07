using UnityEngine;

public class Maker : MonoBehaviour {
    [SerializeField] private GameObject prefab = null;

    public void Spawn() {
        Instantiate(prefab, transform.position, transform.rotation, null);
    }
    
    public void Spawn(GameObject obj) {
        Instantiate(obj, transform.position, transform.rotation, null);
    }
}