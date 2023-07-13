using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum ClipIdentifier { SPIN = 0, WIN, INVALID  }

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource, sfxSource;
    public List<ClipDesc> sfxClips;
    public AudioClip soundLoop;

    public float volumetest;
    public float sfxVolume;


    // Start is called before the first frame update
    void Start()
    {
        sfxSource.volume = sfxVolume;
        musicSource.volume = volumetest;
        musicSource.clip = soundLoop;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
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
