using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZenvaVR
{
    public class ObjectPool : MonoBehaviour {

        //prefab that the pool will use
        public List<GameObject> poolPrefab;

        //collection
        private List<GameObject> pooledObjects;

        //init pool
        void Awake()
        {
            // if the pool has already been init, don't init again
            if(pooledObjects == null)
            {
                InitPool();
            }
        }

        //init pool
        public void InitPool()
        {
            //init list
            pooledObjects = new List<GameObject>();

			CreateObj();
		}

        //create a new object
        GameObject CreateObj()
        {
			UnityEngine.Random.InitState(System.Guid.NewGuid().GetHashCode());
			// create a new object
			GameObject newObj = Instantiate(poolPrefab[selectAnimal(UnityEngine.Random.Range(0, 71))]);

            // set this new object to inactive
            newObj.SetActive(false);

            // add it to the list
            pooledObjects.Add(newObj);

            return newObj;
        }

        // retrieve an object from the pool
        public GameObject GetObj()
        {
			// search our list for an inactive object
            for(int i = 0; i < pooledObjects.Count; i++)
			{

				if (pooledObjects[i] == null)
				{
					pooledObjects.RemoveAt(i);
					continue;
				}

				// if we find an inactive object
                if(!pooledObjects[i].activeInHierarchy)
                {
                    //enable it (set it to active)
                    pooledObjects[i].SetActive(true);

                    // return that object
                    return pooledObjects[i];
                }
            }

            // increase our pool (create a new object)
            GameObject newObj = CreateObj();

            // enable that new object
            newObj.SetActive(true);

            // return that object
            return newObj;
        }

        // get all active objects
        public List<GameObject> GetAllActive()
        {
            List<GameObject> activeObjs;
            activeObjs = new List<GameObject>();

            // search our list for active objects
            for (int i = 0; i < pooledObjects.Count; i++)
            {
                // if we find an active object
                if (pooledObjects[i].activeInHierarchy)
                {
                    activeObjs.Add(pooledObjects[i]);
                }
            }

            return activeObjs;
        }

		private int selectAnimal(int parameter)
		{
			//Debug.Log(parameter);
			if (parameter <= 10)//0~10 Rabbit
				return 0;
			else if (parameter <= 20)//11~20 Boar
				return 1;
			else if (parameter <= 30)//21~30 Wolf
				return 2;
			else if (parameter <= 40)//31~40 Ibex
				return 3;
			else if (parameter <= 50)//41~50 Goat
				return 4;
			else if (parameter <= 60)//51~60 Deer
				return 5;
			else //61~70 Deer2
				return 6;
		}
    }
}