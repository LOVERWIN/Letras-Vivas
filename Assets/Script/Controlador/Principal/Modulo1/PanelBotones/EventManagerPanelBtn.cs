
using TMPro;
using UnityEngine;
public class EventManagerPanelBtn : MonoBehaviour
{
    private GameObject currentSelection;
    private SetEventPanelBtn currentSetEvent;
    [SerializeField] private TextMeshProUGUI currentSelectionText;

    private void Awake()
    {
        currentSelection = null;    
        
        //lastSelection = null;
    }
    public void SetCurrentSelection(GameObject selection,string titulo)
    {
         
        if (selection != currentSelection )
        {
            if (currentSetEvent != null)
            {
                //lastSelection.GetComponent<SetEvent>().deselect();
                currentSetEvent.Deselect();
                
            }
            currentSetEvent = selection.GetComponent<SetEventPanelBtn>();
            currentSetEvent.Select();
            currentSelection = selection;
            currentSelectionText.text = titulo;

        }
    }
}
