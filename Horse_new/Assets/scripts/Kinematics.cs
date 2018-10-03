using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Pos
{

    public float x;
    public float y;
    public float z;

};


public struct Angle
{

    public float swingleg;
    public float thign;
    public float calf;

};

public struct Angle_V {

    public float swingleg_V;
    public float thign_V;
    public float calf_V;

};



public static class LegLen { 


    public const double Leg_L1 = 0.2;
    public const double Leg_L2 = 0.2;


}


public class Kinematics : MonoBehaviour {


    public Angle IK(float x, float y, float z) {

        Angle a;

        float y_z, x_y_z, l1, l2;

        y_z = Mathf.Pow(y, 2) + Mathf.Pow(z, 2);
        x_y_z = Mathf.Pow(x, 2) + y_z;
        l1 = Mathf.Pow((float)LegLen.Leg_L1, 2);
        l2 = Mathf.Pow((float)LegLen.Leg_L2, 2);


        a.swingleg = Mathf.Acos(y / Mathf.Sqrt(y_z));
        a.thign = -Mathf.PI/2+Mathf.Atan2(Mathf.Sqrt(y_z) , x) - Mathf.Acos((float)((l2 - l1 - x_y_z) / (-2 * Mathf.Sqrt(x_y_z) * LegLen.Leg_L1)));
        a.calf = Mathf.Acos((float)((x_y_z - l1 - l2) / (2 * LegLen.Leg_L1 * LegLen.Leg_L2)));

        return a;
    }

    public Pos DK(float theta1, float theta2, float theta3)
    {

        Pos p;

        p.x = (float)(LegLen.Leg_L1 * Mathf.Sin(theta2) + LegLen.Leg_L2 * Mathf.Cos(theta2) * Mathf.Sin(theta3) + LegLen.Leg_L2 * Mathf.Cos(theta3) * Mathf.Sin(theta2));
        p.y = (float)(LegLen.Leg_L2 * Mathf.Sin(theta3) * Mathf.Cos(theta1) * Mathf.Sin(theta2) - LegLen.Leg_L1 * Mathf.Cos(theta1) * Mathf.Cos(theta2) - LegLen.Leg_L2 * Mathf.Cos(theta3) * Mathf.Cos(theta1) * Mathf.Cos(theta2));

        p.z = (float)(-LegLen.Leg_L2 * Mathf.Cos(theta3) * Mathf.Cos(theta2) * Mathf.Sin(theta1) - LegLen.Leg_L2 * Mathf.Sin(theta3) * Mathf.Sin(theta1) * Mathf.Sin(theta2) - LegLen.Leg_L1 * Mathf.Cos(theta2) * Mathf.Sin(theta1));

        return p;
    }



}
