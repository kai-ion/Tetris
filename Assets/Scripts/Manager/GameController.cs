using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    Layout layout;      //reference to layout script

    [SerializeField]
    Spawner spawner;    //refernce spawner script

    // Start is called before the first frame update
    void Start()
    {
        //add references to layout and spawner component
        layout = GameObject.FindWithTag("Layout").GetComponent<Layout>();
        spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        /**
        FindObjectOfType<s>, same function as above but slower
        **/
        //layout = GameObject.FindObjectOfType<Layout>();   //another find script method
        //spawner = GameObject.FindObjectOfType<Spawner>();   //another find script method

        /**
        incase for null elements
        **/
        if (spawner)
        {
            spawner.transform.position = Vectorf.Round(spawner.transform.position);
        }

        if(!layout)
        {
            Debug.LogWarning("No Layout Defined!");
        }

        if(!spawner)
        {
            Debug.LogWarning("No Spawner Defined!");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
