using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable, RequireComponent(typeof(AudioSource)), ExecuteAlways]
public class AudioManager : MonoBehaviour
{
    new static AudioManager audio;

    [SerializeField]
    public List<Sound> sounds = new List<Sound>();

    static AudioSource audioSource;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    public static void PlaySound(string audioName)
    {
        audioSource.PlayOneShot(GetClip(audioName));
    }

    public static void PlaySound(string audioName, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(GetClip(audioName), position);
    }

    static AudioClip GetClip(string clipName)
    {
        foreach (Sound clip in audio.sounds)
        {
            if (clip.name == clipName)
            {
                return clip.clip;
            }
        }
        Debug.LogWarning("No audio clip found with name: " + clipName);
        return null;
    }

    static void Start()
    {
        AudioManager audio = FindObjectOfType<AudioManager>() ?? null;
        if (audio == null)
        {
            Debug.LogWarning("No AudioManager found in scene.");
        }
    }
}

public struct Sound
{
    public string name;
    public AudioClip clip;

    public Sound(string audioName, AudioClip audioClip)
    {
        name = audioName;
        clip = audioClip;
    }

    public void SetAudio(string newName, AudioClip newAudio)
    {
        name = newName;
        clip = newAudio;
    }

    public void SetClip(AudioClip newClip)
    {
        clip = newClip;
    }

    public void SetName(string newName)
    {
        name = newName;
    } 
}