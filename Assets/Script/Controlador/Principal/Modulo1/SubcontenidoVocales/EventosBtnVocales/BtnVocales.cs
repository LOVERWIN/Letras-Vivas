using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BtnVocales : MonoBehaviour
{
    [SerializeField] private GameObject ventanEmergente;
    [SerializeField] private int numero;

    private bool usarMayusculas = false;

    private void Start()
    {
        CargarDatosGameObjectVocal();
    }

    private LetraItem[] vocales = {
        new LetraItem("a", "btn_progreso_on", "a", "", ""),
        new LetraItem("e", "icono_redondo_verde_vocal", "e", "", ""),
        new LetraItem("i", "content_circulo_verde", "i", "", ""),
        new LetraItem("o", "btn_progreso_on", "o", "", ""),
        new LetraItem("u", "icono_redondo_verde_vocal", "u", "", "")
    };

    private LetraItem[] vocalesMayus = {
        new LetraItem("A", "btn_progreso_on", "a", "", ""),
        new LetraItem("E", "icono_redondo_verde_vocal", "e", "", ""),
        new LetraItem("I", "content_circulo_verde", "i", "", ""),
        new LetraItem("O", "btn_progreso_on", "o", "", ""),
        new LetraItem("U", "icono_redondo_verde_vocal", "u", "", "")
    };

    private LetraItem[] GetArregloActual()
    {
        return usarMayusculas ? vocalesMayus : vocales;
    }

    public void ActualizarDatos(bool mayusculas)
    {
        usarMayusculas = mayusculas;
        CargarDatosGameObjectVocal();
        
    }

    private void CargarDatosGameObjectVocal()
    {
        LetraItem[] arreglo = GetArregloActual();

        GameObject vocal = gameObject;
        var image = vocal.GetComponent<Image>();
        var texto = vocal.GetComponentInChildren<TextMeshProUGUI>();

        if (numero < 0 || numero >= arreglo.Length)
        {
            Debug.LogError("Índice fuera de rango en BtnVocales: " + numero);
            return;
        }

        image.sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/" + arreglo[numero].fondo);
        texto.text = arreglo[numero].letra;
        
    }

    public void ClickBtnVocal()
    {
        LetraItem[] arreglo = GetArregloActual();

        
        AudioManager.instance.ReproducirAudioExclusivo("Letras/" + arreglo[numero].musica);
       
    }
}
