using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AudioManager am = (AudioManager)target;

        if (GUILayout.Button("Add Sound"))
        {
            am.sounds.Add(new Sound());
        }
        for (int i = 0; i < am.sounds.Count; i++)
        {
            Sound sound = am.sounds[i];

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Play"))
            {
                AudioManager.PlaySound(am.sounds[i].clip);
            }

            sound.clip = (AudioClip)EditorGUILayout.ObjectField(sound.clip, typeof(AudioClip), false);
            sound.name = (EditorGUILayout.TextField(sound.name));

            if (GUILayout.Button("Remove"))
            {
                am.sounds.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();

            am.sounds[i] = sound;
        }
    }
}
