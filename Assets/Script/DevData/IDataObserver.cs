using System.Collections.Generic;

public interface IDataObserver
{
    void OnDataReceived(List<Currencies> data);
}