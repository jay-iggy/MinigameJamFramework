using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XiaoHuanXiong.UI
{
    public class CountDownTimer : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] List<Sprite> timeDigits;
        [SerializeField] private Image minuteTime;
        [SerializeField] private Image secondTime1;
        [SerializeField] private Image secondTime2;
        [SerializeField] private Image middleSymbol;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void UpdateCountDownTimer(int mm, int ss)
        {
            string minute = mm.ToString();
            string second = ss.ToString();
            
            minuteTime.sprite = timeDigits[mm];
            secondTime1.sprite = timeDigits[ss / 10];
            secondTime2.sprite = timeDigits[ss % 10];

        }
        public void UpdateCountDownTimer(int timeLeft)
        {
            int minutes = timeLeft / 60;
            int seconds = timeLeft % 60;
            if (timeLeft <= 20)
            {
                minuteTime.color = Color.red;
                secondTime1.color = Color.red;
                secondTime2.color = Color.red;
                middleSymbol.color = Color.red;
            }
            UpdateCountDownTimer(minutes, seconds);
        }
    }
}
