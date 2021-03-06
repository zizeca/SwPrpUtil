using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SwPrpUtil.Infrastructure.Commands.Deprecated
{
	public interface IAsyncCommand : ICommand
	{
		Task ExecuteAsync();
		bool CanExecute();
		void HandleException(Exception ex);
	}

	public interface IAsyncCommand<T> : ICommand
	{
		Task ExecuteAsync(T parameter);
		bool CanExecute(T parameter);
		void HandleException(Exception ex);
	}
}
