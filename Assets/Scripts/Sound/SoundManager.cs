using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SoundType{NonDiagetic, Diagetic}

public enum SourceState{NotPlaying, isPlaying, Default}

[System.Serializable]
public class Sound
{
    AudioSource source;
    public string nameClip;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 3f)]
    public float pitch;
    public float maxvol;
    public bool loop = false;
    
    public SoundType type;

    public SourceState state;

    public bool isPlaying = false;

    private Coroutine fadeCoroutine;
    
    private Coroutine fadeInCourtine;

    private Coroutine sfxCoroutine;

     private Coroutine DialogueCoroutine;

    public void setSource(AudioSource sourceClip)
    {
        source = sourceClip;
        source.clip = clip;
        source.volume = volume;
        maxvol = volume;
        source.pitch = pitch;
        source.playOnAwake = isPlaying;
        source.loop = loop;
    }

    public AudioSource getSource()
    {
        return source;
    }

    public void SetVolume(float vol)
    {
        if (source != null)
        {
            source.volume = Mathf.Clamp01(vol) * maxvol;
        }
    }

    public void PauseSource()
    {
         if(source == null) return;
         source.Pause();
    }

    public void UnPuaseSource()
    {
        if(source == null) return;
        source.UnPause();
    }

    public IEnumerator FadeOutTransition(float duration)
    {
        if(source == null) yield break;

        float oringalsound = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if(source == null) yield break;

            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(oringalsound, 0f, elapsed / duration);

            yield return null;
        }

        if (source != null)
        {
         source.Stop();
         source.volume = oringalsound; 

        }

        state = SourceState.NotPlaying;
        
    }

    public IEnumerator FadeInTransition(float duration)
    {
        if(source == null) yield break;

        float oringalsound = source.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if(source == null) yield break;

            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, oringalsound, elapsed / duration);

            yield return null;
        }

        if (source != null)
        {
         source.Play();
         source.volume = oringalsound; 

        }

        state = SourceState.isPlaying;
        
    }

    public void FadeOutCourtine(MonoBehaviour runningScript)
    {
        if (fadeCoroutine != null) runningScript.StopCoroutine(fadeCoroutine);
            fadeCoroutine = runningScript.StartCoroutine(FadeOutTransition(1f));
    }

     public void FadeInCourtine(MonoBehaviour runningScript)
    {
        if (fadeInCourtine != null) runningScript.StopCoroutine(fadeInCourtine);
            fadeInCourtine= runningScript.StartCoroutine(FadeInTransition(1f));
    }

    public void Stop()
    {
        source.Stop();
        if (state != SourceState.Default) state = SourceState.NotPlaying;  
           
    }

    public void Play(MonoBehaviour runningScript)
    {  
          source.Play(); 
          if(state != SourceState.Default) state = SourceState.isPlaying;   
        

        if(!loop && state != SourceState.Default)
        {
             if (sfxCoroutine != null) runningScript.StopCoroutine(sfxCoroutine);
             sfxCoroutine = runningScript.StartCoroutine(PlaySFX());
        }   
        
    }

    public void PlayDialogue(MonoBehaviour runningScript)
    {
        source.Play();
        if(state != SourceState.Default) state = SourceState.isPlaying;

        if(!loop && state != SourceState.Default)
        {

          if (DialogueCoroutine != null) runningScript.StopCoroutine(DialogueCoroutine);
             DialogueCoroutine = runningScript.StartCoroutine(PlayDialogueClip());   

        } 
         
    }

    private IEnumerator PlaySFX()
    {
        if(source == null) yield break;

        while (source.time < source.clip.length)
        {

           if (state == SourceState.NotPlaying) yield break;
           yield return null;
        }

        if (state == SourceState.isPlaying) state = SourceState.NotPlaying;
        
    }

     private IEnumerator PlayDialogueClip()
     {
        if(source == null) throw new UnityException("source is null");

         while (source.time < source.clip.length)
        {
           if (state == SourceState.NotPlaying) yield break; 
           yield return null;
        }

        if (state == SourceState.isPlaying) state = SourceState.NotPlaying;

       // Debug.Log("PlayedDialogue");
     }

    public void PauseSound()
    {
        source.Pause();
    }

    public void UnpauseSound()
    {
        source.UnPause();
    }
}

public class SoundManager : Singleton<SoundManager>
{

     [SerializeField]
     private Sound[] sounds;
     public Sound[] GetSounds()
    {
        return sounds;
    }
     public override void Awake()
    {
        base.Awake();

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject play = new GameObject("Sound : " + i + " : " + sounds[i].nameClip);
            play.transform.SetParent(this.transform);
            sounds[i].setSource(play.AddComponent<AudioSource>());
        }
        
       // PlaySound("theme");
    }


    public void PlaySound(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].nameClip == name)
            {
                sounds[i].Play(this);
                return;
            }
        }
    }
    

    public void FadeOutTransition(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].nameClip == name)
            {
                sounds[i].FadeOutCourtine(this);
                return;
            }
        }
    }

    public void FadeInTransition(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].nameClip == name)
            {
                sounds[i].FadeInCourtine(this);
                return;
            }
        }
    }

    public void StopMusic(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].nameClip == name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }

    public void VolumeAmount(float vol)
    {
        for (int i = 0; i < sounds.Length; i++)
        {  
            if (sounds[i].type == SoundType.Diagetic) sounds[i].SetVolume(vol);
        }
    }
    public void MusicAmount(float vol)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].type == SoundType.NonDiagetic) sounds[i].SetVolume(vol);   
            
        }
    }

    public void PauseAudio()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].state == SourceState.isPlaying) sounds[i].PauseSound();
        }
    }

    public void UnPauseAudio()
    {
         for (int i = 0; i < sounds.Length; i++)
        {
            if(sounds[i].state == SourceState.isPlaying) sounds[i].UnpauseSound();
        }
    }

     public void StopAllSounds()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].state == SourceState.isPlaying)
            {
                sounds[i].Stop();
            }
        }
    }
}