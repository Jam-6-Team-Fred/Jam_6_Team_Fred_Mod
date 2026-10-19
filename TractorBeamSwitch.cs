using NewHorizons.Utility;
using OWML.ModHelper;
using UnityEngine;

namespace Jam6
{
	public class TractorBeamSwitch : MonoBehaviour
	{
		public enum State
		{
			OFF = 0,
			FORWARD = 1,
			REVERSE = 2
		}

		[SerializeField]
		private State _initialState;

        public enum Position
        {
            UPPER = 0,
            LOWER = 1,
        }

		[SerializeField]
		public Position _position;

        [SerializeField]
		private TractorBeamController[] _tractorBeams;

		[SerializeField]
		private TractorBeamSwitch[] _linkedSwitches;

		private TractorBeamSwitch linkedSwitch;

        [SerializeField]
		private NomaiInterfaceSlot _forwardSlot;

		[SerializeField]
		private NomaiInterfaceSlot _reverseSlot;

		[SerializeField]
		private NomaiInterfaceSlot _offSlot;

		[SerializeField]
		private NomaiInterfaceOrb _orb;

		private void Awake()
		{
			_forwardSlot.OnSlotActivated += OnActivated;
			_reverseSlot.OnSlotActivated += OnActivated;
			_offSlot.OnSlotActivated += OnActivated;
			_forwardSlot.OnSlotDeactivated += OnDeactivated;
			_reverseSlot.OnSlotDeactivated += OnDeactivated;
		}

		private void Start()
		{
			switch (_position)
			{
				case Position.UPPER:
					linkedSwitch = SearchUtilities.Find("Disc/Past/Mountain/Welcome Center/Docking_Tractor_Beam/Docking_Tractor_Beam (1)/TractorBeamSwitch_Lower").GetComponent<TractorBeamSwitch>();
                    break;
				case Position.LOWER:
                    linkedSwitch = SearchUtilities.Find("Disc/Past/Mountain/Welcome Center/Docking_Tractor_Beam/Docking_Tractor_Beam (1)/TractorBeamSwitch_Upper").GetComponent<TractorBeamSwitch>();
                    break;
            }
			_linkedSwitches[0] = linkedSwitch;
			_tractorBeams[0] = SearchUtilities.Find("Disc/Past/Mountain/Welcome Center/Docking_Tractor_Beam/Docking_Tractor_Beam (1)/Tractor Beam (2)").GetComponent<TractorBeamController>();
            switch (_initialState)
			{
				case State.OFF:
					MoveOrbToSlot(_offSlot);
					break;
				case State.FORWARD:
					MoveOrbToSlot(_forwardSlot);
					OnActivated(_forwardSlot);
					break;
				case State.REVERSE:
					MoveOrbToSlot(_reverseSlot);
					OnActivated(_reverseSlot);
					break;
			}
		}

		private void OnDestroy()
		{
			_forwardSlot.OnSlotActivated -= OnActivated;
			_reverseSlot.OnSlotActivated -= OnActivated;
			_offSlot.OnSlotActivated -= OnActivated;
			_forwardSlot.OnSlotDeactivated -= OnDeactivated;
			_reverseSlot.OnSlotDeactivated -= OnDeactivated;
		}

		public void SetInitialState(State state)
		{
			_initialState = state;
		}

		public void MoveOrbToSlot(bool beamActive, bool beamReversed)
		{
			if (beamActive && !beamReversed)
			{
				MoveOrbToSlot(_forwardSlot);
			}
			else if (beamActive && beamReversed)
			{
				MoveOrbToSlot(_reverseSlot);
			}
			else
			{
				MoveOrbToSlot(_offSlot);
			}
		}

		private void MoveOrbToSlot(NomaiInterfaceSlot slot)
		{
			_orb.SetOrbPosition(slot.transform.position);
		}

		private void OnActivated(NomaiInterfaceSlot slot)
		{
			for (int i = 0; i < _linkedSwitches.Length; i++)
			{
				_linkedSwitches[i].MoveOrbToSlot(slot != _offSlot, slot == _reverseSlot);
			}
			if (slot != _offSlot)
			{
				for (int j = 0; j < _tractorBeams.Length; j++)
				{
					_tractorBeams[j].SetActivation(active: true);
					_tractorBeams[j].SetReversed(slot == _reverseSlot);
				}
			}
		}

		private void OnDeactivated(NomaiInterfaceSlot slot)
		{
			for (int i = 0; i < _tractorBeams.Length; i++)
			{
				_tractorBeams[i].SetActivation(active: false);
			}
		}
	}
}