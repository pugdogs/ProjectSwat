using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public bool walking = false;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    Vector3 lastFloorHitPoint;
    PlayerShooting playerShooting;

    void Awake ()
    {
        floorMask = LayerMask.GetMask ("Floor");
        anim = GetComponent<Animator> ();
        playerRigidbody = GetComponent<Rigidbody> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
    }

    void FixedUpdate ()
    {
        if (Input.GetButton ("Fire2")) {
            Vector3 inputMousePosition = Input.mousePosition;
            Ray camRay = Camera.main.ScreenPointToRay (inputMousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
                lastFloorHitPoint = floorHit.point;
            }
        }

        float h = 0f;
        float v = 0f;
        if (lastFloorHitPoint != null) {
            Vector3 playerToMouse = lastFloorHitPoint - transform.position;
            if (playerToMouse.x > 0.1 || playerToMouse.x < -0.1) {
                h = playerToMouse.x;
            }
            if (playerToMouse.z > 0.1 || playerToMouse.z < -0.1) {
                v = playerToMouse.z;
            }
            
            walking = h != 0f || v != 0f;

            if (walking) {
                Vector3 movement = Move (h, v);
                Turning(movement);
            }
        }

        if (playerShooting.target != null) {
            Vector3 shootPos = playerShooting.target.transform.position - transform.position;
            shootPos.y = 0;
            Turning(shootPos);
        }
        Animating (walking);
    }

    Vector3 Move (float h, float v)
    {
        movement.Set (h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition (transform.position + movement);
        
        return movement;
    }

    void Turning (Vector3 movement)
    {
        Quaternion newRotation = Quaternion.LookRotation (movement);
        playerRigidbody.MoveRotation (newRotation);
    }

    void Animating (bool walking)
    {
        anim.SetBool ("IsWalking", walking);
    }
}
