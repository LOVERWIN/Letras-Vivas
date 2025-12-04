using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private ItemFactory _factory;


    private void Awake()
    {
        
    }

    public void Generarprefabs()
    {
        for (int i = 0; i < 4; i++)
        {

            _factory.Create("modulo");

        }
    }
}
