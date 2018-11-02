using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Constants
{

    public static double Altitude = 0.3;      //初始的腿部高度
    public static double []DIR_ = new double[4] { 0.03, 0.01,0.05,0.05} ;          //腿部运动与终点差别

}

public enum Mode
{

    Hold = 0,
    Stop,
    Walk,
    Run,

};

public enum LegID
{

    Leg_FL = 0,
    Leg_FR,
    Leg_BL,
    Leg_BR,
    None

};
public enum LegStatus
{

    Leg_FLRB_DOWNING = 0,
    Leg_FRLB_DOWNING,
    Leg_FLRB_UPING,
    Leg_FRLB_UPING,
    Leg_ALL_DOWN

};


public class IOW {


    public struct PML
    {

        public float xLen;
        public float yLen;
        public float zLen;

    };

    public Kinematics kinematics = new Kinematics();
    PID pid_H = new PID();   //HOLD 
    PID pid_W = new PID();   //WALK 
    PID pid_S = new PID();

    public LegStatus LegMode;
    Mode ModeTransition;

    public Mode IOWMode;

    LegID []LegChoose = new LegID[2];

  
    public float []LegYAltitude = new float[4] ;     //腿部初始高度


    //定义腿部的运动长度
    public PML[] HoldMotionLen = new PML[4];
    public PML[] StopMotionLen = new PML[4];
    public PML[] WalkMotionLen = new PML[4];
    public PML[] RunMotionLen = new PML[4];

    //定义运动目标点
    public Pos[] targetPos = new Pos[4];
    public Pos[] nowPos = new Pos[4];

    public Angle[] targetAngle = new Angle[4];
    public Angle[] nowAngle = new Angle[4];

    public Angle_V []angle_v = new Angle_V[4];

    float[] Ta = new float[4];
    float[] Tb = new float[4];


    public  void IdeaOfWalkingInit()        //启动初始化
    {
        
        PMLInit();

        LegMode = LegStatus.Leg_ALL_DOWN;

        ModeTransition = Mode.Stop;

        for (short i = 0; i < 4; i++)
        {
            LegYAltitude[i] = -(float)Constants.Altitude;

            targetPos[i].x = 0;
            targetPos[i].y = LegYAltitude[i];
            targetPos[i].z = 0;

            angle_v[i].swingleg_V = 400;
            angle_v[i].thign_V = 400;
            angle_v[i].calf_V = 400;

        }


        pid_H.PID_Init(0.15, 0, 0.02, 0.06,-0.05);
        pid_W.PID_Init( 0.15,0,0.02,0.05,-0.05);
        pid_S.PID_Init(0.04, 0, 0.02, 0.05, -0.05);

    }

    /// <summary>
    /// 腿部运动长度默认初始化
    /// </summary>
    void PMLInit()
    {   //默认步长初始化

        int c;

        for (c = 0; c < 4; c++)
        {

            HoldMotionLen[c].xLen = 0;
            HoldMotionLen[c].yLen = (float)0.1;
            HoldMotionLen[c].zLen = (float)0;

            StopMotionLen[c].xLen = 0;
            StopMotionLen[c].yLen = 0;
            StopMotionLen[c].zLen = 0;

            WalkMotionLen[c].xLen = (float)0.15;
            WalkMotionLen[c].yLen = (float)0.1;
            WalkMotionLen[c].zLen = (float)0;

            Ta[c] = (4 * WalkMotionLen[c].yLen) / (Mathf.Pow(WalkMotionLen[c].xLen, 2));
            Tb[c] = (float)(Constants.Altitude - WalkMotionLen[c].yLen);


            RunMotionLen[c].xLen = (float)0.1;
            RunMotionLen[c].yLen = (float)0.06;
            RunMotionLen[c].zLen = 0;

        }

    }


