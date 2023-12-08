namespace NeuralNetwork.Interfaces
{
    public interface IDeserializer<out T>
    {
        T Deserialize(string path);
    }
}