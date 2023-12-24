using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniVRM10;

namespace MovementSystem
{
    public class BoneLookAt : MonoBehaviour
    {
        private Transform target;
        public float rotationSpeed = 10f;
        public HumanBodyBones selectedBone = HumanBodyBones.Head;
        public float disableSmoothAnimationUnderAngle = 1f;

        private Vrm10Instance vrmInstance;
        private Quaternion lastRotation;
        private bool goToAnimationRotation;

        private void Awake()
        {
            vrmInstance = GetComponent<Vrm10Instance>();
            if (vrmInstance != null)
            {
                target = vrmInstance.LookAtTarget;
            }

            Transform boneTransform;
            if (vrmInstance.TryGetBoneTransform(selectedBone, out boneTransform))
            {
                if (lastRotation == null)
                {
                    lastRotation = boneTransform.rotation;
                }
            }
        }

        private void LateUpdate()
        {
            if (vrmInstance != null && lastRotation != null)
            {
                Transform boneTransform;
                if (vrmInstance.TryGetBoneTransform(selectedBone, out boneTransform))
                {
                    if (target != vrmInstance.LookAtTarget)
                    {
                        target = vrmInstance.LookAtTarget;
                        if (target != null)
                        {
                            goToAnimationRotation = true;
                        }
                    }

                    if (target != null)
                    {
                        Quaternion targetRotation = Quaternion.LookRotation(target.position - boneTransform.position);

                        lastRotation = Quaternion.Slerp(lastRotation, targetRotation, rotationSpeed * Time.deltaTime);
                        boneTransform.rotation = lastRotation;
                    }
                    else if (goToAnimationRotation)
                    {
                        Quaternion targetRotation = boneTransform.rotation;

                        lastRotation = Quaternion.Slerp(lastRotation, targetRotation, rotationSpeed * Time.deltaTime);
                        boneTransform.rotation = lastRotation;

                        if (Quaternion.Angle(lastRotation, targetRotation) < disableSmoothAnimationUnderAngle)
                        {
                            goToAnimationRotation = false;
                        }
                    }
                }
            }
        }
    }
}