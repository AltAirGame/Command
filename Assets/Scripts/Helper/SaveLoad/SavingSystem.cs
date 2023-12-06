using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace GameSystems.Core
{
    public class SavingSystem : MonoBehaviour, IResourcesService
    {
        private string GetFilePath(string fileName)
        {
            
            var filepath= Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log($"<color=green>File Path is {filepath} </green>");
            return filepath;
        }

        public ResourceResponse Save(string saveFile, string saveSubject)
        {
            var response = new ResourceResponse();
            var path = GetFilePath(saveFile);

            try
            {
                File.WriteAllText(path, saveSubject);
                response.isSuccess = true;
                response.message = $"Data saved to {path}";
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = $"Save error: {ex.Message}";
            }

            Util.ShowMessage(response.message, TextColor.Yellow);
            return response;
        }

        public ResourceResponse Load(string saveFile)
        {
            var response = new ResourceResponse();
            var path = GetFilePath(saveFile);

            if (!File.Exists(path))
            {
                response.isSuccess = false;
                response.message = "File not found.";
                Util.ShowMessage(response.message, TextColor.Yellow);
                return response;
            }

            try
            {
                var loadedData = File.ReadAllText(path);
                response.isSuccess = true;
                response.message = "Data loaded successfully.";
                response.body = string.IsNullOrEmpty(loadedData) ? "null" : loadedData;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.message = $"Load error: {ex.Message}";
            }

            Util.ShowMessage(response.message, TextColor.Yellow);
            return response;
        }
    }
}
