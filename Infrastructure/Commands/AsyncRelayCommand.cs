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

		private readonly Func<Task> _callBackAsync;
		private readonly Func<bool> _canExecute;
		private readonly Action<Exception> _exception;

		public AsyncRelayCommand(Func<Task> callBackAsync, Func<bool> canExecute = null, Action<Exception> extension = null)
		{
			_callBackAsync = callBackAsync ?? throw new ArgumentNullException(nameof(callBackAsync));
			_callBackAsync = callBackAsync;
			_canExecute = canExecute;
			_exception = extension;
		}

		public override bool CanExecute()
		{
			return _canExecute?.Invoke() ?? true;
		}

		public override void HandleException(Exception ex)
		{
			if(_exception != null)
				_exception.Invoke(ex);
			else
				base.HandleException(ex);
		}

		public override async Task ExecuteAsync()
		{
			await _callBackAsync.Invoke();
		}
	}
}
