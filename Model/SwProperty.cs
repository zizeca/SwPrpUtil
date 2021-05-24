using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Model
{
	public enum PrpType
	{
		TEXT,
		DATE,
		NUMBER,
		YES_OR_NO
	}

	internal class SwProperty
	{
		private string _propertyName;
		private PrpType _typePrp;
		private string _expression;

		public string PropertyName { get => _propertyName; set => _propertyName = value; }
		public PrpType TypePrp { get => _typePrp; set => _typePrp = value; }
		public string Expression { get => _expression; set => _expression = value; }
	}
}