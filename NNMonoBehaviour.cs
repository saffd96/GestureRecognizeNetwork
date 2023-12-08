using UnityEngine;

namespace NeuralNetwork
{
    public class NNMonoBehaviour : MonoBehaviour
    {
        [SerializeField] private int epochs;

        private NNController _nnController;

        private void Start()
        {
            InitNetwork();
        }

        private void InitNetwork()
        {
            _nnController = new NNController(new JSONSerializer<GestureData>(), new GestureFileLoader());

            _nnController.TrainModel(epochs);

            _nnController.GetPrediction(_nnController.NnTrainingDataContainer.GetTrainingData()[0].Item1);
        }
    }
}