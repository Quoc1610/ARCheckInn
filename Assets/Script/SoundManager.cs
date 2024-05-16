using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;

    public AudioClip[] audioClips;
    public Button[] button;
    public void OnSound_Click(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public void PlaySound(int index)
    {
        if (!audioSource.isPlaying)
        {
            Debug.Log("OlaySounds");
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }

    /// <summary>
    /// Clear audio source
    /// </summary>
    public void Clear()
    {
        audioSource.clip = null;
        audioSource.Stop();
    }
}
