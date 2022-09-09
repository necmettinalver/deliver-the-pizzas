using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;



public class playerManager : MonoBehaviour
{
    private Vector3 direction;
    private Camera cam;
    [SerializeField] private float moveSpeed;
    private Animator maleAnimation;
    [SerializeField] private List<Transform> pizzas = new List<Transform>();
    [SerializeField] private Transform pizzaPlace;
    private float YAxis, delay;

    void Start()
    {
        cam = Camera.main;
        maleAnimation = GetComponent<Animator>();
        pizzas.Add(pizzaPlace);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, transform.position);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (plane.Raycast(ray, out var distance))
                direction = ray.GetPoint(distance);

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(direction.x, 0f, direction.z), moveSpeed*Time.deltaTime );

            var offset = direction - transform.position;

            if(offset.magnitude>1f)
                transform.LookAt(direction);

        }

        if (Input.GetMouseButtonDown(0))
        {
            
            if (pizzas.Count > 1)
            {
                maleAnimation.SetBool("isCarryRun", true);
                maleAnimation.SetBool("isCarry", false);
            }
            else
            {
                maleAnimation.SetBool("isRun", true);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            maleAnimation.SetBool("isRun", false);

            if (pizzas.Count>1)
            {
                maleAnimation.SetBool("isCarryRun", false);
                maleAnimation.SetBool("isCarry", true);
            }
        }

        if (pizzas.Count>1)
        {
            for (int i = 1; i < pizzas.Count; i++)
            {
                var firstPizza = pizzas.ElementAt(i - 1);
                var secondPizza = pizzas.ElementAt(i);
            
                secondPizza.position = new Vector3(Mathf.Lerp(secondPizza.position.x, firstPizza.position.x, Time.deltaTime * 15f), Mathf.Lerp(secondPizza.position.y, firstPizza.position.y+0.17f, Time.deltaTime * 15f),firstPizza.position.z);


            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out var hit, 1f))
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.green);
            if (hit.collider.CompareTag("table") && pizzas.Count < 14)
            {
                if (hit.collider.transform.childCount > 2)
                {
                    var pizza = hit.collider.transform.GetChild(1);
                    pizzas.Add(pizza);
                    pizza.parent = null;

                    if (hit.collider.transform.parent.GetComponent<Cooker>().count_pizzas>1)
                    {
                        hit.collider.transform.parent.GetComponent<Cooker>().count_pizzas--;
                    }

                    if (hit.collider.transform.parent.GetComponent<Cooker>().YAxis>0f)
                    {
                        hit.collider.transform.parent.GetComponent<Cooker>().YAxis -= 0.12f;
                    }
                    maleAnimation.SetBool("isCarry", true);
                    maleAnimation.SetBool("isRun",false);
                }

            }
            if (hit.collider.CompareTag("pizzaPlace")&& pizzas.Count>1)
            {
                var dinner_table = hit.collider.transform;
                if (dinner_table.childCount>0)
                {
                    YAxis = dinner_table.GetChild(dinner_table.childCount - 1).position.y;
                }
                else
                {
                    YAxis = dinner_table.position.y;
                }

                for (var index = pizzas.Count - 1 ; index>=1; index--)
                {
                    pizzas[index].DOJump(new Vector3(dinner_table.position.x,YAxis,dinner_table.position.z),2f,1,0.5f).SetDelay(delay).SetEase(Ease.Flash);

                    pizzas.ElementAt(index).parent = dinner_table;
                    pizzas.RemoveAt(index);
                    
                    YAxis += 0.12f;
                    delay += 0.02f;
                }
                dinner_table.parent.GetChild(dinner_table.parent.childCount - 1).GetComponent<Renderer>().enabled = false;

                if (pizzas.Count<=1)
                {
                    
                    maleAnimation.SetBool("idle", true);
                    maleAnimation.SetBool("isCarryRun", false);
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pizzaPlace"))
        {
            other.GetComponent<DinnerTable>().Eat();
        }
         
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("pizzaPlace"))
        {
            //maleAnimation.SetBool("isCarry", false);
            maleAnimation.SetBool("isCarryRun", false);
            maleAnimation.SetBool("idle", false); 
            maleAnimation.SetBool("isRun", true);
            delay = 0f;
        }

        if (other.CompareTag("table"))
        {
            if(pizzas.Count>1)
            {
                maleAnimation.SetBool("isCarry", false);
                maleAnimation.SetBool("isCarryRun", true);
            }
            else
            {
                maleAnimation.SetBool("isRun", true);
            }
        }
    }
}
