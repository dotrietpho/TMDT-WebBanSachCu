using System.Text;
using System.Text.RegularExpressions;

namespace ReBook.Models.Helper
{
    public static class StringHelper
    {
        public static string convertToUnSign(string s)
        {
            // chuyển thành chữ thường
            s = s.ToLower();

            //help convert sign character to un sign character
            //chuyen co dau thanh khong dau
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd');
        }

        public static bool NumberValid(string s)
        {
            return int.TryParse(s, out int i);
        }

        public static string beautifyPrice(int price)
        {
            string priceString = price.ToString();
            int n = priceString.Length / 3;
            for (int i = 1; i <= n; i++)
            {
                priceString = priceString.Insert(priceString.Length - (3 * i), ".");
            }
            return priceString;
        }
        public static string beautifyPrice(double price)
        {
            string priceString = price.ToString();
            int n = priceString.Length / 3;
            for (int i = 1; i <= n; i++)
            {
                priceString = priceString.Insert(priceString.Length - (3 * i), ".");
            }
            return priceString;
        }
        public static string beautifyPrice(string price)
        {
            string priceString = price.ToString();
            int n = priceString.Length / 3;
            for (int i = 1; i <= n; i++)
            {
                priceString = priceString.Insert(priceString.Length - (3 * i), ".");
            }
            return priceString;
        }
    }
}