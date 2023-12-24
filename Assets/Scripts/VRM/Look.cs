using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UniVRM10;

namespace MovementSystem
{
    public class Look : MonoBehaviour
    {
        public Vrm10Instance vrmInstance;
        public Transform lookFrom;
        public List<GameObject> ObjectsInSight;

        private void OnTriggerEnter(Collider other)
        {
            if (vrmInstance.LookAtTarget == null)
            {
                vrmInstance.LookAtTarget = other.transform;
            }

            if (!ObjectsInSight.Contains(other.gameObject))
            {
                ObjectsInSight.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (ObjectsInSight.Contains(other.gameObject))
            {
                ObjectsInSight.Remove(other.gameObject);
            }

            UpdateTarget();
        }

        private void UpdateTarget()
        {
            if (vrmInstance != null)
            {
                if (ObjectsInSight.Count == 0) {
                    vrmInstance.LookAtTarget = null;
                }
                else if (ObjectsInSight.Count == 1)
                {
                    vrmInstance.LookAtTarget = ObjectsInSight[0].transform;
                }
                else if (lookFrom != null)
                {
                    GameObject nearestObject = ObjectsInSight[0];
                    for (int i = 1; i < ObjectsInSight.Count; i++)
                    {
                        if (Vector3.Distance(lookFrom.position, ObjectsInSight[i].transform.position) < Vector3.Distance(lookFrom.position, nearestObject.transform.position))
                        {
                            nearestObject = ObjectsInSight[i];
                        }
                    }
                    vrmInstance.LookAtTarget = nearestObject.transform;
                }
            }
        }


    }
}
