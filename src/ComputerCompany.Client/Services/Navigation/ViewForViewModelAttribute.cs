namespace ComputerCompany.Presentation.Services.Navigation;

[AttributeUsage(AttributeTargets.Class)]
public class ViewForViewModelAttribute(Type viewType) : Attribute
{
    public Type ViewType { get; } = viewType;
}