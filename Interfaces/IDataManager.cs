namespace NeuralNetwork.Interfaces
{
    public interface IDataManager<T>
    {
       IDataSaver DataSaver {get;}
       IDataRecorder<T> DataRecorder {get;}
    }
}