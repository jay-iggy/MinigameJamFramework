using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XiaoHuanXiong.Audio
{
    public class PlayAudioAfterFewSeconds : MonoBehaviour
    {
        // Start is called before the first frame update
        public AudioSource musicSource;
        public float delay = 2f;
        void Start()
        {
            StartCoroutine(FadeInMusic(musicSource, 2f));

        }
        //The music fade in 2 seconds
        private IEnumerator FadeInMusic(AudioSource source, float duration)
        {
            source.volume = 0;
            yield return new WaitForSeconds(delay);
            source.time = 60;
            source.Play();

            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                source.volume = Mathf.Lerp(0, 1, time / duration);
                yield return null;
            }
        }
    }
}
