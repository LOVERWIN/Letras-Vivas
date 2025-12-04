
using UnityEngine;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private UIContenedorScrollItems uicontenedorscrollitems; 


    void Start()
    {

        uicontenedorscrollitems.ListasContenedor();
        AudioManager.instance.ReproducirAudioExclusivo("Vistas/MenuPrincipal/Principal");
        
    }

    void AddListenerBotones()
    {
        var ui = uicontenedorscrollitems.GetComponent<GameObject>();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
