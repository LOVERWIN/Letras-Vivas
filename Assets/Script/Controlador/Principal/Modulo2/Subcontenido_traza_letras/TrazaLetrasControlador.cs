using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Script.Controlador.Principal.Modulo1.SubcontenidoEjercicios.SubcontenidoEncontrarLetras.modelo;
using UnityEngine.Networking;
using UnityEngine.Video;

namespace Script.Controlador.Principal.Modulo2.Subcontenido_traza_letras
{
    public class TrazaLetrasControlador : MonoBehaviour
    {
        [SerializeField] private Button Repetir;
        [SerializeField] private Button Siguiente;
        [SerializeField] private TextMeshProUGUI LetraTrazar;
        [SerializeField] private TextMeshProUGUI InferenciaLetra;
        [SerializeField] private Slider ProgresoValidacion;
        [SerializeField] private Slider progreso;
        [SerializeField] private Image FillImagen;
        [SerializeField] private TextMeshProUGUI ProgresoNiveles;
        [SerializeField] private GameObject video;
        [SerializeField] private Image ImagenTrazo;
        [SerializeField] private GameObject LineRender;
        private VideoPlayer videoPlayer;
        private List<string> letras;
        private int nivelActual;

        private string ultimaInferencia = "";
        private float tiempoMismaInferencia = 0f;
        private const float TIEMPO_VALIDACION = 2f;
        private bool letraValidada = false;

        public EjercicioTrazo[] ejercicios =
        {
            new EjercicioTrazo("A", "letra_A", "letra_A"),
            new EjercicioTrazo("a", "letra_aa","letra_aa"),
            new EjercicioTrazo("B", "letra_B", "letra_B"),
            new EjercicioTrazo("b", "letra_bb", "letra_bb"),
            new EjercicioTrazo("E", "letra_E", "letra_E"),
            new EjercicioTrazo("e", "letra_ee", "letra_ee"),
            new EjercicioTrazo("l", "letra_I", "letra_I"),
            new EjercicioTrazo("i", "letra_ii", "letra_ii"),    
            new EjercicioTrazo("M", "letra_M", "letra_M"),
            new EjercicioTrazo("m", "letra_mm", "letra_mm"),
            new EjercicioTrazo("O", "letra_O", "letra_O"),
            new EjercicioTrazo("P", "letra_P", "letra_P"),
            new EjercicioTrazo("p", "letra_pp", "letra_pp"),
            new EjercicioTrazo("S", "letra_S", "letra_S"),
            new EjercicioTrazo("s", "letra_ss", "letra_ss"),
            new EjercicioTrazo("U", "letra_U", "letra_U"),
           
        };

        private void Start()
        {
            Debug.Log("Letra actual: " + ejercicios[nivelActual].letra);
            
          
            videoPlayer = video.GetComponentInChildren<VideoPlayer>();

            nivelActual = 0;
            MostrarLetraActual();

            Repetir.onClick.AddListener(RepetirNivel);
            Siguiente.onClick.AddListener(AvanzarNivel);
            Siguiente.interactable = false;

            progreso.maxValue = ejercicios.Length;
            progreso.value = 1;
            ProgresoNiveles.text = $"{nivelActual + 1} de {ejercicios.Length}";
        }

        private void Update()
        {
            if (letraValidada)
                return;

            string inferenciaActual = InferenciaLetra.text.Trim();
            string letraEsperada = ejercicios[nivelActual].letra.Trim();

            if (inferenciaActual == letraEsperada)
            {
                if (inferenciaActual == ultimaInferencia)
                {
                    tiempoMismaInferencia += Time.deltaTime;
                    float progresoValidacion = tiempoMismaInferencia / TIEMPO_VALIDACION;
                    ProgresoValidacion.value = progresoValidacion;

                    if (progresoValidacion < 0.5f)
                        FillImagen.color = Color.red;
                    else if (progresoValidacion < 0.9f)
                        FillImagen.color = Color.yellow;
                    else
                        FillImagen.color = Color.green;

                    if (tiempoMismaInferencia >= TIEMPO_VALIDACION)
                    {
                        ValidarLetraCorrecta();
                    }
                }
                else
                {
                    ultimaInferencia = inferenciaActual;
                    tiempoMismaInferencia = 0f;
                    ProgresoValidacion.value = 0f;
                    FillImagen.color = Color.red;
                }
            }
            else
            {
                ultimaInferencia = "";
                tiempoMismaInferencia = 0f;
                ProgresoValidacion.value = 0f;
                FillImagen.color = Color.red;
            }
        }

        private void ValidarLetraCorrecta()
        {
            AudioManager.instance.ReproducirGanar();
            letraValidada = true;
            Siguiente.interactable = true;
            ProgresoValidacion.value = 1f;
            FillImagen.color = Color.green;
            Debug.Log($"¡Letra {ejercicios[nivelActual]} trazada correctamente!");
        }

        private void MostrarLetraActual()
        {
            if (nivelActual < ejercicios.Length)
            {
                if (nivelActual == 0)
                {
                    LineRender.GetComponent<RectTransform>().sizeDelta = new Vector2(500f, 500f);
                }
                else
                {
                    LineRender.GetComponent<RectTransform>().sizeDelta = new Vector2(1200f, 1200f);
                }
                
                LetraTrazar.text = ejercicios[nivelActual].letra;
                InferenciaLetra.text = "";
                Siguiente.interactable = false;
                letraValidada = false;
                ultimaInferencia = "";
                tiempoMismaInferencia = 0f;

                progreso.value = nivelActual + 1;
                ProgresoNiveles.text = $"{nivelActual + 1} de {ejercicios.Length}";

                string nombreVideo = ejercicios[nivelActual].video;
                ImagenTrazo.sprite = Resources.Load<Sprite>("Sprite/imgTrazos/"+ejercicios[nivelActual].trazo);
                StartCoroutine(CargarYReproducirVideo(nombreVideo));
            }
            else
            {
                Debug.Log("¡Todas las letras trazadas!");
                nivelActual = 0;
                MostrarLetraActual();
            }
        }
        private IEnumerator CargarYReproducirVideo(string nombreVideo)
        {
            if (!string.IsNullOrEmpty(nombreVideo))
            {
                string ruta = System.IO.Path.Combine(Application.streamingAssetsPath, $"{nombreVideo}.mp4");

        #if UNITY_ANDROID
                UnityWebRequest request = UnityWebRequest.Get(ruta);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    videoPlayer.url = ruta;
                    video.SetActive(true);
                    videoPlayer.Play();
                }
                else
                {
                    Debug.LogWarning("Video no encontrado (Android): " + ruta);
                    video.SetActive(false);
                }
        #else
                if (System.IO.File.Exists(ruta))
                {
                    videoPlayer.url = ruta;
                    video.SetActive(true);
                    videoPlayer.Play();
                }
                else
                {
                    Debug.LogWarning("Video no encontrado: " + ruta);
                    video.SetActive(false);
                }
        #endif
            }
            else
            {
                video.SetActive(false);
            }
        }


        private void AvanzarNivel()
        {
            nivelActual++;
            MostrarLetraActual();
        }

        private void RepetirNivel()
        {
            MostrarLetraActual();
        }
    }
}
