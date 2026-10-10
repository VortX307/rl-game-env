using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerInput : MonoBehaviour
{
    [SerializeField] private CarController car;
    
    private void Update()
    {
        if(Keyboard.current==null)
        {
            car.GetInputs(0f, 0f, 0f);
        }

        float steer = 0f;
        float throttle = 0f;
        float brake = 0f;

        if (Keyboard.current.aKey.isPressed) steer -= 1.0f;
        if (Keyboard.current.dKey.isPressed) steer += 1.0f;

        if (Keyboard.current.wKey.isPressed) throttle += 1.0f;
        if (Keyboard.current.sKey.isPressed) throttle -= 1.0f;

        if (Keyboard.current.spaceKey.isPressed) brake = 1.0f;

        car.GetInputs(steer, throttle, brake);
    }
}
