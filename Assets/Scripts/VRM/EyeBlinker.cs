using System.Collections;
using UnityEngine;
using UniVRM10;

namespace MovementSystem
{
    public class EyeBlinker : MonoBehaviour
    {
        protected Vrm10Instance vrmInstance;
        protected ExpressionKey blinkExpressionKey;

        [Header("Properties")]
        public AnimationCurve blinkProbabilityCurve;
        public float blinkEyeCloseDuration = 0.06f;
        public float blinkOpeningSeconds = 0.03f;
        public float blinkClosingSeconds = 0.1f;

        protected Coroutine blinkCoroutine;

        private void Awake()
        {
            vrmInstance = GetComponent<Vrm10Instance>();
            blinkExpressionKey = ExpressionKey.Blink;
        }

        private IEnumerator BlinkRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(blinkProbabilityCurve.Evaluate(Random.value));

                float value = 0f;
                float closeSpeed = 1.0f / blinkClosingSeconds;

                while (value < 1)
                {
                    value += Time.deltaTime * closeSpeed;
                    vrmInstance.Runtime.Expression.SetWeight(blinkExpressionKey, value);
                    yield return null;
                }
                vrmInstance.Runtime.Expression.SetWeight(blinkExpressionKey, 1);

                yield return new WaitForSeconds(blinkEyeCloseDuration);

                value = 1f;
                float openSpeed = 1.0f / blinkOpeningSeconds;

                while (value > 0)
                {
                    value -= Time.deltaTime * openSpeed;
                    vrmInstance.Runtime.Expression.SetWeight(blinkExpressionKey, value);
                    yield return null;
                }
                vrmInstance.Runtime.Expression.SetWeight(blinkExpressionKey, 0);
            }
        }

        private void OnEnable()
        {
            blinkCoroutine = StartCoroutine(BlinkRoutine());
        }

        private void OnDisable()
        {
            if (blinkCoroutine != null)
            {
                StopCoroutine(blinkCoroutine);
                blinkCoroutine = null;
            }
        }
    }
}
