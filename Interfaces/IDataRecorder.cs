namespace NeuralNetwork.Interfaces
{
    public interface IDataRecorder<T>
    {
        bool IsRecording { get; set; }

        void StartRecording(T gesture);
        void Record();
        void StopRecording();
        void CancelRecording();
    }
}