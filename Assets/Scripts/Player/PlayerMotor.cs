using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private float crouchTimer = 1;
    private bool crouching = false;
    private bool lerpCrouch = false;
    private bool sprinting = false;

    public AudioSource audioSource;
    public AudioClip[] audioClipArray;

    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;

    [SerializeField] GameObject pauseMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource.PlayOneShot(RandomClip());
    }

    AudioClip RandomClip()
    {
        return audioClipArray[Random.Range(0, audioClipArray.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if(crouching) {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            } else {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if(p > 1) {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePauseMenu();
        }
    }

    public void Crouch ()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }
    public void Sprint()
    {
        sprinting = !sprinting;
        if(sprinting) {
            speed = speed * 1.6f;
        } else {
            speed = speed / 1.6f;
        }
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0) {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if(isGrounded) {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void SpeedBoost()
    {
        speed = speed * 1.2f;

        // Find and destroy the clone object
        GameObject cloneObject = GameObject.FindGameObjectWithTag("SpeedBoostClone");
        Destroy(cloneObject);
    }

    public void TogglePauseMenu()
    {
        bool isPaused = pauseMenu.activeSelf;
        pauseMenu.SetActive(!isPaused);
        Time.timeScale = isPaused ? 1f : 0f;
    }
}
