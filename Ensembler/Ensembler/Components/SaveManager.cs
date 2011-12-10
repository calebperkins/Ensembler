using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

namespace Ensembler.Components
{
    /// <summary>
    /// This is a game component that saves the game asynchronously (mostly).
    /// Currently, the presence of WaitOne calls blocks the thread. Figure out how to do this better, thanks.
    /// </summary>
    public class SaveManager : GameComponent
    {
        bool saveInitiated = false;
        IAsyncResult gameSaveResult;
        GameState state;

        const string dir = "Scores";
        const string filename = "scores.xml";
        const string filename2 = "cities.xml";

        public SaveManager(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Load the saved game file if it exists.
        /// </summary>
        public override void Initialize()
        {
            state = Game.Services.GetService(typeof(GameState)) as GameState;


            
            // Open a storage container.
            IAsyncResult loadResult = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            loadResult.AsyncWaitHandle.WaitOne();
            StorageDevice device = StorageDevice.EndShowSelector(loadResult);
            IAsyncResult result = device.BeginOpenContainer(dir, null, null);

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            StorageContainer container = device.EndOpenContainer(result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            // Check to see whether the save exists. If not, dispose of the container and return.
            if (container.FileExists(filename))
            {
                Stream stream = container.OpenFile(filename, FileMode.Open);
                XmlSerializer serializer = new XmlSerializer(typeof(DataTypes.GameData));
                DataTypes.GameData data = serializer.Deserialize(stream) as DataTypes.GameData;

                // Merge scores
                Dictionary<string, DataTypes.LevelSummary> scoredLevels;
                try
                {
                    scoredLevels = data.Levels.ToDictionary<DataTypes.LevelSummary, string>(k => k.Title);
                }
                catch (ArgumentException)
                {
                    // Console.Error.WriteLine("Save file corrupted.");
                    scoredLevels = new Dictionary<string, DataTypes.LevelSummary>();
                }
                for (int i = 0; i < state.Levels.Length; i++)
                {
                    try
                    {
                        state.Levels[i].HighScore = scoredLevels[state.Levels[i].Title].HighScore;
                        state.Levels[i].HighCombo = scoredLevels[state.Levels[i].Title].HighCombo;
                    }
                    catch (KeyNotFoundException) { }
                }

            }
            container.Dispose();
            base.Initialize();
        }

        /// <summary>
        /// If a save has been requested, initiate a save.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (state.SaveRequested && !Guide.IsVisible && !saveInitiated)
            {
                saveInitiated = true;
                state.SaveRequested = false;
                gameSaveResult = StorageDevice.BeginShowSelector(PlayerIndex.One, null, null);
            }
            if (saveInitiated && gameSaveResult.IsCompleted)
            {
                StorageDevice device = StorageDevice.EndShowSelector(gameSaveResult);
                if (device != null && device.IsConnected)
                {
                    //try
                    {
                        IAsyncResult result = device.BeginOpenContainer(dir, null, null);
                        result.AsyncWaitHandle.WaitOne(); // todo: replace
                        StorageContainer container = device.EndOpenContainer(result);
                        // Close the wait handle.
                        result.AsyncWaitHandle.Close();

                        // Check to see whether the save exists.
                        if (container.FileExists(filename))
                        {
                            // Delete it so that we can create one fresh.
                            container.DeleteFile(filename);
                        }

                        if (container.FileExists(filename2))
                        {
                            container.DeleteFile(filename2);
                        }

                        // Create the file.
                        Stream stream = container.CreateFile(filename);
                        Stream stream2 = container.CreateFile(filename2);

                        // Convert the object to XML data and put it in the stream.
                        XmlSerializer serializer = new XmlSerializer(typeof(DataTypes.GameData));
                        XmlSerializer serializer2 = new XmlSerializer(typeof(DataTypes.WorldData));

                        serializer.Serialize(stream, state.Serialized());
                        serializer2.Serialize(stream2, state.Serialized2());

                        container.Dispose();
                    }
                    //catch { }
                }
                // Reset the request initiation flag
                saveInitiated = false;
            }
            base.Update(gameTime);
        }
    }
}
