using Presets;
using UnityEngine;
using UnityEngine.UI;
using Action = System.Action;

namespace NeuralNetwork.DataRecord
{
    public class RecordButton : MonoBehaviour
    {
        [SerializeField] private GestureType gesture;
        [SerializeField] private Button button;
    
        public GestureType Gesture => gesture;
        public Button Button => button;

        private void OnValidate()
        {
            if (!button)
            {
                button = GetComponent<Button>();
            }
        }

        public void SetOnClickAction(Action action)
        {
            button.onClick.AddListener(()=> action?.Invoke());
        }
    }
}