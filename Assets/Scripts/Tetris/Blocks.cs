using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{
    public bool canRotate = true;   //check if block can rotate

    //movemethod
    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    public void MoveLeft()
    {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    public void RotateLeft()
    {
        if (canRotate)                  // only rotate if it can 
        {
            transform.Rotate(0, 0 , -90);
        }
    }

    public void RotateRight()
    {
        if (canRotate)                  // only rotate if it can 
        {
            transform.Rotate(0, 0 , 90);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("MoveDown", 0, 0.5f);       //calls down method and make block fall every .5 sec        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
