using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Input;

namespace SwPrpUtilWpfTest.Infrastructure.Commands.Base
{
	public abstract class AsyncCommandBase : IAsyncCommand
	{
		private bool _isExecutting;
		//private Action<Exception> _onExeption;
		//private Func<bool> _canExecute;

		public bool IsExecuting
		{
			get => _isExecutting;
			set
			{
				_isExecutting = value;
				CanExecuteChanged?.Invoke(this, new EventArgs());
			}
		}

		public event EventHandler CanExecuteChanged;

		bool ICommand.CanExecute(object parameter)
		{
			//return !IsExecuting && (_canExecute?.Invoke() ?? true);
			return !IsExecuting && CanExecute();
		}
/*
		public AsyncCommandBase(Action<Exception> onExeption = null)
		{
			_onExeption = onExeption;
		}
*/


		public async void Execute(object parameter)
		{
			IsExecuting = true;
			try
			{
				await ExecuteAsync();
			}
			catch (Exception ex)
			{
				
				//_onExeption?.Invoke(ex);

				HandleException(ex);
			}

			IsExecuting = false;
		}


		public virtual void HandleException(Exception ex)
		{
			Debug.WriteLine(string.Format("Catch exception - {0}", ex.Message));
		}
		public virtual bool CanExecute() => true;
		public abstract Task ExecuteAsync();


	}

}
