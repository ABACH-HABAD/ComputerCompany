namespace ComputerCompany.Application.Client.ViewModels.Events;

public class LoadCollectionEventArgs<T>(IEnumerable<T> collection) : EventArgs
{
    public IEnumerable<T> Collection = collection;
}