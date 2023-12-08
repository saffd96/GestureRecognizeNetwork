using NeuralNetwork.Interfaces;

namespace NeuralNetwork.DataRecord
{
    public class DataManager<T> : IDataManager<T>
    {
        public IDataSaver DataSaver { get; }
        public IDataRecorder<T> DataRecorder { get; }

        public DataManager(IDataSaver dataSaver,IDataRecorder<T> dataRecorder)
        {
            DataSaver = dataSaver;
            DataRecorder = dataRecorder;
        }
    }
}