
using UnityEngine;

namespace Script.Controlador.Principal.Modulo1.SubcontenidoVocales
{
    public class VocalesControlador : MonoBehaviour
    {
        private bool mostrarMayusculas = false;
        public void EjecutarTodosBotones()
        {
            mostrarMayusculas = !mostrarMayusculas;

            var botones = FindObjectsOfType<BtnVocales>();
            foreach (var btn in botones)
            {
                btn.ActualizarDatos(mostrarMayusculas);
            }
        }
    }
}