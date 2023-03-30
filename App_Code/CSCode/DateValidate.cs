using System;
using System.Threading;
using System.Globalization;
using System.Configuration;

namespace aaproject
{
	public class DateValidate
	{
		private string CultString = ConfigurationManager.AppSettings["cultureString"].ToString();
        public DateValidate()
		{
			
		}

		/// <summary>
		/// This function converts the DateTime string from DB Culture to requested UI Culture.
		/// </summary>
		/// <param name="strDate">Date string.</param>
		/// <param name="culture">Current UI Culture.</param>
		/// <returns>Returns string format of date.</returns>
		public string convertFromDB(string strDate,CultureInfo culture)
		{
			DateTimeFormatInfo formatInfo = new CultureInfo(CultString, false).DateTimeFormat;
			//Create DateTimeFormat object of requested culture.
			DateTimeFormatInfo currentFormat = new CultureInfo(culture.Name).DateTimeFormat;
			//DateTime expDt = DateTime.Parse(strDate, formatInfo);
			DateTime expDt = DateTime.Parse(strDate, culture);
			string format = currentFormat.ShortDatePattern.ToString();
			string[] dateFormat = format.Split(currentFormat.DateSeparator.ToCharArray());
			string retDT = string.Empty;
			for(int i = 0; i < dateFormat.Length;i ++)
			{
				switch(dateFormat[i])
				{
					case "dd":
					case "d":
					case "ddd":
					case"DDD":
					case "DD":
					case "D":
						retDT = retDT + expDt.Day + "/";
						break;
					case "MM":
					case "M":
					case "MMM":
						retDT = retDT + expDt.Month + "/";
						break;
					case "yyyy":
					case "yyy":
					case "yy":
					case "y":
					case "YYYY":
					case "YYY":
					case "YY":
					case"Y":
						retDT = retDT + expDt.Year + "/";
						break;
				}
			}
			retDT = retDT.Remove(retDT.LastIndexOf('/'),1);
			return retDT;//DateTime.Parse(retDT,culture).ToShortDateString();
		}

		/// <summary>
		/// This function converts DateTime string from Current UI Culture to DB Culture.
		/// </summary>
		/// <param name="strDate">DateTime String</param>
		/// <param name="culture">Current UI Culture.</param>
		/// <returns>Returns DateTime string in DB Culture.</returns>
		public string convertDate(string strDate,CultureInfo culture)
		{
			///Build datetime object as per the requested UI Culture.
			DateTimeFormatInfo formatInfo = new CultureInfo(culture.Name,false).DateTimeFormat;
			DateTime dt = DateTime.Parse(strDate,formatInfo);

			//Build DatetimeFormat object as per the DB Culture.
			DateTimeFormatInfo dbFormat = new CultureInfo(CultString,false).DateTimeFormat;
			//CultureInfo cul = new CultureInfo(CultString);
			//Retrieve ShortDatePattern of DB Culture. 
			//And split with the date separator character.
			string format = dbFormat.ShortDatePattern.ToString();
			string[] dateFormat = format.Split(dbFormat.DateSeparator.ToCharArray());
			string retDT = string.Empty;
			for(int i = 0; i < dateFormat.Length;i ++)
			{
				switch(dateFormat[i])
				{
					case "dd":
					case "d":
					case "ddd":
					case"DDD":
					case "DD":
					case "D":
						retDT = retDT + dt.Day + "/";
						break;
					case "MM":
					case "M":
					case "MMM":
						retDT = retDT + dt.Month + "/";
						break;
					case "yyyy":
					case "yyy":
					case "yy":
					case "y":
					case "YYYY":
					case "YYY":
					case "YY":
					case"Y":
						retDT = retDT + dt.Year + "/";
						break;
				}
			}

			//Remove last occurence of /
			retDT = retDT.Remove(retDT.LastIndexOf('/'),1);

			//return short date string in DB Culture format.
			return retDT;
		}
	}
}
