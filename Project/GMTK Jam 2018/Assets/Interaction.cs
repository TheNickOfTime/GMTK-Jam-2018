using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject plate, trash, plant, dog, stereo;
    private void Start()
    {
        plate.SetActive(false);
        trash.SetActive(false);
        plant.SetActive(false);
        dog.SetActive(false);
        stereo.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Plate")
        {
            plate.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("I've broken a plate!");
            }
                       
        }
        else
        {
            plate.SetActive(false);
        }
        if (collision.gameObject.tag == "Trash" )
        {
            //trash.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("I farted near the trashcan.");
            }
        }
        else
        {
            trash.SetActive(false);
        }
        if (collision.gameObject.tag == "Plant")
        {
            //plant.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("I've pooped in the plant!");            
            }
        }
        else
        {
            plant.SetActive(false);
        }
        if (collision.gameObject.tag == "Dog")
        {
            //dog.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("I'm blaming my farts on Spike here.");            
            }
        }
        else
        {
            dog.SetActive(false);
        }
        if (collision.gameObject.tag == "Stereo")
        {
            stereo.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Turning the music louder so I can let out some farts.");            
            }
        }
        else
        {
            stereo.SetActive(false);
        }
    }
}
