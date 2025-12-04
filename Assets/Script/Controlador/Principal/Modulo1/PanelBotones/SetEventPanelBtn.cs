
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetEventPanelBtn : MonoBehaviour
{
    private EventTrigger eventTrigger;
    private EventManagerPanelBtn eventManager;
    private Image fondo;
    private TextMeshProUGUI txt;
    [SerializeField] private string Titulo;
    private void Awake()
    {
        eventTrigger = gameObject.AddComponent<EventTrigger>();
        eventManager = FindAnyObjectByType<EventManagerPanelBtn>();
        if (eventManager == null)
        {
            Debug.LogError("No se pudo crear el item: " + eventManager);
            return;
        }

        
        fondo = gameObject.GetComponent<Image>();
        if (fondo == null)
        {
            Debug.LogError("Erro al obtener el componente fondo :"+fondo);
        }
        txt = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        EventTrigger.Entry pointerClickEntry = new EventTrigger.Entry();
        pointerClickEntry.eventID = EventTriggerType.PointerClick;
        pointerClickEntry.callback.AddListener((data) => { OnItemClicked((PointerEventData)data); });
        eventTrigger.triggers.Add(pointerClickEntry);
    }
    public void OnItemClicked(PointerEventData data)
    {
        eventManager.SetCurrentSelection(gameObject,Titulo);
    }

    public void Select()
    {
        fondo.sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/btn_selct_submodul_on");
        txt.color = Color.white;

    }

    public void Deselect()
    {
        fondo.sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/btn_selct_submodul_off");
        txt.color = Color.black;

    }
}
