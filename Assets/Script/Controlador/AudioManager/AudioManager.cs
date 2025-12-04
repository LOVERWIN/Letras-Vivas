
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip sonidoGanar;
    public AudioClip sonidoPerder;
    public AudioClip click;
    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    void Awake()
    {   
        // Singleton simple
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: para persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReproducirGanar()
    {
        audioSource.PlayOneShot(sonidoGanar);
    }

    public void ReproducirPerder()
    {
        audioSource.PlayOneShot(sonidoPerder);
    }

    public void ReproducirClick()
    {
        audioSource.PlayOneShot(click);
    }
    
    public void ReproducirAudio(string nombreClip)
    {
        if (!clips.ContainsKey(nombreClip))
        {
            AudioClip clip = Resources.Load<AudioClip>($"Audios/{nombreClip}");
            if (clip != null)
            {
                clips[nombreClip] = clip;
            }
            else
            {
                Debug.LogWarning($"Audio '{nombreClip}' no encontrado en Resources/Audios/");
                return;
            }
        }
        audioSource.Stop(); // Detiene el audio actual si hay uno
        audioSource.clip = clips[nombreClip];
        audioSource.Play(); // Reproduce el nuevo clip (no se superpone)
    }
    public void ReproducirAudioExclusivo(string nombreClip)
    {
        if (!clips.ContainsKey(nombreClip))
        {
            AudioClip clip = Resources.Load<AudioClip>($"Audios/{nombreClip}");
            if (clip != null)
            {
                clips[nombreClip] = clip;
            }
            else
            {
                Debug.LogWarning($"Audio '{nombreClip}' no encontrado en Resources/Audios/");
                return;
            }
        }

        audioSource.Stop(); // Detiene el audio actual si hay uno
        audioSource.clip = clips[nombreClip];
        audioSource.Play(); // Reproduce el nuevo clip (no se superpone)
    }

   
    
    public void DetenerAudioActual()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

}