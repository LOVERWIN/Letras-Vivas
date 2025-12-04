using System;
using System.Collections.Generic;
using Script.Controlador.Principal.Modulo1.SubcontenidoSonidos.modelo;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Por si quieres usar TMP para los botones
using Random = UnityEngine.Random;

namespace Script.Controlador.Principal.Modulo1.SubcontenidoSonidos
{
    public class EjercicioSonidosControlador : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Button btnAudio;
        [SerializeField] private Transform itemcontenedor;
        [SerializeField] private Button repetir;
        [SerializeField] private Button siguiente;
        [SerializeField] private ItemConfiguracion itemConfiguracion;
        [SerializeField] private string nameItem;
        [SerializeField] private Slider barraProgreso;
        [SerializeField] private TextMeshProUGUI textoProgreso;
        [SerializeField] private TextMeshProUGUI textoIntentos;
        [SerializeField] private GameObject banner;
        private ItemFactory itemFactory;
        private List<NivelSonido> niveles;
        private int nivelActual;
        private int intentos;
        private List<string> bancoLetras = new List<string> 
        { 
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", 
            "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", 
            "T", "U", "V", "W", "X", "Y", "Z" 
        };
        private List<Button> itemPool = new List<Button>();

        private void Awake()
        {
                // Corregido: Pasa la configuración directamente, sin instanciarla.
                itemFactory = new ItemFactory(itemConfiguracion);
        }

        public void EntrarBanner()
        {
            banner.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/Contenedor_boton_sonidos");
            banner.transform.Find("titulo").GetComponent<TextMeshProUGUI>().text = "Identificar sonidos";
        }

        private void Start()
        {
            
           
            niveles = new List<NivelSonido>
            {
                new NivelSonido("Letras/a", "A"),
                new NivelSonido("Letras/b", "B"),
                new NivelSonido("Letras/c", "C"),
                new NivelSonido("Letras/d", "D"),
                new NivelSonido("Letras/e", "E"),
                new NivelSonido("Letras/f", "F"),
                new NivelSonido("Letras/g", "G"),
                new NivelSonido("Letras/h", "H"),
                new NivelSonido("Letras/i", "I"),
                new NivelSonido("Letras/j", "J"),
                new NivelSonido("Letras/k", "K"),
                new NivelSonido("Letras/l", "L"),
                new NivelSonido("Letras/m", "M"),
                new NivelSonido("Letras/n", "N"),
                new NivelSonido("Letras/ñ", "Ñ"),
                new NivelSonido("Letras/o", "O"),
                new NivelSonido("Letras/p", "P"),
                new NivelSonido("Letras/q", "Q"),
                new NivelSonido("Letras/r", "R"),
                new NivelSonido("Letras/s", "S"),
                new NivelSonido("Letras/t", "T"),
                new NivelSonido("Letras/u", "U"),
                new NivelSonido("Letras/v", "V"),
                new NivelSonido("Letras/w", "W"),
                new NivelSonido("Letras/x", "X"),
                new NivelSonido("Letras/y", "Y"),
                new NivelSonido("Letras/z", "Z"),
                //new NivelSonido("Audios/a_arbol_juan_01", "H")
            };

            repetir.onClick.AddListener(ReiniciarNivel);
            siguiente.onClick.AddListener(AvanzarNivel);
            
            // Se asigna el listener una sola vez para evitar duplicados.
            btnAudio.onClick.AddListener(() => ReproducirAudio());

            nivelActual = 0;
            intentos = 3;
        
            barraProgreso.minValue = 0;
            barraProgreso.maxValue = niveles.Count; // La cantidad total de niveles
            barraProgreso.value = 1;
            ActualizarTextoProgreso();
            ActualizarIntentosDisponible();
            CargarNivel();
        }

