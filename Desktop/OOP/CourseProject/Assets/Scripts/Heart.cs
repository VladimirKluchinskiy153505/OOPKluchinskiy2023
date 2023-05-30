using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Serializator;

public class Heart : MonoBehaviour,IDataPersistance
{
    [SerializeField] private string id;
    private bool collected = false;
    public void LoadData(GameData data)
    {
        Debug.Log("heartsCollected size is " + data.heartsCollected.Count);
        data.heartsCollected.TryGetValue(id, out collected);//if found, remove
        if(collected)
        {
            gameObject.SetActive(false);
            Debug.Log("Removed heart with id " + id);
        }
    }
    public void SaveData(GameData data)
    {
        if(data.heartsCollected.ContainsKey(id))
        {
            data.heartsCollected.Remove(id);
        }
        data.heartsCollected.Add(id, collected);
        Debug.Log("To gameData was added heart with id: " + id+ " and collected"+ collected);
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
