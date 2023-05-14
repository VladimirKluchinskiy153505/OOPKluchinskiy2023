using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsMenu : Menu
{
    [Header("menu navigation")]
    [SerializeField] private MainMenu1 mainMenu;
    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;
    private SaveSlot[] saveSlots;
    private bool isLoadingGame = false;
    private int saveSlotsCount = 4;
    private void Awake()
    {
        saveSlots=this.GetComponentsInChildren<SaveSlot>();
    }
    public async void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        Debug.Log("On Save Slot clicked");
        DataPersistanceManager.Instance.ChangeSelectedSaveId(saveSlot.GetSaveId());

        if (!isLoadingGame)// Make new SaveObject
        {
            Debug.Log("New Game on Clicked");
           await DataPersistanceManager.Instance.NewGame();
        }
        //DataPersistanceManager.Instance.SaveGame();
        SceneManager.LoadScene(DataPersistanceManager.Instance.GetLevel()+2);
    }
    public void OnBackClicked()
    {
        mainMenu.ActivateMenu();
        this.DeactivateMenu();
    }
    //private void Start()
    //{
    //    ActivateMenu();
    //}
    public async void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);
        this.isLoadingGame= isLoadingGame;
        GameObject firstSelected = backButton.gameObject;
        Dictionary<int, GameData> profilesGameData =await DataPersistanceManager.Instance.GetAllProfilesGameData();

        int dictionarySize = profilesGameData.Count;
        Debug.Log("Dict Count is: " + dictionarySize);
        int Iter = 0;
        foreach(var item in profilesGameData) {
            saveSlots[Iter].SetSaveId(item.Key);
            saveSlots[Iter].SetData(item.Value);
            saveSlots[Iter].SetInteractible(true);
            if (firstSelected.Equals(backButton.gameObject))
            {
                firstSelected = saveSlots[Iter].gameObject;
            }
            ++Iter;
        }
        while (Iter < saveSlotsCount)
        {
            if (isLoadingGame)
            {
                saveSlots[Iter].SetInteractible(false);
            }
            else
            {
                saveSlots[Iter].SetInteractible(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlots[Iter].gameObject;
                }
            }
            Iter++;
        }
        Button firstSelectedButton=firstSelected.GetComponent<Button>();
        SetFirstSelected(firstSelectedButton);
    }
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
    public void DisableMenuButtons()
    {
        foreach(SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractible(false);
        }
        backButton.interactable = false;
    }
}
