using Game.MinigameFramework.Scripts.Framework.Minigames;
using UnityEditor;
using UnityEngine;

public class PackAutomation : AssetModificationProcessor{
    private static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions options) {
        MinigamePack pack = AssetDatabase.LoadAssetAtPath<MinigamePack>(assetPath);
        if(pack!=null) {
            MinigameManager manager = AssetDatabase.LoadAssetAtPath<MinigameManager>("Assets/Game/Framework/MinigameManager.prefab");
            if (manager != null) {
                Debug.Log($"Deleting Minigame Pack: Removing references from Minigame Manager.");
                manager.allPacks.Remove(pack);
                manager.activePacks.Remove(pack);
            }
        }
        return AssetDeleteResult.DidNotDelete;
    }
}
