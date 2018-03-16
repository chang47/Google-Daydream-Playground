using UnityEngine;

public class PlayerMenuController : MonoBehaviour
{
    public GameObject MenuContainer; // Our Menu Container prefab

    private GameObject _menuInstance; // A reference to the menu container we create from our prefab
    private Camera _camera; // A reference to our camera for positioning

	void Start ()
	{
	    _menuInstance = null;
        _camera = Camera.main;
	}
	
	void Update () {
	    if (GvrControllerInput.AppButtonUp)
	    {
	        if (_menuInstance)
	        {
                // If we have a menu already, close it
	            DestroyMenu();
	        }
	        else
	        {
                // If we don't have a menu, create one.
	            CreateMenu();
	        }
	    }
	}

    // Create a menu that will be displayed right in front of the player
    private void CreateMenu()
    {
        // Set the position of the camera to be in front of where the player is looking
        // at, adjusted with the y position of the menu container.
        Vector3 direction = _camera.transform.forward * 3;
        direction.y += MenuContainer.transform.position.y;

        // creates an instance of our Menu Container to be in our game
        _menuInstance = Instantiate(MenuContainer, direction , _camera.transform.rotation);
    }

    // Removes the menu from our game
    private void DestroyMenu()
    {
        Destroy(_menuInstance);
        _menuInstance = null;
    }
}