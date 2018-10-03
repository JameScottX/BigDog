using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg_RF : MonoBehaviour {

    public GameObject Leg_rf1, Leg_rf2, Leg_rf3;

    //min_angle ,max_angle ,velocity ,force
    short[] Leg_rf1_Init = { 0, 0, 300, 500 };
    short[] Leg_rf2_Init = { -45, -15, -300, 500 };
    short[] Leg_rf3_Init = { 10, 90, 300, 500 };

    void Leg_RF_Init() {

        HingeJoint hinge_ = Leg_rf1.GetComponent<HingeJoint>();

        JointLimits limits = hinge_.limits;
        JointMotor motor = hinge_.motor;

        limits.min = Leg_rf1_Init[0];
        limits.max = Leg_rf1_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_rf1_Init[2];
        motor.force = Leg_rf1_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;



        hinge_ = Leg_rf2.GetComponent<HingeJoint>();

        limits = hinge_.limits;
        motor = hinge_.motor;

        limits.min = Leg_rf2_Init[0];
        limits.max = Leg_rf2_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_rf2_Init[2];
        motor.force = Leg_rf2_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;



        hinge_ = Leg_rf3.GetComponent<HingeJoint>();

        limits = hinge_.limits;
        motor = hinge_.motor;

        limits.min = Leg_rf3_Init[0];
        limits.max = Leg_rf3_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_rf3_Init[2];
        motor.force = Leg_rf3_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;

    }


    void Start () {

        Leg_rf1 = GameObject.Find("leg_rf1");
        Leg_rf2 = GameObject.Find("leg_rf2");
        Leg_rf3 = GameObject.Find("leg_rf3");

        Leg_RF_Init();
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

        float angle_rad = hinge_.angle * Mathf.Deg2Rad;  //rad标准化

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
