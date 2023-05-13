using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameProjest
{
    [System.Serializable]
    public class GameData
    {
        //  public int heartCount;
        public int levelNum;
        public long lastUpdated;
        public Vector3 playerPosition;
        public SerializableDictioanary<string, bool> heartsCollected;
        public int characterLives;
        public GameData()
        {
            levelNum = 1;
            characterLives = 14;
            playerPosition = Vector3.zero;
            heartsCollected = new SerializableDictioanary<string, bool>();
        }
        public int GetpercentageComplete()
        {
            int totalCollected = 0;
            foreach (bool collected in heartsCollected.Values)
            {
                if (collected)
                {
                    totalCollected++;
                }
            }
            int percentageCompleted = -1;
            if (heartsCollected.Count != 0)
            {
                percentageCompleted = (totalCollected * 100 / heartsCollected.Count);
            }
            return percentageCompleted;
        }
    }
}