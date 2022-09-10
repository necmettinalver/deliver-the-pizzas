using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DinnerTable : MonoBehaviour                                         
{
    public Animator woman_customer_anim;
    public Animator man_customer_anim;

    [SerializeField] private Transform moneyPlace;
    [SerializeField] private GameObject money;
    private float YAxis;
    private IEnumerator makeMoneyIE;
    int moneyPlaceIndex = 0;

    private void Start()
    {
        //makeMoneyIE = MakeMoney();
    }
    public void Eat()
    {
        woman_customer_anim.SetBool("isEating",true);
        man_customer_anim.SetBool("isEating", true);
        InvokeRepeating("DOSumbitPizza", 2f, 1f);

       // StartCoroutine(makeMoneyIE);
    }
    /*
    private IEnumerator MakeMoney()
    {
        var counter = 0;
        var moneyPlaceIndex = 0;

        yield return new WaitForSecondsRealtime(2);

        while (counter<transform.childCount)
        {
            GameObject newMoney = Instantiate(money, new Vector3(moneyPlace.GetChild(moneyPlaceIndex).position.x, YAxis, moneyPlace.GetChild(moneyPlaceIndex).position.z), money.transform.rotation);
            
            //newMoney.transform.DOScale(new Vector3(8f, 8f, 8f), 0.5f).SetEase(Ease.OutElastic);
            if (moneyPlaceIndex<moneyPlace.childCount - 1)
            {
                moneyPlaceIndex++;
            }
            else
            {
                moneyPlaceIndex = 0;
                YAxis += 0.2f;
            }
            yield return new WaitForSecondsRealtime(3f);
        }
    }
    */
    void DOSumbitPizza()
    {
        moneyPlaceIndex = 0;
        if (transform.childCount>0)
        {
            Destroy(transform.GetChild(transform.childCount - 1).gameObject, 1f);
            GameObject newMoney = Instantiate(money, new Vector3(moneyPlace.GetChild(moneyPlaceIndex).position.x, YAxis, moneyPlace.GetChild(moneyPlaceIndex).position.z), money.transform.rotation);

            if (moneyPlaceIndex < moneyPlace.childCount - 1)
            {
                moneyPlaceIndex++;
            }
            else
            {
                moneyPlaceIndex = 0;
                YAxis += 0.2f;
            }
        }
        else
        {
            woman_customer_anim.SetBool("isEating", false);
            man_customer_anim.SetBool("isEating", false);

            var desk = transform.parent;
            desk.GetChild(desk.childCount - 1).GetComponent<Renderer>().enabled = true;
            //StopCoroutine(makeMoneyIE);
            YAxis = 0f;
        }
    }
}
