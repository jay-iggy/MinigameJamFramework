using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowfort
{
    public class MusicManager : MonoBehaviour
    {
        public AudioSource calm;
        public AudioSource intense;

        public float fadeTime;
        public float volume = 1;

        float timer;
        bool toCalm = true;

        public void SetIntenseMusic(bool playIntense)
        {
            toCalm = playIntense;
        }

        private void Update()
        {
            timer += Time.deltaTime * (toCalm ? -1 : 1);

            timer = Mathf.Clamp(timer, 0, fadeTime);

            calm.volume = volume * (1 - (timer / fadeTime));
            intense.volume = volume * (timer / fadeTime);
        }
    }
}