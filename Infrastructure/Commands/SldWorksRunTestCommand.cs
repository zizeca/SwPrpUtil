using SwPrpUtil.Infrastructure.Commands.Base;
using SwPrpUtil.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Infrastructure.Commands
{
	class SldWorksRunTestCommand : AsyncCommandBase
	{

		public override bool CanExecute()
		{
			return true;
		}

		public override async Task ExecuteAsync()
		{
			await SwHolder.Instance.GetSwAppAsync();
		}


	}
}
