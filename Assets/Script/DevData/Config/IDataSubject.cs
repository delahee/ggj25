using System.Collections.Generic;

public interface IDataSubject
{
    void AttachObserver(IDataObserver observer);
    void DetachObserver(IDataObserver observer);
    void NotifyObservers(List<Currencies> loreFacts);
}