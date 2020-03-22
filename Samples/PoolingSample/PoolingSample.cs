using elZach.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace elZach.Samples
{
    public class PoolingSample : MonoBehaviour
    {
        public enum SpawnSamples { Instantiate, Spawn, SpawnAndStart, SpawnAndReset }

        public GameObject prefab;
        Component prefabBehaviour;
        public SpawnSamples spawnType = SpawnSamples.Spawn;
        public float spawnTime = 1f;
        public float radius = 1f;

        public float despawnTime = 4f;
        List<GameObject> spawned = new List<GameObject>();
        List<float> spawnedTime = new List<float>();

        void Start()
        {
            StartCoroutine(SpawnRecurrent(spawnTime));
            prefabBehaviour = prefab.GetComponent<ICallStart>();//new Component[] { prefab.GetComponent<ICallStart>(), prefab.GetComponent<Rigidbody>() };
        }

        private void Update()
        {
            float t = Time.time;
            for (int i = spawned.Count - 1; i >= 0; i--)
            {
                if (t - spawnedTime[i] >= despawnTime)
                {
                    if (spawnType == SpawnSamples.Instantiate)
                        Destroy(spawned[i]);
                    else
                        spawned[i].Despawn();
                    spawned.RemoveAt(i);
                    spawnedTime.RemoveAt(i);
                }
            }
        }

        IEnumerator SpawnRecurrent(float time)
        {
            yield return new WaitForSeconds(time);
            GameObject clone;
            switch (spawnType)
            {
                case SpawnSamples.Instantiate:
                    clone = Instantiate(prefab);
                    break;
                case SpawnSamples.Spawn:
                    clone = prefab.Spawn();
                    break;
                case SpawnSamples.SpawnAndStart:
                    clone = prefab.SpawnAndStart();
                    break;
                case SpawnSamples.SpawnAndReset:
                    clone = prefab.SpawnAndReset(null, prefabBehaviour);
                    break;
                default:
                    clone = Instantiate(prefab);
                    break;
            }

            clone.transform.position = transform.position + Random.insideUnitSphere * radius;
            clone.GetComponent<Rigidbody>().Reset();
            spawned.Add(clone);
            spawnedTime.Add(Time.time);
            StartCoroutine(SpawnRecurrent(spawnTime));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}
