using System.Collections.Generic;
using NeuralNetwork.Interfaces;
using Presets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NeuralNetwork.DataRecord
{
    public class DataRecorder : MonoBehaviour
    {
        [SerializeField] private List<RecordButton> _moveButtons = new List<RecordButton>();
        [SerializeField] private Button cancel, stop;
        [SerializeField] private TextMeshProUGUI textMeshPro;
        
        private bool _isRecording;
        private GestureType _currentGesture;

        private IDataSaver _dataSaver;
        private IDataRecorder<GestureType> _dataRecorder;
        private DataManager<GestureType> _dataManager;

        private void OnValidate()
        {
            foreach (var moveButton in _moveButtons)
            {
                moveButton.GetComponentInChildren<TextMeshProUGUI>().text = moveButton.Gesture.ToString();
            }
        }

        private void Awake()
        {
            _dataSaver = new GestureDataSaver(new JSONSerializer<GestureData>());
            _dataRecorder = new GestureRecorder(_dataSaver, 60, 32);

            _dataManager = new DataManager<GestureType>(_dataSaver, _dataRecorder);

            Input.gyro.enabled = true;

            SetupButtons();

            ChangeButtonsState(false);
        }

        private void Update()
        {
            _dataManager.DataRecorder.Record();
        }

        private void SetupButtons()
        {
            foreach (var moveButton in _moveButtons)
            {
                moveButton.SetOnClickAction(() => StartRecording(moveButton.Gesture));
            }

            stop.onClick.AddListener(StopRecording);
            cancel.onClick.AddListener(CancelRecording);
        }

        private void StartRecording(GestureType gesture)
        {
            _dataManager.DataRecorder.StartRecording(gesture);
            ChangeButtonsState(true);
        }

        private void StopRecording()
        {
            _dataManager.DataRecorder.StopRecording();

            textMeshPro.text = _dataManager.DataSaver.FilePath;
            ChangeButtonsState(false);
        }

        private void CancelRecording()
        {
            _dataManager.DataRecorder.CancelRecording();
            ChangeButtonsState(false);
        }

        private void ChangeButtonsState(bool isRecordingState)
        {
            foreach (var button in _moveButtons)
            {
                button.Button.interactable = !isRecordingState;
            }

            stop.interactable = isRecordingState;
            cancel.interactable = isRecordingState;
        }
    }
}