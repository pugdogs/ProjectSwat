using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
    public bool targetAcquired = false;
    public GameObject target;

    float timer;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;
    PlayerMovement playerMovement;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
        playerMovement = GetComponentInParent <PlayerMovement> ();
    }

    void Update ()
    {
        timer += Time.deltaTime;

        if (!playerMovement.walking && target == null) {
            AcquireTarget();
        }
        
        if ((target!=null || Input.GetButton ("Fire1")) && timer >= timeBetweenBullets) {
            Debug.Log(target);
            Shoot ();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime) {
            DisableEffects ();
        }
    }

    void AcquireTarget() {

    }

    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
                target = shootHit.collider.gameObject;
            } else {
                target = null;
            }
            gunLine.SetPosition (1, shootHit.point);
        } else {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
