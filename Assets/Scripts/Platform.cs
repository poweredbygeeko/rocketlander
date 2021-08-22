using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

    float maxFallSpeed = Controller.maxFallSpeed;

    void Start() {

    }

    void Update() {

    }

    
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.transform.parent != null) {
            if(collision.gameObject.transform.parent.name.Equals("Rocket") || collision.gameObject.transform.parent.parent.name.Equals("Rocket")) {
                Controller.maxFallSpeed = 0;
                Controller.currentSpeed = 0;
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        Controller.maxFallSpeed = maxFallSpeed;
    }
}
