namespace BLL.Helpers
{
    public static class Int64Extensions
    {
        public static bool IsZero(this long value)
        {
            if (value == 0)
                return true;
            else return false;
        }
    }
}