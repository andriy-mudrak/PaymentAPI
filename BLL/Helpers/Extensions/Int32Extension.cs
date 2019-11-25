namespace BLL.Helpers
{
    public static class Int32Extension
    {
        public static bool IsZero(this int value)
        {
            if (value == 0)
                return true;
            else return false;
        }
    }
}