using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    GameObject target_Leg_LF;
    GameObject target_Leg_RF;
    GameObject target_Leg_LB;
    GameObject target_Leg_RB;

    Leg_LF leg_lf;
    Leg_RF leg_rf;
    Leg_LB leg_lb;
    Leg_RB leg_rb;

    IOW iow;

    Mode IOWMode;


    void Start()
    {

        target_Leg_LF = GameObject.Find("leg_lf");
        target_Leg_RF = GameObject.Find("leg_rf");
        target_Leg_LB = GameObject.Find("leg_lb");
        target_Leg_RB = GameObject.Find("leg_rb");

        leg_lf = target_Leg_LF.GetComponent<Leg_LF>();
        leg_rf = target_Leg_RF.GetComponent<Leg_RF>();
        leg_lb = target_Leg_LB.GetComponent<Leg_LB>();
        leg_rb = target_Leg_RB.GetComponent<Leg_RB>();

        iow = new IOW();

        iow.IdeaOfWalkingInit();

        IOWMode = Mode.Stop;  //初始化暂停

    }

    private void OnGUI()
    {
        short i = 0;
        if (GUI.Button(new Rect(10, 20, 60, 60), "Walk"))
        {
            IOWMode = Mode.Walk;
            for (; i < 4; i++)
            {

                iow.HoldMotionLen[i].zLen = 0;
                iow.WalkMotionLen[i].zLen = 0;
            }

        } else if (GUI.Button(new Rect(10, 90, 60, 60), "Hold")) {

            IOWMode = Mode.Hold;
            for (; i < 4; i++)
            {

                iow.HoldMotionLen[i].zLen = 0;
                iow.WalkMotionLen[i].zLen = 0;
            }

        }
        else if (GUI.Button(new Rect(10, 160, 60, 60), "Stop")) {

            IOWMode = Mode.Stop;

            for (; i < 4; i++)
            {

                iow.HoldMotionLen[i].zLen = 0;
                iow.WalkMotionLen[i].zLen = 0;
            }


        } else if (GUI.Button(new Rect(10, 230, 60, 60), "Left")) {

            for (;i<4;i++) {

                iow.HoldMotionLen[i].zLen = (float)0.07;
                iow.WalkMotionLen[i].zLen = (float)0.07;
            }


        } else if (GUI.Button(new Rect(80, 230, 60, 60), "Right")) {

            for (; i < 4; i++)
            {

                iow.HoldMotionLen[i].zLen = -(float)0.07;
                iow.WalkMotionLen[i].zLen = -(float)0.07;
            }
        }

            
    }

    void FixedUpdate()
    {

        short i = 0;
        //Pos pos,pos2;
        //Angle angle;

        iow.nowAngle[(short)LegID.Leg_FL] = Leg_LFGetAngle();
        iow.nowAngle[(short)LegID.Leg_FR] = Leg_RFGetAngle();
        iow.nowAngle[(short)LegID.Leg_BL] = Leg_LBGetAngle();
        iow.nowAngle[(short)LegID.Leg_BR] = Leg_RBGetAngle();


        //pos2 = iow.kinematics.DK(iow.nowAngle[0].swingleg, iow.nowAngle[0].thign, iow.nowAngle[0].calf);
        //pos.x = (float)0.2;
        //pos.y = -(float)0;
        //pos.z = 0;

        //angle = iow.kinematics.IK(pos.x, pos.y, pos.z);

        //angle = iow.rad2deg(angle);

        ////Debug.Log();
        //angle.swingleg = angle.swingleg - 180;

        //Debug.Log(angle.swingleg.ToString() + "*" + angle.thign.ToString() + "*" + angle.calf.ToString());
        //Debug.Log(pos2.x.ToString() + "+" + pos2.y.ToString() + "+" + pos2.z.ToString());


        iow.IdeaOfWalking(IOWMode);


        i = (short)LegID.Leg_FL;


        if (iow.targetPos[i].z == 0)
        {

            iow.targetAngle[i].swingleg = 0;
        }
        else
        {
            if (iow.targetPos[i].z > 0) iow.targetAngle[i].swingleg = iow.targetAngle[i].swingleg - 180; //此处坑壁
            else iow.targetAngle[i].swingleg = 180 - iow.targetAngle[i].swingleg;
        }

        Leg_LFRunAngle(iow.targetAngle[i]);



        //Debug.Log(iow.targetAngle[i].thign); //此处有坑


        i = (short)LegID.Leg_FR;


        if (iow.targetPos[i].z == 0)
        {

            iow.targetAngle[i].swingleg = 0;
        }
        else
        {
            if (iow.targetPos[i].z < 0) iow.targetAngle[i].swingleg = -iow.targetAngle[i].swingleg + 180; //此处坑壁
            else iow.targetAngle[i].swingleg = iow.targetAngle[i].swingleg - 180;
        }

        Leg_RFRunAngle(iow.targetAngle[i]);




        i = (short)LegID.Leg_BL;


        if (iow.targetPos[i].z == 0)
        {

            iow.targetAngle[i].swingleg = 0;
        }
        else
        {
            if (iow.targetPos[i].z < 0) iow.targetAngle[i].swingleg = -iow.targetAngle[i].swingleg + 180; //此处坑壁
            else iow.targetAngle[i].swingleg = iow.targetAngle[i].swingleg - 180;
        }

        Leg_LBRunAngle(iow.targetAngle[i]);


        i = (short)LegID.Leg_BR;

        if (iow.targetPos[i].z == 0)
        {

            iow.targetAngle[i].swingleg = 0;
        }
        else
        {
            if (iow.targetPos[i].z > 0) iow.targetAngle[i].swingleg = iow.targetAngle[i].swingleg - 180; //此处坑壁
            else iow.targetAngle[i].swingleg = 180 - iow.targetAngle[i].swingleg;
        }

        Leg_RBRunAngle(iow.targetAngle[i]);

    }


    Angle Leg_LFGetAngle()
    {
        Angle angle;

        angle.swingleg = leg_lf.Leg_GetAngle(leg_lf.Leg_lf1);//rad
        angle.thign = -leg_lf.Leg_GetAngle(leg_lf.Leg_lf2);
        angle.calf = -leg_lf.Leg_GetAngle(leg_lf.Leg_lf3);

        return angle;

    }


    Angle Leg_RFGetAngle()
    {
        Angle angle;

        angle.swingleg = leg_rf.Leg_GetAngle(leg_rf.Leg_rf1);
        angle.thign = - leg_rf.Leg_GetAngle(leg_rf.Leg_rf2);
        angle.calf = -leg_rf.Leg_GetAngle(leg_rf.Leg_rf3);

        return angle;
    }


    Angle Leg_LBGetAngle() {

        Angle angle;


        angle.swingleg = leg_lb.Leg_GetAngle(leg_lb.Leg_lb1);
        angle.thign = -leg_lb.Leg_GetAngle(leg_lb.Leg_lb2);
        angle.calf = -leg_lb.Leg_GetAngle(leg_lb.Leg_lb3);

        return angle;

    }

    Angle Leg_RBGetAngle() {

        Angle angle;

        angle.swingleg = leg_rb.Leg_GetAngle(leg_rb.Leg_rb1);
        angle.thign = -leg_rb.Leg_GetAngle(leg_rb.Leg_rb2);
        angle.calf = -leg_rb.Leg_GetAngle(leg_rb.Leg_rb3);

        return angle;

    }


    void Leg_LFRunAngle (Angle angle)
    {

        short i = (short)LegID.Leg_FL;

        leg_lf.Leg_SetLimits(leg_lf.Leg_lf1,angle.swingleg,iow.angle_v[i].swingleg_V );
        leg_lf.Leg_SetLimits(leg_lf.Leg_lf2, angle.thign, iow.angle_v[i].thign_V);
        leg_lf.Leg_SetLimits(leg_lf.Leg_lf3,angle.calf, iow.angle_v[i].calf_V);


    }


    void Leg_RFRunAngle(Angle angle) {

        short i = (short)LegID.Leg_FR;

        leg_rf.Leg_SetLimits(leg_rf.Leg_rf1, angle.swingleg, iow.angle_v[i].swingleg_V );
        leg_rf.Leg_SetLimits(leg_rf.Leg_rf2, angle.thign, iow.angle_v[i].thign_V);
        leg_rf.Leg_SetLimits(leg_rf.Leg_rf3, angle.calf, iow.angle_v[i].calf_V);

    }


    void Leg_RBRunAngle(Angle angle) {


        short i = (short)LegID.Leg_BR;


        leg_rb.Leg_SetLimits(leg_rb.Leg_rb1, angle.swingleg, iow.angle_v[i].swingleg_V );
        leg_rb.Leg_SetLimits(leg_rb.Leg_rb2, angle.thign, iow.angle_v[i].thign_V);
        leg_rb.Leg_SetLimits(leg_rb.Leg_rb3, angle.calf, iow.angle_v[i].calf_V);

    }

    void Leg_LBRunAngle(Angle angle) {

        //Angle Limits;
        short i = (short)LegID.Leg_BL;

        //Limits = iow.kinematics.IK(iow.targetPos[i].x, iow.targetPos[i].y, iow.targetPos[i].z);
        //Limits = iow.rad2deg(Limits);
        //Limits.swingleg = - Limits.swingleg + 180;
        //leg_lb.Leg_SetLimits(leg_lb.Leg_lb1, Limits.swingleg);
        //leg_lb.Leg_SetLimits(leg_lb.Leg_lb2, Limits.thign);
        //leg_lb.Leg_SetLimits(leg_lb.Leg_lb3, Limits.calf);

        leg_lb.Leg_SetLimits(leg_lb.Leg_lb1, angle.swingleg, iow.angle_v[i].swingleg_V);
        leg_lb.Leg_SetLimits(leg_lb.Leg_lb2, angle.thign, iow.angle_v[i].thign_V);
        leg_lb.Leg_SetLimits(leg_lb.Leg_lb3, angle.calf, iow.angle_v[i].calf_V);


    }






}
