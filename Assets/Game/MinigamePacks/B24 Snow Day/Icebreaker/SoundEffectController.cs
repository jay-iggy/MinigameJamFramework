using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public float volume = 1f;
    public float pitch = 1f;
    public bool loop = false;
    [HideInInspector]
    public AudioSource source;

    public Sound()
    {

    }
    public Sound(AudioClip ac, AudioSource aso, float v, float p, bool l)
    {
        clip = ac;
        source = aso;
        volume = v;
        pitch = p;
        loop = l;
    }
}

public class SoundEffectController : MonoBehaviour
{
    public List<Sound> Sounds = new List<Sound>();

    private void Awake()
    {
        foreach (Sound s in Sounds)
        {
            s.source = this.gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }
    public AudioSource GetSource(string name)
    {
        Sound s = Sounds.Find(sound => sound.name == name);
        return s.source;
    }
    public void Play(string name)
    {
        Sound s = Sounds.Find(sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Sounds.Find(sound => sound.name == name);
        s.source.Stop();
    }
}

