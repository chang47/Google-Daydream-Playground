using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public float TimeDelay = 0.5f; // Time to hold touchpad to move to next menu button
    public float DirectionSensitivity = 0.1f; // Only works up to 0.5f

    private Button[] _buttons; // List of menu buttons
    private int _currentIndex; // Current button index that we're selecting. -1 if we're not using it.
    private float _currentSelectTime; // Current time we use to keep track how long we've been holding

    enum MenuScrollState { None, Down, Up } // Enum of button press states that we could be in
    private MenuScrollState _currentState; // The current state that we're in

    void Start ()
	{
	    _buttons = GetComponentsInChildren<Button>();
	    _currentIndex = -1;
	    _currentSelectTime = 0f;
        _currentState = MenuScrollState.None;
	}
	
	void Update ()
    {
        if (GvrControllerInput.ClickButtonUp)
        {
            // If we click our main button, we select our current menu item.
            SelectButton();
        }
        else if (GvrControllerInput.IsTouching)
        {
            // If we're touching the touch pad we should figure out if we should move 
            // our current index selection.
            SelectTouchPad();
        }
	}

    // Uses the user's position on the touchpad to figure out when to move to select the next button
    private void SelectTouchPad()
    {
        float y = GvrControllerInput.TouchPos.y - 0.5f; // Get the y position after subtracting the center point
        // Note: the top left corner of our touch pad is position 0,0, so if y == 0, we're touching the top
        // if y == 1, we're touching the bottom.
        switch (_currentState)
        {
            case MenuScrollState.None:
                if (y > DirectionSensitivity)
                {
                    // If we're currently in the None state and we're holding far enough down
                    // on the touchpad, we transition to the down state
                    SetState(MenuScrollState.Down, TimeDelay);
                }
                else if (y < -DirectionSensitivity)
                {
                    // If we're currently in the None state and we're holding far enough up
                    // on the touchpad, we transition to the down state
                    SetState(MenuScrollState.Up, TimeDelay);
                }
                break;
            case MenuScrollState.Down:
                if (y > DirectionSensitivity)
                {
                    // If we're in the down state and we're still still holding far enough down
                    // on the touchpad, we should move to the next button if we meet the condition.
                    ShouldSelectNextButton(1);
                }
                else if (y < -DirectionSensitivity)
                {
                    // If we're in the down state and we're holding far enough up
                    // on the touchpad, we should transition to the up state.
                    SetState(MenuScrollState.Up, TimeDelay);
                }
                else
                {
                    // If we're not holding far enough in either direction on teh touchpad, go back
                    // to the None state
                    SetState(MenuScrollState.None, 0f);
                }
                break;
            case MenuScrollState.Up:
                if (y < -DirectionSensitivity)
                {
                    // If we're in the up state and we're still still holding far enough up
                    // on the touchpad, we should go to the previous button in our menu.
                    ShouldSelectNextButton(-1);
                }
                else if (y > DirectionSensitivity)
                {
                    // If we're in the up state and we're holding far enough down
                    // on the touchpad, we should transition to the down state.
                    SetState(MenuScrollState.Down, TimeDelay);
                }
                else
                {
                    // If we're in the Up state and we're not holding far enough in any
                    // direction on the touchpad we go back to the none state.
                    SetState(MenuScrollState.None, 0f);
                }
                break;
        }
    }

    // Handle the logic to switch to the next button in our menu.
    // Takes in an int that we'll use for direction. 1 for next, -1 for prev.
    private void ShouldSelectNextButton(int direction)
    {
        _currentSelectTime += Time.deltaTime;
        // If we hold onto the button long enough to reach our time dealy
        // we can move our current index to the next/prev menu button.
        if (_currentSelectTime >= TimeDelay)
        {
            SelectNextButton(direction);
            _currentSelectTime = 0f;
        }
    }

    // Moves us to the next index based off of the direction that we were given.
    // Direction should either be 1 to move to the next button and -1 to move to the
    // previous button.
    private void SelectNextButton(int direction)
    {
        if (_currentIndex != -1)
        {
            // If this isn't the first time we select a menu, there
            // must have already selected a button, deselect it.
            _buttons[_currentIndex].OnDeselect(null);
        }
        else if (_currentIndex == -1 && direction == -1)
        {
            _currentIndex = 0; // Edge case for going backwards at the beginning
        }
        _currentIndex = Mod((_currentIndex + direction), _buttons.Length); // Get next button index to use
        _buttons[_currentIndex].OnSelect(null); // Select the new menu button to highlight
    }

    // Select the button that we're currently highlighting.
    private void SelectButton()
    {
        if (_currentIndex != -1)
        {
            _buttons[_currentIndex].Select();
            // Reset ourselves back to the starting state.
            _currentIndex = -1;
            SetState(MenuScrollState.None, 0f);
        }
    }

    // Helper to change the state we're at and set the time we should be using.
    // When we first just switch to up or down, we don't want to wait or Delay Time,
    // so we immediately set our time to be the delay time.
    private void SetState(MenuScrollState newState, float newTime)
    {
        _currentState = newState;
        _currentSelectTime = newTime;
    }

    // Resets our state if the user decides to point at the menu using the pointer
    // instead of using the touchpad.
    public void PointerEnterObject()
    {
        if (_currentIndex != -1)
        {
            _buttons[_currentIndex].OnDeselect(null);
            _currentIndex = -1;
            SetState(MenuScrollState.None, 0f);
        }
    }

    // Mod function courtesy of https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain
    // to help us find a positive value whne looking for remainders in division.
    private static int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }

    // Does an action based off of what menu button that was clicked on.
    // We don't have any specific behavior, but we could add them if we needed to.
    public void MenuButtonClick(int index)
    {
        switch (index)
        {
            case 0:
                print("Clicked button " + index);
                break;
            case 1:
                print("Clicked button " + index);
                break;
            case 2:
                print("Clicked button " + index);
                break;
            case 3:
                print("Clicked button " + index);
                break;
            case 4:
                print("Clicked button " + index);
                break;
            case 5:
                print("Clicked button " + index);
                break;
            case 6:
                print("Clicked button " + index);
                break;
            default:
                break;
        }
    }
}
