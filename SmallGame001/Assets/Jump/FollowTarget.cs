using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XJump
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;

        private Vector3 velocity = Vector3.zero;

        private void Start()
        {
            Staying();
        }

        public void Staying()
        {
            transform.localPosition = new Vector3(0, 0, -10);
        }

        public void GameOver()
        {
            Staying();
        }

        void Update()
        {
            if (target)
            {
                Vector3 pos = Camera.main.WorldToScreenPoint(target.position);

                Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, pos.z));

                Vector3 des = transform.position + delta;

                des.x = 0;

                if (des.y > transform.position.y)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, des, ref velocity, 0.5f);
                }
            }
        }
    }
}