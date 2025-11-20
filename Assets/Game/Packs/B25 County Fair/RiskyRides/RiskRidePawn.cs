using Game.MinigameFramework.Scripts.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class RiskRidePawn : Pawn 
{
    public RiskyRidesGameManager.Team team;

    [SerializeField]
    [Tooltip("Player acceleration in m/s^2")]
    float acceleration = 3.0f;
    [SerializeField]
    [Tooltip("Player deacceleration (when no input) in m/s^2")]
    float deacceleration = 3.0f;
    [SerializeField]
    [Tooltip("Player top running speed in m/s")]
    float maxSpeed = 3.0f;
    [SerializeField]
    [Tooltip("The acceleration down due to the gravity while the player is moving up")]
    float gravityUp = 5.0f;
    [SerializeField]
    [Tooltip("The acceleration down due to the gravity while the player is moving down")]
    float gravityDown = 10.0f;
    [SerializeField]
    [Tooltip("Jump impulse in m/s")]
    float jumpStrength = 5.0f;

    [SerializeField]
    [Tooltip("The number of physics ticks for Jump Buffering")]
    byte jumpMemory = 10;
    [SerializeField]
    [Tooltip("The number of physics ticks for Coyote Time")]
    byte groundedMemory = 10;

    [SerializeField]
    [Tooltip("Controls the box in which another player can be punched by this player. Assume the player is facing right")]
    Rect punchBox;
    [SerializeField]
    [Tooltip("Impulse that a punch applies to another player, in m/s. Assume a right facing punch")]
    Vector2 punchImpulse;
    [SerializeField]
    [Tooltip("Player Layermask for punch player detection")]
    LayerMask punchMask;

    Vector2 movementInput = Vector2.zero;
    short jumpFlag = 0;
    bool jumpedLastTick = false;

    short groundedFlag = 0;
    Vector2 groundedNormal = Vector2.zero;

    Vector2 relativeVelocityLast = Vector2.zero;
    GameObject relativeObjectLast = null;

    [SerializeField]
    byte punchCooldownTicks = 35;

    byte punchCooldown;
    bool punchFlag = false;
    bool punchLeft = false;

    bool isFlipped = false;

    Rigidbody2D body;
    Animator animator;
    Transform sprite;
    AudioSource punchSound;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0);
        animator = sprite.GetComponent<Animator>();
        punchSound = GetComponent<AudioSource>();
        UpdatePlayer();
    }

    bool skipFrameCorrections = false;

    void FixedUpdate()
    {

        InertialReferenceFrame frame = GetInertialReferenceFrame();

        Vector2 relativeVelocity = body.velocity - frame.globalVelocity;

        if (relativeObjectLast == frame.referenceObject && frame.isGrounded)
        {
            // We want to "stick" to the object we are on, not get dragged along by it
            body.velocity -= relativeVelocity - relativeVelocityLast;
        }

        relativeVelocity = body.velocity - frame.globalVelocity;

        if (frame.isGrounded && !jumpedLastTick)
        {
            groundedFlag = groundedMemory;
            groundedNormal = (Vector2)frame.surfaceNormal;
        }
        jumpedLastTick = false;

        if (groundedFlag > 0)
        {
            groundedFlag--;

            if ((Mathf.Abs(movementInput.x) > 0 && ((movementInput.x >= 0 && relativeVelocity.x >= 0) || (movementInput.x <= 0 && relativeVelocity.x <= 0))) || !frame.isGrounded) // Even though we have forgivness on grounding, that is for jumping and could feel crappy if you deaccelerate when leaving a surface
            {
                float desiredAcceleration = movementInput.x * (acceleration) * Time.fixedDeltaTime;
                float maxAcceleration = (movementInput.x < 0) ? (-maxSpeed - relativeVelocity.x) : (maxSpeed - relativeVelocity.x);
                if (Mathf.Abs(relativeVelocity.x) > maxSpeed && ((movementInput.x >= 0 && relativeVelocity.x >= 0) || (movementInput.x <= 0 && relativeVelocity.x <= 0)))
                    maxAcceleration = 0;

                body.velocity += new Vector2((Mathf.Abs(desiredAcceleration) <= Mathf.Abs(maxAcceleration)) ? desiredAcceleration : maxAcceleration, ((body.velocity.y < 0) ? -gravityDown : -gravityUp) * Time.fixedDeltaTime);
            }
            else
            {
                float desiredAcceleration = ((relativeVelocity.x > 0) ? -1 : 1) * deacceleration * Time.fixedDeltaTime;
                float maxAcceleration = -relativeVelocity.x; ;

                body.velocity += new Vector2((Mathf.Abs(desiredAcceleration) <= Mathf.Abs(maxAcceleration)) ? desiredAcceleration : maxAcceleration, ((body.velocity.y < 0) ? -gravityDown : -gravityUp) * Time.fixedDeltaTime);
            }

            if (jumpFlag > 0)
            {
                Vector2 jumpNormal = new Vector2(groundedNormal.x, (groundedNormal.y > -0.1f) ? groundedNormal.y + 1f : groundedNormal.y).normalized;
                if (relativeVelocity.y < 0)
                    body.velocity -= new Vector2(0, relativeVelocity.y);
                body.velocity += jumpStrength * jumpNormal;
                groundedFlag = 0;
                jumpFlag = 0;
                relativeVelocityLast = body.velocity;
                relativeObjectLast = null;
                skipFrameCorrections = true;
                jumpedLastTick = true;
            }
        }
        else
        {
            float desiredAcceleration = movementInput.x * acceleration * Time.fixedDeltaTime;
            float maxAcceleration = (movementInput.x < 0) ? (-maxSpeed - relativeVelocity.x) : (maxSpeed - relativeVelocity.x);
            if (Mathf.Abs(relativeVelocity.x) > maxSpeed && ((movementInput.x >= 0 && relativeVelocity.x >= 0) || (movementInput.x <= 0 && relativeVelocity.x <= 0)))
                maxAcceleration = 0;

            body.velocity += new Vector2((Mathf.Abs(desiredAcceleration) <= Mathf.Abs(maxAcceleration)) ? desiredAcceleration : maxAcceleration, ((body.velocity.y < 0) ? -gravityDown : -gravityUp) * Time.fixedDeltaTime);

            if (jumpFlag > 0)
                jumpFlag--;
        }

        if (punchFlag && punchCooldown == 0)
        {
            punchFlag = false;
            Collider2D[] hits = Physics2D.OverlapBoxAll((Vector2)transform.position + (punchLeft ? -punchBox.position : punchBox.position), punchBox.size, 0, punchMask);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform == transform) continue;

                hits[i].GetComponent<RiskRidePawn>().Push(new Vector2(punchLeft ? -punchImpulse.x : punchImpulse.x, punchImpulse.y));
            }
            punchSound.time = 0;
            punchSound.Play();
            punchCooldown = punchCooldownTicks;
        }
        else if (punchCooldown > 0)
        {
            punchCooldown--;
        }
        punchFlag = false;

        if (!skipFrameCorrections)
        {
            Vector2 totalRelativeVelocity = body.velocity - frame.globalVelocity;
            if (frame.referenceObject == null)
            {
                relativeVelocityLast = totalRelativeVelocity;
            }
            else
            {
                Vector2 tangent = new Vector2(((Vector2)frame.surfaceNormal).y, -((Vector2)frame.surfaceNormal).x);
                float tangentRelativeVelocity = Vector2.Dot(totalRelativeVelocity, tangent);
                relativeVelocityLast = tangent * tangentRelativeVelocity;
            }
            relativeObjectLast = frame.referenceObject;
        }

        skipFrameCorrections = false;

        animator.SetBool("IsRunning", Mathf.Abs(relativeVelocity.x) > 0.2f);
        animator.SetBool("IsGrounded", frame.isGrounded);
        isFlipped = (isFlipped) ? relativeVelocity.x < 0.1f : relativeVelocity.x < -0.1f;
        sprite.localScale = (isFlipped) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    public void Push(Vector2 pushVelocity)
    {
        body.velocity += pushVelocity;
        groundedFlag = 0;
        relativeVelocityLast = body.velocity;
        relativeObjectLast = null;
        jumpedLastTick = true;
        skipFrameCorrections = true;
        punchCooldown = punchCooldownTicks;
    }

    public void UpdatePlayer()
    {
        animator.SetBool("IsBlue", team == RiskyRidesGameManager.Team.BLUE);
    }

    protected override void OnActionPressed(InputAction.CallbackContext context)
    {
        if (context.action.name == PawnAction.Move)
        {
            movementInput = context.ReadValue<Vector2>();
            if (movementInput.x != 0) punchLeft = (movementInput.x < 0);
        }

        if (context.action.name == PawnAction.ButtonA)
        {
            jumpFlag = jumpMemory;
        }

        if (context.action.name == PawnAction.ButtonX)
        {
            punchFlag = true;
        }
    }

    struct InertialReferenceFrame
    {
        public Vector2 globalVelocity;  // Velocity of the reference frame in global space
        public Vector2? surfaceNormal;  // Normal of our contact with the surface, pointing from the other object to us
        public bool isGrounded;         // Is contacting another object
        public GameObject referenceObject; // We need to be able to check if our reference is the same object after physics updates

        public InertialReferenceFrame(GameObject reference = null, Vector2? velocity = null)
        {
            globalVelocity = (velocity != null) ? ((Vector2) velocity) : Vector2.zero;
            surfaceNormal = null;
            isGrounded = false;
            referenceObject = reference;
        }

        public InertialReferenceFrame(Vector2 velocity, Vector2 normal, GameObject? reference)
        {
            globalVelocity = velocity;
            surfaceNormal = normal;
            isGrounded = true;
            referenceObject = reference;
        }
    }

    InertialReferenceFrame GetInertialReferenceFrame()
    {
        ContactPoint2D[] contacts = { new ContactPoint2D(), new ContactPoint2D(), new ContactPoint2D(), new ContactPoint2D(), new ContactPoint2D() };
        int numContacts = body.GetContacts(contacts);

        if (numContacts == 0)
        {
            return new InertialReferenceFrame();
        }
        else
        {
            // We are going to choose the contact closest to the bottom center of the player, where their feet are
            int best = 0;

            for (int i = 1; i < numContacts; i++)
            {
                if (contacts[i].point.y < contacts[best].point.y)
                    best = i;
                else if (contacts[i].point.y == contacts[best].point.y && Mathf.Abs(contacts[i].point.x) < Mathf.Abs(contacts[best].point.x))
                    best = i;
            }

            return new InertialReferenceFrame(contacts[best].rigidbody.velocity, contacts[best].normal, contacts[best].rigidbody.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2) transform.position + punchBox.position, punchBox.size);
        Gizmos.DrawWireCube((Vector2) transform.position - punchBox.position, punchBox.size);
    }
}