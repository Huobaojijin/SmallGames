using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XJump
{
    public class PlayerController : MonoBehaviour
    {
        public Camera _camera;

        Rigidbody2D rigidbody2;

        private bool canControl = false;

        float speed = 0;

        bool isUp = false;

        private float originHeight = -6;
        private float maxHeight = 0;
        // Use this for initialization
        void Start()
        {
            rigidbody2 = GetComponent<Rigidbody2D>();
            rigidbody2.simulated = false;
            maxHeight = originHeight;
        }

        // Update is called once per frame
        void Update()
        {
            if (!canControl) return;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                speed = -3;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                speed = 0;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                speed = 3;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                speed = 0;
            }

            rigidbody2.velocity = new Vector3(speed, rigidbody2.velocity.y, 0);

            isUp = rigidbody2.velocity.y > 0;

            if (!isUp)
            {
                float bottomBorder = _camera.ViewportToWorldPoint(new Vector3(0, 0)).y;//相机底部边界，本项目中值为：高度y - 视野size
                if (transform.position.y <= bottomBorder)
                {
                    Dead();
                }
            }
            else
            {
                maxHeight = Mathf.Max(transform.localPosition.y, maxHeight);
                GameController.instance.TotalScores(maxHeight - originHeight);
            }
        }

        public void PlayerStaying()
        {
            transform.position = new Vector3(0, originHeight, 0);
            maxHeight = originHeight;
        }

        public void PlayerStart()
        {
            canControl = true;
            rigidbody2.simulated = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isUp) return;

            rigidbody2.velocity = new Vector3(rigidbody2.velocity.x, 10, 0);
        }

        private void Dead()
        {
            canControl = false;
            rigidbody2.simulated = false;
            GameController.instance.GameOver();
        }
    }
}