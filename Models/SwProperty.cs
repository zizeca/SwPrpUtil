﻿using SolidWorks.Interop.swconst;
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