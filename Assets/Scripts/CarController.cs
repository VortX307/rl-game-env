using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class CarController : MonoBehaviour
{
    //Wheels
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider backRight;

    //Properties
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float motorTorque = 500f;
    [SerializeField] private float brakeTorque = 9000f;
    [SerializeField] private float maxSpeed = 350f;     //Kmph


    [SerializeField] private Rigidbody rb;      //Experiment with different values of COM
    
    [SerializeField] private float steerInput;  //-1.0 to 1.0
    [SerializeField] private float throttleInput;   //-1.0 to 1.0
    [SerializeField] private float brakeInput;  //0.0 to 1.0


    public float speed => rb.linearVelocity.magnitude * 3.6f;   //3.6 is conversion factor m/s to km/h

    public void GetInputs(float steer, float throttle, float brake)
    {
        steerInput = Mathf.Clamp(steer, -1.0f, 1.0f);
        throttleInput = Mathf.Clamp(throttle, -1.0f, 1.0f);
        brakeInput = Mathf.Clamp01(brake);
    }

    private void FixedUpdate()  //Try different physics steps
    {
        Steer();
        Throttle();
        Brake();
    }

    private void Steer()
    {
        float angle = steerInput * maxSteerAngle;

        frontLeft.steerAngle = angle;
        frontRight.steerAngle = angle;

        backLeft.steerAngle = 0f;
        backRight.steerAngle = 0f;
    }

    private void Throttle()     //RWD
    {
        float torque = throttleInput * motorTorque;

        if (speed >= maxSpeed && throttleInput > 0f)
            torque = 0f;

        backLeft.motorTorque = torque;
        backRight.motorTorque = torque;

        frontLeft.motorTorque = 0f;
        frontRight.motorTorque = 0f;
    }
    private void Brake()    //4 wheel braking
    {
        float torque = brakeInput * brakeTorque;

        frontLeft.brakeTorque = torque;
        frontRight.brakeTorque = torque;
        backLeft.brakeTorque = torque;
        backRight.brakeTorque = torque;
    }
}
