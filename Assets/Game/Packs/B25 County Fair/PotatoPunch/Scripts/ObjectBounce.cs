using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BouncePad;

public class ObjectBounce : MonoBehaviour
{
    [HideInInspector] public bool isShot = false;

    public enum ObjectType
    {
        Scarecrow,
        Potato
    }

    public ObjectType objectType;
}
