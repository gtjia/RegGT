using System;
using System.Collections.Generic;
using System.Text;

namespace Gt.Core.Utilities
{
	public class BussinessException : Exception
	{
		public BussinessException(string message): base(message)
		{
		}
	}
}
