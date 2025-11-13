using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float stamina;
    public float maxStamina;
    public float chargeRate;
    public Image staminaBar;

    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    float speed = 1.0f;
    float sprintSpeed = 1.5f;
    float runCost = 40.0f;
    bool isRunning = false;
    bool isRecharging = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        } 
        else
        {
            m_AudioSource.Stop();
        }

        speed = isRunning ? sprintSpeed : 1.0f;
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    private void Update()
    {
        Run();
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * speed);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0)
            {
                isRunning = true;
                speed = sprintSpeed;
                stamina = Mathf.Clamp(stamina - runCost * Time.deltaTime, 0f, maxStamina);

                staminaBar.fillAmount = stamina / maxStamina;
                StopCoroutine(RechargeStamina());
            }
        }
        else
        {
            speed = 1.0f;
            StartCoroutine(RechargeStamina());
        }
    }

    IEnumerator RechargeStamina()
    {
        if (stamina < maxStamina)
        {
            isRecharging = true;
            yield return new WaitForSeconds(1f);
            stamina = Mathf.Clamp(stamina + chargeRate * Time.deltaTime, 0f, maxStamina);
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }
}
