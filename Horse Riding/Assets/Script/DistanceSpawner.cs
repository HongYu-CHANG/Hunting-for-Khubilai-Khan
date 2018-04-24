using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZenvaVR;

namespace ZenvaVR
{
    [RequireComponent(typeof(ObjectPool))]
    public class DistanceSpawner : MonoBehaviour
    {
        // reference game object
        public Transform reference;

        // minimim distance to spawn an object
        public float genDistance;

        // min scale
        public float minScale = 1;

        // max scale
        public float maxScale = 1;

        // object pool component
        ObjectPool pool;

        // newly generated object
        GameObject newObj;

		private bool isGenerating;

		// Use this for initialization
		void Awake()
        {
			//get the object pool component
			pool = GetComponent<ObjectPool>();

            //init pool
            pool.InitPool();

            //initial population of object
            while (Vector3.Distance(reference.position, transform.position) <= genDistance)
            {
                HandleSpawning();
            }
			isGenerating = false;
        }

        // Update is called once per frame
        void Update()
        {
			HandleSpawning();
        }

        void HandleSpawning()
        {
			if (!isGenerating)
				StartCoroutine(RandomSpawn());
		}

		IEnumerator RandomSpawn()
		{
			isGenerating = true;
			UnityEngine.Random.InitState(System.Guid.NewGuid().GetHashCode());
			yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 5.0f));
			isGenerating = false;
			Spawn();
		}

        // spawn a new object
        void Spawn()
        {
            //get an object from the pool
            newObj = pool.GetObj();

			//set position
			newObj.transform.position = new Vector3(transform.position.x + UnityEngine.Random.Range(-50.0f, 50.0f), transform.position.y, transform.position.z + UnityEngine.Random.Range(-50.0f, 50.0f));
			//generate a random scale number
			float scale = UnityEngine.Random.Range(minScale, maxScale);

            //scale object
            newObj.transform.localScale = Vector3.one * scale;
        }
    }
}
