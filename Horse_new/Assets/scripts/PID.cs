using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PID {


    public double K_,I_,D_;
    double error_last = 0;
    public double OutMax , OutMin;


    public void PID_Init(double k, double i, double d, double outmax,double outmin) {

        K_ = k;
        I_ = i;
        D_ = d;

        OutMax = outmax;
        OutMin = outmin;


     }

    public double  PD_cal(double  Input, double Val) {

        double output_;

        double error_new = Val - Input;

        double Kout = K_ * error_new;

        double Dout = D_ * (error_new - error_last);

        error_last = error_new;

        output_ = Kout + Dout;

        if (output_ > OutMax) {

            output_ = OutMax;

        } else if (output_ < OutMin) {

            output_ = OutMin;
        }

        return output_;
    }

}
