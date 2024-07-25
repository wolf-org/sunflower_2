using System;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.TouchInput
{
    [EditorIcon("icon_controller"), HideMonoScript]
    public class TouchInputManager : MonoBehaviour
    {
        public static event Action<Vector3> InputEventTouchBegin;
        public static event Action<Vector3> InputEventTouchMove;
        public static event Action<Vector3> InputEventTouchStationary;
        public static event Action<Vector3> InputEventTouchEnd;
        public static event Action<Vector3> InputEventTouchCancel;

        [ShowIf(nameof(IsPlaying)), SerializeField]
        private bool preventTouch;

        [Space, ShowIf(nameof(IsPlaying)), SerializeField]
        private Vector3 touchPosition;

        private bool IsPlaying => Application.isPlaying;
        private bool _mouseDown;
        private bool _mouseUpdate;
        private static TouchInputManager ins;

        private void Awake()
        {
            if (ins == null)
            {
                ins = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        public static bool PreventTouch
        {
            get => ins.preventTouch;
            set => ins.preventTouch = value;
        }

        private void Update()
        {
            if (preventTouch) return;
#if UNITY_EDITOR
            if (UnityEngine.Device.SystemInfo.deviceType != DeviceType.Desktop)
            {
                HandleTouch();
            }
            else
            {
                HandleMouse();
            }
#else
            HandleTouch();
#endif
        }

        void HandleTouch()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        InputEventTouchBegin?.Invoke(touch.position);

                        break;
                    case TouchPhase.Moved:
                        InputEventTouchMove?.Invoke(touch.position);

                        break;
                    case TouchPhase.Stationary:
                        InputEventTouchStationary?.Invoke(touch.position);

                        break;
                    case TouchPhase.Ended:
                        InputEventTouchEnd?.Invoke(touch.position);

                        break;
                    case TouchPhase.Canceled:
                        InputEventTouchCancel?.Invoke(touch.position);

                        break;
                }

                touchPosition = touch.position;
            }
        }

        void HandleMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!_mouseDown)
                {
                    _mouseDown = true;
                    _mouseUpdate = true;
                    InputEventTouchBegin?.Invoke(Input.mousePosition);
                    touchPosition = Input.mousePosition;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _mouseDown = false;
                _mouseUpdate = false;
                InputEventTouchEnd?.Invoke(Input.mousePosition);
                touchPosition = Input.mousePosition;
            }

            if (_mouseDown && _mouseUpdate)
            {
                InputEventTouchMove?.Invoke(Input.mousePosition);
                touchPosition = Input.mousePosition;
            }
        }
    }
}