using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YkinikY
{
    public class PlayerController : MonoBehaviour
    {
        [Header("(c) Ykiniky")]
        [Header("Movement")]
        public bool canMove = true;
        public bool canJump = true;
        public float velocity = 1;
        public float jumpAmount = 6;
        public Transform groundChecker;
        public LayerMask groundMask;
        public float radius = 0.2f;

        [Header("Camera")]
        public PlayerCameraFollow_ykiniky playerCameraFollow;
        public Vector2 lastCheckpoint;

        private Rigidbody2D rb;
        private bool touchesGround;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        // FixedUpdate handles physics
        private void FixedUpdate()
        {
            touchesGround = Physics2D.OverlapCircle(groundChecker.position, radius, groundMask);

            if (canMove)
            {
                float inputMovement = Input.GetAxis("Horizontal");
                rb.linearVelocity = new Vector2(inputMovement * velocity * 5, rb.linearVelocity.y);
            }
        }

        // Update handles movement and camera logic
        void Update()
        {
            if (canMove)
                MovementUpdate();

            if (playerCameraFollow != null)
            {
                if (transform.position.x > 0) playerCameraFollow.FollowX();
                else playerCameraFollow.DontFollowX();

                if (transform.position.y > 0) playerCameraFollow.FollowY();
                else playerCameraFollow.DontFollowY();
            }
        }

        private void MovementUpdate()
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.position += 5 * Time.deltaTime * velocity * Vector3.left;
                GetComponent<SpriteRenderer>().flipX = true;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.position += 5 * Time.deltaTime * velocity * Vector3.right;
                GetComponent<SpriteRenderer>().flipX = false;
            }

            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && touchesGround && canJump)
            {
                canJump = false;
                rb.linearVelocity = Vector2.up * jumpAmount;
            }
        }

        public void GameOver()
        {
            canMove = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            canJump = true;

            if (collision.gameObject.name == "PlayerSlower")
                BecomeSlow();
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.name == "PlayerSlower")
                BecomeNormal();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Checkpoint")
                lastCheckpoint = transform.position;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.name == "Elevator")
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 6);

            if (collision.name == "Down_elevator")
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -4);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.name == "Nastro trasportatore s")
                rb.AddForce(new Vector3(-20, rb.linearVelocity.y));

            if (collision.gameObject.name == "Nastro trasportatore d")
                rb.AddForce(new Vector3(20, rb.linearVelocity.y));
        }

        public void TeleportPlayerX(float playerX)
        {
            transform.position = new Vector2(playerX, transform.position.y);
        }

        public void TeleportPlayerY(float playerY)
        {
            transform.position = new Vector2(transform.position.x, playerY);
        }

        public void BecomeSlow()
        {
            velocity = 0.37f;
        }

        public void BecomeNormal()
        {
            velocity = 1f;
        }
    }
}
