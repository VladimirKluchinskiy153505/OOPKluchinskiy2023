using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }
    public async void PlayGame()
    {
       await DataPersistanceManager.Instance.NewGame();
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        Debug.Log("Game closed");
        Application.Quit();
    }
}
