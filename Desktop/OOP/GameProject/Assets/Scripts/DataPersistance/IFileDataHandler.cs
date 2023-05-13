using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameProjest
{
    public interface IFileDataHandler
    {
        void Init(string dataDirPath, string dataFileName);
        GameData Load(string profileId);
        void Save(GameData data, string profileId);
        Dictionary<string, GameData> LoadAllProfiles();
        string GetMostRecentlyUpdatedProfile();
    }
}