using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogWindow : Menu
{
    [SerializeField] private TMP_Text message;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Image backgroundImage;
    private bool isActive = false;
    private void Start()
    {
        CloseDialog();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (isActive)
            {
                CloseDialog();
                ResumeGame();
                isActive = false;
            }
            else
            {
                Activate();
                PauseGame();
                ShowDialog();
                isActive = true;
            }
        }
    }
    public void ShowDialog()
    {
        message.gameObject.SetActive(true);
        saveButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);
        backgroundImage.gameObject.SetActive(true);
    }

    public void CloseDialog()
    {
        message.gameObject.SetActive(false);
        saveButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
    }
    public async void OnSaveClicked()
    {
        Debug.Log("SaveButtonClicked");
        await DataPersistanceManager.Instance.SaveGame();
        ResumeGame();
        SceneManager.LoadScene(2);
    }
    public void WithoutSaving()
    {
        Debug.Log("WithoutSavingClicked");
        ResumeGame();
        SceneManager.LoadScene(2);
    }
    private void PauseGame()
    {
        Time.timeScale = 0.01f;
    }
    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
