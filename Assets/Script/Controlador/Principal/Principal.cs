
using UnityEngine;
using UnityEngine.UI;

public class Principal : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    private GameObject btnIntit;
    private EventManagerPanelBtn eventManager;
    [SerializeField] private UIContenedorScrollItemsLetras uiContenedorScrollItemsLetras;

    private void Awake()
    {
      //
    }

    public void Init()
    {
        eventManager = FindAnyObjectByType<EventManagerPanelBtn>();
        if (eventManager != null)
        {
            
            var panelbotones = gameObject.transform.Find("Vertical_contenido/panel_botones").gameObject;
            
            if (panelbotones != null)
            {
                
                if (panelbotones.activeSelf)
                {
                    btnIntit = panelbotones.transform.Find("btn_letras").gameObject;
                    Transform viewTransform = uiManager.transform.Find("principal/Vertical_contenido/panel_subcontenido_letras");
                    GameObject view = viewTransform.gameObject;
                    uiManager.OpenView(view);
                }
                
            }
            else
            {
                Debug.Log("El panel subcontenido no esta habilitado ");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
        Init();
        
        eventManager.SetCurrentSelection(btnIntit, "Conociendo las letras");
        
        //uiContenedorScrollItemsLetras.ListasContenedor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Onclick(Button button)
    {
            
    }
}
