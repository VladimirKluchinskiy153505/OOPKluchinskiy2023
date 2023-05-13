using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using Serializator;
public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    [SerializeField] private bool overrideSelectedProfileId = false;
    [SerializeField] private string testSelectedProfileId = "test";
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistance> dataPersistanceobjects;
    // private FileDataHandler dataHandler;
    IFileDataHandler dataHandler;
    private string selectedProfileId = "";
    public static DataPersistanceManager Instance { get; private set; }
    private void Awake()
    {
        //string assemblyPath = ("C:\\NewGame\\SerializatorDLL\\Serializator\\bin\\Debug\\Serializator.dll");
        //Assembly asm = Assembly.LoadFrom(assemblyPath);
        //Debug.Log(asm.FullName);
        //Type[] types = asm.GetTypes();
        ////foreach (Type t in types)
        ////{
        ////    Debug.Log(t.Name);
        ////}
        //Type serializator = asm.GetType("Serializator.FileDataHandler");
        ////Type serializator = types[types.Length - 1];
        //Debug.Log(serializator.FullName);
        //dataHandler = (IFileDataHandler)Activator.CreateInstance(serializator);
        dataHandler = (IFileDataHandler)new Serializator.FileDataHandler(Application.persistentDataPath, fileName);
        if (dataHandler != null)
        {
            Debug.Log("Success");
        }
        else { Debug.Log("Error"); }
        if (Instance != null)
        {
            Debug.Log("Found more than one Data persistance manager in the scene.Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        if(disableDataPersistence)
        {
            Debug.LogWarning("Data persistance is currently disabled!");
        }

       // this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.selectedProfileId=dataHandler.GetMostRecentlyUpdatedProfile();
        if(overrideSelectedProfileId)
        {
            this.selectedProfileId=testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }
    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded+= OnSceneUnloaded;
    }
    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("On Scene Loaded Called");
        this.dataPersistanceobjects = FindAllDataPersistanceObjects();
        LoadGame();
    }
    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("On SceneUnloaded Called");
       // SaveGame();
    }
    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;
        LoadGame();
    }
    public void NewGame()
    {
        this.gameData=new GameData();
        dataHandler.Save(this.gameData, selectedProfileId) ;

    }
    public void LoadGame()
    {
        if(disableDataPersistence)
        {
            return;
        }
        this.gameData = dataHandler.Load(selectedProfileId);
        if(this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        if(this.gameData==null)
        {
            Debug.Log("No Data was found.A new game needs to be started before data can be loaded");
            return;
        }
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceobjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        if(this.gameData == null)
        {
            Debug.LogWarning("No data was found.A new game needs to be started before data can be saved");
            return;
        }
        foreach (IDataPersistance dataPersistanceObj in dataPersistanceobjects)
        {
            dataPersistanceObj.SaveData(gameData);
        }
        gameData.lastUpdated=System.DateTime.Now.ToBinary();
        dataHandler.Save(gameData,selectedProfileId);

        //dataHandler.SaveLevelNumber(SceneManager.GetActiveScene().buildIndex, selectedProfileId);
    }
    public int GetLevel()
    {
        return gameData.levelNum;
    }
    public void GoToNextLevel()
    {
        gameData.playerPosition = Vector3.zero;
        gameData.levelNum++;
        //gameData.characterLives = FindObjectOfType<Character>().Lives;
        gameData.characterLives = 14;
        dataHandler.Save(gameData,selectedProfileId);
    }
    //public void ResetPoition()
    //{
    //    dataHandler.Save(new GameData(), selectedProfileId);
    //}
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        //FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistance> dataPersistanceObjects=FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }
    public bool HasGameData()
    {
        return gameData != null;
    }
    public Dictionary<string, GameData> GetAllProfilesDameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
