using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu1 : Menu
{
    [Header("Menu navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [Header("Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueGameButton;
    [SerializeField] private Button loadGameButton;
    private const int firstSceneIndex = 1;
    private void Start()
    {
        if (!DataPersistanceManager.Instance.HasGameData())
        {
            continueGameButton.interactable= false;
            loadGameButton.interactable= false;
        }
    }
    public void OnNewGameClicked()
    {
        saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }
    public void OnLoadGameClicked()
    {
        saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }
    //public void OnNewGameClicked()
    //{
    //    DisableMenuButtons();
    //    Debug.Log("New Game Clicked");
    //    DataPersistanceManager.Instance.NewGame();
    //    SceneManager.LoadSceneAsync(firstSceneIndex);
    //}
    public void OnContinueGameCliked()
    {
        DisableMenuButtons();
        Debug.Log("On Continue Game Clicked");
        DataPersistanceManager.Instance.LoadGame();
        SceneManager.LoadSceneAsync(DataPersistanceManager.Instance.GetLevel());
    }
    private void DisableMenuButtons()
    {
       newGameButton.interactable= false;
        continueGameButton.interactable= false;
    }
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);

    }
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
