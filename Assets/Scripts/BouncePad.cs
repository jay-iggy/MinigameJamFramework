using HotPotatoGame;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class BouncePad : MonoBehaviour
{

    public enum BounceDirction
    {
        Up,
        Right,
        Down,
        Left
    }

    private int order;
    public BounceDirction dir => (BounceDirction)order;

    private GameObject BounceIndicator;
    private GameObject bounceIndicatorManager;


    public void setUp(GameObject parent, GameObject bi, Vector3 offset)
    {

        StartDirection();
        bounceIndicatorManager = parent;
        BounceIndicator = Instantiate(bi, new Vector3(transform.position.x, 0, transform.position.z) + offset, Quaternion.identity);
        UpdateIndicator();
    }

    public void StartDirection()
    {
        order = Random.Range(0, 4);
        Debug.Log(dir);
    }
    
    public void ChangeDirection()
    {
        if (order + 1 >= 4)
        {
            order = 0;
        }
        else
        {
            order++;
        }

        UpdateIndicator();
    }

    private int bounceIndicatorRotation()
    {
        switch (order)
        {
            case 0:
              return 90;
            case 1:
                return 0;
            case 2:
                return 270;
            case 3: 
                return 180;
            default:
                throw new System.Exception("Value error!"); ;
        }
    }

    private void UpdateIndicator()
    {
        Vector3 rot = BounceIndicator.transform.GetChild(0).localEulerAngles;
        rot.z = bounceIndicatorRotation();
        BounceIndicator.transform.GetChild(0).localEulerAngles = rot;
        BounceIndicator.transform.SetParent(bounceIndicatorManager.transform, true);
    }
}
