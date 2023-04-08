namespace Blocks {
    public abstract class Brick : Block {
        protected override bool ShouldBreak() => true;
    }
}