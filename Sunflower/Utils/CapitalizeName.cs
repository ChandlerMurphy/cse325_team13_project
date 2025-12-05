namespace Sunflower.Helpers
{
    public static class CapitalizeHelper
    {
        public static string CapitalizeName(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            var words = text.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if(words[i].Length > 0)
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
            return string.Join(' ', words);
        }
    }
}