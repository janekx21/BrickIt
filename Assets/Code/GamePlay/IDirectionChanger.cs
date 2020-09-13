using Blocks;
using UnityEngine;

namespace GamePlay {
    public interface IDirectionChanger {
        Vector2 GetPosition();
        Direction GetDirection();
    }
}