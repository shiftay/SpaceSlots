using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ClipIdentifier { SPIN = 0, WIN, INVALID  }

public class AudioManager : MonoBehaviour
{
    public const float BASEMUSICVOLUME = 0.25f;
    public const float BASESFXVOLUME = 0.5f;
    public AudioSource musicSource, sfxSource;
    public List<ClipDesc> sfxClips;
    public AudioClip soundLoop;

    public float musicVolume;
    public float sfxVolume;


    // Start is called before the first frame update
    void Start()
    {
        sfxSource.volume = sfxVolume;
        musicSource.volume = musicVolume;
        musicSource.clip = soundLoop;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void UpdateSFXVolume(bool enabled) {
        sfxSource.volume = enabled ? BASESFXVOLUME : 0;
    }

    public void UpdateMusicVolume(bool enabled) {
        musicSource.volume = enabled ? BASEMUSICVOLUME : 0;
    }

    public void PlaySFX(ClipIdentifier id) {
        sfxSource.clip = sfxClips.Find(n => n.clipID == (int)id).clip;
        sfxSource.Play();
    }

    [Button("SFX TEST")]
    private void PlaySFX()
    {
        sfxSource.clip = sfxClips[0].clip;
        sfxSource.volume = sfxVolume;
        sfxSource.Play();
    }

    [System.Serializable]
    public class ClipDesc {
        public AudioClip clip;
        public int clipID;
    }

} 
