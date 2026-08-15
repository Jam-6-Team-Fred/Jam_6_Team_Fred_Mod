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

        [SerializeField]
        public GameObject gravity1;

        [SerializeField]
        public GameObject gravity2;

        [Space]
        [SerializeField]
        public OWAudioSource _audioSource;

        [SerializeField]
        public AudioType _onClip;

        [SerializeField]
        public AudioType _offClip;

        private bool _powerOn;

        public void Awake()
        {
            _powerOn = _startOn;
            _slot.OnSlotActivated += OnSlotActivated;
            _slot.OnSlotDeactivated += OnSlotDeactivated;
            Jam6.Instance.NewHorizons.GetBodyLoadedEvent().AddListener(FindVolumes);
        }

        public void OnDestroy()
        {
            _slot.OnSlotActivated -= OnSlotActivated;
            _slot.OnSlotDeactivated -= OnSlotDeactivated;
            Jam6.Instance.NewHorizons.GetBodyLoadedEvent().RemoveListener(FindVolumes);
        }

        public void PowerOn()
        {
            if (!_powerOn)
            {
                _powerOn = true;

                //gravity1.SetActive(false);
                //gravity2.SetActive(false);
                gravity1.GetComponent<DirectionalForceVolume>().SetFieldMagnitude(0);
                gravity2.GetComponent<DirectionalForceVolume>().SetFieldMagnitude(0);
                Jam6.Instance.ModHelper.Console.WriteLine($"Gravities: 1-{gravity1.active}, 2-{gravity2.active}", OWML.Common.MessageType.Success);

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

                //gravity1.SetActive(true);
                //gravity2.SetActive(true);
                gravity1.GetComponent<DirectionalForceVolume>().SetFieldMagnitude(12);
                gravity2.GetComponent<DirectionalForceVolume>().SetFieldMagnitude(12);
                Jam6.Instance.ModHelper.Console.WriteLine($"Gravities: 1-{gravity1.active}, 2-{gravity2.active}", OWML.Common.MessageType.Success);

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

        public void FindVolumes(string planetName)
        {
            if (planetName == "Disc")
            {
                Jam6.Instance.ModHelper.Console.WriteLine("Found Disc", OWML.Common.MessageType.Success);
                gravity1 = SearchUtilities.Find("Disc/Past/GravityVolume");
                gravity2 = SearchUtilities.Find("Disc/Future/GravityVolume");
            }
        }
    }
}
