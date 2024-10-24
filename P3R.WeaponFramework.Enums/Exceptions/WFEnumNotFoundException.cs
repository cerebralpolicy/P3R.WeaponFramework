﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P3R.WeaponFramework.Enums.Exceptions;

[Serializable]
public class WFEnumNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WFEnumNotFoundException"/> class.
    /// </summary>
    public WFEnumNotFoundException()
        : base()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WFEnumNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public WFEnumNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WFEnumNotFoundException"/> class with a specified error message and
    /// a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">
    /// The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference,
    /// the current exception is raised in a <c>catch</c> block that handles the inner exception.
    /// </param>
    public WFEnumNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
