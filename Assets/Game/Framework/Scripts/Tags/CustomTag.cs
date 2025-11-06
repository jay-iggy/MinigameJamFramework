using System.Collections.Generic;
using UnityEngine;

// created by Xarbrough on the Unity Forums
// https://discussions.unity.com/t/multiple-tags-for-one-gameobject/203921/4

namespace Game.MinigameFramework.Scripts.Tags {
    public class CustomTag : MonoBehaviour 
    {
        [SerializeField]
        private List<string> tags = new List<string>();
	
        public bool HasTag(string tag)
        {
            return tags.Contains(tag);
        }
	
        public IEnumerable<string> GetTags()
        {
            return tags;
        }
	
        public void Rename(int index, string tagName)
        {
            tags[index] = tagName;
        }

        public void Remove(string tagName) {
            tags.Remove(tagName);
        }

        public void Add(string tagName) {
            tags.Add(tagName);
        }
	
        public string GetAtIndex(int index)
        {
            return tags[index];
        }
	
        public int Count
        {
            get { return tags.Count; }
        }
    }
}