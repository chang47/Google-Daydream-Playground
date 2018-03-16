using System.Collections;
using UnityEngine;

public class ViewHelper : MonoBehaviour
{
    
    private RectTransform _canvasRectTransform; // holds the rectangle transform of our menu
    private Camera _mainCamera;
    private float _yGap; // Keeps our menu up so the center is in the front of our gaze.

    enum State { InScreen, NotInScreen, Moving }; // The state our game object can be in
    private State _currentState;
    
    void Start ()
	{
	    _canvasRectTransform = GetComponentInChildren<RectTransform>();
        _mainCamera = Camera.main;
        _currentState = State.InScreen;
	    _yGap = 2f;

	}
	
	void Update ()
	{
	    switch (_currentState)
	    {
            case State.InScreen:
                if (!IsFullyVisibleFrom(_canvasRectTransform, _mainCamera))
                {
                    // If the menu isn't fully visible anymore switch to NotInScreen state.
                    _currentState = State.NotInScreen;
                }
                break;
            case State.NotInScreen:
                // If the menu isn't in the screen anymore, start moving it towards the player.
                _currentState = State.Moving;
                StartCoroutine(MoveToFrontOfPlayer());
                break;
	    }
	}

    // Get the Vector location of the foward of our camera with some 
    // distance adjustments.
    private Vector3 GetCameraFoward()
    {
        Vector3 forward = _mainCamera.transform.forward * 3;
        forward.y += _yGap;
        return forward;
    }

    // Coroutine that will move our menu to the front of our player every frame
    private IEnumerator MoveToFrontOfPlayer()
    {
        // While we're not directly in front of the player, slowly move the menu to the front of our player
        while (transform.position != GetCameraFoward())
        {
            transform.eulerAngles = _mainCamera.transform.eulerAngles; // set our angle to be the same as the one the camera is facing
            float speed = 4f * Time.deltaTime; // the speed we're going to move the position of our camera
            transform.position = Vector3.MoveTowards(transform.position, GetCameraFoward(), speed); // move the position of our menu to our camera's foward.
            yield return null;
        }
        _currentState = State.InScreen; // change back to our normal state after the menu goes back to the front of the camera
    }

    /// <summary>
    /// Counts the bounding box corners of the given RectTransform that are visible from the given Camera in screen space.
    /// </summary>
    /// <returns>The amount of bounding box corners that are visible from the Camera or -1 if a corner isn't in the screen</returns>
    /// <param name="rectTransform">Rect transform.</param>
    /// <param name="camera">Camera.</param>
    private int CountCornersVisibleFrom(RectTransform rectTransform, Camera camera)
    {
        Rect screenBounds = new Rect(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
        Vector3[] objectCorners = new Vector3[4];
        rectTransform.GetWorldCorners(objectCorners);

        int visibleCorners = 0;
        for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
        {
            Vector3 tempScreenSpaceCorner = camera.WorldToScreenPoint(objectCorners[i]); // Transform world space position of corner to screen space
            if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
            {
                visibleCorners++;
            }
            else
            {
                return -1;
            }
        }
        return visibleCorners;
    }

    /// <summary>
    /// Determines if this RectTransform is fully visible from the specified camera.
    /// Works by checking if each bounding box corner of this RectTransform is inside the cameras screen space view frustrum.
    /// </summary>
    /// <returns><c>true</c> if is fully visible from the specified camera; otherwise, <c>false</c>.</returns>
    /// <param name="rectTransform">Rect transform.</param>
    /// <param name="camera">Camera.</param>
    private bool IsFullyVisibleFrom(RectTransform rectTransform, Camera camera)
    {
        return CountCornersVisibleFrom(rectTransform, camera) == 4; // True if all 4 corners are visible
    }
}
