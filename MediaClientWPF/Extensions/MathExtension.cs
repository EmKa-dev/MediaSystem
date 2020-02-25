namespace MediaSystem.DesktopClientWPF.Extensions
{
    public static class MathExtension
    {
        public static int Clamp(this int val, int min, int max)
        {
            if (val < min) return min;
            else if (val > max) return max;
            else return val;
        }
    }
}
