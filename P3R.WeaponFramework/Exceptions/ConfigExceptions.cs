using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Exceptions;

public class InvalidShellException : Exception
{
	public InvalidShellException() { }
	public InvalidShellException(string message) : base(message) { }
	public InvalidShellException(string message, Exception inner) : base(message, inner) { }
}

[Serializable]
public class InvalidPriceException : Exception
{
	public InvalidPriceException() { }
	public InvalidPriceException(string message) : base(message) { }
	public InvalidPriceException(string message, Exception inner) : base(message, inner) { }
}
