using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    IDatabaseHandler databaseHandler = null;
    [SerializeField] private TMP_InputField userNameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_InputField confirmPasswordInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button loginButton;
    private void Awake()
    {
        databaseHandler = new DatabaseHandler();
        userNameInput.interactable = true;
        passwordInput.interactable = true;
        confirmPasswordInput.interactable = true;
        registerButton.interactable = true;
        loginButton.interactable = true;
    }
    void Start()
    {
        registerButton.onClick.AddListener(RegisterUser);
        loginButton.onClick.AddListener(GoToLoginPage);
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
            return;
        }
        nameTaken = await databaseHandler.UserExistsAsync(userName);
        if (nameTaken)
        {
            Debug.Log("This name is already taken, enter another");
            return;
        }
        string password, repeatPassword;
        bool passwordMismatch = true, emptyPassword;
        password = passwordInput.text;
        emptyPassword = (password == null) || (password == string.Empty);
        if (emptyPassword)
        {
            Debug.Log("Incorrent password, try again");
            return;
        }
        repeatPassword = confirmPasswordInput.text;
        passwordMismatch = (password != repeatPassword);
        if (passwordMismatch)
        {
            Debug.Log("mismatch in password, try again");
            return;
        }
        await databaseHandler.AddUserAsync(new User
        {
            Name = userName,
            Password = password
        });
        Debug.Log("Registered Successfully");
    }
    void GoToLoginPage()
    {
        SceneManager.LoadScene(1);
    }
}
