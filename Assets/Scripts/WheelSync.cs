using UnityEngine;

public class WheelSync : MonoBehaviour
{
    [System.Serializable]
    public class Wheel
    {
        public WheelCollider collider;
        public Transform mesh;
    }

    public Wheel[] wheels;

    private void LateUpdate()
    {
        foreach (Wheel wheel in wheels)
        {
            if (wheel.collider == null || wheel.mesh == null)
                continue;

            wheel.collider.GetWorldPose(
                out Vector3 position,
                out Quaternion rotation
            );

            Debug.DrawLine(
                wheel.collider.transform.position,
                position,
                Color.red
            );

            wheel.mesh.SetPositionAndRotation(position, rotation);
        }
    }
}
