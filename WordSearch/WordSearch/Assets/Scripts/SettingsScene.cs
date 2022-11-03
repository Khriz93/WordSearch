using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsScene : MonoBehaviour
{
    public GameLevelData levelData;

    public void ClearGameData()
    {
        DataSaver.ClearGameData(levelData); 
    }
}
