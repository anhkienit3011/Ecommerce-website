using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ThucTapProject.Helper
{
    public static class CommonFunctions
    {
        public static string NameFormat(string Name)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            string[] ArrName = Name.Split();

            foreach (string str in ArrName)
            {
                if (!str.IsNullOrEmpty())
                {
                    stringBuilder.Append(str.Substring(0, 1).ToUpper() + str.Substring(1) + " ");
                }
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            return stringBuilder.ToString();
        }
    }

}
