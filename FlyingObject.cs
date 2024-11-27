using UnityEngine;
using Zappar;

public class FlyingObject : MonoBehaviour
{
    [SerializeField] private ZapparInstantTrackingTarget trackingTarget; // Zappar Instant Tracking Target
    [SerializeField] private float amplitude = 0.5f;                    // Vertical amplitude of the floating motion
    [SerializeField] private float speed = 1.0f;                        // Speed of the floating motion
    private Vector3 initialLocalPosition;                               // Initial local position

    void Start()
    {
        // Save the initial local position based on the tracking target's local coordinate system
        if (trackingTarget != null)
        {
            initialLocalPosition = trackingTarget.transform.InverseTransformPoint(transform.position);
            Debug.Log($"Initial Local Position: {initialLocalPosition}");
        }
        else
        {
            Debug.LogError("Tracking Target is not assigned!");
        }
    }

    void Update()
    {
        if (trackingTarget != null)
        {
            // Calculate the vertical floating motion using a sine wave
            float offsetY = Mathf.Sin(Time.time * speed) * amplitude;

            // Apply the vertical floating motion in the local coordinate system
            Vector3 newLocalPosition = initialLocalPosition + new Vector3(0, offsetY, 0);

            // Update the position in the world coordinate system based on the tracking target's coordinate system
            transform.position = trackingTarget.transform.TransformPoint(newLocalPosition);

            Debug.Log($"Updated Position: {transform.position}");
        }
    }
}
