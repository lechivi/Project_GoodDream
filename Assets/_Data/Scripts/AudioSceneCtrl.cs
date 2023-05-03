using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSceneCtrl : MonoBehaviour
{
    [SerializeField] private List<AudioClip> clipList = new List<AudioClip>();

    private void Start()
    {
        if (this.clipList.Count == 0) return;
        this.PlayMusicOnScene(this.clipList[0]);
    }

    private void PlayMusicOnScene(AudioClip clip)
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM(clip.name);
        }
    }
}
