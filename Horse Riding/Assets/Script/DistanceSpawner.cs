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

        // direction of the spawner
        //public Vector3 direction;

        // min gap
        //public float minGap;

        // max gap
        //public float maxGap;

        // min scale
        public float minScale = 1;

        // max scale
        public float maxScale = 1;

        // spawning / disapp time
        //public float timeStep = 1;

        // object pool component
        ObjectPool pool;

        // newly generated object
        GameObject newObj;

		private bool isGenerating;

		// Use this for initialization
		void Awake()
        {
			//horse = GameObject.FindWithTag("horse");
			
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
			//execute spawning and hiding only at certain frequency
			//InvokeRepeating("HandleSpawning", 0, timeStep);
            //InvokeRepeating("HandleHiding", 0, timeStep + 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
			HandleSpawning();
            //HandleHiding();
        }

        void HandleSpawning()
        {
			if (!isGenerating)
				StartCoroutine(RandomSpawn());

			// Check distance
			/*if (Vector3.Distance(reference.position, transform.position) <= genDistance)
            {
                // Spawn object
                

                // Reposition distance spawner
                //Reposition();
            }*/
		}

		IEnumerator RandomSpawn()
		{
			isGenerating = true;
			UnityEngine.Random.InitState(System.Guid.NewGuid().GetHashCode());
			yield return new WaitForSeconds(UnityEngine.Random.Range(1.0f, 5.0f));
			isGenerating = false;
			Spawn();

		}

		//handle deactivation of objects if they are too far
		/*void HandleHiding()
        {
            // get the active objects of the pool
            List<GameObject> actives = pool.GetAllActive();

            // go through all of them
            for(int i = 0; i < actives.Count; i++)
            {
                // check distance
                if(Vector3.Distance(reference.position, actives[i].transform.position) > genDistance)
                {
                    // deactivate them
                    //actives[i].SetActive(false);
                }
            }
        }*/

		// reposition the spawner to it's next location
		/*void Reposition()
        {
            // move: 1) size of the new object 2) gap

            // gap
            float gap = UnityEngine.Random.Range(minGap, maxGap);

            // size of the model
            float size = 0;

            //get renderer of the object
            if(newObj.GetComponent<Renderer>() != null)
            {
                Vector3 dirFilter = Vector3.Scale(newObj.GetComponent<Renderer>().bounds.size, direction);
                size = Mathf.Max(dirFilter.x, dirFilter.y, dirFilter.z);
            }
            //get the renderer of the child
            else if(newObj.GetComponentInChildren<Renderer>())
            {
                Vector3 dirFilter = Vector3.Scale(newObj.GetComponentInChildren<Renderer>().bounds.size, direction);
                size = Mathf.Max(dirFilter.x, dirFilter.y, dirFilter.z);
            }

            // total distance we have to move it
            float total = gap + size;

            // repositioning
            transform.Translate(direction * total, Space.World);
            
        }
		*/
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