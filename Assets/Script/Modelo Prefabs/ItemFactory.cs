using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory
{
    [SerializeField] private readonly ItemConfiguracion itemConfiguracion;

    public ItemFactory(ItemConfiguracion itemConfiguracion) {

        this.itemConfiguracion = itemConfiguracion;
    }



    public Item Create(string id)
    {
        var item  = itemConfiguracion.GetItemPrefabById(id);
        return Object.Instantiate(item);
    }
}