        private void CargarNivel()
        {
            // Devolver botones anteriores a la piscina en lugar de destruirlos.
            while (itemcontenedor.childCount > 0)
            {
                Transform child = itemcontenedor.GetChild(0);
                child.SetParent(null); // Desvincular del contenedor.
                child.gameObject.SetActive(false);
                itemPool.Add(child.GetComponent<Button>());
            }

            //ReproducirAudio();

            // Crear opciones de respuesta
            List<string> opciones = new List<string>();
            opciones.Add(niveles[nivelActual].Respuesta); // La correcta

            // Conseguir 2 letras aleatorias que NO sean la respuesta correcta
            List<string> bancoDisponible = new List<string>(bancoLetras);
            bancoDisponible.Remove(niveles[nivelActual].Respuesta); // Sacar la correcta para que no la repita

            for (int i = 0; i < 2; i++) // Agregar dos incorrectas
            {
                int randomIndex = Random.Range(0, bancoDisponible.Count);
                opciones.Add(bancoDisponible[randomIndex]);
                bancoDisponible.RemoveAt(randomIndex); // Sacar para no repetir
            }

            // Mezclar las opciones
            opciones = MezclarLista(opciones);

            foreach (var opcion in opciones)
            {
                Button itemButton;
                if (itemPool.Count > 0)
                {
                    // Reutilizar un botón de la piscina.
                    itemButton = itemPool[0];
                    itemPool.RemoveAt(0);
                }
                else
                {
                    // Solo crear uno nuevo si la piscina está vacía.
                    // Corregido: No hay doble instanciación.
                    var newItem = itemFactory.Create(nameItem);
                    itemButton = newItem.GetComponent<Button>();
                }

                itemButton.transform.SetParent(itemcontenedor);
                itemButton.gameObject.SetActive(true);
                itemButton.name = opcion;
                
                var texto = itemButton.transform.GetComponentInChildren<TextMeshProUGUI>();
                texto.text = opcion;

                // Limpiar listeners anteriores y añadir el nuevo para evitar acumulaciones.
                itemButton.onClick.RemoveAllListeners();
                itemButton.onClick.AddListener(() => EvaluarRespuesta(opcion));
            }

            // repetir.gameObject.SetActive(false);
            siguiente.interactable = false;
        }

        private void ReproducirAudio()
        {
            // Cargar audio
            AudioManager.instance.ReproducirAudioExclusivo(niveles[nivelActual].ResourceSonido);
            // AudioClip clip = Resources.Load<AudioClip>(niveles[nivelActual].ResourceSonido);
            // _audioSource.clip = clip; 
            // _audioSource.Play();
        }

        private void EvaluarRespuesta(string respuestaSeleccionada)
        {
            if (respuestaSeleccionada == niveles[nivelActual].Respuesta)
            {
                Debug.Log("¡Correcto!");
                siguiente.interactable = true;
                AudioManager.instance.ReproducirGanar();

            }
            else
            {
                intentos--;
                ActualizarIntentosDisponible();
                Debug.Log("¡Incorrecto! Intentos restantes: " + intentos);
                AudioManager.instance.ReproducirPerder();

                if (intentos <= 0)
                {
                    foreach (Transform child in itemcontenedor)
                    {
                        child.GetComponent<Button>().interactable= false;
                    }
                }
            }
        }
        private void ActualizarTextoProgreso()
        {
            textoProgreso.text = $"{nivelActual + 1} de {niveles.Count}";
          
        }

        private void ActualizarIntentosDisponible()
        {
            textoIntentos.text = $"Intentos Disponibles {intentos} ";
        }

        private void AvanzarNivel()
        {
            if (nivelActual < niveles.Count - 1)
            {
                nivelActual++;
                intentos = 3;
                barraProgreso.value = nivelActual;
                ActualizarTextoProgreso();
                CargarNivel();
                ReproducirAudio();
                
            }
            else
            {
                Debug.Log("¡Todos los niveles completados!");
                // Puedes mostrar un mensaje de victoria aquí
                ReiniciarTodosLosNivels();
            }
        }

        private void ReiniciarNivel()
        {
            intentos = 3;
            CargarNivel();
            ReproducirAudio();
        }

        private void ReiniciarTodosLosNivels()
        {
            nivelActual = 0;
            intentos = 3;
            barraProgreso.minValue = 0;
            barraProgreso.maxValue = niveles.Count; // La cantidad total de niveles
            barraProgreso.value = 1;
            ActualizarTextoProgreso();
            ActualizarIntentosDisponible();
            CargarNivel();
            ReproducirAudio();
        }

        private List<string> MezclarLista(List<string> lista)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                string temp = lista[i];
                int randomIndex = Random.Range(i, lista.Count);
                lista[i] = lista[randomIndex];
                lista[randomIndex] = temp;
            }
            return lista;
        }

        public void salirBanner()
        {
            banner.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/Contenedor_nombre_modulo");
            banner.transform.Find("titulo").GetComponent<TextMeshProUGUI>().text = "Ejercicios";
        }
    }
}
