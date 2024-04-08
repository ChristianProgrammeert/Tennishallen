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


    public override bool IsValid(object? value)
    {
        if (!(value is DateTime birthdate))
            return true;
        return birthdate < DateTime.Now;
    }
}