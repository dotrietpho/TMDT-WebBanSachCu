using System.Text;
using System.Text.RegularExpressions;

namespace ReBook.Controllers
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
    }
}