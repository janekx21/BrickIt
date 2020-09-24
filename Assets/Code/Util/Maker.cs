using GamePlay;
using UnityEngine;

namespace Util {
    public class Maker : MonoBehaviour, ICanMake {
        [SerializeField] private GameObject prefab = null;

        public void Spawn() {
            Spawn(prefab);
        }

        public void Spawn(GameObject obj) {
            var particles = Instantiate(obj, transform.position, transform.rotation, null);
            var main = particles.GetComponent<ParticleSystem>().main;
            main.startColor = GetComponent<IColored>().GetColor();
        }

        public void Spawn(GameObject obj, Color color) {
            var clone = Instantiate(obj, transform.position, transform.rotation, null);
            clone.GetComponent<IColored>().SetColor(color);
        }
    }
}