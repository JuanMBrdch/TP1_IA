using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerManager : MonoBehaviour
{
    [SerializeField] Item[] itemsToSpawn;
    [SerializeField] List<float> itemWeight;
    private Dictionary<Item, float> itemDictionary = new Dictionary<Item, float>();
    [SerializeField] Transform[] spawnPoints;
    private float totalWeight = 0;

    private void Awake()
    {
        itemWeight.Sort();

        foreach(float weight in itemWeight)
        {
            totalWeight += weight;
        }

        //Cargar Diccionario
        for(int i = 0; i < itemsToSpawn.Length; i++)
        {
            itemDictionary.Add(itemsToSpawn[i],itemWeight[i]);
            //print(itemsToSpawn[i].gameObject.name);
        }

        

    }

    private void Start()
    {
        foreach(Transform location in spawnPoints)
        {
            GameObject temp = RollItem().gameObject;

            if (temp != null)
            {
                Instantiate(temp, location);
            }
            
        }
    }

    private Item RollItem()
    {
        float weight = Random.Range(0, itemWeight[itemWeight.Count-1]);
        print(weight);
        foreach (var item in itemDictionary)
        {
            weight -= item.Value;

            if (weight <= 0)
            {
                return item.Key;
            }
        }

        return default;
    }
}
