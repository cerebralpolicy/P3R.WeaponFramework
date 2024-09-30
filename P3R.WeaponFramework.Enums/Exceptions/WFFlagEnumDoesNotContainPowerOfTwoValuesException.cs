﻿namespace P3R.WeaponFramework.Enums.Exceptions;

public class WFFlagEnumDoesNotContainPowerOfTwoValuesException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WFFlagEnumDoesNotContainPowerOfTwoValuesException"/> class.
    /// </summary>
    public WFFlagEnumDoesNotContainPowerOfTwoValuesException()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WFFlagEnumDoesNotContainPowerOfTwoValuesException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public WFFlagEnumDoesNotContainPowerOfTwoValuesException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WFFlagEnumDoesNotContainPowerOfTwoValuesException"/> class with a specified error message and
    /// a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference,
    /// the current exception is raised in a <c>catch</c> block that handles the inner exception.
    /// </param>
    public WFFlagEnumDoesNotContainPowerOfTwoValuesException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

}
