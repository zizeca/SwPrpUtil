using SolidWorks.Interop.swconst;

namespace SwPrpUtil.Models
{

    /// <summary>
    /// Custom properties structure
    /// </summary>
    public class SwProperty
	{
		public SwProperty()
		{
			PropertyName = string.Empty;
			TypePrp = swCustomInfoType_e.swCustomInfoUnknown;
			Expression = string.Empty;
		}

		public SwProperty(string propertyName, swCustomInfoType_e typePrp, string expression)
		{
			PropertyName = propertyName;
			TypePrp = typePrp;
			Expression = expression;
		}

		public string PropertyName { get; set; }

		public swCustomInfoType_e TypePrp { get; set; }

		public string Expression { get; set; }
	}
}