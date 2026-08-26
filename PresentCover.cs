using OWML.ModHelper;
using OWML.Utils;
using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using static Jam6.SchedulingArecibo;

namespace Jam6
{
    public class PresentCover : MonoBehaviour
    {
        [SerializeField]
        public Vector3 openPosition;
        [SerializeField]
        public float durationToMoveY = 3f;
        [SerializeField]
        public float durationToMoveX = 3f;

        [NonSerialized]
        public Vector3 startPosition;
        [NonSerialized]
        public Vector3 endPositionY;
        [NonSerialized]
        public Vector3 endPositionX;
        [NonSerialized]
        public float currentTime;
        [NonSerialized]
        public float timeStamp;
        [NonSerialized]
        public LerpState progress;
        [NonSerialized]
        public bool isLerping = false;
        [NonSerialized]
        public LerpType whatLerp;

        [NonSerialized]
        public ModBehaviour mod;

        public void Awake()
        {
            mod = Jam6.Instance;
            PresentSwitch.OpenPresent += Open;
            PresentSwitch.ClosePresent += Close;
        }

        public void Start()
        {
            startPosition = transform.localPosition;
            endPositionY = new Vector3(0, openPosition.y, 0);
            endPositionX = openPosition;
        }

        public void OnDestroy()
        {
            PresentSwitch.OpenPresent -= Open;
            PresentSwitch.ClosePresent -= Close;
        }

        public void Open()
        {
            mod.ModHelper.Console.WriteLine("Got Open Present", OWML.Common.MessageType.Success);

            timeStamp = currentTime;
            progress = LerpState.Start;
            whatLerp = LerpType.Open;
            isLerping = true;
        }

        public void Close()
        {
            mod.ModHelper.Console.WriteLine("Got Close Present", OWML.Common.MessageType.Success);

            timeStamp = currentTime;
            progress = LerpState.EndX;
            whatLerp = LerpType.Close;
            isLerping = true;
        }

        public void Update()
        {
            currentTime = TimeLoop.GetSecondsElapsed();
            if (isLerping)
            {
                switch (whatLerp)
                {
                    case LerpType.Open:
                        switch (progress)
                        {
                            case LerpState.Start:
                                //Y move
                                MovePresentCover(timeStamp, startPosition, endPositionY, durationToMoveY, LerpState.EndY);
                                break;
                            case LerpState.EndY:
                                //X move
                                MovePresentCover(timeStamp, endPositionY, endPositionX, durationToMoveX, LerpState.EndX);
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
                                MovePresentCover(timeStamp, endPositionX, endPositionY, durationToMoveX, LerpState.EndY);
                                break;
                            case LerpState.EndY:
                                //Y move
                                MovePresentCover(timeStamp, endPositionY, startPosition, durationToMoveY, LerpState.Start);
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

        public void MovePresentCover(float startTime, Vector3 start, Vector3 end, float duration, LerpState nextState)
        {
            //The funny thing
            float num = Mathf.InverseLerp(startTime, startTime + duration, currentTime);

            float smoothStep = Mathf.SmoothStep(0f, 1f, num);

            //The Lerp
            transform.localPosition = Vector3.Lerp(start, end, smoothStep);

            if (smoothStep == 1)
            {
                timeStamp = currentTime;
                progress = nextState;
                mod.ModHelper.Console.WriteLine($"Done! The next state is {nextState}");
            }
        }
    }
}
