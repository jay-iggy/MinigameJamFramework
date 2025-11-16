using HotPotatoGame;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

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
    private GameObject firepoint;

    public float bounceForcePotato;
    public float bounceForceScarecrow;
    public float bounceHeightImpulse;


    public void setUp(GameObject parent, GameObject bi, Vector3 offset)
    {

        StartDirection();
        firepoint = transform.GetChild(0).gameObject;
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        ObjectBounce objectBounce = other.GetComponent<ObjectBounce>();

        if (!objectBounce.isShot)
        {
            StartCoroutine(ToFirePoint(other.transform, rb, objectBounce));
        }
    }

    IEnumerator ToFirePoint(Transform obj, Rigidbody rb, ObjectBounce objectBounce)
    {
        rb.isKinematic = true;
        objectBounce.isShot = true;

        Vector3 start = obj.position;

        float duration = 0.1f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            obj.position = Vector3.Lerp(start, firepoint.transform.position, t);
            yield return null;
        }

        obj.position = firepoint.transform.position;
        StartCoroutine(LaunchCoolDown(obj,rb, objectBounce));
    }


    private Vector3 LaunchDirection(float bhi)
    {
        Vector3 direction = Vector3.zero;

        switch (order)
        {
            case 0:
                return direction = new Vector3(0, bhi, 1f);
            case 1:
                return direction = new Vector3(1f, bhi, 0f);
            case 2:
                return direction = new Vector3(0f, bhi, -1f);
            case 3:
                return direction = new Vector3(-1f, bhi, 0f);
            default:
                throw new System.Exception("Value error!"); ;
        }
    }

    IEnumerator LaunchCoolDown(Transform obj, Rigidbody rb, ObjectBounce objectBounce)
    {
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(ObjBounceCooldown(objectBounce));
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;

        float bounceForce = 0;
        float newBounceHeightImpulse = 0;

        if (objectBounce.objectType == ObjectBounce.ObjectType.Potato)
        {
            obj.gameObject.GetComponent<ThrowableBehavior>().Drop();
            bounceForce = bounceForcePotato;
            newBounceHeightImpulse = bounceHeightImpulse;
        } else if(objectBounce.objectType == ObjectBounce.ObjectType.Scarecrow)
        {
            bounceForce = bounceForceScarecrow;
            newBounceHeightImpulse = bounceHeightImpulse / 2f;
        }

        obj.gameObject.GetComponent<Rigidbody>().AddForce(LaunchDirection(newBounceHeightImpulse) * bounceForce, ForceMode.Impulse);
    }

    IEnumerator ObjBounceCooldown(ObjectBounce objectBounce)
    {
        float waitTime;

        if(objectBounce.objectType == ObjectBounce.ObjectType.Scarecrow)
        {
            waitTime = 0.4f;
        }
        else
        {
            waitTime = 0.05f;
        }

        yield return new WaitForSeconds(waitTime);
        objectBounce.isShot = false;
    }
}
