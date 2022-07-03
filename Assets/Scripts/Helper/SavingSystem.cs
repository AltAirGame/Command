using System;
using System.IO;
using System.Text;
using MHamidi;
using Newtonsoft.Json;
using UnityEngine;

namespace Helper
{
    
    
    //You Can Replace this part with An Other Module Like a Clint Wich Request to The Server and Load the Data form the Server 
    public class SavingSystem : MonoBehaviour, IResources
    {
        public ResurceResponse Save(string saveFile, String SaveSubject)
        {
            var response = new ResurceResponse();
            
            var path = Util.GetPersistentDataPath(saveFile);

            using (var stream = File.Open(path, FileMode.Create))
            {
                var bytes = Util.SerilizeStringToByte(SaveSubject);
                stream.Write(bytes, 0, bytes.Length);
                
            }

            response.isSuccess = true;
            response.message = $"We Saved To {path}";
            Util.ShowMessag($"{response.message}",TextColor.Yellow);
            response.body = "Okay";
            return response;
        }

        public ResurceResponse Load(string saveFile)
        {
            var response = new ResurceResponse();
            var path = Util.GetPersistentDataPath(saveFile);
            if (!File.Exists(path))
            {
                response.body = String.Empty;
                response.isSuccess = false;
                response.message = "There is No File in the Directory";
                Util.ShowMessag($"{response.message}",TextColor.Yellow);
                return response;
            }

            using (var stream = File.Open(path, FileMode.Open))
            {
                var buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);
                var st = Encoding.UTF8.GetString(buffer);
                if (string.IsNullOrWhiteSpace(st))
                {
                    response.body = String.Empty;
                    response.isSuccess = false;
                    response.message = "The Loaded String Is Empty Or Null";
                    Util.ShowMessag($"{response.message}",TextColor.Yellow);
                    return response;
                }
                else
                {
                    response.body = st;
                    if (response.body=="null")
                    {
                        //there is A Bad Saved File So W Must Make A Default type to Load
                        var gameData = new GameData();
                        st = JsonConvert.SerializeObject(gameData);
                        response.body = st;
                    }
                    response.isSuccess = true;
                    response.message = "No Error";
                    Util.ShowMessag($"{response.message}",TextColor.Yellow);

                    return response;
                }
            }
        }


       
    }
}