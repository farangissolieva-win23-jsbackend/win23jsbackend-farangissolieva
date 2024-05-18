using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Filters;

public class CheckBoxRequired : ValidationAttribute
{
    public override bool IsValid(object? value) => value is bool b && b;
}
