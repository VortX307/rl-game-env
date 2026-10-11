using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


public class CarController : MonoBehaviour
{
    //Wheels
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider backLeft;
    [SerializeField] WheelCollider backRight;

    //Properties
    [SerializeField] private float maxSteerAngle = 30f;
    [SerializeField] private float motorTorque = 2000f;
    [SerializeField] private float brakeTorque = 9000f;
    [SerializeField] private float engineBrakeTorque = 150f;
    [SerializeField] private float maxSpeed = 350f;     //Kmph
    float k = 0.0092f;


    [SerializeField] private Rigidbody rb;      //Experiment with different values of COM
    
    [SerializeField] private float steerInput;  //-1.0 to 1.0
    [SerializeField] private float throttleInput;   //-1.0 to 1.0
    [SerializeField] private float brakeInput;  //0.0 to 1.0

    [Header("Car Params")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float currentSteerAngle;


    public float Speed => rb.linearVelocity.magnitude * 3.6f;   //3.6 is conversion factor m/s to km/h


    public void GetInputs(float steer, float throttle, float brake)
    {
        steerInput = Mathf.Clamp(steer, -1.0f, 1.0f);
        throttleInput = Mathf.Clamp(throttle, -1.0f, 1.0f);
        brakeInput = Mathf.Clamp01(brake);
    }

    private void FixedUpdate() 
    {
        currentSpeed = Speed;
        Steer();
        Throttle();
        Brake();
        SpeedCap();
        if (throttleInput < 0.75f && throttleInput > -0.75 && brakeInput == 0) EngineBrake();
    }

    private void Steer()
    {
        float angle = steerInput * maxSteerAngle * Mathf.Exp(-k * currentSpeed);
        currentSteerAngle = angle;

        frontLeft.steerAngle = angle;
        frontRight.steerAngle = angle;

        backLeft.steerAngle = 0f;
        backRight.steerAngle = 0f;
    }

    private void Throttle()
    {
        float torque = throttleInput * motorTorque;

        if (Speed >= maxSpeed && throttleInput > 0f) torque = 0f;

        backLeft.motorTorque = torque;
        backRight.motorTorque = torque;

        frontLeft.motorTorque = torque;
        frontRight.motorTorque = torque;
    }
    private void Brake()    //4 wheel braking
    {
        float torque = brakeInput * brakeTorque;

        frontLeft.brakeTorque = torque;
        frontRight.brakeTorque = torque;
        backLeft.brakeTorque = torque;
        backRight.brakeTorque = torque;
    }

    private void SpeedCap()
    {
        float maxSpeedMS = maxSpeed / 3.6f;

        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0f;

        if (horizontalVelocity.magnitude > maxSpeedMS)
        {
            Vector3 limitedVelocity = horizontalVelocity.normalized * maxSpeedMS;

            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
    }
    
    private void EngineBrake()
    {
        frontLeft.brakeTorque = engineBrakeTorque;
        frontRight.brakeTorque = engineBrakeTorque;
        backLeft.brakeTorque = engineBrakeTorque;
        backRight.brakeTorque = engineBrakeTorque;
    }
}
