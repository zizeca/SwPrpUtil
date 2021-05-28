using SwPrpUtil.Infrastructure.Commands.Base;
using SwPrpUtil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Infrastructure.Commands
{
	class SldWorksRunTestCommand : AsyncCommandBase
	{
		public override async Task ExecuteAsunc(object parameter)
		{
			await SwHolder.GetSwAppAsync();
		}
	}
}
