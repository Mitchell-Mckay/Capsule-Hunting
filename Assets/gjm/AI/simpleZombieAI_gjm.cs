using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleZombieAI_gjm : MonoBehaviour
{
    [SerializeField] float damage;
    float lastAttackTime = 0;
    float attackCooldown = 2;

    [SerializeField] float stoppingDistance;

    public Transform player;
    
    public Rigidbody attractor;

    public LayerMask Ground;

    public LayerMask enemy;

    bool isGrounded = true;
    public Transform attractorGroundCheck_gjm;

    public float attractorSpeed = 5;

    Vector3 playerPosition;
    Vector3 attractorPosition;

    Vector3 x;
    Vector3 xNorm;

    public float tht = 0f;

    float range = 1000;

    public bool enemyToPlayerRayStatus = false;

    Vector3 targetPosition;

    public bool rayHitEnemy;

    public bool isTouchingWall;

    public LayerMask wallLayer_gjm;
    

    public Transform wallCheckTransform;
    public float wallTouchBuffer_gjm = 0.2f;

    public Transform dummaryVar2;

    public float noiseParameter;

    public bool isTouchingStair = false;
    public bool isTouchingEnemy = false;

    public Vector3 computedVelocity;


    public LayerMask includeLayersInNavRaycast;

    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    public float timeBetweenYells = 10f;
    float timer;

    void getEnemyToPlayerPathStatus()
    {
        RaycastHit rayHit = new RaycastHit();
        Ray ray = new Ray(transform.position, player.position - transform.position);
        if (Physics.Raycast(ray, out rayHit, 1000, includeLayersInNavRaycast))
        {

            enemyToPlayerRayStatus = rayHit.transform == player;

            rayHitEnemy = rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy");

        }
    }


    void followTarget()
    {
        attractor.isKinematic = false;
        // * Get the player and attractor positions
        targetPosition = player.position;
        attractorPosition = attractor.position;

        // * Compute the vector difference x = a-b
        x = targetPosition - attractorPosition;

        // * Force AI to grounded path to player!!!
        x.y = 0f;

        // * Compute the normalized vector x
        xNorm = x / x.magnitude;
        computedVelocity = xNorm * attractorSpeed;
        computedVelocity.y = attractor.velocity.y;

        // * Set the attractor velocity
        attractor.velocity = computedVelocity;

        // * Check to see if stuck on wall
        wallCheck_gjm();

    }


    void wallCheck_gjm()
        {
            isTouchingWall  = Physics.CheckSphere(wallCheckTransform.position, wallTouchBuffer_gjm, wallLayer_gjm);
            isTouchingStair = Physics.CheckSphere(wallCheckTransform.position, wallTouchBuffer_gjm, Ground);
            

            if(isTouchingWall || rayHitEnemy)
            {
                attractor.velocity = attractor.velocity + new Vector3 (Random.Range(0.0f,noiseParameter*10f),0f,Random.Range(0.0f,noiseParameter*10f));
            }
            
            if(isTouchingStair)
            {
                attractor.velocity = attractor.velocity + new Vector3 (0f,Random.Range(noiseParameter*1.0f,noiseParameter*5f),0f);
            }

        }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        float dist = Vector3.Distance(transform.position, player.transform.position);
        if(dist < stoppingDistance)
        {
            StopEnemy();
            if(Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                player.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
        else
        {
            followTarget();
        }

        timer += Time.deltaTime;
        if (timer > timeBetweenYells)
        {
            audioSource.PlayOneShot(RandomClip());
            timer = 0;
        }
    }

    private void StopEnemy()
    {
        attractor.isKinematic = true;
    }

    AudioClip RandomClip()
    {
        return audioClipArray[Random.Range(0, audioClipArray.Length-1)];
    }
}
