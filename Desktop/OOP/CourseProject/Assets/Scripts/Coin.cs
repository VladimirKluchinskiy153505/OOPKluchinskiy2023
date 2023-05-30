using Serializator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IDataPersistance
{
    [SerializeField] private string id;
    private bool collected = false;
    public void LoadData(GameData data)
    {
        data.coinsCollected.TryGetValue(id, out collected);//if found, remove
        if (collected)
        {
            gameObject.SetActive(false);
        }
    }
    public void SaveData(GameData data)
    {
        if (data.coinsCollected.ContainsKey(id))
        {
            data.coinsCollected.Remove(id);
        }
        data.coinsCollected.Add(id, collected);
    }

    [ContextMenu("Geneate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Character character = collider.GetComponent<Character>();
        if (character)
        {
            ++character.Coins;
            collected = true;
            gameObject.SetActive(false);
        }
    }
}
