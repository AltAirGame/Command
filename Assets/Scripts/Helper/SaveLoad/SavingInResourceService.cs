using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace GameSystems.Core
{
    public class SavingInResourceService : MonoBehaviour, IResourcesService
    {
        public ResourceResponse Save(string saveFile, String SaveSubject)
        {
            var response = new ResourceResponse();
            var path = $"Assets/Resources/Save/{saveFile}";
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.Write(SaveSubject);
                }
            }
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            response.isSuccess = true;
            response.message = $"We Saved To {path}";
            Util.ShowMessage($"{response.message}", TextColor.Yellow);
            response.body = "Okay";
            return response;
        }

        public ResourceResponse Load(string saveFile)
        {
            var response = new ResourceResponse();
            var path = "Save/" + saveFile;


            var correctedPath = Path.ChangeExtension(path, null);


            var someString = Resources.Load<TextAsset>(correctedPath);
            var st = someString.text;

            response.body = st;
            if (response.body == "null")
            {
                //there is A Bad Saved File So W Must Make A Default type to Load
                var gameData = new GameData();
                st = JsonConvert.SerializeObject(gameData);
                response.body = st;
            }
            else
            {
                response.body = st;
            }


            response.isSuccess = true;
            response.message = "No Error";
            Util.ShowMessage($" We Loaded From {correctedPath}");
            return response;
        }
    }
}