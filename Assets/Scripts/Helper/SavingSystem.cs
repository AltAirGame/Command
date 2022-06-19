using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Helper
{
    public static class SavingSystem 
    {
    
    
        public static void Save(string saveFile,String SaveSubject)
        {
            var path = GetPathFromSaveFile(saveFile);

            using (var stream = File.Open(path, FileMode.Create))
            {
                var bytes = SerializeString(SaveSubject);
                stream.Write(bytes, 0, bytes.Length);
            }

  
            Debug.Log($" Saving to {path}");
        }

        public static string Load(string saveFile) 
        {
            var path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return string.Empty;
            }
            using (var stream = File.Open(path, FileMode.Open))
            {
                var buffer = new byte[stream.Length];
          
                stream.Read(buffer, 0, buffer.Length);
                var st=Encoding.UTF8.GetString(buffer);
                Debug.Log(st);
                return st;

            }

       
        }


        private static Byte[] SerializeString(string file)
        {
            return Encoding.UTF8.GetBytes(file);
       
        }
        private static string GetPathFromSaveFile(string SaveFile)
        {
            return Path.Combine(Application.persistentDataPath, SaveFile);
        }
    }
}