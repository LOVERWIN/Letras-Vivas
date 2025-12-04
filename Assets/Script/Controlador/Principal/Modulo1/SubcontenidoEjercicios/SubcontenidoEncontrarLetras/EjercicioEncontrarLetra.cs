
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Controlador.Principal.Modulo1.SubcontenidoEjercicios.SubcontenidoEncontrarLetras
{
    public class EjercicioEncontrarLetra : MonoBehaviour
    {
        [SerializeField] private ItemConfiguracion itemConfiguracion;
        [SerializeField] private string nameItem;
        private ItemFactory itemFactory;
        [SerializeField] private Transform contenedor;
        [SerializeField] private GameObject prefabLetraObjetivo; // Prefab de una letra en la lista de letras objetivo
        [SerializeField] private Transform contenedorLetrasObjetivo;
        [SerializeField] private TextMeshProUGUI textoCronometro;
        [SerializeField] private Button btnRepetir;
        [SerializeField] private Button btnSiguiente;
        [SerializeField] private Slider barraProgreso;
        [SerializeField] private TextMeshProUGUI textoProgreso;
        [SerializeField] private GameObject banner;
        [SerializeField] private GameObject letraEncontrar;
        private List<string> letras = new List<string>
        {
            "A","B","C","D","E","F","G","H","I","J",
            "K","L","M","N","Ñ","O","P","Q","R","S",
            "T","U","V","W","X","Y","Z"
        };

       

        private int nivelActual = 0;
        private string letraActual;
        private int letrasEncontradas = 0;
        private float tiempoRestante = 30f;
        private bool cronometroActivo = false;
        private TextMeshProUGUI letra;

        private List<TextMeshProUGUI> letrasObjetivoUI = new List<TextMeshProUGUI>();
        private List<Image> letrasObjetivoUIImage = new List<Image>();

        public void EntrarBanner()
        {
            banner.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/Contenedor_boton_sonidos2");
            banner.transform.Find("titulo").GetComponent<TextMeshProUGUI>().text = "Encuentra las letras";
        }
        private void Start()
        {
           
            letra = letraEncontrar.transform.GetComponentInChildren<TextMeshProUGUI>();
            itemFactory = new ItemFactory(Instantiate(itemConfiguracion));      
            btnRepetir.onClick.AddListener(ReiniciarNivel);
            btnSiguiente.onClick.AddListener(AvanzarNivel);
            
            barraProgreso.minValue = 0;
            barraProgreso.maxValue = letras.Count;
            Debug.Log($"Cantidod de niveles : " +letras.Count);// La cantidad total de niveles
            barraProgreso.value = 1;
            IniciarNivel();
        }
        private void ActualizarTextoProgreso()
        {
            textoProgreso.text = $"{nivelActual + 1} de {letras.Count}";
          
        }
       
        private void Update()
        {
            if (cronometroActivo)
            {
                tiempoRestante -= Time.deltaTime;
                textoCronometro.text = Mathf.CeilToInt(tiempoRestante).ToString();

                if (tiempoRestante <= 0)
                {
                    tiempoRestante = 0;
                    cronometroActivo = false;
                    BloquearSopa();
                    //btnSiguiente.interactable = true;
                }
            }
        }

        private void IniciarNivel()
        {
            letraActual = letras[nivelActual];
            letra.text = letraActual;
            letrasEncontradas = 0;
            tiempoRestante = 30f;
            cronometroActivo = true;
            btnSiguiente.interactable = false;

            // Limpiar sopas anteriores
            foreach (Transform child in contenedor)
            {
                var btn = child.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners(); // 🔥 QUITAR EVENTOS
                }
                Destroy(child.gameObject);
            }
            foreach (Transform child in contenedorLetrasObjetivo)
            {
                Destroy(child.gameObject);
            }
            letrasObjetivoUI.Clear();
            letrasObjetivoUIImage.Clear();

            // Crear letras objetivo
            for (int i = 0; i < 5; i++)
            {
                
                var obj = Instantiate(prefabLetraObjetivo, contenedorLetrasObjetivo);
                var txt = obj.GetComponentInChildren<TextMeshProUGUI>();
                var imagenIcon = obj.GetComponent<Image>();
                txt.color = new Color(125f / 255f, 156f / 255f, 85f / 255f);
                txt.text = letraActual;
                letrasObjetivoUI.Add(txt);
                letrasObjetivoUIImage.Add(imagenIcon);
            }

            // Crear sopa
            List<string> sopa = new List<string>();

            // 5 letras correctas
            for (int i = 0; i < 5; i++)
            {
                sopa.Add(letraActual);
            }

            // 31 letras random que no sean la letra actual
            List<string> banco = new List<string>(letras);
            banco.Remove(letraActual);

            for (int i = 0; i < 43; i++)
            {
                int randomIndex = Random.Range(0, banco.Count);
                sopa.Add(banco[randomIndex]);
            }

            sopa = MezclarLista(sopa);

            foreach (string letra in sopa)
            {
                var item = itemFactory.Create(nameItem);
                if (item == null)
                {
                    Debug.Log("El componente item es nulo: "+item);
                }
                var letraObj =  item.GetComponent<RectTransform>();
                letraObj.SetParent(contenedor, false);
                letraObj.name = letra;
                var txt = letraObj.GetComponentInChildren<TextMeshProUGUI>();
                txt.text = letra;
                Button boton = letraObj.GetComponent<Button>();

                boton.onClick.AddListener(() => SeleccionarLetra(boton, letra));
            }
        }

        private void SeleccionarLetra(Button boton, string letra)
        {
            if (!cronometroActivo) return;

            boton.interactable = false; // Bloquear botón para no seleccionar dos veces
            //boton.GetComponent<Image>().color = Color.gray; // Marcar selección
          
            AudioManager.instance.ReproducirClick();

        if (letra == letraActual)
            {
                boton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/icono_sopa_select");
                letrasEncontradas++;

                if (letrasEncontradas <= letrasObjetivoUI.Count)
                {
                    letrasObjetivoUI[letrasEncontradas - 1].color = Color.white;
                    letrasObjetivoUIImage[letrasEncontradas - 1].GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/icon_verde_encotradaLetra");
                }

                if (letrasEncontradas == 5)
                {
                    cronometroActivo = false;
                    btnSiguiente.interactable = true;
                    AudioManager.instance.ReproducirGanar();
                    foreach (var txt in letrasObjetivoUI)
                    {
                        txt.color = Color.yellow; // Colorear todos de amarillo al completar
                    }
                }
            }
            else
            {
                tiempoRestante -= 5f;
                if (tiempoRestante < 0f) tiempoRestante = 0f; // No dejar que sea negativo
                AudioManager.instance.ReproducirPerder(); // Si tienes un sonido de error
            }
        }

        private void ReiniciarNivel()
        {
            IniciarNivel();
        }

        private void AvanzarNivel()
        {
            if (nivelActual < letras.Count - 1)
            {
                nivelActual++;
                barraProgreso.value = nivelActual;
                ActualizarTextoProgreso();
                IniciarNivel();
            }
            else
            {
                Debug.Log("¡Todos los niveles completados!");
            }
        }

        private void BloquearSopa()
        {
            AudioManager.instance.ReproducirPerder();

            foreach (Transform child in contenedor)
            {
                child.GetComponent<Button>().interactable = false;
            }
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

        public void SalirBanner()
        {
            banner.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/Alfabetizacion/Contenedor_nombre_modulo");
            banner.transform.Find("titulo").GetComponent<TextMeshProUGUI>().text = "Ejercicios";

        }
    }
}
