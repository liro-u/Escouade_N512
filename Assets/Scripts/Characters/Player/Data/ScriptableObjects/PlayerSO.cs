using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    [CreateAssetMenu(fileName = "Player", menuName = "Custom/Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field: SerializeField] public PlayerMovementSO PlayerMovement { get; private set; }
        [field: SerializeField] public GameObject CharacterModel { get; private set; }
    }
}
