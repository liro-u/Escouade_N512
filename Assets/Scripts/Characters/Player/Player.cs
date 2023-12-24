using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;

namespace MovementSystem
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }
        [field: SerializeField] public GameObject ConcreteCharacterModel { get; private set; }

        [field: Header("Collisions")]
        [field: SerializeField] public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
        [field: SerializeField] public LookAtUtility LookAtUtility { get; private set; }

        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

        [field: Header("Cameras")]
        [field: SerializeField] public PlayerCameraUtility CameraUtility { get; private set; }

        [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }

        public PlayerMovementSO MovementData { get; private set; }

        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }

        public PlayerInput Input { get; private set; }

        public Transform MainCameraTransform { get; private set; }

        private PlayerMovementStateMachine movementStateMachine;

        private void Awake()
        {
            MovementData = Data.PlayerMovement;
            if (this != null && Data.CharacterModel != null && Data.CharacterModel != ConcreteCharacterModel)
            {
                GameObject newChild = ReplaceChild(ConcreteCharacterModel, Data.CharacterModel);
                if (LookAtUtility != null && newChild != null)
                {
                    LookAtUtility.Initialize(newChild.GetComponent<Vrm10Instance>(), CameraUtility.CameraLookAt);
                }
            }

            Rigidbody = GetComponent<Rigidbody>();
            Input = GetComponent<PlayerInput>();

            LookAtUtility.Initialize(ConcreteCharacterModel.GetComponent<Vrm10Instance>(), CameraUtility.CameraLookAt);

            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();
            CameraUtility.Initialize();
            AnimationData.Initialize();

            MainCameraTransform = Camera.main.transform;

            movementStateMachine = new PlayerMovementStateMachine(this);
        }

        private GameObject ReplaceChild(GameObject originalChild, GameObject replacementObject)
        {
            if (originalChild != null)
            {
                Transform originalTransform = originalChild.transform.parent;

                GameObject newChild = Instantiate(replacementObject, originalTransform);

                newChild.name = replacementObject.name;

                UnityEditor.Undo.RecordObject(this, "Replace Child");
                UnityEditor.EditorUtility.SetDirty(this);

                ConcreteCharacterModel = newChild;

                DestroyImmediate(originalChild);
                Animator = newChild.GetComponent<Animator>();
                return newChild;
            }
            return null;
        }

        private void OnValidate()
        {
            if (this != null && Data.CharacterModel != null && Data.CharacterModel != ConcreteCharacterModel)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    GameObject newChild = ReplaceChild(ConcreteCharacterModel, Data.CharacterModel);
                    if (LookAtUtility != null && newChild != null)
                    {
                        LookAtUtility.Initialize(newChild.GetComponent<Vrm10Instance>(), CameraUtility.CameraLookAt);
                    }
                };
            }

            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();
        }

        private void Start()
        {
            movementStateMachine.ChangeState(movementStateMachine.IdlingState);
        }

        private void OnTriggerEnter(Collider collider)
        {
            movementStateMachine.OnTriggerEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            movementStateMachine.OnTriggerExit(collider);
        }

        private void Update()
        {
            movementStateMachine.HandleInput();

            movementStateMachine.Update();
        }

        private void FixedUpdate()
        {
            movementStateMachine.PhysicsUpdate();
        }

        public void OnMovementStateAnimationEnterEvent()
        {
            movementStateMachine.OnAnimationEnterEvent();
        }

        public void OnMovementStateAnimationExitEvent()
        {
            movementStateMachine.OnAnimationExitEvent();
        }

        public void OnMovementStateAnimationTransitionEvent()
        {
            movementStateMachine.OnAnimationTransitionEvent();
        }
    }
}
