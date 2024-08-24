using UnityEngine;
[CreateAssetMenu(menuName = "cars/tires", order = 0)]
public class TireDataScriptable : ScriptableObject
{   // Forward Friction properties
    [Header("Forward friction")]
    [SerializeField] private float forwardExtremumSlip = 0.4f;
    [SerializeField] private float forwardExtremumValue = 1f;
    [SerializeField] private float forwardAsymptoteSlip = 0.8f;
    [SerializeField] private float forwardAsymptoteValue = 0.5f;
    [SerializeField] private float forwardStiffness = 1f;

    [Header("sideways friction")]
    // Sideways Friction properties
    [SerializeField] private float sidewaysExtremumSlip = 0.2f;
    [SerializeField] private float sidewaysExtremumValue = 1f;
    [SerializeField] private float sidewaysAsymptoteSlip = 0.5f;
    [SerializeField] private float sidewaysAsymptoteValue = 0.75f;
    [SerializeField] private float sidewaysStiffness = 1f;

    ///this function will be used to apply the properties of the tires 
    /// it will apply the values stored int it to the wheel collider(parameter)


    public void ApplyChanges(WheelCollider wheel)
    {
        WheelFrictionCurve frictionCurve = wheel.forwardFriction;
        frictionCurve.asymptoteSlip = forwardAsymptoteSlip;
        frictionCurve.asymptoteValue = forwardAsymptoteValue;
        frictionCurve.extremumValue = forwardExtremumValue;
        frictionCurve.extremumSlip = forwardExtremumSlip;
        frictionCurve.stiffness = forwardStiffness;
        wheel.forwardFriction = frictionCurve;

        WheelFrictionCurve SidewaysfrictionCurve = wheel.sidewaysFriction;
        SidewaysfrictionCurve.asymptoteSlip = sidewaysAsymptoteSlip;
        SidewaysfrictionCurve.asymptoteValue = sidewaysAsymptoteValue;
        SidewaysfrictionCurve.extremumSlip = sidewaysExtremumSlip;
        SidewaysfrictionCurve.extremumValue = sidewaysExtremumValue;
        SidewaysfrictionCurve.stiffness = sidewaysStiffness;
        wheel.sidewaysFriction = SidewaysfrictionCurve;
    }
}
