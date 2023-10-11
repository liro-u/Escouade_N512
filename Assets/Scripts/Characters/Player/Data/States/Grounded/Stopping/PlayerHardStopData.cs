using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    [Serializable]
    public class PlayerHardStopData
    {
        [field: SerializeField][field: Range(0f, 15f)] public float DecelerationForce { get; private set; } = 5f;
    }
}
