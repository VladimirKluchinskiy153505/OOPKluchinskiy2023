using System.Threading.Tasks;
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
    [SerializeField] private Button registrationButton;
    [SerializeField] private Button scoreButton;
    private void Start()
    {
        registrationButton.onClick.AddListener(() => {
            DataPersistanceManager.Instance.HardReset();
            SceneManager.LoadScene(0);});
        scoreButton.onClick.AddListener(GoToScorePage);
        if (!DataPersistanceManager.Instance.HasGameData())
        {
            continueGameButton.interactable = false;
            //loadGameButton.interactable = false;
        }
    }
    public async void OnNewGameClicked()
    {
       await saveSlotsMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }
    public async void OnLoadGameClicked()
    {
        Debug.Log("On Load Clicked");
        await saveSlotsMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }
    //public void OnNewGameClicked()
    //{
    //    DisableMenuButtons();
    //    Debug.Log("New Game Clicked");
    //    DataPersistanceManager.Instance.NewGame();
    //    SceneManager.LoadSceneAsync(firstSceneIndex);
    //}
    public async void OnContinueGameCliked()
    {
        DisableMenuButtons();
        Debug.Log("On Continue Game Clicked");
        await DataPersistanceManager.Instance.LoadGame();
        SceneManager.LoadScene(DataPersistanceManager.Instance.GetLevel()+2);
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
    private void GoToScorePage()
    {
        SceneManager.LoadScene(7);
    }
}
