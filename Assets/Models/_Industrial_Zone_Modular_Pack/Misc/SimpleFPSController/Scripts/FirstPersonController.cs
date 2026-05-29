using System;
using UnityEngine;

/**
 * 
 * This controller is based on the Unity's FirstPersonController included in the Standard Assets !
 * It is just a simple controller with most stuff removed !
 * 
 */

namespace SimpleFPSController
{
    
    [RequireComponent(typeof(CharacterController)), RequireComponent(typeof(AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {

        [SerializeField] private MouseLook m_MouseLook;

        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;

        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
            m_MouseLook.Init(transform, m_Camera.transform);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {

            // Rotate View
            m_MouseLook.LookRotation(transform, m_Camera.transform);

            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = Input.GetKeyDown(KeyCode.Space);
            }

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayLandingSound()
        {
            // Do not play jump sound if it is not specified
            if (!m_LandSound) return;
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + 0.5f;
        }

        /// <summary>
        /// 
        /// </summary>
        private void LateUpdate()
        {
            float speed;
            GetInput(out speed);

            // Always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // Get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast( transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x * speed;
            m_MoveDir.z = desiredMove.z * speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.deltaTime;
            }

            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.deltaTime);

            ProgressStepCycle(speed);

            m_MouseLook.UpdateCursorLock();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayJumpSound()
        {
            // Do not play jump sound if one is not specified
            if (!m_JumpSound) return;
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                             Time.deltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }

        /// <summary>
        /// 
        /// </summary>
        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }

            if (m_FootstepSounds != null && m_FootstepSounds.Length > 0)
            {
                // Pick & Play a random footstep sound from the array,
                // excluding sound at index 0
                int n = UnityEngine.Random.Range(1, m_FootstepSounds.Length);
                m_AudioSource.clip = m_FootstepSounds[n];
                m_AudioSource.PlayOneShot(m_AudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                m_FootstepSounds[n] = m_FootstepSounds[0];
                m_FootstepSounds[0] = m_AudioSource.clip;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="speed"></param>
        private void GetInput(out float speed)
        {
            // Read input

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // On standalone builds, walk/run speed is modified by a key press.
            // Keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);

            // Set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // Normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1f)
            {
                m_Input.Normalize();
            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="hit"></param>
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            // Dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
