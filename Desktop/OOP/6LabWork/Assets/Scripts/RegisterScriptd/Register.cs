using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField confirmPasswordInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text messageText;
    private void Awake()
    {
        userNameInput.interactable = true;
        passwordInput.interactable = true;
        confirmPasswordInput.interactable = true;
        registerButton.interactable = true;
        loginButton.interactable = true;
    }
    void Start()
    {
        messageText.gameObject.SetActive(false);
        registerButton.onClick.AddListener(RegisterUser);
        loginButton.onClick.AddListener(GoToLoginPage);
    }
    private void ShowMessage(string message)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
    }
    async void RegisterUser()
    {
        string userName;
        bool nameTaken = true, nameEmpty;
        userName = userNameInput.text;
        nameEmpty = (userName == null) || (userName == string.Empty);
        if (nameEmpty)
        {
            Debug.Log("Incorrect name, try again");
            ShowMessage("Incorrect name, try again");
            return;
        }
        nameTaken = await DataPersistanceManager.Instance.dataHandler.UserExistsAsync(userName);
        if (nameTaken)
        {
            Debug.Log("This name is already taken, enter another");
            ShowMessage("This name is already taken, enter another");
            return;
        }
        string password, repeatPassword;
        bool passwordMismatch = true, emptyPassword;
        password = passwordInput.text;
        emptyPassword = (password == null) || (password == string.Empty);
        if (emptyPassword)
        {
            Debug.Log("Incorrent password, try again");
            ShowMessage("Incorrent password, try again");
            return;
        }
        repeatPassword = confirmPasswordInput.text;
        passwordMismatch = (password != repeatPassword);
        if (passwordMismatch)
        {
            Debug.Log("Mismatch in password, try again");
            ShowMessage("Mismatch in password, try again");
            return;
        }
        await DataPersistanceManager.Instance.dataHandler.AddUserAsync(new Serializator.User
        {
            Name = userName,
            Password = password
        });
        Debug.Log("Registered Successfully");
        ShowMessage("Registered Successfully");
    }
    void GoToLoginPage()
    {
        SceneManager.LoadScene(1);
    }
}
