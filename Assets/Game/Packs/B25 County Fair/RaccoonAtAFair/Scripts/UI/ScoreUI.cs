using Game.MinigameFramework.Scripts.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XiaoHuanXiong.Player;

namespace XiaoHuanXiong.UI{
public class ScoreUI : MonoBehaviour
{
     // Start is called before the first frame update
    public RaccoonPawn racconPawn;
    [Header("Two digit UI images")]
    public Image TensDigitImage;
    public Image OnesDigitImage;

    [Header("Sprite Lookup Table (0-9)")]
    public Sprite[] digitSprites;
     void Start()
    {
            
        }

    // Update is called once per frame
    void Update()
    {

    }
    
        private void OnEnable()
     {
            racconPawn.OnScoreChanged += UpdateScoreDisplay;
            UpdateScoreDisplay();
     }
     private void OnDisable()
     {
            racconPawn.OnScoreChanged -= UpdateScoreDisplay;
        }
     //Update the UI score corrspond with player score
        private void UpdateScoreDisplay()
    {
        int score = racconPawn.Score;
         //   Debug.Log(score);
        if (score < 0) score = 0;
        if (score > 99) score = 99; 

        int tens = score / 10;
        int ones = score % 10;

        TensDigitImage.sprite = digitSprites[tens];
        OnesDigitImage.sprite = digitSprites[ones];
    }
}
}
