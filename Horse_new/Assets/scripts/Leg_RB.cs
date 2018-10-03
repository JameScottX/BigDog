using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg_RB : MonoBehaviour {

    public GameObject Leg_rb1, Leg_rb2, Leg_rb3;

    short[] Leg_rb1_Init = { 0, 0, 300, 500 };
    short[] Leg_rb2_Init = { -45, -15, -300, 500 };
    short[] Leg_rb3_Init = { 10, 90, 300, 500 };


    void Leg_RB_Init()
    {

        HingeJoint hinge_ = Leg_rb1.GetComponent<HingeJoint>();

        JointLimits limits = hinge_.limits;
        JointMotor motor = hinge_.motor;

        limits.min = Leg_rb1_Init[0];
        limits.max = Leg_rb1_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_rb1_Init[2];
        motor.force = Leg_rb1_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;



        hinge_ = Leg_rb2.GetComponent<HingeJoint>();

        limits = hinge_.limits;
        motor = hinge_.motor;

        limits.min = Leg_rb2_Init[0];
        limits.max = Leg_rb2_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_rb2_Init[2];
        motor.force = Leg_rb2_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;



        hinge_ = Leg_rb3.GetComponent<HingeJoint>();

        limits = hinge_.limits;
        motor = hinge_.motor;

        limits.min = Leg_rb3_Init[0];
        limits.max = Leg_rb3_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_rb3_Init[2];
        motor.force = Leg_rb3_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;

    }


    void Start () {

        Leg_rb1 = GameObject.Find("leg_rb1");
        Leg_rb2 = GameObject.Find("leg_rb2");
        Leg_rb3 = GameObject.Find("leg_rb3");

        Leg_RB_Init();
    }

    public void Leg_RunSpeed(GameObject object_, float speed)
    {

        HingeJoint hinge_ = object_.GetComponent<HingeJoint>();

        JointMotor motor = hinge_.motor;

        motor.targetVelocity = speed;

        hinge_.motor = motor;

    }

    public float Leg_GetAngle(GameObject object_)
    {

        HingeJoint hinge_ = object_.GetComponent<HingeJoint>();

        float angle_rad = hinge_.angle * Mathf.Deg2Rad;

        return angle_rad;

    }

    public void Leg_RunAngle(GameObject object_, float angle_next, float speed)
    {

        HingeJoint hinge_ = object_.GetComponent<HingeJoint>();

        float angle_now = hinge_.angle;

        JointMotor motor = hinge_.motor;

        if (angle_next > angle_now)
        {

            motor.targetVelocity = speed;

        }
        else
        {

            motor.targetVelocity = -speed;

        }
        hinge_.motor = motor;
    }

    short LimitParam = 2;

    public void Leg_SetLimits(GameObject object_, float angle_lim, float speed)
    {

        HingeJoint hinge_ = object_.GetComponent<HingeJoint>();

        float angle_now = hinge_.angle;

        JointLimits limits = hinge_.limits;
        JointMotor motor = hinge_.motor;
        if (angle_lim >= angle_now + 0.001)
        {

            limits.max = angle_lim + LimitParam;
            limits.min = angle_now;
            motor.targetVelocity = speed;
        }
        else if (angle_lim + 0.001 < angle_now)
        {

            limits.min = angle_lim - LimitParam;
            limits.max = angle_now;
            motor.targetVelocity = -speed;
        }

        hinge_.limits = limits;
        hinge_.motor = motor;
    }



}
