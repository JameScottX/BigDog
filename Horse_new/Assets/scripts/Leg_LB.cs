using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leg_LB : MonoBehaviour {

    public GameObject Leg_lb1, Leg_lb2, Leg_lb3;

    //min_angle ,max_angle ,velocity ,force
    short[] Leg_lb1_Init = { 0, 0, 300, 500 };
    short[] Leg_lb2_Init = { -45, -15, -300, 500 };
    short[] Leg_lb3_Init = { 10, 90, 300, 500 };


    void Leg_LB_Init() {

        HingeJoint hinge_ = Leg_lb1.GetComponent<HingeJoint>();

        JointLimits limits = hinge_.limits;
        JointMotor motor = hinge_.motor;

        limits.min = Leg_lb1_Init[0];
        limits.max = Leg_lb1_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_lb1_Init[2];
        motor.force = Leg_lb1_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;



        hinge_ = Leg_lb2.GetComponent<HingeJoint>();

        limits = hinge_.limits;
        motor = hinge_.motor;

        limits.min = Leg_lb2_Init[0];
        limits.max = Leg_lb2_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_lb2_Init[2];
        motor.force = Leg_lb2_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;



        hinge_ = Leg_lb3.GetComponent<HingeJoint>();

        limits = hinge_.limits;
        motor = hinge_.motor;

        limits.min = Leg_lb3_Init[0];
        limits.max = Leg_lb3_Init[1];
        hinge_.useLimits = true;
        hinge_.limits = limits;

        motor.targetVelocity = Leg_lb3_Init[2];
        motor.force = Leg_lb3_Init[3];
        motor.freeSpin = false;
        hinge_.useMotor = true;
        hinge_.motor = motor;


    }
    void Start () {

        Leg_lb1 = GameObject.Find("leg_lb1");
        Leg_lb2 = GameObject.Find("leg_lb2");
        Leg_lb3 = GameObject.Find("leg_lb3");
        Leg_LB_Init();
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

        float angle = 0;
        float angle2 = 0;

        if (object_ == Leg_lb1)
        {
            //HingeJoint hinge_ = object_.GetComponent<HingeJoint>();

            angle = object_.transform.localEulerAngles.x;

        }
        else if (object_ == Leg_lb2)
        {

            angle = object_.transform.localEulerAngles.z - 360;


        }
        else if (object_ == Leg_lb3)
        {

            angle2 = 360 - Leg_lb2.transform.localEulerAngles.z;
            angle = angle2 + object_.transform.localEulerAngles.z;

        }

        float angle_rad = angle * Mathf.Deg2Rad;  //rad标准化

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


    

    public void Leg_SetLimits(GameObject object_, float angle_lim, float speed)
    {

        HingeJoint hinge_ = object_.GetComponent<HingeJoint>();

        float angle_now = hinge_.angle;

        JointLimits limits = hinge_.limits;
        JointMotor motor = hinge_.motor;
        if (angle_lim >= angle_now + 0.2)
        {

            limits.max = angle_lim ;
            limits.min = angle_now ;
            motor.targetVelocity = speed;
        }
        else if (angle_lim  < angle_now + 0.2)
        {

            limits.min = angle_lim ;
            limits.max = angle_now;
            motor.targetVelocity = -speed;
        }

        hinge_.limits = limits;
        hinge_.motor = motor;
    }


}
