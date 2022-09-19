using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    Vector2 movementVector;
    Vector3 rayOrgin;
    GameObject aboveHead;
    // Start is called before the first frame update
    void Start()
    {
        movementVector = Vector2.zero;
        aboveHead = transform.Find("aboveHead").gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Wspó³rzêdne gracza: " + transform.position.ToString());
        //Debug.Log("Wspó³rzêdne przed nami: " + getPositionInFront().ToString());
        //GameObject inFront = getObjectInFront();
        //if(inFront != null)
            //Debug.Log("Przed nami znajduje siê: " + inFront.ToString());
    }
    void FixedUpdate()
    {
        if(movementVector != Vector2.zero)
        {
            //we will be moving or rotating
            //check if something in front

            //calculate new ray orgin
            //rayOrgin = transform.position + Vector3.up * 2;

            //check if works as intended
            //RaycastHit hit;
            //Debug.DrawRay(rayOrgin, transform.TransformDirection(Vector3.forward) * 2, Color.yellow);
            //if (Physics.Raycast(rayOrgin, transform.TransformDirection(Vector3.forward), out hit, 2))
            //{
                
            //    Debug.Log("Trafi³em");
            //}
                
            //rotation left / right
            float rotateAngle = movementVector.x * 90;
            transform.Rotate(0, rotateAngle, 0);
            //moving front / back
            //moving front?
            if(movementVector.y > 0)
            {
                //we moving front
                //check if front empty
                if (getObjectInFront() == null)
                    transform.position += transform.forward * movementVector.y;
            } 
            else if(movementVector.y < 0)
            {
                //we moving back
                if(getObjectInTheBack() == null)
                    transform.position += transform.forward * movementVector.y;
            }
            
            

            movementVector = Vector2.zero;
        }
        
    }
    //get position directly in front of the player
    Vector3 getPositionInFront()
    {
        Vector3 position = transform.position + transform.forward;
        return position;
    }
    //get position directly in the back
    Vector3 getPositionInTheBack()
    {
        Vector3 position = transform.position - transform.forward;
        return position;
    }
    //get object directly in front of the player
    GameObject getObjectInFront()
    {
        //get all coliders in a sphere of radius 0.9 in front of the player
        Collider[] coliders = Physics.OverlapSphere(getPositionInFront(), 0.9f);
        if (coliders.Length > 0)
        {
            //quick and dirty - get first one
            return coliders[0].transform.gameObject;
        }
        else
        {
            return null;
        }
    }
    //get objet in the back
    GameObject getObjectInTheBack()
    {
        //get all coliders in a sphere of radius 0.9 in front of the player
        Collider[] coliders = Physics.OverlapSphere(getPositionInTheBack(), 0.9f);
        if (coliders.Length > 0)
        {
            //quick and dirty - get first one
            return coliders[0].transform.gameObject;
        }
        else
        {
            return null;
        }
    }
    void PickUpCrate(GameObject crate)
    {
        
        if(crate.transform.parent != null)
            if(crate.transform.parent.CompareTag("Stand"))
            {
                crate.transform.parent.GetComponent<BoxCollider>().enabled = true;
            }
        
        //zmiena hierarchii na scenie
        crate.transform.SetParent(aboveHead.transform, false);
        //zerowanie pozycji
        crate.transform.localPosition = Vector3.zero;

    }
    void PutDownCrate()
    {
        //czy mamy skrzynkê
        if(aboveHead.transform.childCount > 0)
        {
            GameObject crate = aboveHead.transform.GetChild(0).gameObject;
            GameObject stand = getObjectInFront();
            crate.transform.SetParent(stand.transform, false);
            stand.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
            
    }
    void OnMove(InputValue input)
    {
        movementVector = input.Get<Vector2>();
        //quick and dirty fix
        movementVector.x = (float)Math.Round(movementVector.x);
        movementVector.y = (float)Math.Round(movementVector.y);
        //Debug.Log(movementVector.ToString());
    }
    void OnFire()
    {
        //Debug.Log("Naciœniêto spust.");
        //RaycastHit hit;
        //sprawdz czy mamy przed soba cokolwiek
        //if (Physics.Raycast(rayOrgin, transform.TransformDirection(Vector3.forward), out hit, 2))
        //{
        //    //sprawdz czy to co mamy przed soba to jest skrzynka
        //   if(hit.transform.gameObject.CompareTag("Crate"))
        //    {
        //        PickUpCrate(hit.transform.gameObject);
        //    }
        //}
        
        GameObject inFront = getObjectInFront();
        if(inFront != null)
        {
            if(inFront.CompareTag("Crate"))
            {
                //przed nami skrzynka
                PickUpCrate(inFront);
            }
            if(inFront.CompareTag("Stand"))
            {
                //przed nami paleta
                PutDownCrate();
            }
        }
            
    }
}
