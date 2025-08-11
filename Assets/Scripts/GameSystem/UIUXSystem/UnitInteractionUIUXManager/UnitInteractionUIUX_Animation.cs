using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GameSystem.UIUXSystem.UnitInteractionUIUX
{
    public interface IUnitInteractionUIUX_Animation
    {
        public void InitialSetting(UnitInteractionUIUXData unitInteractionUIUXData);
        public IEnumerator Show_UIUX_Coroutine();
    }

    public class UnitInteractionUIUX_Animation : MonoBehaviour, IUnitInteractionUIUX_Animation
    {

        [SerializeField] private RectTransform TopUIUXRectTransform;
        [SerializeField] private RectTransform BottomUIUXRectTransform;

        private UnitInteractionUIUXData myUnitInteractionUIUXData;

        [SerializeField] private UIAnimationData ShowData_TopUIUX;
        [SerializeField] private UIAnimationData ShowData_BottomUIUX;

        public void InitialSetting(UnitInteractionUIUXData unitInteractionUIUXData)
        {
            this.myUnitInteractionUIUXData = unitInteractionUIUXData;
        }

        public IEnumerator Show_UIUX_Coroutine()
        {
            this.StartCoroutine(this.DoAnimation(this.TopUIUXRectTransform, this.ShowData_TopUIUX));
            this.StartCoroutine(this.DoAnimation(this.BottomUIUXRectTransform, this.ShowData_BottomUIUX));

            yield return MaxAnimationTime();
        }

        public IEnumerator DoAnimation(RectTransform UITranform_, UIAnimationData UIAnimationData_)
        {
            // 경과 시간을 초기화합니다.
            float elapsedTime = 0f;

            Vector2 startPosition = UITranform_.anchoredPosition;
            Vector2 endPosition = UIAnimationData_.TargetPosition;

            if (UIAnimationData_.KeepX) endPosition.x = startPosition.x;
            if (UIAnimationData_.KeepY) endPosition.y = startPosition.y;

            // 애니메이션이 끝날 때까지 루프를 실행합니다.
            while (elapsedTime < UIAnimationData_.AnimationTime)
            {
                // 경과 시간의 비율을 계산합니다.
                float t = elapsedTime / UIAnimationData_.AnimationTime;
                // 애니메이션 곡선을 따라 비율을 조정합니다.
                float curveValue = UIAnimationData_.AnimatinoCurve.Evaluate(t);

                // Lerp를 사용하여 현재 위치를 계산합니다.
                Vector2 currentPosition = Vector2.Lerp(startPosition, endPosition, curveValue);
                // UI 요소의 위치를 업데이트합니다.
                UITranform_.anchoredPosition = currentPosition;

                // 프레임별로 경과 시간을 업데이트합니다.
                elapsedTime += Time.deltaTime;
                // 다음 프레임까지 대기합니다.
                yield return null;
            }

            // 애니메이션이 끝난 후, 정확한 끝 위치로 설정합니다.
            UITranform_.anchoredPosition = UIAnimationData_.TargetPosition;
        }

        private float MaxAnimationTime()
        {
            return this.ShowData_TopUIUX.AnimationTime > this.ShowData_BottomUIUX.AnimationTime ? this.ShowData_TopUIUX.AnimationTime : this.ShowData_BottomUIUX.AnimationTime;
        }
    }

    [System.Serializable]
    public class UIAnimationData
    {
        [SerializeField] public bool KeepX;
        [SerializeField] public bool KeepY;

        [SerializeField] public Vector2 TargetPosition;
        [SerializeField] public float AnimationTime;
        [SerializeField] public AnimationCurve AnimatinoCurve;
    }
}