using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Serializator;

public class SaveSlot : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private int profileId =0;
    private int saveId;
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI percentageCompleteText;
    [SerializeField] private TextMeshProUGUI deathCountText;
    private Button saveSlotButton;
    public void Awake()
    {
        saveSlotButton= GetComponent<Button>();
        saveId = 0;//not initialized
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
    public int GetProfileId() { return this.profileId; }
    public void SetSaveId(int Id) { this.saveId = Id; }
    public int GetSaveId() { return this.saveId; }
    public void SetInteractible(bool interactible)
    {
        saveSlotButton.interactable= interactible;
    }
}
