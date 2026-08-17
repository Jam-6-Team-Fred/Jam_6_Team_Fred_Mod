using UnityEngine;
using NewHorizons.Utility;
using OWML.ModHelper;

namespace Jam6
{
    public class PresentSwitch : MonoBehaviour
    {
        [Space]
        [SerializeField]
        public bool _startOn;

        [SerializeField]
        public NomaiInterfaceSlot _slot;

        [Space]
        [SerializeField]
        public OWAudioSource _audioSource;

        [SerializeField]
        public AudioType _onClip;

        [SerializeField]
        public AudioType _offClip;

        private bool _powerOn;

        public delegate void PresentEvent();
        public static event PresentEvent OpenPresent;
        public static event PresentEvent ClosePresent;

        public void Awake()
        {
            _powerOn = _startOn;
            _slot.OnSlotActivated += OnSlotActivated;
            _slot.OnSlotDeactivated += OnSlotDeactivated;
        }

        public void OnDestroy()
        {
            _slot.OnSlotActivated -= OnSlotActivated;
            _slot.OnSlotDeactivated -= OnSlotDeactivated;
        }

        public void PowerOn()
        {
            if (!_powerOn)
            {
                _powerOn = true;

                OpenPresent();

                if (_audioSource != null)
                {
                    _audioSource.PlayOneShot(_onClip);
                }
            }
        }

        public void PowerOff()
        {
            if (_powerOn)
            {
                _powerOn = false;

                ClosePresent();

                if (_audioSource != null)
                {
                    _audioSource.PlayOneShot(_offClip);
                }
            }
        }

        public void OnSlotActivated(NomaiInterfaceSlot slot)
        {
            PowerOn();
            Jam6.Instance.ModHelper.Console.WriteLine("Turning On...", OWML.Common.MessageType.Success);
        }

        public void OnSlotDeactivated(NomaiInterfaceSlot slot)
        {
            PowerOff();
            Jam6.Instance.ModHelper.Console.WriteLine("Turning Off...", OWML.Common.MessageType.Success);
        }
    }
}
