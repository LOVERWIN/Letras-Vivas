using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManagerBtnLetras : MonoBehaviour
{
    private GameObject currentSelection;
    private SetEventBtnLetras currentSetEvent;
    [SerializeField] private GameObject ventanEmergente;

    private void Awake()
    {
        currentSelection = null;
    }

    public void SetCurrentSelection(GameObject selection)
    {
        if (currentSetEvent != null)
        {
            currentSetEvent.deselect();
            AudioManager.instance.DetenerAudioActual();
            
        }

        currentSetEvent = selection.GetComponent<SetEventBtnLetras>();
        currentSetEvent.Select();
        currentSelection = selection;

        if (ventanEmergente == null)
        {
           // StartCoroutine(ReproducirAudiosSecuenciales(selection));
            AudioManager.instance.ReproducirAudioExclusivo("Letras/"+ selection.GetComponent<LetraItem>().musica);
        }
        else
        {
            MostrarVentanaEmergente(selection);
        }
    }

    

    private void MostrarVentanaEmergente(GameObject selection)
    {
        ventanEmergente.SetActive(true);

        var letraItem = selection.GetComponent<LetraItem>();
        var img = ventanEmergente.transform.Find("contenedor_img/borde/Image").GetComponent<Image>();
        var letra = ventanEmergente.transform.Find("contenedor_img/borde/Texto/Letra").GetComponent<TextMeshProUGUI>();
        var resto = ventanEmergente.transform.Find("contenedor_img/borde/Texto/Resto").GetComponent<TextMeshProUGUI>();

        letra.text = letraItem.primeraletra;
        resto.text = letraItem.restoletra;

        ventanEmergente.GetComponent<AudioSource>().Stop();

       AudioManager.instance.ReproducirAudioExclusivo("Letras/"+ letraItem.musica);
        


        Sprite loadedSprite = Resources.Load<Sprite>("Sprite/Imagenes_abecedario/" + letraItem.fondo);
        if (loadedSprite != null)
        {
            img.sprite = loadedSprite;
        }
        else
        {
            Debug.LogError("No se pudo cargar el sprite desde: " + letraItem.fondo);
        }
    }

    public void AlCerrarVentanaEmergente()
    {
        AudioManager.instance.DetenerAudioActual();
    }
}
