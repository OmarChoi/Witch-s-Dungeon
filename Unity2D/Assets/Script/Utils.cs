public class Utils
{
    public static string GetNameExceptClone(string name)
    {
        return name.Replace("(Clone)", "").Trim();
    }
}