    public void IdeaOfWalking(Mode mode) {

        Angle angle;
        Pos pos;

        short i = 0, count = 0;

        ModeChangeTransition(mode); //运动模式过度

        switch (mode)
        {

            case Mode.Hold:    //function: watch, keep balance

                HoldCurve();

                for (; i < 4; i++)
                {

                    pos = kinematics.DK(nowAngle[i].swingleg,  nowAngle[i].thign, nowAngle[i].calf);


                    if (Mathf.Abs(pos.x - targetPos[i].x) <= Constants.DIR_[0] &&
                             Mathf.Abs(pos.y - targetPos[i].y) <= Constants.DIR_[0] &&
                        Mathf.Abs(pos.z - targetPos[i].z) <= Constants.DIR_[0])
                    {

                        count++;
                        continue;

                    }

                    pos.x += (float)pid_H.PD_cal(pos.x, targetPos[i].x);
                    pos.y += (float)pid_H.PD_cal(pos.y, targetPos[i].y);
                    pos.z += (float)pid_H.PD_cal(pos.z, targetPos[i].z);

                    angle = kinematics.IK(pos.x, pos.y, pos.z);
                    
                    angle = rad2deg(angle);
                   
                    
                    targetAngle[i].swingleg = angle.swingleg;
                    targetAngle[i].thign = angle.thign;
                    targetAngle[i].calf = angle.calf;

                }

                break;

            case Mode.Stop:    //function: watch, keep body balance

                StopCurve();

                for (; i < 4; i++)
                {
                    pos = kinematics.DK(nowAngle[i].swingleg, nowAngle[i].thign, nowAngle[i].calf);


                    if (Mathf.Abs(pos.x - targetPos[i].x) <= Constants.DIR_[1] &&
                             Mathf.Abs(pos.y - targetPos[i].y) <= Constants.DIR_[1] &&
                        Mathf.Abs(pos.z - targetPos[i].z) <= Constants.DIR_[1])
                    {

                        count++;
                        continue;

                    }

                    pos.x += (float)pid_S.PD_cal(pos.x, targetPos[i].x);
                    pos.y += (float)pid_S.PD_cal(pos.y, targetPos[i].y);
                    pos.z += (float)pid_S.PD_cal(pos.z, targetPos[i].z);

                    angle = kinematics.IK(pos.x, pos.y, pos.z);

                    angle = rad2deg(angle);

                    targetAngle[i].swingleg = angle.swingleg;
                    targetAngle[i].thign = angle.thign;
                    targetAngle[i].calf = angle.calf;

                }

                break;

            case Mode.Walk:    //function: watch, keep balance

                WalkCruve();

                for (; i < 4; i++)
                {

                    pos = kinematics.DK(nowAngle[i].swingleg, nowAngle[i].thign, nowAngle[i].calf);

                    if (Mathf.Abs(pos.x - targetPos[i].x) <= Constants.DIR_[2] &&
                        Mathf.Abs(pos.y - targetPos[i].y) <= Constants.DIR_[2]  &&
                         Mathf.Abs(pos.z - targetPos[i].z) <= Constants.DIR_[2])
                    {
                        count++;
                        continue;

                    }

                    pos.x += (float)pid_W.PD_cal(pos.x, targetPos[i].x);

                    if (i == (short)LegChoose[0] || i == (short)LegChoose[1])//抬腿动作
                    {

                        // pos.y += ((HoldMotionLen[i].yLen * Mathf.Cos(Mathf.PI / HoldMotionLen[i].xLen * pos.x)) - (float)Constants.Altitude);
                        pos.y = (-Ta[i] * Mathf.Pow(pos.x, 2) - Tb[i]);

                    }
                    else   //腿落地动作
                    {

                        pos.y += (float)0.6*(float)pid_W.PD_cal(pos.y, targetPos[i].y);
                    }

                    pos.z += (float)pid_W.PD_cal(pos.z, targetPos[i].z); 
  

                    angle = kinematics.IK(pos.x, pos.y, pos.z);

                    angle = rad2deg(angle);

                    targetAngle[i].swingleg = angle.swingleg;
                    targetAngle[i].thign = angle.thign;
                    targetAngle[i].calf = angle.calf;

                }


                break;

            case Mode.Run:     //function: keep balance

                break;
            default:      //Stop

                break;

        }

        if (count == 4)
        {

            LegModeChange(mode);
          
        }
    }


    /// <summary>
    /// Hold 模式下的腿部运动终点
    /// </summary>
    void HoldCurve() {

        short i = 0;

        if (LegMode == LegStatus.Leg_FLRB_UPING )
        {

            LegChoose[0] = LegID.Leg_FL;
            LegChoose[1] = LegID.Leg_BR;

        }
        else if (LegMode == LegStatus.Leg_FRLB_UPING )
        {

            LegChoose[0] = LegID.Leg_FR;
            LegChoose[1] = LegID.Leg_BL;

        } else if (LegMode == LegStatus.Leg_FLRB_DOWNING || LegMode == LegStatus.Leg_FRLB_DOWNING) {

        LegChoose[0] = LegID.None;
        LegChoose[1] = LegID.None;

        for (; i < 4; i++)
        {

            targetPos[i].x = 0;
            targetPos[i].y = LegYAltitude[i];

        }
        return;

        } else {

            LegChoose[0] = LegID.None;
            LegChoose[1] = LegID.None;

            return;
        }


        for (; i < 4; i++)
        {

            targetPos[i].x = 0;

            if (i == (short)LegChoose[0] || i == (short)LegChoose[1])
            {

                targetPos[i].y = LegYAltitude[i] + HoldMotionLen[i].yLen;
                targetPos[i].z = HoldMotionLen[i].zLen / 2;

            }
            else
            {
                targetPos[i].y = LegYAltitude[i];
                targetPos[i].z = -HoldMotionLen[i].zLen / 2;

            }
        }
    }


