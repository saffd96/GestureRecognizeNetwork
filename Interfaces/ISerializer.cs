namespace NeuralNetwork.Interfaces
{
    public interface ISerializer<in T>
    {
        string Serialize(T gesture);
    }
}