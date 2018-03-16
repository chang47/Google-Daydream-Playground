using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float SwipeThreshold = 0.5f;

    private Vector2 _startingPosition; // tracks our starting position
    private Vector2 _currentPosition; // tracks the last position touched
    private bool _startedTouch; // tells us if we started swiping

	void Start ()
	{
	    _startedTouch = false;
	    _startingPosition = GvrControllerInput.TouchPosCentered;
	    _currentPosition = GvrControllerInput.TouchPosCentered;
    }
	
	void Update ()
	{
	    if (GvrControllerInput.AppButtonDown)
	    {
	        print("Click App button down");
        }

	    if (GvrControllerInput.ClickButtonDown)
        { 
	        print("Click Touchpad");
        }

	    if (GvrControllerInput.IsTouching)
	    {
	        if (!_startedTouch)
	        {
                // Start our swiping motion
	            _startedTouch = true;
	            _startingPosition = GvrControllerInput.TouchPos;
	            _currentPosition = GvrControllerInput.TouchPos;
	        }
	        else
	        {
                // Tracks our position of where we're swiping
	            _currentPosition = GvrControllerInput.TouchPos;
	        }
	    }
	    else
	    {
	        if (_startedTouch)
	        {
                // Let go of our touchpad, see if we made any swiping motions
                _startedTouch = false;
	            Vector2 delta = _currentPosition - _startingPosition;
                DetectSwipe(delta);
	        }
	    }
	}

    // Prints out the swipe direction (if any) based off of the Vector2 representation of
    // the direction swiped
    private void DetectSwipe(Vector2 delta)
    {
        float y = delta.y;
        float x = delta.x;
        print(delta);

        // x = 0 is far left of touchpad
        // y = 0 is far top of touchpad
        if (y > 0 && Mathf.Abs(x) < SwipeThreshold)
        {
            print("Swiped down");
        }
        else if (y < 0 && Mathf.Abs(x) < SwipeThreshold) 
        {
            print("Swiped up");
        }
        else if (x > 0 && Mathf.Abs(y) < SwipeThreshold)
        {
            print("Swiped right");
        }
        else if (x < 0 && Mathf.Abs(y) < SwipeThreshold)
        {
            print("Swiped left");
        }
    }
}
