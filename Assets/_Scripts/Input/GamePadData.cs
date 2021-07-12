using UnityEngine.Events;
using XInputDotNetPure;

namespace BTA
{
    public enum GamePadID
    {
        Controller1 = 0,
        Controller2,
        Controller3,
        Controller4,
        None
    }

    public enum GamePadInput
    {
        ButtonA,
        ButtonB,
        ButtonX,
        ButtonY,
        RightBumper,
        LeftBumper,
        ButtonStart,
        DPadDown,
        DPadLeft,
        DPadUp,
        DPadRight,
        ButtonARelease,
    }

    public enum GamePadAxis
    {
        LeftJoystickX,
        LeftJoystickY,
        RightJoystickX,
        RightJoystickY,
        LeftTrigger,
        RightTrigger,
    }

    //One GamePad
    
    public class GamePadData
    {
        public int ID;
        public bool playerIndexSet = false;
        public PlayerIndex playerIndex;
        public GamePadState state;
        public GamePadState prevState;
        public GamePadEvent padEvent;
    }

    public class InputEvent : UnityEvent
    { }

    public class GamePadEvent
    {
        public InputEvent OnButtonA = new InputEvent();
        public InputEvent OnButtonARelease = new InputEvent();
        public InputEvent OnButtonB = new InputEvent();
        public InputEvent OnButtonY = new InputEvent();
        public InputEvent OnButtonX = new InputEvent();
        public InputEvent OnButtonStart = new InputEvent();
        public InputEvent OnButtonRightBumper = new InputEvent();
        public InputEvent OnButtonLeftBumper = new InputEvent();
        public InputEvent OnDPadDown = new InputEvent();
        public InputEvent OnDPadLeft = new InputEvent();
        public InputEvent OnDPadUp = new InputEvent();
        public InputEvent OnDPadRight = new InputEvent();
    }
}
