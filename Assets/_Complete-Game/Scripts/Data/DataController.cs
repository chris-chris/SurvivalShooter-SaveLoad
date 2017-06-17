using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataController : MonoBehaviour {

	private string gameDataProjectFilePath = "/data.json";
	private string metaDataProjectFilePath = "/meta.json";

	public GameData gameData;
	public MetaData metaData;

	//싱글톤 객체를 설정하는 부분입니다.
	static DataController _instance;
	public static DataController Instance {
		get {
			if( ! _instance ) {
				GameObject container = new GameObject("DataController");
				_instance = container.AddComponent( 
					typeof( DataController ) ) as DataController;
				
				_instance.LoadGameData ();
				_instance.LoadMetaData ();
				DontDestroyOnLoad( container );
			}
			return _instance;
		}
	}

	public void LoadGameData()
	{
		string filePath = Application.dataPath + gameDataProjectFilePath;

		if (File.Exists (filePath)) {
			Debug.Log ("loaded!");
			string dataAsJson = File.ReadAllText (filePath);
			gameData = JsonUtility.FromJson<GameData> (dataAsJson);
		} else 
		{
			Debug.Log ("Create new");

			gameData = new GameData ();
			gameData.Point = 0;
			gameData.Level = 1;
			gameData.Exp = 0;
			gameData.Health = 100;
			gameData.Damage = 20;

		}
	}

	public void SaveGameData()
	{

		string dataAsJson = JsonUtility.ToJson (gameData);

		string filePath = Application.dataPath + gameDataProjectFilePath;
		File.WriteAllText (filePath, dataAsJson);

	}


	public void LoadMetaData()
	{
		string filePath = Application.dataPath + metaDataProjectFilePath;

		if (File.Exists (filePath)) {
			Debug.Log ("meta loaded!");
			string metaAsJson = File.ReadAllText (filePath);
			metaData = JsonUtility.FromJson<MetaData> (metaAsJson);
		} else 
		{
			Debug.LogError ("MetaData does not exists!");

			metaData = new MetaData ();

			LevelInfo[] levels = new LevelInfo[3];
			levels[0] = new LevelInfo(1, 0);
			levels[1] = new LevelInfo(2, 100);  
			levels[2] = new LevelInfo(3, 250);  

			metaData.LevelInfoList = levels;

			SaveMetaData ();

		}
	}

	public void SaveMetaData()
	{
		Debug.Log ("save new meta");

		string metaAsJson = JsonUtility.ToJson (metaData);

		string filePath = Application.dataPath + metaDataProjectFilePath;
		File.WriteAllText (filePath, metaAsJson);

	}

	public void AddExp(int _exp)
	{
		gameData.Exp += _exp;

		int newLevel = CalcLevel (gameData.Exp);

		if (newLevel > gameData.Level) {
			Debug.Log ("Level UP!!!!");
			gameData.Level = newLevel;
			gameData.Damage *= 2;
			gameData.Health *= 2;
		}

	}

	public int CalcLevel(int _exp)
	{
		int maxLevel = 0;
		foreach (LevelInfo info in metaData.LevelInfoList) {
			if (_exp >= info.Exp) {
				if (info.Level >= maxLevel) {
					maxLevel = info.Level;
				}
			}
		}
		return maxLevel;
	}
}
