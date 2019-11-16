using UnityEngine;

// Static class to calculate values of Sine in 2D and 3D
namespace SineDemoWCalculations
{
    static class Sine
    {
        const float pi = Mathf.PI;

        public static float SineFunction(float x, float z, float time)
        {
            return Mathf.Sin(pi * (x + z + time));
        }

        public static float Sine2DFunction(float x, float z, float time)
        {
            float y = Mathf.Sin(pi * (x + time));
            y += Mathf.Sin(pi * (z + time));
            y *= 0.5f;
            return y;
        }

        public static float MultiSineFunction(float x, float z, float time)
        {
            float y = Mathf.Sin(pi * (x + time));
            y += Mathf.Sin(2f * pi * (x + 2f * time)) / 2f;
            y *= 2f / 3f;
            return y;
        }
    }
}