using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;

    public AudioClip[] audioClips;
    public Button[] button;
    public void OnSound_Click(int index){
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }

    public void PlaySound(int index)
    {
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
