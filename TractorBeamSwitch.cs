using NAudio.Utils;
using NewHorizons.Utility;
using OWML.ModHelper;
using UnityEngine;

namespace Jam6
{
	public class TractorBeamSwitch : MonoBehaviour
	{
		public enum State
		{
			FORWARD = 1,
			REVERSE = 2
		}

		private State _initialState;

        public enum Position
        {
            UPPER = 0,
            LOWER = 1,
        }

		public Position _position;

        public GameObject tractorBeam;

        public TractorBeamController tractorBeamController;

		public TractorBeamSwitch linkedSwitch;

		public NomaiInterfaceSlot _forwardSlot;

		public NomaiInterfaceSlot _reverseSlot;

		public NomaiInterfaceOrb _orb;

		private void Awake()
		{
			_forwardSlot.OnSlotActivated += OnActivated;
			_reverseSlot.OnSlotActivated += OnActivated;
			_forwardSlot.OnSlotDeactivated += OnDeactivated;
			_reverseSlot.OnSlotDeactivated += OnDeactivated;
		}

		private void Start()
        {
            if (tractorBeam == null)
            {
                tractorBeam = this.transform.parent.gameObject;
            }
			if (tractorBeamController == null)
			{
				tractorBeamController = tractorBeam.GetComponent<TractorBeamController>();
			}
            if (linkedSwitch == null)
			{
				switch (_position)
				{
					case Position.UPPER:
						//linkedSwitch = SearchUtilities.Find("Disc/Past/Mountain/Welcome Center/Docking_Tractor_Beam/Docking_Tractor_Beam (1)/TractorBeamSwitch_Lower").GetComponent<TractorBeamSwitch>();
						foreach (GameObject child in tractorBeam.GetAllChildren())
						{
							if (child.name == "TractorBeamSwitch_Lower")
							{
								linkedSwitch = child.GetComponent<TractorBeamSwitch>();
							}
						}
						break;
					case Position.LOWER:
						//linkedSwitch = SearchUtilities.Find("Disc/Past/Mountain/Welcome Center/Docking_Tractor_Beam/Docking_Tractor_Beam (1)/TractorBeamSwitch_Upper").GetComponent<TractorBeamSwitch>();
                        foreach (GameObject child in tractorBeam.GetAllChildren())
                        {
                            if (child.name == "TractorBeamSwitch_Upper")
                            {
                                linkedSwitch = child.GetComponent<TractorBeamSwitch>();
                            }
                        }
                        break;
				}
			}
            switch (_initialState)
			{
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
			_forwardSlot.OnSlotDeactivated -= OnDeactivated;
			_reverseSlot.OnSlotDeactivated -= OnDeactivated;
		}

		public void SetInitialState(State state)
		{
			_initialState = state;
		}

		public void MoveOrbToSlot( bool beamReversed)
		{
			if (!beamReversed)
			{
				MoveOrbToSlot(_forwardSlot);
			}
			else
			{
				MoveOrbToSlot(_reverseSlot);
			}
		}

		private void MoveOrbToSlot(NomaiInterfaceSlot slot)
		{
			_orb.SetOrbPosition(slot.transform.position);
		}

		private void OnActivated(NomaiInterfaceSlot slot)
		{
			linkedSwitch.MoveOrbToSlot(slot == _reverseSlot);
            tractorBeamController.SetActivation(active: true);
            tractorBeamController.SetReversed(slot == _reverseSlot);
		}

		private void OnDeactivated(NomaiInterfaceSlot slot)
		{
            tractorBeamController.SetActivation(active: false);
		}
	}
}