using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerLogic.Classes
{
    internal class UserException: ApplicationException
    {
        public UserException(string message, int ErrorCode) :base(message) { this.ErrorCode = ErrorCode; }

        public int ErrorCode { get; set; }
    }
}
