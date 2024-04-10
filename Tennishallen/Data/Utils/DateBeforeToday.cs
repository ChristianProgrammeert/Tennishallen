using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
namespace Tennishallen.Data.Utils;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DateBeforeToday : ValidationAttribute
{

    public DateBeforeToday() 
        : base($"Datum mag niet later zijn vandaag")
    {
    }

    /// <summary>
    /// Check if value is before today
    /// </summary>
    /// <param name="value">the value to validate</param>
    /// <returns>True if datetime is before now</returns>
    public override bool IsValid(object? value)
    {
        if (!(value is DateTime birthdate))
            return true;
        return birthdate < DateTime.Now;
    }
}