using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XJump
{
    public class FloorController : MonoBehaviour
    {
        public GameObject floorPrefab;

        public float maxInterval;
        public float minInterval;

        private bool isPlaying = true;

        public void Staying()
        {
            lastSpawnHeight = -7.8f;
            transform.GetChild(0).gameObject.SetActive(true);
        }

        public void RandomSpawn()
        {
            StartCoroutine("Spawn");
        }

        public void Stop()
        {
            StopCoroutine("Spawn");

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public void Pause(bool pause)
        {
            isPlaying = !pause;
        }

        private float lastSpawnHeight = -7.8f;
        private float nextSpawnHeight = 0;
        private IEnumerator Spawn()
        {
            while (isPlaying)
            {
                nextSpawnHeight = lastSpawnHeight + Random.Range(minInterval, maxInterval);

                int num = Random.value < 0.7f ? 1 : Random.Range(0, 4);
                for (int i = 0; i < num; i++)
                {
                    Instantiate(floorPrefab, new Vector3(Random.Range(-3.3f, 3.3f), nextSpawnHeight, 0), Quaternion.identity).
                        transform.parent = transform;
                }

                lastSpawnHeight = nextSpawnHeight;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}