using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Loader : MonoBehaviour
{
    public void SaveFile(GameData currentData)
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if(File.Exists(destination)) file = File.OpenWrite(destination);
        else file = File.Create(destination);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, currentData);
        file.Close();
    }
 
    public GameData LoadFile(bool writeNew)
    {
        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (writeNew) 
            return new GameData(10000f, 2.5f, 25.0f, true, true);

        if(File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.Log("Couldn't load");
            return new GameData(10000f, 2.5f, 25.0f, true, true);
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData) bf.Deserialize(file);
        file.Close();

        Debug.Log(data.coinAmount + " " + data.currentBet);

        return data;
    }
}


[System.Serializable]
public class GameData
{
    public float coinAmount;
    public float currentBet;
    public float currentBonusBet;
    public bool currentMusic;
    public bool currentSfx;

    public GameData(float coin, float bet, float bonus, bool music, bool sfx)
    {
        coinAmount = coin;
        currentBet = bet;
        currentBonusBet = bonus;
        currentMusic = music;
        currentSfx = sfx;
    }
}