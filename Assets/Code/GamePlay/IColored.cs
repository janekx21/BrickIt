using Model;

namespace GamePlay {
    public interface IColored {
        ColorType GetColorType();
        void SetColorType(ColorType type);
    }
}