using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Anime : MonoBehaviour
{

    private Animator anim;
    public bool _bFlip;
    public bool _bBack_Flip;
    public bool _bIdle;
    void Start ()
    {
        anim = gameObject.GetComponent<Animator>();
        _bFlip = false;
        _bBack_Flip = false;
    }
	
	
	void Update ()
    {
        anim.SetBool("Flip", _bFlip);
        anim.SetBool("Back", _bBack_Flip);
        //anim.SetBool("Idle", _bIdle);
    }
}
