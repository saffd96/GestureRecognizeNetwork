namespace NeuralNetwork.Interfaces
{
    public interface IDataSaver
    {
        string FilePath { get; set; }
        void SaveData(params object[] par);
    }
}