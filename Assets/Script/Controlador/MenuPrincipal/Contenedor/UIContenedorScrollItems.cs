using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class UIContenedorScrollItems: MonoBehaviour
{
    private float clickDelay = 0.3f; // Tiempo máximo entre clics para considerar doble clic
    private bool isOneClick = false;
    [SerializeField] private Transform itemcontenedor;
    //[SerializeField] private GameObject itemtemplate;
    [SerializeField] private ItemConfiguracion itemConfiguracion;
    [SerializeField] private UIManager uiManager;
    private ItemFactory itemFactory;
    [SerializeField] private string nameItem;
    private Transform viewTransform;
    [SerializeField] private GameObject menuPrincipal;
        
    private ModuloItem[] items = { new ModuloItem("A B C", "Aprendiendo el abecedario", false,true), new ModuloItem("Ma Me", "Construcción sílabica", false, true), new ModuloItem("Casa", "Descubriendo la lectura", false, false), new ModuloItem("Mi mundo", "Descrubiendo la escritura", false, false) };
    private void Awake()
    {
        itemFactory = new ItemFactory(Instantiate(itemConfiguracion));
        
    }

    public void ListasContenedor()
    {
        // ActualizarContenedorItems();
        AddListenerButones();
    }
  

    // private void ActualizarContenedorItems()
    // {
    //
    //     foreach (var item1 in items)
    //     {
    //
    //         var item = itemFactory.Create(nameItem);
    //         var newItem = Instantiate(item,itemcontenedor).GetComponent<RectTransform>();
    //         newItem.name = item1.nombre;
    //         newItem.Find("Titulo").GetComponent<TextMeshProUGUI>().text = item1.nombre;
    //         newItem.Find("Subtitulo").GetComponent<TextMeshProUGUI>().text = item1.descripcion;
    //         newItem.GetComponent<Button>().interactable = item1.status;
    //
    //     }
    //
    // }

    private void CambiarVistaDoble()
    {
        AudioManager.instance.ReproducirAudioExclusivo("Vistas/MenuPrincipal/ConstruccionSilabica");
        AudioManager.instance.ReproducirClick();

    }

    private void CambiarVistaDoble2()
    {
        AudioManager.instance.ReproducirAudioExclusivo("Vistas/MenuPrincipal/aprendiendoAbecedario");
        AudioManager.instance.ReproducirClick();

    }
    private void AddListenerButones()
    {
        Button boton;
        Transform hijo = itemcontenedor.Find("modulo1");

        if (hijo != null)
        {
            boton = hijo.GetComponent<Button>();
            boton.onClick.AddListener(() => OnButtonClick(() => CambiarVistaDoble2(), () => CambiarVista()));
            Debug.Log("Encontrado: " + hijo.name);
        }
        else
        {
            Debug.Log("El es nulo o no se pudo encontrar");
        }

        hijo = itemcontenedor.Find("modulo2");
        if (hijo != null)
        {
            boton = hijo.GetComponent<Button>();
            boton.onClick.AddListener(() => OnButtonClick(() => CambiarVistaDoble(), () => CambiarVista2()));
        }
    }
    
    
// Manejador para distinguir entre clic y doble clic
    private void OnButtonClick(System.Action singleClickAction, System.Action doubleClickAction)
    {
        if (!isOneClick)
        {
            isOneClick = true;
            StartCoroutine(SingleOrDoubleClick(singleClickAction, doubleClickAction));
        }
        else
        {
            // Segundo clic dentro del intervalo = doble clic
            isOneClick = false;
            doubleClickAction.Invoke();
        }
    }

    private IEnumerator SingleOrDoubleClick(System.Action singleClickAction, System.Action doubleClickAction)
    {
        yield return new WaitForSeconds(clickDelay);

        if (isOneClick)
        {
            // Si no hubo segundo clic, ejecutamos el clic simple
            singleClickAction.Invoke();
        }

        isOneClick = false;
    }

    private void CambiarVista2()
    {
        Debug.Log("Entro al metodo de cambiar2");
        viewTransform = uiManager.transform.Find("principal");
        viewTransform.gameObject.SetActive(true);
        viewTransform = viewTransform.transform.Find("Vertical_contenido/panel_subcontenido_modulo2");
    AudioManager.instance.ReproducirAudioExclusivo("Vistas/Principal/PanelModulo2/EjerciciosInteractivos");
    AudioManager.instance.ReproducirClick();
    if (viewTransform != null)
        {
            Debug.Log("Se encontro el elemento buscado ");
      
            viewTransform.gameObject.SetActive(true);
            menuPrincipal.SetActive(false);

        }
        else
        {
            Debug.Log("No se encontro el objeto Principal");
        }
    }

    private void CambiarVista()
    {
         viewTransform = uiManager.transform.Find("modulo1_introduccion");

        if (viewTransform == null)
        {
            Debug.LogError("No se encontró el hijo 'modulo1_introduccion' en uiManager.");
            return;
        }
        AudioManager.instance.ReproducirClick();
        viewTransform.gameObject.SetActive(true);
        menuPrincipal.SetActive(false);
    } 
}
