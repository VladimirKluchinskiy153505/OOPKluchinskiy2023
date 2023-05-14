using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    IDatabaseHandler databaseHandler = null;
    [SerializeField]private TMP_InputField userNameInput;
    [SerializeField]private TMP_InputField passwordInput;
    [SerializeField]private Button loginButton;
    [SerializeField]private Button returnButton;

    private void Awake()
    {
        databaseHandler = new DatabaseHandler();
    }
    void Start()
    {
        loginButton.onClick.AddListener(LoginUser);
        returnButton.onClick.AddListener(GoToSingUpPage);
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
            Debug.Log("Password or UserName Are Incorrect");
            return;
        }
        User user = await databaseHandler.GetUserByNameAsync(userName);
        if (user == null)
        {
            Debug.Log("Password or UserName Are Incorrect");
            return;
        }
        if (user.Password == password)
        {
            DataPersistanceManager.Instance.SetUserId(user.Id);
            Debug.Log("You are entered");
            SceneManager.LoadScene(2);
        }
    }
    private void GoToSingUpPage()
    {
        SceneManager.LoadScene(0);
    }
}
