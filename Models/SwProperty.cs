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

	public class SwProperty
	{
		public string PropertyName { get; set; }

		public swCustomInfoType_e TypePrp { get; set; }

		public string Expression { get; set; }
	}
}