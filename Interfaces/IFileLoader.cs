using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface IFileLoader
    {
        public List<string> LoadedFiles { get; set; }
        public void LoadFiles();
    }
}