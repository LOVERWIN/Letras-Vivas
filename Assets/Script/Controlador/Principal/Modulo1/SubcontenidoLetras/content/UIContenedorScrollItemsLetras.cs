using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;

public class UIContenedorScrollItemsLetras: MonoBehaviour
{
    [SerializeField] private Transform itemcontenedor;
    //[SerializeField] private GameObject itemtemplate;
    [SerializeField] private ItemConfiguracion itemConfiguracion;
    private ItemFactory itemFactory;
    [SerializeField] private string nameItem;
    private string nombreArreglo="letras_mayus";
    private bool esMinusculas = true;
    string recursosPath = Path.Combine(Application.dataPath, "resources/Sprite/Imagenes_abecedario/");
    private List<LetraItem>letrasItems;
    
    private LetraItem[] letras_items =
    {
        
    };
    
    private LetraItem[] letras_items_min = {};
    
    private LetraItem[] letras_items_consonantes = {  
        new LetraItem("B", "biblioteca", "b", "B", "iblioteca"), 
        new LetraItem("C", "casa","c","C", "asa"), 
        new LetraItem("D", "dado", "d", "D", "ado"),
        new LetraItem("F", "familia", "f", "F", "amilia"),
        new LetraItem("G", "guitarra", "g", "G", "uitarra"),
        new LetraItem("H", "hospital", "h", "H", "ospital"), 
        new LetraItem("J", "juguete", "j", "J", "uguete"), 
        new LetraItem("K", "koala", "k", "K", "oala"), 
        new LetraItem("L", "lapiz", "l", "L", "apiz"),
        new LetraItem("M","mochila", "m", "M", "ochila"), 
        new LetraItem("N","nube", "n", "N", "ube"), 
        new LetraItem("Ñ","nandu", "ñ", "Ñ", "andu"), 
        new LetraItem("P", "pincel", "p", "P", "incel"),
        new LetraItem("Q", "queso", "q", "Q", "ueso"), 
        new LetraItem("R", "regla", "r", "R", "egla"), 
        new LetraItem("S", "sol", "s", "S", "ol"), 
        new LetraItem("T", "tren", "t", "T", "ren"),
        new LetraItem("V", "ventana", "v", "V", "entana"), 
        new LetraItem("W", "wafle", "w", "W", "afle"), 
        new LetraItem("X", "xilofono", "x", "X", "xilofono"),
        new LetraItem("Y", "yogurth", "y", "Y", "ogurth"), 
        new LetraItem("Z", "zapatos", "z", "Z", "apato") };

    private LetraItem[] letras_items_consonantes_min = {
        new LetraItem("b", "biblioteca", "b", "b", "iblioteca"),
        new LetraItem("c", "casa", "c", "c", "asa"),
        new LetraItem("d", "dado", "d", "d", "ado"),
        new LetraItem("f", "familia", "f", "f", "amilia"),
        new LetraItem("g", "guitarra", "g", "g", "uitarra"),
        new LetraItem("h", "hospital", "h", "h", "ospital"),
        new LetraItem("j", "juguete", "j", "j", "uguete"),
        new LetraItem("k", "koala", "k", "k", "oala"),
        new LetraItem("l", "lapiz", "l", "l", "apiz"),
        new LetraItem("m", "mochila", "m", "m", "ochila"),
        new LetraItem("n", "nube", "n", "n", "ube"),
        new LetraItem("ñ", "nandu", "ñ", "ñ", "andu"),
        new LetraItem("p", "pincel", "p", "p", "incel"),
        new LetraItem("q", "queso", "q", "q", "ueso"),
        new LetraItem("r", "regla", "r", "r", "egla"),
        new LetraItem("s", "sol", "s", "s", "ol"),
        new LetraItem("t", "tren", "t", "t", "ren"),
        new LetraItem("v", "ventana", "v", "v", "entana"),
        new LetraItem("w", "wafle", "w", "w", "afle"),
        new LetraItem("x", "xilofono", "x", "x", "xilofono"),
        new LetraItem("y", "yogurth", "y", "y", "ogurth"),
        new LetraItem("z", "zapatos", "z", "z", "apato")
    };

    private String[] fondosItemLetras = { "Contorno_letras_mayus","Contorno_letras_mayus_A","Contorno_letras_mayus_V" };

    private void Awake()
    {
        itemFactory = new ItemFactory(Instantiate(itemConfiguracion));
    
        
    }

    private void Start()
    {
        ListasContenedor();
    }
    
