namespace LogiFrame.Drawing
{
    public static class MergeMethods
    {
        public static IMergeMethod Override { get; } = new OverrideMergeMethod();
        public static IMergeMethod Overlay { get; } = new OverlayMergeMethod();
        public static IMergeMethod Transparent { get; } = new TransparentMergeMethod();
        public static IMergeMethod Invert { get; } = new InvertMergeMethod();
    }
}