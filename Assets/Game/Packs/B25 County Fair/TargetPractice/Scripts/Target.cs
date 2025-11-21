using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterMinigame {
    [RequireComponent(typeof(HingeJoint))]
    public class Target : MonoBehaviour {

        private static List<Target> _targets = new List<Target>();

        private bool m_scorable = false;

        public bool Scorable {
            get => m_scorable;
            set {
                m_scorable = value;
                m_joint.useSpring = value;
            }
        }

        private HingeJoint m_joint;

        private void Awake() {
            m_joint = GetComponent<HingeJoint>();
            _targets.Add(this);
        }

        void Start() {
            m_joint.useSpring = false;
        }

        private void OnDestroy() {
            _targets.Remove(this);
        }

        public static void ActivateTargets(int count) {
            List<Target> unset = _targets.FindAll(val => !val.Scorable);

            if (unset.Count == 0)
                return;

            if (unset.Count <= count) {
                foreach (var item in unset) {
                    item.Scorable = true;
                }
            } else {
                for (int i = 0; i < count; i++) {
                    int index = Random.Range(0, unset.Count);
                    unset[index].Scorable = true;
                    unset.RemoveAt(index);

                    if (unset.Count == 0)
                        return;
                }
            }
        }
    }
}
