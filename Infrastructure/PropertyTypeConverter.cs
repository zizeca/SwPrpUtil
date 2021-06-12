using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SwPrpUtil.Infrastructure
{
	public enum PrpType
	{
		TEXT,
		DATE,
		NUMBER,
		YES_OR_NO
		//UNKNOWN = TEXT //all UNKNOWN data cast as text
	}

	internal class PropertyTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			swCustomInfoType_e type_E = (swCustomInfoType_e)value;

			switch (type_E)
			{
				case swCustomInfoType_e.swCustomInfoUnknown:
					//return PrpType.UNKNOWN;
					return PrpType.TEXT;

				case swCustomInfoType_e.swCustomInfoText:
					return PrpType.TEXT;

				case swCustomInfoType_e.swCustomInfoDate:
					return PrpType.DATE;

				case swCustomInfoType_e.swCustomInfoNumber:
					return PrpType.NUMBER;

				case swCustomInfoType_e.swCustomInfoDouble:
					return PrpType.NUMBER;

				case swCustomInfoType_e.swCustomInfoYesOrNo:
					return PrpType.YES_OR_NO;

				default:
					//return PrpType.UNKNOWN;
					return PrpType.TEXT;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			PrpType prp_E = (PrpType)value;

			switch (prp_E)
			{
				case PrpType.TEXT:
					return swCustomInfoType_e.swCustomInfoText;

				case PrpType.DATE:
					return swCustomInfoType_e.swCustomInfoDate;

				case PrpType.NUMBER:
					return swCustomInfoType_e.swCustomInfoNumber;

				case PrpType.YES_OR_NO:
					return swCustomInfoType_e.swCustomInfoYesOrNo;

				default:
					return swCustomInfoType_e.swCustomInfoText;
			}
		}
	}
}