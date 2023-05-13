using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profiled = "";
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI deathCountText;
    private Button saveSlotButton;
    public void Awake()
    {
        saveSlotButton= GetComponent<Button>();
    }
    public void SetData(GameData data)
    {
        if (data == null)
        {
            noDataContent.SetActive(true);
            noDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            noDataContent.SetActive(true);
            percentageCompleteText.text = data.GetpercentageComplete() + "% Complete";
        }
    }
    public string GetProfileId() { return this.profiled; }
    public void SetInteractible(bool interactible)
    {
        saveSlotButton.interactable= interactible;
    }
}
