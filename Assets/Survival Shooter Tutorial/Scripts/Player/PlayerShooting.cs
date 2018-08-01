using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // public bool targetAcquired = false;
    public GameObject target;

    int damagePerShot = 100;
    float timeBetweenBullets = 1f;
    float targetRange = 12f;
    float shootRange = 10f;
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
    PlayerControls playerControls;
    PlayerStates previousPlayerState = PlayerStates.None;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
        playerMovement = GetComponentInParent <PlayerMovement> ();
        playerControls = GetComponentInParent <PlayerControls> ();
    }

    void Update ()
    {
        timer += Time.deltaTime;

        bool isDead = false;
        if (target != null) {
            isDead = target.GetComponent <EnemyHealth> ().currentHealth <= 0;
        }

        if (!playerMovement.walking && target == null || isDead) {
            if (AcquireTarget() && playerControls.getState() != PlayerStates.AttackTarget) {
                previousPlayerState = playerControls.getState();
                playerControls.setState(PlayerStates.AttackTarget);
            } else if(previousPlayerState != PlayerStates.None && playerControls.getState() == PlayerStates.AttackTarget) {
                playerControls.setState(previousPlayerState);
                previousPlayerState = PlayerStates.None;
            }
        }
        
        if (target != null && (target.GetComponent<EnemyHealth>()).currentHealth > 0 && timer >= timeBetweenBullets && !playerMovement.walking) {
            Shoot ();
        }

        if (timer >= timeBetweenBullets * effectsDisplayTime) {
            DisableEffects ();
        }
    }

    bool AcquireTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        bool targetWithinRange = false;
        foreach (GameObject enemy in enemies) {
            bool isDead = enemy.GetComponent <EnemyHealth> ().currentHealth <= 0;
            if (!isDead) {
                closestEnemy = closestEnemy ?? enemy;
                float distance = Vector3.Distance(enemy.transform.position, transform.position);
                if (distance < shootRange - 1f && distance < Vector3.Distance(closestEnemy.transform.position, transform.position)) {
                    closestEnemy = enemy;
                    targetWithinRange = true;
                }
            }
        }
        if (targetWithinRange) {
            target = closestEnemy;
        } else {
            target = null;
        }

        return targetWithinRange;
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
        shootRay.direction = target.transform.position - transform.position;

        if (Physics.Raycast (shootRay, out shootHit, shootRange, shootableMask)) {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if (enemyHealth != null) {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            } else {
                target = null;
            }
            gunLine.SetPosition (1, shootHit.point);
        } else {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * shootRange);
        }
    }
}
