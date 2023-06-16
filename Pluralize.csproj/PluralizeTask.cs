namespace Pluralize
{
	public static class PluralizeTask
	{
		public static string PluralizeRubles(int count)
		{
			int remainder = count % 10;
			string answer = "";
			if (remainder == 1 && count % 100 != 11) answer = "рубль";
			else if (((remainder < 5 && remainder > 1) && !(count % 100 < 21 && count %100 > 10) ))  answer = "рубля";
			//else if (((count > 104 && count < 121) || (count < 21 && count > 4)) || ((remainder > 4 && remainder < 10) || remainder == 0) || ) answer = "рублей";
			else answer = "рублей";
			return answer;


		}
	}
}