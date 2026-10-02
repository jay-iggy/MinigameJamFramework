using UnityEngine;
using UnityEditor;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using System.Runtime.CompilerServices;
using NUnit.Framework.Constraints;

namespace Editor {
    public static class ContextMenuItems {

        private const string FOLDER_PATH = "Assets/Game/Framework/";
        private const string DEBUG_PACK_PATH = "Assets/Game/Packs/DebugPack.asset";
        private const string MINIGAME_MANAGER_PATH = "Assets/Game/Framework/MinigameManager.prefab";


        #region Add Minigame Info to Pack
            [MenuItem("Assets/Add MinigameInfo to Pack/Debug Pack",false,4)]
            public static void PackDemo() {
                AddToPack(DEBUG_PACK_PATH);
            }
            [MenuItem("Assets/Add MinigameInfo to Pack/Debug Pack", true, 4)] 
            public static bool PackValidate() {
                return ValidateAddToPack(DEBUG_PACK_PATH);
            }
        #endregion

        [MenuItem("Assets/Clear Pack/Debug Pack", false, 6)]
        public static void ClearDemoPack() {
            ClearPack(DEBUG_PACK_PATH);
        }
        [MenuItem("Assets/Clear Pack/Debug Pack", true, 6)]
        public static bool ValidateClearDemoPack() {
            return ValidateClearPack(DEBUG_PACK_PATH);
        }

        #region Add Minigame Info to Pack Functions
        private static void AddToPack(string packPath) {
                if (!IsAssetPathValid(packPath)) {
                    Debug.LogError($"Failed to find pack at '{packPath}'.");
                    return;
                }
                MinigamePack pack = AssetDatabase.LoadAssetAtPath<MinigamePack>(packPath);
                MinigameInfo minigame = Selection.activeObject as MinigameInfo;
                if (pack != null && minigame != null) {
                    pack.minigames.Add(minigame);
                    Selection.activeObject = pack;
                }
            }
            private static bool ValidateAddToPack(string packPath) {
                if (!IsAssetPathValid(packPath)) {
                    Debug.LogError($"Failed to find pack at '{packPath}'.");
                    return false;
                }
                MinigamePack pack = AssetDatabase.LoadAssetAtPath<MinigamePack>(packPath);
                MinigameInfo minigame = Selection.activeObject as MinigameInfo;
                bool isDuplicate = false;
                if (pack != null && minigame != null) {
                    if (pack.minigames.Contains(minigame)) {
                        isDuplicate = true;
                    }
                }
                return Selection.activeObject is MinigameInfo && !isDuplicate;
            }
            private static void ClearPack(string packPath) {
                if (!IsAssetPathValid(packPath)) {
                    Debug.LogError($"Failed to find pack at '{packPath}'.");
                    return;
                }
                MinigamePack pack = AssetDatabase.LoadAssetAtPath<MinigamePack>(packPath);
                if (pack != null) {
                    pack.minigames.Clear();
                }
            }
            private static bool ValidateClearPack(string packPath) {
                if (!IsAssetPathValid(packPath)) {
                    Debug.LogError($"Failed to find pack at '{packPath}'.");
                    return false;
                }
                MinigamePack pack = AssetDatabase.LoadAssetAtPath<MinigamePack>(packPath);
                return pack!=null && pack.minigames.Count>0;

            }
        #endregion

        [MenuItem("Assets/Set As Debug Minigame",false,4)]
        public static void SetAsDebugMinigame() {
            if (!IsAssetPathValid(MINIGAME_MANAGER_PATH)) {
                Debug.LogError($"Failed to find Minigame Manager at '{MINIGAME_MANAGER_PATH}'.");
                return;
            }
            MinigameManager manager = AssetDatabase.LoadAssetAtPath<MinigameManager>(MINIGAME_MANAGER_PATH);
            MinigameInfo minigame = Selection.activeObject as MinigameInfo;
            if (manager != null && minigame != null) {
                manager.debugMinigame = minigame;
                Selection.activeObject = manager;
            }
        }
        [MenuItem("Assets/Set As Debug Minigame", true, 4)] 
        public static bool ValidateDebugMinigame() {
            if (!IsAssetPathValid(MINIGAME_MANAGER_PATH)) {
                Debug.LogError($"Failed to find Minigame Manager at '{MINIGAME_MANAGER_PATH}'.");
                return false;
            }
            MinigameManager manager = AssetDatabase.LoadAssetAtPath<MinigameManager>(MINIGAME_MANAGER_PATH);
            MinigameInfo minigame = Selection.activeObject as MinigameInfo;
            bool isDuplicate = false;
            if (manager != null && minigame != null) {
                if(manager.debugMinigame == minigame) {
                    isDuplicate = true;
                }
            }

            return Selection.activeObject is MinigameInfo && !isDuplicate;
        }


        [MenuItem("Assets/Clear Debug Minigame", false, 5)]
        public static void ClearDebugMinigame() {
            if (!IsAssetPathValid(MINIGAME_MANAGER_PATH)) {
                Debug.LogError($"Failed to find Minigame Manager at '{MINIGAME_MANAGER_PATH}'.");
                return;
            }
            MinigameManager manager = AssetDatabase.LoadAssetAtPath<MinigameManager>(MINIGAME_MANAGER_PATH);
            if (manager != null ) {
                manager.debugMinigame = null;
            }
        }
        [MenuItem("Assets/Clear Debug Minigame", true, 5)]
        public static bool ValidateClearDebugMinigame() {
            if (!IsAssetPathValid(MINIGAME_MANAGER_PATH)) {
                Debug.LogError($"Failed to find Minigame Manager at '{MINIGAME_MANAGER_PATH}'.");
                return false;
            }
            MinigameManager manager = AssetDatabase.LoadAssetAtPath<MinigameManager>(MINIGAME_MANAGER_PATH);

            return manager.debugMinigame!=null;
        }




        public static bool IsAssetPathValid(string projectRelativePath) {
            string guid = AssetDatabase.AssetPathToGUID(projectRelativePath, AssetPathToGUIDOptions.OnlyExistingAssets);
            return !string.IsNullOrEmpty(guid);
        }
    }
}