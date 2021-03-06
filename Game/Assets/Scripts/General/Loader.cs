﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System;
using System.Runtime.Serialization;
using System.Reflection;

public class Loader : MonoSingleton<Loader>
{
	class MovableObject
	{
		public Vector3 Pos{ get; set; }
		public Vector3 Rotation{ get; set; }


	}
    //stale nazwy rekordów
    //player
    private const string anySaves = "AnySaves";
    private const string healthStr = "Health";
    private const string syfStr = "Syf";
    private const string ammoStr = "Ammo";
    //poziom trudnosci
    private const string roundStr = "Round";    //ile cykli juz minelo
    private const string waveSizeStr = "WaveSize";
    private const string spawnRateStr = "SpawnRate";
    //autosave
    public const string autoSaveStr = "autoSave";
    private const string levelStr = "Level";
    private static string saveToLoad = null;
    public static bool isLoading = false;
    //ustawiane zanim zmienimy scene zeby bylo wiadomo ktory save wczytac
    
    public void save(string saveName = autoSaveStr)
    {
        Debug.Log("Saving: " + saveName);
		//DODANIE JEZELI NIE ISTENIEJE SAVE DO LISTY SAVE'OW
		checkandAdd(saveName);
		//ZAPISANIE INFORMACJI
		if(saveName != autoSaveStr)	//mozemy tylko zapisywać stan z ostaniego czeckpointu. Nawet jak zapiszemy w dowolnym momencie to po prostu wartosci zostana skopiowane z ostanitego autosave'a
		{
			Debug.Log ("Różny");
			copyValuesFromLastCheckpoint(saveName);
			saveMovableObjects(saveName);
		}
        else
		{
       	 	 PlayerPrefs.SetInt(anySaves, PlayerPrefs.GetInt(anySaves, 0));
        	 PlayerPrefs.SetFloat(healthStr + saveName, PlayerStats.instance.health);
      		 PlayerPrefs.SetInt(syfStr + saveName, PlayerStats.instance.syf);
      		 PlayerPrefs.SetInt(ammoStr + saveName, LaserRifle.instance.ammo);
       		 PlayerPrefs.SetInt(roundStr + saveName, GameMaster.instance.currentRound);
       		 PlayerPrefs.SetInt(waveSizeStr + saveName, GameMaster.instance.waveSize);
       		 PlayerPrefs.SetFloat(spawnRateStr + saveName, GameMaster.instance.spawnRate);
       		 PlayerPrefs.SetInt(levelStr + saveName, Application.loadedLevel);
			 Debug.Log ("Taki sam");
			 saveMovableObjects(saveName);
		}
		PlayerPrefs.Save();
		HUD.instance.setHintvisible("Game Saved", 2);
    }

    public void load(string saveName = autoSaveStr) //domyslnie autoLoad
    {
        saveToLoad = saveName;
        isLoading = true;
        if (saveName == "NewGame")
            Application.LoadLevel(2); //TO DO CHECK NA KONCU CZY 1 TO TEN LEVEL O KTORY NAM CHODZI
        else
            Application.LoadLevel(PlayerPrefs.GetInt(levelStr + saveName));
    }
    public void continueLoading()
    {
        if (!isLoading || saveToLoad == null)
        {
            return;
        }
        string saveName = saveToLoad;
        Debug.Log("Loaded save :" + saveName);
        Debug.Log("Health = " + PlayerPrefs.GetFloat(healthStr + saveName));
        Debug.Log("Syf = " + PlayerPrefs.GetInt(syfStr + saveName));
        Debug.Log("Round = " + PlayerPrefs.GetInt(roundStr + saveName));
        Debug.Log("Ammo = " + PlayerPrefs.GetInt(ammoStr + saveName));
        PlayerStats.instance.health = PlayerPrefs.GetFloat(healthStr + saveName);
        PlayerStats.instance.syf = PlayerPrefs.GetInt(syfStr + saveName);
        LaserRifle.instance.ammo = PlayerPrefs.GetInt(ammoStr + saveName);
        GameMaster.instance.currentRound = PlayerPrefs.GetInt(roundStr + saveName);
        GameMaster.instance.waveSize = PlayerPrefs.GetInt(waveSizeStr + saveName);
        GameMaster.instance.spawnRate = PlayerPrefs.GetFloat(spawnRateStr + saveName);
		loadMovableObjects(saveName);
        saveToLoad = null;
        isLoading = false;
        HUD.instance.setHintvisible("Game Loaded", 2);
    }
	void loadMovableObjects(string SaveName)
	{
		if(LevelSerializer.SavedGames.Count > 0)
		{
			//Look for saved games under the given player name
			foreach(var g in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
			{
				if(g.Name==SaveName){
					foreach(GameObject oldMovableOBJ in GameObject.FindGameObjectsWithTag("movable"))
					{
					        Destroy(oldMovableOBJ);
							Debug.Log("usunieto movable:" + oldMovableOBJ.name);
					}
					g.Load();
					Debug.Log("zaladowano movable");
				}
			}
		}
	}
	void saveMovableObjects(string SaveName)
	{
		LevelSerializer.SaveGame (SaveName);
	}
    public bool areThereAnySaves()
    {
        return PlayerPrefs.HasKey(anySaves);
    }

    public List<string> getSaves()
    {
        List<string> lista = new List<string>();
        int i;
        for (i = 0; PlayerPrefs.HasKey(i.ToString()); i++) {}
        for (i = i-1; i >= 0; i--)
        {
            lista.Add(PlayerPrefs.GetString(i.ToString()));
        }
        return lista;
    }

    void checkandAdd(string saveName)
    {
        int i;
        for (i = 0; PlayerPrefs.HasKey(i.ToString()); i++) {}
        PlayerPrefs.SetString(i.ToString(), saveName);
    }

	void copyValuesFromLastCheckpoint(string saveName)
	{
		PlayerPrefs.SetInt(anySaves, PlayerPrefs.GetInt(anySaves, 0));
		PlayerPrefs.SetFloat(healthStr + saveName, PlayerPrefs.GetFloat(healthStr+autoSaveStr));
		PlayerPrefs.SetInt(syfStr + saveName, PlayerPrefs.GetInt(syfStr+autoSaveStr));
		PlayerPrefs.SetInt(ammoStr + saveName, PlayerPrefs.GetInt(ammoStr+autoSaveStr));
		PlayerPrefs.SetInt(roundStr + saveName, PlayerPrefs.GetInt(roundStr+autoSaveStr));
		PlayerPrefs.SetInt(waveSizeStr + saveName, PlayerPrefs.GetInt(waveSizeStr+autoSaveStr));
		PlayerPrefs.SetFloat(spawnRateStr + saveName, PlayerPrefs.GetFloat(levelStr+autoSaveStr));
		PlayerPrefs.SetInt(levelStr + saveName, Application.loadedLevel);
	}
}

