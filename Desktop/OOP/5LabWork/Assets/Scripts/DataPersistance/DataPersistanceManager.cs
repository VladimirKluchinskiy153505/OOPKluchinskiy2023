using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    private int currentUserId;
    private int selectedSaveId;
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    //[SerializeField] private bool overrideSelectedProfileId = false;
    //[SerializeField] private string testSelectedProfileId = "test";
    //[Header("File Storage Config")]
    //[SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistance> dataPersistanceobjects;
    IDatabaseHandler dataHandler;
    public static DataPersistanceManager Instance { get; private set; }
    public void SetUserId(int userId)
    {
        currentUserId = userId;
    }
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        selectedSaveId = 0;
        dataHandler =new DatabaseHandler();
        if (dataHandler != null)
        {
            Debug.Log("Success");
        }
        else { Debug.Log("Error"); }
        //if (Instance != null)
        //{
        //    Debug.Log("Found more than one Data persistance manager in the scene.Destroying the newest one");
        //    Destroy(this.gameObject);
        //    return;
        //}
        //Instance = this;
        if(disableDataPersistence)
        {
            Debug.LogWarning("Data persistance is currently disabled!");
        }
    }
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit Application");
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
           await DataPersistanceManager.Instance.SaveGame();
            SceneManager.LoadScene(2);
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
    public async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Instance = this;
        Debug.Log("On Scene Loaded Called");
        this.dataPersistanceobjects = FindAllDataPersistanceObjects();
        if (this.selectedSaveId != 0) { await LoadGame(); }
    }
    public void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("On SceneUnloaded Called");
       // SaveGame();
    }
    public void ChangeSelectedSaveId(int newSaveId)
    {
        this.selectedSaveId = newSaveId;
    }
    public async Task NewGame()
    {
        this.gameData=new GameData();
        if (selectedSaveId == 0)
        {
           this.selectedSaveId = await dataHandler.CreateNewGame(this.gameData, currentUserId);
        }
        else
        {
           await dataHandler.Save(this.gameData, currentUserId, selectedSaveId);
        }
    }
    public async Task LoadGame()
    {
        Debug.Log("Manager LoadGame called saveId: " + selectedSaveId);
        if(disableDataPersistence)
        {
            return;
        }
        Debug.Log("Selected Save Id Is" + selectedSaveId);
        this.gameData = await dataHandler.Load(selectedSaveId);
        if(this.gameData == null && initializeDataIfNull)
        {
            await NewGame();
        }
        if(this.gameData==null)
        {
            Debug.Log("No Data was found.A new game needs to be started before data can be loaded");
            return;
        }
        Debug.Log("LoadGame GameData: "+this.gameData.playerPosition.ToString());
        foreach(IDataPersistance dataPersistanceObj in dataPersistanceobjects)
        {
            dataPersistanceObj.LoadData(gameData);
        }
    }
    public async Task SaveGame()
    {
        Debug.Log("SaveGame was called");
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
        await dataHandler.Save(gameData,currentUserId, selectedSaveId);

        //dataHandler.SaveLevelNumber(SceneManager.GetActiveScene().buildIndex, selectedProfileId);
    }
    public int GetLevel()
    {
        if(this.gameData == null)
        {
            Debug.Log("gameData is null");
            this.LoadGame().Wait();
        }
        return this.gameData.levelNum;
    }
    public void GoToNextLevel()
    {
        gameData.playerPosition = Vector3.zero;
        gameData.levelNum++;
        //gameData.characterLives = FindObjectOfType<Character>().Lives;
        gameData.characterLives = 14;
        dataHandler.Save(gameData, currentUserId, selectedSaveId);
    }
    //public void ResetPoition()
    //{
    //    dataHandler.Save(new GameData(), selectedProfileId);
    //}
    private async Task OnApplicationQuit()
    {
       await SaveGame();
    }
    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        //FindObjectsofType takes in an optional boolean to include inactive gameobjects
        IEnumerable<IDataPersistance> dataPersistanceObjects=FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistance>();
        return new List<IDataPersistance>(dataPersistanceObjects);
    }
    public bool HasGameData()
    {
        return selectedSaveId != 0;
    }
    public async Task<Dictionary<int, GameData>> GetAllProfilesGameData()
    {
        return await dataHandler.LoadAllProfiles(currentUserId);
    }
}
