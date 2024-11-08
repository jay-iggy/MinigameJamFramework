using UnityEngine;

namespace Game.MinigameFramework.Scripts.Framework.Minigames {
    [CreateAssetMenu(fileName = "NewMinigame", menuName = "MinigameInfo")]
    public class MinigameInfo : ScriptableObject {
        public string minigameName;
        public MinigameType minigameType;
        [Tooltip("If you change the name of the scene in the project, you must update this field to refresh internal values.")]
        [SerializeField] public SceneField scene;
        [TextArea] public string description;
        [TextArea] public string controls;
    }

    public enum MinigameType {
        FreeForAll,
        TwoVersusTwo,
        ThreeVersusOne
    }
}