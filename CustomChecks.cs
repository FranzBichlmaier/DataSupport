using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DateTimeFunctions;

namespace DataSupport
{
    public static class CustomChecks
    {
		
		/// <summary>
		/// Bestimmt ob die Checksumme einer IBAN gültig ist
		/// </summary>
		/// <param name="iban">Der zu prüfende IBAN (z.B: "DE68 2105 0170 0012 3456 78")</param>
		/// <returns><c>True</c>, wenn <paramref name="value"/> gültig ist. Andernfalls <c>False</c>.</returns>
		public static bool IbanChecksumCheck(this string iban)
		{
			if (string.IsNullOrEmpty(iban)) return false;

			string ibanCleared = iban.ToUpper().Replace(" ", "").Replace("-", "");
			string ibanSwapped = ibanCleared.Substring(4) + ibanCleared.Substring(0, 4);
			string sum = ibanSwapped.Aggregate("", (current, c) => current + (char.IsLetter(c) ? (c - 55).ToString() : c.ToString()));

			var d = decimal.Parse(sum);
			decimal modulo = d % 97;
			return (modulo == 1);
		}
		public static string FormatIban(string iban)
		{
			if (string.IsNullOrEmpty(iban)) return string.Empty;

			string s = iban.ToUpper().Replace(" ", "").Replace("-", "");
			int start = 0;
			int end = s.Length;
			StringBuilder builder = new StringBuilder();
			for (start = 0; start < end; start = start + 4)
			{
				if (start > 0) builder.Append(" ");
				int l = Math.Min(4, end - start);
				builder.Append(s.Substring(start, l));
			}
			return builder.ToString();
		}
		
	
	}
}
