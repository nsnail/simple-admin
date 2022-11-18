namespace SimpleAdmin.WebApi.Aop.Attributes;

/// <summary>
///     必填项约束
/// </summary>
public class RequiredFieldAttribute : RequiredAttribute
{
    /// <inheritdoc />
    public override string FormatErrorMessage(string whatever)
    {
        return !string.IsNullOrEmpty(ErrorMessage) ? ErrorMessage : $"{whatever} 是必填项";
    }
}