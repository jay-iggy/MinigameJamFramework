using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CountyFair.BigBucks {
    public class BBAudio : MonoBehaviour {

        public static BBAudio inst;

        void Awake() {
            inst = this;
        }

        // [SerializeField] AudioClip bgMusicClip;
        AudioSource bgMusic;
        float loopDetector = -1;
        int loopCount = 0;
        
        float initailVolume;

        void Start() {
            bgMusic = GetComponent<AudioSource>();
            initailVolume = bgMusic.volume;
        }
        
        void Update() {

            // if (!bgMusic.isPlaying) { // only when intro finishes
            //     bgMusic.loop = true;
            //     bgMusic.clip = bgMusicClip;
            //     bgMusic.Play();
            // }


            if (bgMusic.time < loopDetector) {
                loopCount++;
                // Used for pitch Scaling ----------------
                // bgMusic.pitch = 1 + loopCount / 10f;
                // bgMusic.outputAudioMixerGroup.audioMixer.SetFloat("Pitch", 1f / bgMusic.pitch);
                bgMusic.time = 3.7f;
            }
            loopDetector = bgMusic.time;

            if (bgMusic.volume != initailVolume) {
                bgMusic.volume += (initailVolume - bgMusic.volume) * Time.deltaTime / 2;
            }
        }

        [SerializeField] GameObject sfxPrefab;

        // Audio Lists
        [SerializeField] List<AudioClip> wrongClips = new List<AudioClip>();

        [SerializeField] List<AudioClip> correctClips = new List<AudioClip>();

        [SerializeField] List<AudioClip> buckedClips = new List<AudioClip>();

        [SerializeField] List<AudioClip> victoryClips = new List<AudioClip>();

        [SerializeField] List<AudioClip> dustClips = new List<AudioClip>();

        private void PlayRandomSoundFromList(List<AudioClip> list, float pitchRange = .125f)
        {
            AudioSource aus = Instantiate(sfxPrefab).GetComponent<AudioSource>();
            aus.clip = list[UnityEngine.Random.Range(0, list.Count)];
            aus.pitch = UnityEngine.Random.Range(1 - pitchRange * .5f, 1 + pitchRange * .5f);
            aus.Play();
            DontDestroyOnLoad(aus.gameObject);
            Destroy(aus.gameObject, aus.clip.length + .1f);
        }

        public void PlayRandomWrong() { PlayRandomSoundFromList(wrongClips); }
        public void PlayRandomCorrect() { PlayRandomSoundFromList(correctClips); }
        public void PlayRandomBucked() { PlayRandomSoundFromList(buckedClips, .1f); bgMusic.volume = .2f; /**ducking**/ }
        public void PlayRandomVictory() { PlayRandomSoundFromList(victoryClips, 0f); bgMusic.enabled = false; }
        public void PlayRandomDust() { PlayRandomSoundFromList(dustClips, .3f); }
    }
}
