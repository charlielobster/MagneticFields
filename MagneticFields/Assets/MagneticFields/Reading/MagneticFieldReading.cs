using UnityEngine;


namespace MagneticFields.Reading
{
    public static class MagneticFieldReading
    {
        public static readonly Vector3 RawVector = Input.compass.rawVector;
        public static readonly float magneticHeading = Input.compass.magneticHeading;
    }
}
