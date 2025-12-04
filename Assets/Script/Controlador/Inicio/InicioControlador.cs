
using UnityEngine;

namespace Script.Controlador.Inicio
{
    public class InicioControlador : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("");
        }

        private void Start()
        {
            AudioManager.instance.ReproducirAudioExclusivo("Vistas/Inicio/ogg__vista1_audio2__v1");
        }
        private void OnEnable()
        {
         
        }

    }
}