using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;
using VirtueSky.Core;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using Button = UnityEngine.UI.Button;


namespace VirtueSky.UIButton
{
    [EditorIcon("icon_button")]
    public class ButtonCustom : Button
    {
        [SerializeField] private bool isHandleEventClickButton = true;
        [SerializeField] private bool isMotion = true;
        [SerializeField] private Ease easingTypes = Ease.OutQuint;

        [SerializeField] private float scale = 0.9f;
        [SerializeField] private bool isShrugOver;
        [SerializeField] private float timeShrug = .2f;
        [SerializeField] private float strength = .2f;

        Vector3 originScale = Vector3.one;
        private bool canShrug = true;
        private Tween _tween;

        protected override void OnEnable()
        {
            base.OnEnable();
            originScale = transform.localScale;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ResetScale();
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            DoScale();
            if (isHandleEventClickButton)
            {
                GlobalStatic.OnClickButtonEvent?.Invoke();
            }
        }


        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            ResetScale();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            Shrug();
        }

        void DoScale()
        {
            if (isMotion)
            {
                _tween = Tween.Scale(transform, originScale * scale, .15f, easingTypes);
            }
        }

        void Shrug()
        {
            if (isMotion && isShrugOver && canShrug)
            {
                canShrug = false;
                if (isMotion && isShrugOver)
                {
                    transform.Shrug(timeShrug, strength, Ease.OutQuad, () => { canShrug = true; });
                }
            }
        }

        void ResetScale()
        {
            if (isMotion)
            {
                _tween.Stop();
                transform.localScale = originScale;
            }
        }
    }
}