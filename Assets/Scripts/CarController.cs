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
    [SerializeField] private float motorTorque = 500f;
    [SerializeField] private float brakeTorque = 9000f;
    [SerializeField] private float maxSpeed = 350f;     //Kmph


    [SerializeField] private Rigidbody rb;      //Experiment with different values of COM
    
    [SerializeField] private float steerInput;  //-1.0 to 1.0
    [SerializeField] private float throttleInput;   //-1.0 to 1.0
    [SerializeField] private float brakeInput;  //0.0 to 1.0
    [SerializeField] private float currentSpeed;


    public float speed => rb.linearVelocity.magnitude * 3.6f;   //3.6 is conversion factor m/s to km/h

    public void GetInputs(float steer, float throttle, float brake)
    {
        steerInput = Mathf.Clamp(steer, -1.0f, 1.0f);
        throttleInput = Mathf.Clamp(throttle, -1.0f, 1.0f);
        brakeInput = Mathf.Clamp01(brake);
    }

    private void FixedUpdate() 
    {
        currentSpeed = speed;
        Steer();
        Throttle();
        Brake();
        SpeedCap();
    }

    private void Steer()
    {
        float angle = steerInput * maxSteerAngle;

        frontLeft.steerAngle = angle;
        frontRight.steerAngle = angle;

        backLeft.steerAngle = 0f;
        backRight.steerAngle = 0f;
    }

    private void Throttle()
    {
        float torque = throttleInput * motorTorque;

        if (speed >= maxSpeed && throttleInput > 0f) torque = 0f;

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
}
