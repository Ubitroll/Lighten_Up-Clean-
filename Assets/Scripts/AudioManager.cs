using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public string[] audioClipNames;
    public AudioClip[] audioClips;

    static Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    private void Start()
    {
        for(int i = 0; i < GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().audioClips.Length; i++)
        {
            clips.Add(GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().audioClipNames[i],
                GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>().audioClips[i]);
        }
    }

    public static void PlaySound(string audioName, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(GetClip(audioName), position);
    }

    public static void PlaySound(string audioName, GameObject point)
    {
        AudioSource.PlayClipAtPoint(GetClip(audioName), point.transform.position);
    }

    // No error catching!!
    static AudioClip GetClip(string clipName)
    {
        return clips[clipName];
    }
}
