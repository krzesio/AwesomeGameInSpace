﻿using UnityEngine;
using System.Collections;

public class TrapInteractive : Interactive {

    public Trap trap;
<<<<<<< HEAD
    private int TrapActivationPrice
    {
        get
        {
            return trap.price;
        }
    }
=======
    public int TrapActivationPrice;
>>>>>>> 1b7534a8dfb5d6db3eb28fc65def62bc9544a0be
    public override string message
    {
        get
        {
            if (trap.used)
            {
                if (PlayerStats.instance.syf >= TrapActivationPrice) return "Hold E to activate trap for " + TrapActivationPrice.ToString() + " Syf";
                else return "Insufficient Syf";

            }
            else if (!trap.used && !trap.triggered) return "Trap is ready";
            else return null;
        }
    }


    //public override void MomentaryAction()
    //{
    //    if(PlayerStats.instance.syf >= TrapActivationPrice)
    //    {
    //        PlayerStats.instance.syf -= TrapActivationPrice;
    //        trap.used = false;
    //        StartCoroutine(trap.activate());
    //    }
    //}


    public override void HoldAction()
    {
        if (PlayerStats.instance.syf >= TrapActivationPrice)
        {
            PlayerStats.instance.syf -= TrapActivationPrice;
            trap.used = false;
            StartCoroutine(trap.activate());
        }
    }
<<<<<<< HEAD
	

=======
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
>>>>>>> 1b7534a8dfb5d6db3eb28fc65def62bc9544a0be
}
