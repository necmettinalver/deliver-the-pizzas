using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class playerManager : MonoBehaviour
{
    private Vector3 direction;
    private Camera cam;
    [SerializeField] private float moveSpeed;
    private Animator maleAnimation;
    [SerializeField] private List<Transform> pizzas = new List<Transform>();
    [SerializeField] private Transform pizzaPlace;


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
            maleAnimation.SetBool("isRun", true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            maleAnimation.SetBool("isRun", false);
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
            if (hit.collider.CompareTag("table") && pizzas.Count < 21)
            {
                if (hit.collider.transform.childCount > 2)
                {
                    var pizza = hit.collider.transform.GetChild(1);
                    pizzas.Add(pizza);
                    pizza.parent = null;
                }

            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
        }
    }
}
