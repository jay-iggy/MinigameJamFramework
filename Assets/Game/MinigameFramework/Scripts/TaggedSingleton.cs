using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaggedSingleton : MonoBehaviour {
    private static Dictionary<string, TaggedSingleton> instances = new Dictionary<string, TaggedSingleton>();

    [SerializeField] private string tagIdentifier;

    public static TaggedSingleton GetInstance(string tag) {
        if (instances.ContainsKey(tag)) {
            return instances[tag];
        }
        return null;
    }

    private void Awake() {
        if (!instances.ContainsKey(tagIdentifier)) {
            instances[tagIdentifier] = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
