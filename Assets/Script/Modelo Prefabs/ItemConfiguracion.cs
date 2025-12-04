using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Power up configuration")]
public class ItemConfiguracion : ScriptableObject
{
    [SerializeField] private Item[] items;
    private Dictionary<string, Item> _iditems;

    private void OnEnable()
    {
        if (_iditems == null)
        {
            _iditems = new Dictionary<string, Item>();

            foreach (var item in items)
            {
                if (item != null && !string.IsNullOrEmpty(item.Id))
                {
                    if (!_iditems.ContainsKey(item.Id))
                        _iditems.Add(item.Id, item);
                    else
                        Debug.LogWarning($"ItemConfiguracion: Duplicated item ID '{item.Id}' detected.");
                }
                else
                {
                    Debug.LogWarning("ItemConfiguracion: Found item with null or empty ID.");
                }
            }
        }
    }

    public Item GetItemPrefabById(string id)
    {
        if (_iditems == null)
            OnEnable(); // Por si acaso

        if (!_iditems.TryGetValue(id, out var item))
        {
            Debug.LogWarning($"Item with id {id} not found.");
        }

        return item;
    }
}