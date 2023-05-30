using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Serializator;
using UnityEngine.UI;

public class NewBehaviourScript : Menu
{
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
    [SerializeField] private Button toLoginButton;
    [SerializeField] private Button toMenuButton;
    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;
    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);
        toMenuButton.gameObject.SetActive(false);
        //highscoreEntryList = new List<HighscoreEntry>()
        //{
        //    new HighscoreEntry{score =6548, name ="AAA"},
        //    new HighscoreEntry{score =7978, name ="ANN"},
        //    new HighscoreEntry{score =3216, name ="CAR"},
        //    new HighscoreEntry{score =11264, name ="JON"},
        //    new HighscoreEntry{score =8044, name ="MIK"},
        //    new HighscoreEntry{score =3510, name ="DAV"}
        //};
        //highscoreEntryList.Sort(delegate (HighscoreEntry x, HighscoreEntry y)
        //{
        //    if (x.score < y.score) return 1;
        //    else return -1;
        //});
        //highscoreEntryTransformList = new List<Transform>();
        //foreach(HighscoreEntry highscoreEntry in highscoreEntryList)
        //{
        //    CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        //}
    }
    private void Start()
    {
        toLoginButton.onClick.AddListener(GoToLoginPage);
        toMenuButton.onClick.AddListener(GoToMenuPage);
        if (DataPersistanceManager.Instance.HasUser())
        {
            toMenuButton.gameObject.SetActive(true);
        }
        Activate();
    }
    private new void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded-= OnSceneLoaded;
    }
    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        highscoreEntryList = new List<HighscoreEntry>();
        List<User> userList = await DataPersistanceManager.Instance.dataHandler.GetUsersAsync();
        foreach (User user in userList)
        {
            highscoreEntryList.Add(new HighscoreEntry
            {
                name = user.Name,
                score = user.Record,
            });
        }
        highscoreEntryList.Sort(delegate (HighscoreEntry x, HighscoreEntry y)
        {
            if (x.score < y.score) return 1;
            else return -1;
        });
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }
    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTranform = entryTransform.GetComponent<RectTransform>();
        entryRectTranform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryRectTranform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
             case 1: rankString = "1ST"; break;
             case 2: rankString = "2ND"; break;
             case 3: rankString = "3RD"; break;
             default:
                 rankString = rank + "TH"; break;
        }
        entryTransform.Find("posText").GetComponent<TMP_Text>().text = rankString;
        int score = highscoreEntry.score;
        entryTransform.Find("scoreText").GetComponent<TMP_Text>().text = score.ToString();
        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TMP_Text>().text = name;
        transformList.Add(entryTransform);
    }
    private void GoToLoginPage()
    {
        SceneManager.LoadScene(1);
    }
    private void GoToMenuPage()
    {
        SceneManager.LoadScene(2);
    }
}
