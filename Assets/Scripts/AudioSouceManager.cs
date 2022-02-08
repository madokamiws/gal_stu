using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSouceManager : MonoBehaviour
{

    public static AudioSouceManager Instance { get; private set; }

    public AudioSource soundAudio;
    public AudioSource musicAudio;
    public AudioSource dialogueAudio;

    public static AudioSouceManager Get
    {
        get
        {
            if (!Instance)
            {
                Instance = new AudioSouceManager();
            }
            return Instance;
        }
    }
    private void Awake()
    {
        Instance = this;  
    }

    /// <summary>
    /// 特效音
    /// </summary>
    /// <param name="soundPath"></param>
    public void PlaySound(string soundPath)
    {
        soundAudio.PlayOneShot(Resources.Load<AudioClip>("AudioClips/Sound/"+ soundPath));
    }
    /// <summary>
    /// 背景音乐
    /// </summary>
    /// <param name="musicPath"></param>
    /// <param name="loop"></param>
    public void PlayMusic(string musicPath, bool loop = true)
    {
        musicAudio.loop = loop;
        musicAudio.clip = Resources.Load<AudioClip>("AudioClips/Music/" + musicPath);
        musicAudio.Play();
    }
    public void StopMusic()
    {
        musicAudio.Stop();
    }
    /// <summary>
    /// 对话
    /// </summary>
    /// <param name="dialoguePath"></param>
    public void PlayDialogue(string dialoguePath)
    {
        dialogueAudio.Stop();
        dialogueAudio.PlayOneShot(Resources.Load<AudioClip>("AudioClips/Dialogue/"+dialoguePath));
    }
    public void StopDialogue()
    {
        dialogueAudio.Stop();
    }

}
