using UnityEngine;

namespace Game.MinigameFramework.Scripts.Framework.Minigames {
    [CreateAssetMenu(fileName = "NewMinigame", menuName = "MinigameInfo")]
    public class MinigameInfo : ScriptableObject {
        public string minigameName;
        public Sprite thumbnail;
        [Tooltip("If you change the name of the scene in the project, you must update this field to refresh internal values.")]
        [SerializeField] public SceneField scene;
        [Tooltip("Fewest players needed to be connected for game to not crash/softlock"), Range(1,4)] public int minimumPlayers;
        [TextArea] public string description;
        [TextArea] public string controls;
        [Header("Credits")]
        [Tooltip("Credit authors")][TextArea] public string credits;
        [Tooltip("Credit assets and packages used")][TextArea] public string attributions;

        public MinigamePack GetPack() {
            foreach (MinigamePack pack in MinigameManager.instance.allPacks) {
                if(pack.minigames.Contains(this)) {
                    return pack;
                }
            }
            return null;
        }
    }
}