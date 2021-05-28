using SwPrpUtil.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Infrastructure.Commands
{
	public class AsyncRelayCommand : AsyncCommandBase
	{
		private readonly Func<Task> _callBack;


		public AsyncRelayCommand(Func<Task> callBack, Action<Exception> onExeption ) : base(onExeption)
		{
			_callBack = callBack;
		}

		public override async Task ExecuteAsunc(object parameter)
		{
			await _callBack();
		}
	}
}
