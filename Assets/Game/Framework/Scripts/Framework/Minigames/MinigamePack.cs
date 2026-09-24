using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.MinigameFramework.Scripts.Framework.Minigames {
    [CreateAssetMenu(fileName = "MinigamePack")]
    public class MinigamePack : ScriptableObject {
        public string packName;
        [Tooltip("Displayed next to minigame names")] public Sprite icon;
        public Color packColor =  Color.white;
        [TextArea] public string description;
        public List<MinigameInfo> minigames;

        #if UNITY_EDITOR
        void Awake() {
            MinigameManager manager = AssetDatabase.LoadAssetAtPath<MinigameManager>("Assets/Game/Framework/MinigameManager.prefab");
            if(manager!=null) {
                Debug.Log($"Created Minigame Pack '{name}': Adding to MinigameManager::allPacks automatically.");

                manager.allPacks.Add(this);
            }
        }
        #endif
    }
}