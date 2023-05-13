using System.Collections;
using System.Collections.Generic;
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
    private void Awake()
    {
        saveSlots=this.GetComponentsInChildren<SaveSlot>();
    }
    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        Debug.Log("On Save Slot clicked");
        DataPersistanceManager.Instance.ChangeSelectedProfileId(saveSlot.GetProfileId());
        if (!isLoadingGame)
        {
            DataPersistanceManager.Instance.NewGame();
        }
        DataPersistanceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync(DataPersistanceManager.Instance.GetLevel());
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
    public void ActivateMenu(bool isLoadingGame)
    {
        this.gameObject.SetActive(true);
        this.isLoadingGame= isLoadingGame;
        Dictionary<string, GameData> profilesGameData = DataPersistanceManager.Instance.GetAllProfilesDameData();
       GameObject firstSelected=backButton.gameObject;
        foreach(SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            if(profileData==null && isLoadingGame)
            {
                saveSlot.SetInteractible(false);
            }
            else
            {
                saveSlot.SetInteractible(true);
                if(firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected=saveSlot.gameObject;
                }
            }
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
