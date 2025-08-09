using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BinPhysics : MonoBehaviour
{

    public float forceMultiplier = 1000f;
    public float torqueMultiplier = 500f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
// funny bin physics
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ai"))
        {
            Debug.Log("Hit");
            Vector3 forceDirection = (transform.position - other.transform.position).normalized;
            Vector3 force = forceDirection * forceMultiplier;

            // Apply the force and random torque
            rb.AddForce(force, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * torqueMultiplier, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
