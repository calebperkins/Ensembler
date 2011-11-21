using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using System.Xml.Serialization;
using System.IO;

namespace EnsemPro.Controllers
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
                state.Levels = data.Levels;
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
                    IAsyncResult result = device.BeginOpenContainer(dir, null, null);
                    result.AsyncWaitHandle.WaitOne(); // todo: replace
                    StorageContainer container = device.EndOpenContainer(result);
                    // Close the wait handle.
                    result.AsyncWaitHandle.Close();

                    // Check to see whether the save exists.
                    if (container.FileExists(filename))
                        // Delete it so that we can create one fresh.
                        container.DeleteFile(filename);

                    // Create the file.
                    Stream stream = container.CreateFile(filename);

                    // Convert the object to XML data and put it in the stream.
                    XmlSerializer serializer = new XmlSerializer(typeof(DataTypes.GameData));

                    serializer.Serialize(stream, state.Serialized());

                    container.Dispose();
                }
                // Reset the request initiation flag
                saveInitiated = false;
            }
            base.Update(gameTime);
        }
    }
}
