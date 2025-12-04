using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Script.Controlador.Principal.Modulo1.SubcontenidoEjercicios.SubcontenidoVocalOConsonante
{
    public class EjercicioVocalOConsonanteControlador : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI TextoAResolver;
        [SerializeField] private Button Consonante;
        [SerializeField] private Button Vocal;
        [SerializeField] private Button Repetir;
        [SerializeField] private Button Siguiente;
        [SerializeField] private Slider BarraProgreso;
        [SerializeField] private TextMeshProUGUI TxtIntentos;
        [SerializeField] private TextMeshProUGUI TxtProgreso;
        [SerializeField] private Transform ContenedorBotones; // Un contenedor vacío donde están los botones
        [SerializeField] private Transform banner;
        private List<string> letras;
        private int nivelActual;
        private int intentos;

        private HashSet<string> vocales = new HashSet<string> { "A", "E", "I", "O", "U" };


        public void EntrarBanner()
        {
            banner.transform.Find("titulo").GetComponent<TextMeshProUGUI>().text = "¿Vocal o consonante?";
        }
        private void Start()
        {
            //banner.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/Contenedor_boton_sonidos");
            
            letras = new List<string>
            {
                "A","B","C","D","E","F","G","H","I","J","K","L","M",
                "N","Ñ","O","P","Q","R","S","T","U","V","W","X","Y","Z"
            };

            nivelActual = 0;
            intentos = 3;

            BarraProgreso.minValue = 0;
            BarraProgreso.maxValue = letras.Count;
            BarraProgreso.value = 1;

            Consonante.onClick.AddListener(() => EvaluarRespuesta("Consonante"));
            Vocal.onClick.AddListener(() => EvaluarRespuesta("Vocal"));
            Repetir.onClick.AddListener(ReiniciarNivel);
            Siguiente.onClick.AddListener(AvanzarNivel);

            ActualizarTextoProgreso();
            ActualizarIntentosDisponible();
            CargarNivel();
        }

        private void CargarNivel()
        {
            TextoAResolver.text = letras[nivelActual];
            Siguiente.interactable = false;

            AsignarBotonesAleatorios();
            MezclarBotones();
            AnimarBotones();
        }
        private void AsignarBotonesAleatorios()
        {
            // Randomizamos cuál botón será "Vocal" y cuál será "Consonante"
            bool esVocalPrimero = Random.Range(0, 2) == 0;

            if (esVocalPrimero)
            {
                ConfigurarBoton(Vocal, "Vocal");
                ConfigurarBoton(Consonante, "Consonante");
            }
            else
            {
                ConfigurarBoton(Vocal, "Consonante");
                ConfigurarBoton(Consonante, "Vocal");
            }
        }
        
        
        private void ConfigurarBoton(Button boton, string tipo)
        {
            // Primero limpio listeners anteriores
            boton.onClick.RemoveAllListeners();

            // Le asigno el nuevo texto
            boton.GetComponentInChildren<TextMeshProUGUI>().text = tipo;

            // Le asigno el comportamiento correcto
            boton.onClick.AddListener(() => EvaluarRespuesta(tipo));
        }
        private void EvaluarRespuesta(string respuestaSeleccionada)
        {
            string letraActual = letras[nivelActual];

            bool esVocal = vocales.Contains(letraActual);
            string respuestaCorrecta = esVocal ? "Vocal" : "Consonante";

            if (respuestaSeleccionada == respuestaCorrecta)
            {
                Debug.Log("¡Correcto!");
                AudioManager.instance.ReproducirGanar();

                Siguiente.interactable = true;
            }
            else
            {
                intentos--;
                ActualizarIntentosDisponible();
                AudioManager.instance.ReproducirPerder();

                Debug.Log("¡Incorrecto! Intentos restantes: " + intentos);

                if (intentos <= 0)
                {
                    Consonante.interactable = false;
                    Vocal.interactable = false;
                }
            }
        }

        private void ActualizarTextoProgreso()
        {
            TxtProgreso.text = $"{nivelActual + 1} de {letras.Count}";
        }

        private void ActualizarIntentosDisponible()
        {
            TxtIntentos.text = $"Intentos disponibles: {intentos}";
        }

        private void AvanzarNivel()
        {
            if (nivelActual < letras.Count - 1)
            {
                nivelActual++;
                intentos = 3;
                BarraProgreso.value = nivelActual + 1;
                Consonante.interactable = true;
                Vocal.interactable = true;
                ActualizarTextoProgreso();
                ActualizarIntentosDisponible();
                CargarNivel();
            }
            else
            {
                Debug.Log("¡Todos los niveles completados!");
                ReiniciarTodosLosNiveles();
            }
        }

        private void ReiniciarNivel()
        {
            intentos = 3;
            Consonante.interactable = true;
            Vocal.interactable = true;
            ActualizarIntentosDisponible();
            CargarNivel();
        }

        private void ReiniciarTodosLosNiveles()
        {
            nivelActual = 0;
            intentos = 3;
            BarraProgreso.value = 1;
            Consonante.interactable = true;
            Vocal.interactable = true;
            ActualizarTextoProgreso();
            ActualizarIntentosDisponible();
            CargarNivel();
        }

        private void MezclarBotones()
        {
            // Cambiar de orden los botones de forma aleatoria
            List<Transform> botones = new List<Transform> { Vocal.transform, Consonante.transform };
            
            for (int i = 0; i < botones.Count; i++)
            {
                Transform temp = botones[i];
                int randomIndex = Random.Range(i, botones.Count);
                botones[i] = botones[randomIndex];
                botones[randomIndex] = temp;
            }

            // Aplicar nuevo orden
            foreach (Transform boton in botones)
            {
                boton.SetParent(ContenedorBotones);
            }
        }

        private void AnimarBotones()
        {
            foreach (Transform boton in ContenedorBotones)
            {
                StartCoroutine(AnimacionEscala(boton));
            }
        }

        private System.Collections.IEnumerator AnimacionEscala(Transform boton)
        {
            Vector3 escalaInicial = Vector3.one * 0.8f;
            Vector3 escalaFinal = Vector3.one;
            float duracion = 0.3f;
            float tiempo = 0f;

            boton.localScale = escalaInicial;

            while (tiempo < duracion)
            {
                tiempo += Time.deltaTime;
                float t = tiempo / duracion;
                // Puedes usar una curva de easing aquí, tipo "easeOutBack" casero
                t = 1f - Mathf.Pow(1f - t, 3); // Suavizado básico

                boton.localScale = Vector3.Lerp(escalaInicial, escalaFinal, t);
                yield return null;
            }

            boton.localScale = escalaFinal;
        }

        public void salirBanner()
        {
            banner.transform.Find("titulo").GetComponent<TextMeshProUGUI>().text = "Ejercicios";
        }

    }
}
