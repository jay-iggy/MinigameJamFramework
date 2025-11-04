using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.BundledUp {
    public class AudioManager : MonoBehaviour {
        public static AudioManager inst;

        [SerializeField] List<AudioSource> ambienceSounds = new();
        [SerializeField] GameObject SFXPrefab;
        [SerializeField] AudioClip highlightSound, hoverSound, correctSound, incorrectSound;

        private void Awake() {
            inst = this;
        }

        private void Start() {
            foreach (AudioSource aus in ambienceSounds)
                aus.Play();
        }

        public void SceneChanged() {
            if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Search) {
                playAmbience(true);
            }
            else if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Select) {
                playAmbience(false);
            }
            else if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Score) { }
        }

        public void playAmbience(bool doPlay) {
            if (doPlay)
                foreach (AudioSource aus in ambienceSounds)
                    aus.UnPause();
            else
                foreach (AudioSource aus in ambienceSounds)
                    aus.Pause();
        }

        private void PlaySound(AudioClip sound) {
            AudioSource sfx = Instantiate(SFXPrefab).GetComponent<AudioSource>();
            sfx.clip = sound;
            sfx.Play();
            Destroy(sfx.gameObject, sound.length + .1f);
        }

        public void PlayHover() {
            PlaySound(hoverSound);
        }

        public void PlayHighlight() {
            PlaySound(highlightSound);
        }

        public void PlayCorrect() {
            PlaySound(correctSound);
        }

        public void PlayIncorrect() {
            PlaySound(incorrectSound);
        }
    }
}