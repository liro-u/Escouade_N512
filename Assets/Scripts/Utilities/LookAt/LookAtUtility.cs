using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;

namespace MovementSystem
{
    [Serializable]
    public class LookAtUtility
    {
        [field: SerializeField] public GameObject LookColliderGameObject { get; private set; }

        public void Initialize(Vrm10Instance vrmInstance, Transform lookFrom)
        {
            Look lookComponent = LookColliderGameObject.GetComponent<Look>();
            lookComponent.vrmInstance = vrmInstance;
            lookComponent.lookFrom = lookFrom;
        }
    }
}
