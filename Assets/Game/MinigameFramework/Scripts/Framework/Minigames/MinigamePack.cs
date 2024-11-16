using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.MinigameFramework.Scripts.Framework.Minigames {
    [CreateAssetMenu(fileName = "MinigamePack")]
    public class MinigamePack : ScriptableObject {
        public string packName;
        [Tooltip("Displayed next to minigame names")]public Sprite icon;
        [TextArea] public string description;
        public List<MinigameInfo> minigames;
    }
}