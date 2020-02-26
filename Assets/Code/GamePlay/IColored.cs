using UnityEngine;

namespace GamePlay {
    public interface IColored {
        Color GetColor();
        void SetColor(Color color);
    }
}