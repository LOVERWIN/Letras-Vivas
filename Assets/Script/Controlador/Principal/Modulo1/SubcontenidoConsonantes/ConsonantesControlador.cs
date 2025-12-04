
using UnityEngine;

namespace Script.Controlador.Principal.Modulo1.SubcontenidoConsonantes
{
    public class ConsonantesControlador : MonoBehaviour
    {
        [SerializeField] private UIContenedorScrollItemsLetras uiContenedorScrollItemsLetras;
        private void Start()
        {
            uiContenedorScrollItemsLetras.ActualizarContenedorItemConsonantes();
        }
    }
}   