    /// <summary>
    /// Walk 模式下的腿部运动终点
    /// </summary>
    void WalkCruve()
    {

        short i = 0;

        if (LegMode == LegStatus.Leg_FLRB_UPING ) //Hold向Walk切换
        {  

            LegChoose[0] = LegID.Leg_FL;
            LegChoose[1] = LegID.Leg_BR;

        }
        else if (LegMode == LegStatus.Leg_FRLB_UPING  )
        {

            LegChoose[0] = LegID.Leg_FR;
            LegChoose[1] = LegID.Leg_BL;

        }
        else
        {

            LegChoose[0] = LegID.None;
            LegChoose[1] = LegID.None;

             return;

        }


        for (; i < 4; i++)
        {

            if (i == (short)LegChoose[0] || i == (short)LegChoose[1])
            {

                targetPos[i].x = -WalkMotionLen[i].xLen / 2;
                targetPos[i].y = LegYAltitude[i];
                targetPos[i].z = WalkMotionLen[i].zLen / 2;


            }
            else
            {

                targetPos[i].x = WalkMotionLen[i].xLen / 2;
                targetPos[i].y = LegYAltitude[i];
                targetPos[i].z = -WalkMotionLen[i].zLen / 2;

            }
        }
    }


    void StopCurve() {

        short i = 0;

        if (LegMode != LegStatus.Leg_FLRB_DOWNING || LegMode != LegStatus.Leg_FRLB_DOWNING)
        {

            LegChoose[0] = LegID.None;
            LegChoose[1] = LegID.None;

            for (; i < 4; i++)
            {

                targetPos[i].x = StopMotionLen[i].xLen;
                targetPos[i].y = LegYAltitude[i];
                targetPos[i].z = StopMotionLen[i].zLen;
            }
            return;
        }

    }




    /// <summary>
    /// 运动模式切换时的腿部模式过渡
    /// </summary>
    /// <param name="mode"></param>
    void ModeChangeTransition(Mode mode) {

        if(ModeTransition !=mode)
        {
            if (ModeTransition == Mode.Walk  && mode == Mode.Hold) {

                if (LegMode == LegStatus.Leg_FLRB_UPING) {


                    LegMode = LegStatus.Leg_FLRB_DOWNING;

                } else if (LegMode == LegStatus.Leg_FRLB_UPING) {

                    LegMode = LegStatus.Leg_FRLB_DOWNING;
                }
            }
        
        }
        ModeTransition = mode;

    }


    /// <summary>
    /// 腿部状态切换机
    /// </summary>
    /// <param name="mode"></param>
    void LegModeChange(Mode mode)
    {

        if (mode == Mode.Hold)
        {
            
            if (LegMode == LegStatus.Leg_FLRB_UPING)
            {

                LegMode = LegStatus.Leg_FLRB_DOWNING;

            }
            else if (LegMode == LegStatus.Leg_FLRB_DOWNING)
            {
          
                LegMode = LegStatus.Leg_FRLB_UPING;

            }
            else if (LegMode == LegStatus.Leg_FRLB_UPING)
            {
               
                LegMode = LegStatus.Leg_FRLB_DOWNING;

            }
            else if (LegMode == LegStatus.Leg_FRLB_DOWNING)
            {
             
                LegMode = LegStatus.Leg_FLRB_UPING;

            }
            else if (LegMode == LegStatus.Leg_ALL_DOWN)
            {
               
                LegMode = LegStatus.Leg_FLRB_UPING;
            }

        }
        else if (mode == Mode.Walk || mode == Mode.Run)
        {


            if (LegMode == LegStatus.Leg_FLRB_UPING || LegMode == LegStatus.Leg_FLRB_DOWNING)    //Hold向Walk切换使用
            {

                LegMode = LegStatus.Leg_FRLB_UPING;

            }
            else if (LegMode == LegStatus.Leg_FRLB_UPING || LegMode == LegStatus.Leg_FRLB_DOWNING)
            {
     
                LegMode = LegStatus.Leg_FLRB_UPING;

            }
            else if (LegMode == LegStatus.Leg_ALL_DOWN)
            {

                LegMode = LegStatus.Leg_FLRB_UPING;

            }

        }
        else if (mode == Mode.Stop)
        {

            LegMode = LegStatus.Leg_ALL_DOWN;

        }
        else
        {

            LegMode = LegStatus.Leg_ALL_DOWN;
        }
    }





    /// <summary>
    /// 腿部运动长度设置，主要由上层结构发送指令
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="xl"></param>
    /// <param name="yl"></param>
    /// <param name="zl"></param>
    public void PMLset(Mode mode, float xl,float yl,float zl) {    //上层控制机设置步长变化



    }


    /// <summary>
    /// 设置运动的腿部高度，姿态
    /// </summary>
    /// <param name="altitude"></param>
    public void setAltitude(float []altitude) {   //设置腿部基本高度参照


        for (short i =0; i < 4; i++)
        {
            LegYAltitude[i] = -altitude[i];
        }
    }



    public Angle rad2deg (Angle angle){

        Angle a;

        a.swingleg = Mathf.Rad2Deg * angle.swingleg;
        a.thign = Mathf.Rad2Deg * angle.thign;
        a.calf = Mathf.Rad2Deg * angle.calf;

        return a;
    }


}
