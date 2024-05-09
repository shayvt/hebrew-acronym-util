// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices;

public class RequiredMemberAttribute : Attribute { }
public class CompilerFeatureRequiredAttribute : Attribute
{
    public CompilerFeatureRequiredAttribute(string name) { }
}