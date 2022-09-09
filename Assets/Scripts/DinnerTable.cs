using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinnerTable : MonoBehaviour                                         
{
    public Animator woman_customer_anim;
    public Animator man_customer_anim;

    public void Eat()
    {
        woman_customer_anim.SetBool("isEating",true);
        man_customer_anim.SetBool("isEating", true);
        InvokeRepeating("DOSumbitPizza", 2f, 1f);
    }

    void DOSumbitPizza()
    {
        if (transform.childCount>0)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject, 1f);
        }
        else
        {
            woman_customer_anim.SetBool("isEating", false);
            man_customer_anim.SetBool("isEating", false);
        }
    }
}
