using P3R.WeaponFramework.Weapons.Models;

namespace P3R.WeaponFramework.Exceptions;

[Serializable]
public class NoMatchingWeaponException : Exception
{
	public NoMatchingWeaponException() { }
	public NoMatchingWeaponException(string message) : base(message) { }
	public NoMatchingWeaponException(string message, Exception inner) : base(message, inner) { }
}


[Serializable]
public class NoEmptySlotException : Exception
{
	public NoEmptySlotException() { }
	public NoEmptySlotException(string message) : base(message) { }
	public NoEmptySlotException(string message, Exception inner) : base(message, inner) { }
}


[Serializable]
public class EmptyWeaponListException : Exception
{
	public EmptyWeaponListException() { }
	public EmptyWeaponListException(string message) : base(message) { }
	public EmptyWeaponListException(string message, Exception inner) : base(message, inner) { }
	protected EmptyWeaponListException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}