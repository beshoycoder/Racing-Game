using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// this my script for making a car controller 
/// we will start simple thne advance over time 
/// first: basic controls and movement
/// second: feedback and sounds i want to add sounds to car when moving and when sliding
/// also add speedometer basic
/// </summary>
/// 

public enum WheelType
{
    Forward, Backward,
    ForwardSteer, BackwardSteer

}
[Serializable]
public class Wheels
{
    [SerializeField] private WheelType wheelType;
    [SerializeField] private GameObject WheelMesh;
    [SerializeField] private WheelCollider colider;
    public WheelCollider GetCollider() { return colider; }
    public GameObject GetMeshes() { return WheelMesh; }
    public WheelType GetWheelType() { return wheelType; }

}
public class MyCarController : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxRevSpeed;
    [SerializeField] private float accSpeed;
    [SerializeField] private float BreakForce;
    [SerializeField] private float maxAngle;
    [SerializeField] private float CameraResetDelay;
    [SerializeField] private Vector3 CenterOfGravity;
    [SerializeField] private Wheels[] CarWheels;
    [SerializeField] private TireDataScriptable tireType;
    [SerializeField] private GameObject targetLookat;
    [SerializeField] private AudioClip engineSound;
    [SerializeField] private AudioSource source;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private Rigidbody rb;
    private float Hinputvalue;
    private float Vinputvalue;
    private Vector2 LookDirection;
    private bool Isbreaking;
    float currentSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = CenterOfGravity;
        ApplyTiresData();
    }

    private void ApplyTiresData()
    {
        foreach (Wheels wheel in CarWheels)
        {
            tireType.ApplyChanges(wheel.GetCollider());
        }
    }

    void OnAccelartion(InputValue Value)
    {
        Vinputvalue = Value.Get<float>();
    }
    void OnSteering(InputValue Value)
    {
        Hinputvalue = Value.Get<float>();
    }
    void OnHandBreak(InputValue value)
    {
        Isbreaking = value.isPressed;

    }
    void OnCamera(InputValue inputValue)
    {
        LookDirection = inputValue.Get<Vector2>();
        StartCoroutine(RotateCameraEvent());
    }
    IEnumerator RotateCameraEvent()
    {
        while (true)
        {
            if (LookDirection != Vector2.zero)
            {

                targetLookat.transform.Rotate(Vector3.up, LookDirection.x * Time.deltaTime, Space.World);
                targetLookat.transform.Rotate(Vector3.left, LookDirection.y * Time.deltaTime, Space.World);
            }

            new WaitForSecondsRealtime(CameraResetDelay);
            ReturnCamera();


            yield return null;

        }
    }
    void ReturnCamera()
    {
        targetLookat.transform.forward = Vector3.Lerp(targetLookat.transform.forward, transform.forward, Time.deltaTime);
    }

    void Drive(float Vinput)
    {


        foreach (Wheels wheel in CarWheels)
        {
            switch (Vinput)
            {
                case -1:
                    currentSpeed = maxRevSpeed * Time.deltaTime * Vinput;
                    wheel.GetCollider().motorTorque += Mathf.Clamp(currentSpeed, -maxRevSpeed, 0);

                    break;
                case 1:
                    currentSpeed += accSpeed * Time.deltaTime * Vinput;
                    wheel.GetCollider().motorTorque += Mathf.Clamp(currentSpeed, 0, maxSpeed);

                    break;
                default:
                    //this when there is no input but the car still moving
                    //it will make the car slowly lose speed until it compleletly stops
                    currentSpeed = 0;
                    wheel.GetCollider().motorTorque = 0;
                    break;
            }
        }
    }

    void Steer(float Hinput)
    {
        foreach (Wheels wheel in CarWheels)
        {
            if (wheel.GetWheelType() == WheelType.ForwardSteer || wheel.GetWheelType() == WheelType.BackwardSteer)
            {
                float SteerAngle = maxAngle * Hinput;

                wheel.GetCollider().steerAngle = Mathf.Lerp(wheel.GetCollider().steerAngle, SteerAngle, 0.6f);
            }
        }
    }
    void ApplyBraeks(bool Bool)
    {

        foreach (Wheels wheel in CarWheels)
        {
            if (Bool)
            {
                wheel.GetCollider().brakeTorque += BreakForce;
            }
            else
            {
                wheel.GetCollider().brakeTorque = 0;
            }
        }

    }
    void RotateCamera()
    {
        if (LookDirection != Vector2.zero)
        {

            targetLookat.transform.Rotate(Vector3.up, LookDirection.x, Space.World);
            targetLookat.transform.Rotate(Vector3.left, LookDirection.y, Space.World);
        }
        else
        {
            targetLookat.transform.forward = Vector3.Lerp(targetLookat.transform.forward, transform.forward, Time.deltaTime);
        }
    }
    void UpdateAnimation()
    {
        Quaternion QOT;
        Vector3 POS;

        // Check if the car's speed is nearly zero
        bool isCarStationary = rb.velocity.magnitude < 0.1f; // You can adjust the threshold

        foreach (Wheels wheel in CarWheels)
        {
            //if (isCarStationary)
            //{
            //    wheel.GetCollider().rotationSpeed = 0;
            //    wheel.GetMeshes().transform.localRotation = Quaternion.Euler(
            //        Mathf.Lerp(wheel.GetCollider().rotationSpeed, 0, Time.deltaTime),
            //        wheel.GetCollider().steerAngle,
            //        0);
            //}
            //else
            //{
            //    wheel.GetCollider().GetWorldPose(out POS, out QOT);
            //    wheel.GetMeshes().transform.position = POS;
            //    wheel.GetMeshes().transform.rotation = QOT;
            //}
            wheel.GetCollider().GetWorldPose(out POS, out QOT);
            wheel.GetMeshes().transform.position = POS;
            wheel.GetMeshes().transform.rotation = QOT;
        }
    }
    void CheckMaxSpeed()
    {
        if (CarWheels[0].GetCollider().motorTorque > maxSpeed)
        {
            foreach (Wheels wheel in CarWheels)
            {
                wheel.GetCollider().motorTorque = maxSpeed;

            }
        }


    }

    private void Update()
    {
        UpdateAnimation();
        textMeshPro.text = $"current speed : {rb.velocity.magnitude * 3.6:F2} Km/h";
        CheckMaxSpeed();
    }
    private void FixedUpdate()
    {
        Drive(Vinputvalue);
        Steer(Hinputvalue);
        ApplyBraeks(Isbreaking);

    }
}
