using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HummerController : MonoBehaviour
{
    public BoxCollider hitbox;
    public GameObject highlight;
    private IcecubeController currentIce;
    private IcebreakerPawn currentPenguin;
    public GameObject iceEffect;
    public GameObject iceEffectSpawn;
    void Awake()
    {

    }

    private void Update()
    {
        IcecubeController ice = null;
        currentPenguin = null;
        float targetDist = float.PositiveInfinity;
        foreach (Collider col in Physics.OverlapBox(hitbox.transform.position, hitbox.size * 2))
        {
            if (col.gameObject.GetComponent<IcebreakerPawn>() != null && Vector3.Distance(transform.position, col.gameObject.transform.position) < targetDist) {
                currentPenguin = col.gameObject.GetComponent<IcebreakerPawn>();
                targetDist = Vector3.Distance(transform.position, col.transform.position);
            } else if (col.gameObject.GetComponent<IcecubeController>() != null && Vector3.Distance(transform.position, col.transform.position) < targetDist)
            {
                ice = col.gameObject.GetComponent<IcecubeController>();
                targetDist = Vector3.Distance(transform.position, col.transform.position);
            }
        }
        currentIce = ice;
        if (ice != null)
        {
            highlight.SetActive(true);
            highlight.transform.position = ice.transform.position;
        }
        else
        {
            highlight.SetActive(false);
        }
    }

    public void SmashIce()
    {
        if (currentPenguin != null && !currentPenguin.GetStunned()) {
            StartCoroutine(currentPenguin.Stun());
        } else if (currentIce != null) {
            currentIce.Smash();
            Instantiate(iceEffect, currentIce.transform.position + new Vector3(0f, 0.25f, 0f), iceEffect.transform.rotation);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(hitbox.transform.position, hitbox.size * 2);
    }
}
