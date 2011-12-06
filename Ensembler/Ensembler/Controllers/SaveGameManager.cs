using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Storage;
using System.IO;
using System.Xml.Serialization;

namespace EnsemPro
{
    [Serializable]
    public struct SaveGameData
    {   public uint[] IDs;
        public DataTypes.WorldData.CityState[] States;
        public string[] Titles;
        public int[] HighestScores;
        public int[] HighestCombos;
    }

    public class SaveGameManager
    {

	    public SaveGameManager()
	    {
            
	    }

        private static void SaveGame(StorageDevice device)
        {
            // need to get all data
            
            SaveGameData data = new SaveGameData();
            
            // and add all data
            

            IAsyncResult result = device.BeginOpenContainer("EnsemPro", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();
            string filename = "SavedGame.sav";
            if (container.FileExists(filename)) container.DeleteFile(filename);
            Stream stream = container.CreateFile(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(SaveGameData));
            serializer.Serialize(stream, data);
            stream.Close();
            container.Dispose();
        }

        private static void LoadGame(StorageDevice device)
        {

        }
    }
}