    LetraItem[] GenerarItems(bool changeArrelgo)
    {
        List<LetraItem> itemsList = new List<LetraItem>();
        string alfabeto = "abcdefghijklmnñopqrstuvwxyz";

        foreach (char letra in alfabeto)
        {
            string letraStr = letra.ToString();
            string carpetaLetra = (letraStr == "ñ") ? "_n" : letraStr;

            // Ruta dentro de Resources (sin "Resources/" y sin extensión)
            string pathEnResources = $"Sprite/Imagenes_abecedario/{carpetaLetra}";

            Sprite[] imagenes = Resources.LoadAll<Sprite>(pathEnResources);

            if (imagenes.Length == 0)
            {
                Debug.Log($"No hay imágenes para: {pathEnResources}");
                continue;
            }

            Sprite imagenSeleccionada = imagenes[Random.Range(0, imagenes.Length)];
            string nombre = imagenSeleccionada.name;

            string primeraletra = nombre.Substring(0, 1);
            string restoletra = nombre.Length > 1 ? nombre.Substring(1) : "";
            string rutaRelativa = carpetaLetra + "/" + nombre;

            if (changeArrelgo)
            {
                itemsList.Add(new LetraItem(
                    letraStr.ToUpper(),
                    rutaRelativa,
                    rutaRelativa,
                    primeraletra.ToUpper(),
                    restoletra
                ));
            }
            else
            {
                itemsList.Add(new LetraItem(
                    letraStr,
                    rutaRelativa,
                    rutaRelativa,
                    primeraletra.ToLower(),
                    restoletra
                ));
            }
        }

        return itemsList.ToArray();
    }



    public void ListasContenedor()
    {
        ActualizarContenedorItems();
       // AddListenerButones();
    }

    private LetraItem[] CambiarArreglo(string name)
    {
        if (name.Equals("letras_mayus"))
        {
            return letras_items = GenerarItems(true);
        }
        else if (name.Equals("letras_min"))
        {
            return letras_items = GenerarItems(false);
        }
        else if (name.Equals("letras_consonantes"))
        {
            return letras_items_consonantes;
        }
        else if (name.Equals("letras_items_consonantes_min"))
        {
            return letras_items_consonantes_min;
        }
        {
            return null;
        }
    }
  

    private void ActualizarContenedorItems()
    {
        LimpiarContenedor();
        int fondoIndex = 0; // Índice para recorrer los fondos
        foreach (var item1 in CambiarArreglo(nombreArreglo))
        {
            // Ya viene instanciado
            var item = itemFactory.Create(nameItem);
            // Cargar sprite del fondo correspondiente
            string rutaSprite = $"Sprite/Alfabetizacion/{fondosItemLetras[fondoIndex]}";
            item.GetComponent<Image>().sprite = Resources.Load<Sprite>(rutaSprite);
            // Avanzar el índice y hacer que sea circular
            fondoIndex = (fondoIndex + 1) % fondosItemLetras.Length;
            
            var newItem = item.GetComponent<RectTransform>();
            // Asegura que esté bajo el contenedor correcto
            newItem.SetParent(itemcontenedor, false);
            // Cambiar el nombre para evitar "(Clone)"
            newItem.name = "Letra_" + item1.letra;
            
            // Setear texto
            var tituloTransform = newItem.Find("Titulo");
            if (tituloTransform != null)
            {
                var texto = tituloTransform.GetComponent<TextMeshProUGUI>();
                if (texto != null)
                {
                    texto.text = item1.letra;
                }
            }

            // Obtener componente y asignar valores
            var letraItem = newItem.GetComponent<LetraItem>();
            if (letraItem != null)
            {
                letraItem.fondo = item1.fondo;
                letraItem.letra = item1.letra;
                letraItem.primeraletra = item1.primeraletra;
                letraItem.restoletra = item1.restoletra;
                letraItem.musica = item1.musica;
            }
            // Agregar evento si no existe aún
            if (newItem.GetComponent<SetEventBtnLetras>() == null)
            {
                newItem.gameObject.AddComponent<SetEventBtnLetras>();
            }
            
        }


    }
    private void LimpiarContenedor()
    {
        Debug.Log("Entrando al metodo de eliminar Letras");
        foreach (Transform child in itemcontenedor)
        {
            Destroy(child.gameObject);
        }
    }


    public void ActualizarContenedorItemsMinus()
    {
        nombreArreglo = "letras_min";
        ActualizarContenedorItems();
        nombreArreglo = "letras_mayus";
    }
    public void ActualizarContenedorItemConsonantes()
    {
        nombreArreglo = "letras_items_consonantes_min";
        ActualizarContenedorItems();
        nombreArreglo = "letras_consonantes";
    }
    public void ToggleActualizarContenedor()
    {
        if (esMinusculas)
        {
            ActualizarContenedorItemsMinus();
        }
        else
        {
            ActualizarContenedorItems();
        }

        esMinusculas = !esMinusculas; // Cambia el estado para el próximo clic
    }

    public void TogleActualizarContenedorConsonantes()
    {
        
        if (esMinusculas)
        {
            nombreArreglo = "letras_consonantes";
            ActualizarContenedorItems();
        }
        else
        {
            nombreArreglo = "letras_items_consonantes_min";
            ActualizarContenedorItems();
        }
        esMinusculas = !esMinusculas;
    }
    
}
