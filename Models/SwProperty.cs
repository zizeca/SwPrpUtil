using SolidWorks.Interop.swconst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Models
{
	/*
	public enum PrpType
	{
		TEXT,
		DATE,
		NUMBER,
		YES_OR_NO
	}
		//public PrpType TypePrp;
	*/

	public struct SwProperty
	{
		public string PropertyName;

		public swCustomInfoType_e TypePrp;

		public string Expression;
	}
}