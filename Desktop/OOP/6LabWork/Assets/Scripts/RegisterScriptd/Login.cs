using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Serializator;

public class Login : MonoBehaviour
{
    [SerializeField]private TMP_InputField userNameInput;
    [SerializeField]private TMP_InputField passwordInput;
    [SerializeField]private Button loginButton;
    [SerializeField]private Button returnButton;
    [SerializeField] private TMP_Text messageText;

    void Start()
    {
        messageText.gameObject.SetActive(false);
        loginButton.onClick.AddListener(LoginUser);
        returnButton.onClick.AddListener(GoToSingUpPage);
    }
    private void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
    }
    private async void LoginUser()
    {
        Debug.Log("Login Clicked");
        string userName, password;
        bool emptyName, emptyPassword;
        userName = userNameInput.text;
        password = passwordInput.text;
        emptyName = (userName == null) || (userName == string.Empty);
        emptyPassword = (password == null) || (password == string.Empty);
        if (emptyName || emptyPassword)
        {
            Debug.Log("Password or UserName Is Incorrect");
            ShowMessage("Password or UserName Is Incorrect");
            return;
        }
        User user = await DataPersistanceManager.Instance.dataHandler.GetUserByNameAsync(userName);
        if (user == null)
        {
            Debug.Log("Password or UserName Is Incorrect");
            ShowMessage("Password or UserName Is Incorrect");
            return;
        }
        if (user.Password == password)
        {
            DataPersistanceManager.Instance.SetUserId(user.Id);
            Debug.Log("You are entered");
            ShowMessage("You are entered");
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.Log("Password or UserName Is Incorrect");
            ShowMessage("Password or UserName Is Incorrect");
        }
    }
    private void GoToSingUpPage()
    {
        SceneManager.LoadScene(0);
    }
}
