using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEditor;

    namespace Serializator
    {
        public class FileDataHandler:IFileDataHandler
        {
            // Start is called before the first frame update
            private string dataDirPath = "";
            private string dataFileName = "";
            public FileDataHandler() { }
        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
        }
        public void Init(string dataDirPath, string dataFileName)
            {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
            }
            public GameData Load(string profileId)
            {
                if (profileId == null)
                {
                    return null;
                }
                string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
                GameData loadedData = null;
                if (File.Exists(fullPath))
                {
                    try
                    {
                        string dataToLoad = "";
                        using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                dataToLoad = reader.ReadToEnd();
                            }
                        }
                        loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                    }
                    catch (System.Exception e)
                    {
                        //Debug.LogError("Failed loading data");
                    }
                }
                return loadedData;
            }
            public void Save(GameData data, string profileId)
            {
                if (profileId == null)
                {
                    return;
                }
                string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                    string dataToStore = JsonUtility.ToJson(data, true);
                    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(dataToStore);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    //Debug.LogError(ex.Message);
                }
            }
            public Dictionary<string, GameData> LoadAllProfiles()
            {
                Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();
                IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
                foreach (DirectoryInfo dirInfo in dirInfos)
                {
                    string profileId = dirInfo.Name;
                    string fullPath = Path.Combine(dataDirPath, profileId, dataFileName);
                    if (!File.Exists(fullPath))
                    {
                        //Debug.LogWarning("Skipping directory when loding al profiles, it doesn't contains data " + profileId);
                        continue;
                    }
                    GameData profileData = Load(profileId);
                    if (profileData != null)
                    {
                        profileDictionary.Add(profileId, profileData);
                    }
                    else
                    {
                        //Debug.LogError("Tried to load but smth went wrong");
                    }
                }
                return profileDictionary;
            }
            public string GetMostRecentlyUpdatedProfile()
            {
                string mostRecentProfileId = null;
                Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
                foreach (KeyValuePair<string, GameData> pair in profilesGameData)
                {
                    string profileId = pair.Key;
                    GameData gameData = pair.Value;
                    if (gameData == null)
                    {
                        continue;
                    }
                    if (mostRecentProfileId == null)
                    {
                        mostRecentProfileId = profileId;
                    }
                    else
                    {
                        DateTime mostRecentDateTime = DateTime.FromBinary(profilesGameData[mostRecentProfileId].lastUpdated);
                        DateTime newDateTime = DateTime.FromBinary(gameData.lastUpdated);
                        if (newDateTime > mostRecentDateTime)
                        {
                            mostRecentProfileId = profileId;
                        }
                    }
                }
                return mostRecentProfileId;
            }
        }

    }

