
using UnityEngine;
using UnityEngine.EventSystems;


public class SetEventBtnLetras : MonoBehaviour
{
    private EventTrigger eventTrigger;
    private EventManagerBtnLetras eventManagerBtnLetras;
    
    private void Awake()
    {
        eventTrigger = gameObject.AddComponent<EventTrigger>();
        eventManagerBtnLetras = FindAnyObjectByType<EventManagerBtnLetras>();
        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerClick;
        pointerClickEntry.callback.AddListener((data) => { OnItemClicked((PointerEventData)data); });
        eventTrigger.triggers.Add(pointerClickEntry);
    }
    public void OnItemClicked(PointerEventData data)
    {
        eventManagerBtnLetras.SetCurrentSelection(gameObject);
    }

    public void Select()
    {


        //fondo.sprite = Resources.Load<Sprite>("btn_selct_submodul_on");
        //txt.color = Color.white;


    }

    public void deselect()
    {
        
    }
}
