using GamePlay;
using Model;
using UnityEngine;

namespace Util {
    public class Maker : MonoBehaviour, ICanMake {
        [SerializeField] private GameObject prefab;

        public void Spawn() {
            Spawn(prefab);
        }

        public void Spawn(GameObject obj) {
            var particles = Instantiate(obj, transform.position, transform.rotation, null);
            var main = particles.GetComponent<ParticleSystem>().main;
            main.startColor = ColorConversion.Convert(GetComponent<IColored>().GetColorType());
        }

        public void Spawn(GameObject obj, ColorType colorType) {
            var clone = Instantiate(obj, transform.position, transform.rotation, null);
            clone.GetComponent<IColored>().SetColorType(colorType);
        }
    }
}