using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwPrpUtil.Infrastructure.Commands.Deprecated
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}
