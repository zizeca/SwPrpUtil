using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	public enum PrpType
	{
		TEXT,
		DATE,
		NUMBER,
		YES_OR_NO
	}

	public struct SwProperty
	{
		string PropertyName;
		PrpType TypePrp;
		string Expression;
	}
}