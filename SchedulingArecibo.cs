using NewHorizons.Utility;
using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Jam6
{
    public class SchedulingArecibo : MonoBehaviour
    {
        [SerializeField]
        public GameObject tunnelCovers;
        [SerializeField]
        public float signalDuration = 60f;
        [SerializeField]
        public float amountToMoveY = 1.5f;
        [SerializeField]
        public float durationToMoveY = 1;
        [SerializeField]
        public float amountToMoveX = -9f;
        [SerializeField]
        public float durationToMoveX = 1;

        [NonSerialized]
        public ModBehaviour mod;
        [NonSerialized]
        public AudioSignal signalSource;
        [NonSerialized]
        public float timeStamp;
        [NonSerialized]
        public float signalStart;
        [NonSerialized]
        public float currentTime;
        [NonSerialized]
        public Vector3 startPosition;
        [NonSerialized]
        public Vector3 endPositionY;
        [NonSerialized]
        public Vector3 endPositionX;
        [NonSerialized]
        public LerpState progress;
        [NonSerialized]
        public bool isLerping = false;
        [NonSerialized]
        public LerpType whatLerp;

        public enum LerpState
        {
            Start,
            EndY,
            EndX
        }

        public enum LerpType
        {
            Open,
            Close
        }

        public void Awake()
        {
            mod = Jam6.Instance;
            SchedulingSocket.BeforeAScheduledEvent += FireSignal;
        }

        public void OnDestroy()
        {
            SchedulingSocket.BeforeAScheduledEvent -= FireSignal;
        }

        public void Start()
        {
            signalSource = SearchUtilities.Find("Disc_Body/Sector/TelescopeSignal").GetComponent<AudioSignal>();
            signalSource?.SetSignalActivation(false, 0f);
            if (tunnelCovers == null)
            {
                tunnelCovers = transform.Find("Tunnel Covers").gameObject;
            }
            startPosition = tunnelCovers.transform.localPosition;
            endPositionY = startPosition + new Vector3(0, amountToMoveY, 0);
            endPositionX = endPositionY + new Vector3(amountToMoveX, 0, 0);
        }

        public void FireSignal(SchedulingItem item, bool isAlwaysActive)
        {
            if (!isAlwaysActive)
            {
                signalSource?.SetSignalActivation(true, 2f);
                mod.ModHelper.Console.WriteLine($"Signal Source - {signalSource}, Active - {signalSource._active}");
                timeStamp = currentTime;
                signalStart = timeStamp;
                progress = LerpState.Start;
                whatLerp = LerpType.Open;
                isLerping = true;
                mod.ModHelper.Console.WriteLine("Arecibo fires", OWML.Common.MessageType.Success);
            }
        }

        public void Update()
        {
            if (signalSource != null)
            {
                currentTime = TimeLoop.GetSecondsElapsed();
                if (signalSource._active && currentTime - signalStart >= signalDuration)
                {
                    signalSource?.SetSignalActivation(false, 2f);
                    timeStamp = currentTime;
                    progress = LerpState.EndX;
                    whatLerp = LerpType.Close;
                    isLerping = true;
                }
            }

            if (isLerping)
            {
                switch (whatLerp)
                {
                    case LerpType.Open:
                        switch (progress)
                        {
                            case LerpState.Start:
                                //Y move
                                MoveTunnelCover(timeStamp, startPosition, endPositionY, durationToMoveY, LerpState.EndY);
                                break;
                            case LerpState.EndY:
                                //X move
                                MoveTunnelCover(timeStamp, endPositionY, endPositionX, durationToMoveX, LerpState.EndX);
                                break;
                            case LerpState.EndX:
                                isLerping = false;
                                mod.ModHelper.Console.WriteLine("Opening lerp Done", OWML.Common.MessageType.Success);
                                break;
                        }
                        break;
                    case LerpType.Close:
                        switch (progress)
                        {
                            case LerpState.EndX:
                                //X move
                                MoveTunnelCover(timeStamp, endPositionX, endPositionY, durationToMoveX, LerpState.EndY);
                                break;
                            case LerpState.EndY:
                                //Y move
                                MoveTunnelCover(timeStamp, endPositionY, startPosition, durationToMoveY, LerpState.Start);
                                break;
                            case LerpState.Start:
                                isLerping = false;
                                mod.ModHelper.Console.WriteLine("Closing lerp Done", OWML.Common.MessageType.Success);
                                break;
                        }
                        break;
                }
            }
        }

        public void MoveTunnelCover(float startTime, Vector3 start, Vector3 end, float duration, LerpState nextState)
        {
            //The funny thing
            float num = Mathf.InverseLerp(startTime, startTime + duration, currentTime);

            float smoothStep = Mathf.SmoothStep(0f, 1f, num);

            //The Lerp
            tunnelCovers.transform.localPosition = Vector3.Lerp(start, end, smoothStep);

            if (smoothStep == 1)
            {
                timeStamp = currentTime;
                progress = nextState;
                mod.ModHelper.Console.WriteLine($"Done! The next state is {nextState}");
            }
        }
    }
}
