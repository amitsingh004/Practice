using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
   
    [SerializeField] private SoundLib soundLib;
    public static AudioMgr Instance;
    private AudioSource audioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
   public void PlaySound(SoundType soundType)
    {
        AudioClip clip = soundLib.GetSound(soundType);
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("Audio clip not found or AudioSource is missing!");
        }
    }
    
}
