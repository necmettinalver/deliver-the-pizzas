using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using DG.Tweening;

public class Cooker : MonoBehaviour
{

    [SerializeField] private Transform[] pizzasPlace = new Transform[12];
    [SerializeField] private GameObject pizza;
    [SerializeField] private float pizzaDeliveryTime, YAxis;

    void Start()
    {
        for (int i = 0; i < pizzasPlace.Length; i++)
        {
            pizzasPlace[i] = transform.GetChild(0).GetChild(i);
        }
        StartCoroutine(CookPizza(pizzaDeliveryTime));
    }

    public IEnumerator CookPizza(float delay)
    {
        var count_pizzas = 0;
        var pizza_index = 0;

        while (count_pizzas<100)
        {
            GameObject hotPizza = Instantiate(pizza, new Vector3(transform.position.x, -3f, transform.position.z),Quaternion.identity,transform.GetChild(1));

            hotPizza.transform.DOJump(new Vector3(pizzasPlace[pizza_index].position.x, pizzasPlace[pizza_index].position.y+YAxis, pizzasPlace[pizza_index].position.z),2f,1,0.5f).SetEase(Ease.OutQuad);

            if (pizza_index < 11)
            {
                pizza_index += 1;
            }
            else
            {
                pizza_index = 0;
                YAxis += 0.12f;
            }
            yield return new WaitForSecondsRealtime(delay);
        }
    }
}
