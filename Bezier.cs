/*
   ___ ___  ___   ___ ___ ___  _   _ ___    _   _        
  | _ \ _ \/ _ \ / __| __|   \| | | | _ \  /_\ | |       
  |  _/   / (_) | (__| _|| |) | |_| |   / / _ \| |__     
  |_| |_|_\\___/ \___|___|___/ \___/|_|_\/_/ \_\____|    
                                                        
 ___ __  __   _   ___ ___ _  _   _ _____ ___ ___  _  _ 
|_ _|  \/  | /_\ / __|_ _| \| | /_\_   _|_ _/ _ \| \| |
 | || |\/| |/ _ \ (_ || || .` |/ _ \| |  | | (_) | .` |
|___|_|  |_/_/ \_\___|___|_|\_/_/ \_\_| |___\___/|_|\_|
                                                           

                               BÉZIER CURVES CALCULATOR

                                   Author: Matteo Lusso
                                                 © 2020

*/

/*
 
This is a DLL for Unity that adds a Bezier curve calculator.

Compile to generate the DLL file.

*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DLL_BezierCurves
{
    public static class Bezier
    {
        // A simple implementation of the Bezier Curve from its general definition (https://en.wikipedia.org/wiki/B%C3%A9zier_curve#General_definition).
        public static List<Vector3> NewBezierCurve(List<Vector3> controlPoints, int curvePoints)
        {
            List<Vector3> bezierCurve = new List<Vector3>();

            for (int k = 0; k < curvePoints; k++)
            {
                float t = k / (float)(curvePoints - 1);
                bezierCurve.Add(CalculateNPoint(t, controlPoints));
            }

            return bezierCurve;
        }

        private static Vector3 CalculateNPoint(float t, List<Vector3> controlPoints)
        {

            Vector3 bezierPoint = Vector3.zero;
            int n = controlPoints.Count;

            for (int i = 0; i < n; i++)
            {
                long binCoeff = binomialCoeffFactorial2(n, i);

                bezierPoint += binCoeff * controlPoints[i] * (Mathf.Pow((1 - t), ((n - 1) - i)) * Mathf.Pow(t, i));
            }

            return bezierPoint;
        }

        // This is an inefficient way to calculate the binomial coefficients (n k).
        // But it's ok to generate a quadratic curve (4 control points or less).
        private static long binomialCoeffFactorial(int n, int k)
        {
            return (long)Factorial(n - 1) / (Factorial(k) * Factorial((n - 1) - k));
        }

        // Instead of calculating the binomial coefficient of n control points,
        // we can do it for n - 1 control points with the next formula.
        private static long binomialCoeffFactorial2(int n, int k)
        {
            return binomialCoeffFactorial(n - 1, k) + binomialCoeffFactorial(n - 1, k - 1);
        }

        private static int Factorial(int number)
        {
            int factNumber = 1;
            for (int i = 1; i <= number; i++)
            {
                factNumber *= i;
            }

            return factNumber;
        }

        // Generate a new Bezier Curve from another one with a fixed size of each segment.
        // Please note the two curves will not fit perfectly and the last point of the new curve
        // will not match with the last control point used to generate the first curve.
        public static List<Vector3> FixedSegmentLength(List<Vector3> rawBezierCurve, float segmentSize)
        {
            List<Vector3> fixedCurve = new List<Vector3>();

            fixedCurve.Add(rawBezierCurve[0]);

            int i = 0;
            int k = 1;

            while (k < rawBezierCurve.Count)
            {
                if ((rawBezierCurve[k] - fixedCurve[i]).magnitude < segmentSize)
                {
                    k++;
                }
                else
                {
                    fixedCurve.Add(fixedCurve[i] + ((rawBezierCurve[k] - fixedCurve[i]).normalized * segmentSize));
                    i++;
                }
            }

            return fixedCurve;
        }
    }
}
