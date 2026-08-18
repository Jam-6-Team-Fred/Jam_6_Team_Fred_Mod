using UnityEngine;
using NewHorizons.Utility;
using OWML.ModHelper;

namespace Jam6
{
    public class OrbSwitch : MonoBehaviour
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

        public delegate void ZeroGEvent();
        public static event ZeroGEvent ZeroG;
        public static event ZeroGEvent NormalG;

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
                Jam6.Instance.ModHelper.Console.WriteLine("Low Gravity...", OWML.Common.MessageType.Info);
                _powerOn = true;

                ZeroG();

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
                Jam6.Instance.ModHelper.Console.WriteLine("Normal Gravity...", OWML.Common.MessageType.Info);
                _powerOn = false;

                NormalG();

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
