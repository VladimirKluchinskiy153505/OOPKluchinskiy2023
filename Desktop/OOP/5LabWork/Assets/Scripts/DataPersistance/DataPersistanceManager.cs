using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Serializator;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("Debugging")]
    private int currentUserId;
    private int selectedSaveId;
    [SerializeField] private bool disableDataPersistence = false;
    [SerializeField] private bool initializeDataIfNull = false;
    private GameData gameData;
    private List<IDataPersistance> dataPersistanceobjects;
    public IDatabaseHandler dataHandler;
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
        dataHandler = (IDatabaseHandler)new Serializator.DatabaseHandler();
        if (dataHandler != null)
        {
            Debug.Log("Success");
        }
        else { Debug.Log("Error"); }
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
           await SaveGame();
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
        Debug.Log("On Scene Loaded Called");
        this.dataPersistanceobjects = FindAllDataPersistanceObjects();
        if (this.selectedSaveId != 0) { await LoadGame(); }
    }
    public void OnSceneUnloaded(Scene scene)
    {
          Debug.Log("On SceneUnloaded Called");
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
        gameData.characterLives = 14;
        dataHandler.Save(gameData, currentUserId, selectedSaveId);
    }
    private void OnApplicationQuit()
    {
        SaveGame().Wait();
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
    public void Reset1()
    {
        this.selectedSaveId = 0;
        this.currentUserId = 0;
        this.gameData = null;
    }
}
