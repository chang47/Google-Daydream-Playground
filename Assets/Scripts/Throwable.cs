using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Material _outlineMaterial;
    private Rigidbody _rigidbody;
    private Vector3 _currentGrabbedLocation; // The tracked location of our object for us to throw
    private bool _isGrabbed;

    private const string OutlineWidthKey = "_Outline";
    private const float OutlineWidthValue = 0.03f;

    void Start ()
    {
        _outlineMaterial = GetComponent<Renderer>().materials[1];
	    _outlineMaterial.SetFloat(OutlineWidthKey, 0);

        _rigidbody = GetComponent<Rigidbody>();
        _currentGrabbedLocation = new Vector3();
        _isGrabbed = false;
    }

    void Update()
    {
        if (_isGrabbed)
        {
            _currentGrabbedLocation = transform.position;
        }
    }

    // Shows the outline by setting the width to be a fixed avalue when we are 
    // pointing at it.
    public void ShowOutlineMaterial()
    {
        _outlineMaterial.SetFloat(OutlineWidthKey, OutlineWidthValue);
    }

    // Hides the outline by making the width 0 when we are no longer 
    // pointing at it.
    public void HideOutlineMaterial()
    {
        _outlineMaterial.SetFloat(OutlineWidthKey, 0);
    }

    // Setup our throwable game object for when it is grabbed. Set the object that
    // grabbed it as its parent and disables kiniematics
    public void GetGrabbed(GameObject controllerObject)
    {
        transform.parent = controllerObject.transform; // Set object as a child so it'll follow our controller
        _rigidbody.isKinematic = true; // Stops physics from affecting the grabbed object
        _isGrabbed = true;
    }

    // Releases the throwable game object from being grabbed. It is sent flying 
    // by the force given by the controller gmae object.
    public void GetReleased()
    {
        if (_isGrabbed)
        {
            transform.parent = null; // Un-parent throwable object so it doesn't follow the controller
            _rigidbody.isKinematic = false; // Re-enables the physics engine.

            Vector3 throwVector = transform.position - _currentGrabbedLocation;
            _rigidbody.AddForce(throwVector * 10, ForceMode.Impulse); // Throws the ball by applying the given force
            _isGrabbed = false;
        }
    }
}
