using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Instancia única del AudioManager
    public static AudioManager instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    public Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();

    // Ahora guardamos el GameObject temporal asociado a cada nombre de sonido
    private Dictionary<string, GameObject> sonidosActivos = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        LoadSFXClips();
        LoadMusicClips();
    }

    private void LoadSFXClips()
    {
        sfxClips["Gema"] = Resources.Load<AudioClip>("SFX/getKey");
        sfxClips["Arrancar"] = Resources.Load<AudioClip>("SFX/arrancar");
        sfxClips["Turbo"] = Resources.Load<AudioClip>("SFX/Turbo");
        
    }

    private void LoadMusicClips()
    {
        musicClips["Music1"] = Resources.Load<AudioClip>("Music/BackgroundMusic_1");
        musicClips["Music2"] = Resources.Load<AudioClip>("Music/BackgroundMusic_2");
        musicClips["Music3"] = Resources.Load<AudioClip>("Music/BackgroundMusic_3");
        musicClips["Music4"] = Resources.Load<AudioClip>("Music/BackgroundMusic_4");
        musicClips["Music5"] = Resources.Load<AudioClip>("Music/BackgroundMusic_5");
        musicClips["TTGL"] = Resources.Load<AudioClip>("Music/TTGL SRW Z2");
        musicClips["MazingerZ"] = Resources.Load<AudioClip>("Music/Mazinger SRW Z2");
    }

    public void PlaySFX(string clipName)
    {
        if (sfxClips.ContainsKey(clipName))
        {
            sfxSource.clip = sfxClips[clipName];
            sfxSource.Play();
        }
        else Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de sfxClips.");
    }

    public void PlayMusic(string clipName, bool loop = true)
    {
        if (musicClips.ContainsKey(clipName))
        {
            AudioClip cancionNueva = musicClips[clipName];

            // Comprobamos si la canción que nos piden YA está puesta en el reproductor y está sonando.
            if (musicSource.clip == cancionNueva && musicSource.isPlaying)
            {
                // Si es la misma, usamos 'return' para salir de la función inmediatamente.
                // Así evitamos que se reinicie y dejamos que siga fluyendo.
                return;
            }

            // Si es una canción diferente (o estaba en silencio), la ponemos y le damos al Play
            musicSource.clip = cancionNueva;
            musicSource.loop = loop;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("El AudioClip " + clipName + " no se encontró en el diccionario de musicClips.");
        }
    }

    public void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    public void StopSFX()
    {
        sfxSource.Stop();
    }
     
    public void PlaySFXOneShot(string clipName, float volume = 1.0f)
    {
        if (sfxClips.ContainsKey(clipName))
        {
            AudioClip clip = sfxClips[clipName];

            // Creamos un GameObject temporal vacío por CADA gema que cojas
            GameObject tempAudioObj = new GameObject("TempAudio_" + clipName);

            // Le añadimos un componente AudioSource y lo configuramos
            AudioSource tempSource = tempAudioObj.AddComponent<AudioSource>();
            tempSource.clip = clip;
            tempSource.volume = volume;
            tempSource.playOnAwake = false;
            tempSource.spatialBlend = 0f;
             
            tempSource.Play();

            // Destruimos este clon temporal exactamente cuando termine su sonido 
            Destroy(tempAudioObj, clip.length);
        }
        else
        {
            Debug.LogWarning("El AudioClip " + clipName + " no se encontró.");
        }
    }
}