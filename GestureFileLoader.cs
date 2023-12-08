using System.Collections.Generic;
using System.IO;
using System.Linq;
using NeuralNetwork.Interfaces;
using UnityEngine;

namespace NeuralNetwork
{
    public class GestureFileLoader : IFileLoader
    {
        private readonly string _path;
        private readonly string _containingPart;
        public List<string> LoadedFiles { get; set; }

        public GestureFileLoader()
        {
            _path = Application.persistentDataPath;
            _containingPart = "Data";
        }

        public void LoadFiles()
        {
            LoadedFiles = Directory.GetFiles(_path)
                .Where(f => Path.GetFileNameWithoutExtension(f).Contains(_containingPart)).ToList();
        }
    }
}