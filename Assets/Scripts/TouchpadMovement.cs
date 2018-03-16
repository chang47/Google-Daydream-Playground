using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchpadMovement : MonoBehaviour
{
    private float _speedSlowDown;
    private Camera _mainCamera;

    void Start()
    {
        _speedSlowDown = 0.01f;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        if (GvrControllerInput.IsTouching)
        {
            Vector2 touchPos = GvrControllerInput.TouchPos;
            Vector3 movementVector = new Vector3(touchPos.x - 0.5f, 0, touchPos.y - 0.5f); //adjust for center to be at 0.5, 0.5
            Vector3 rotatedVector = RotateVector(movementVector, _mainCamera.transform.eulerAngles.y);
            transform.Translate(rotatedVector.x * _speedSlowDown, 0, -rotatedVector.z * _speedSlowDown); // negative to adjust for the vector speed   
        }
    }

    // Given a direction and a degree, we'll rotate the direction vector by the given degree amount
    private Vector3 RotateVector(Vector3 direction, float degree)
    {
        float radian = Mathf.Deg2Rad * degree; // convert our degree to be in radians

        // calculate our rotation vector using matrix multiplication
        // source: https://en.wikipedia.org/wiki/Rotation_matrix
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);
        float newX = direction.x * cos - direction.z * sin;
        float newZ = direction.x * sin + direction.z * cos;
        return new Vector3(newX, 0, newZ);
    }
}
