using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Heart : MonoBehaviour,IDataPersistance
{
    [SerializeField] private string id;
    private bool collected = false;
    public void LoadData(GameData data)
    {
        data.heartsCollected.TryGetValue(id, out collected);//if found, remove
        if(collected)
        {
            gameObject.SetActive(false);
        }
    }
    public void SaveData(GameData data)
    {
        if(data.heartsCollected.ContainsKey(id))
        {
            data.heartsCollected.Remove(id);
        }
        data.heartsCollected.Add(id, collected);
    }

[ContextMenu("Geneate guid for id")]
    private void GenerateGuid()
    {
        id=System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character)
        {
            ++character.Lives;
            collected=true;
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
