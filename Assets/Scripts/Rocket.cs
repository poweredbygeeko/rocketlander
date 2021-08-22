using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {


    void Start() {

    }

    void Update() {
        checkSpeed();
        move();
    }

    public static Vector3 getSize() {

        float bottomPoint = GameObject.Find("Wing_2").transform.localPosition.y - GameObject.Find("Wing_2").GetComponent<Renderer>().bounds.size.y / 2;
        float topPoint = GameObject.Find("Head").transform.localPosition.y + GameObject.Find("Head").GetComponent<Renderer>().bounds.size.y / 2;

        float height = topPoint - bottomPoint;

        float leftPoint = GameObject.Find("Wing_0").transform.localPosition.x - GameObject.Find("Wing_0").GetComponent<Renderer>().bounds.size.x / 2;
        float rightPoint = GameObject.Find("Wing_2").transform.localPosition.x + GameObject.Find("Wing_2").GetComponent<Renderer>().bounds.size.x / 2;

        float width = rightPoint - leftPoint;

        return new Vector3(width, height, width);
    }


    public static bool isAccelerating() {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) {
            return true;
        }
        return false;
    }

    void checkSpeed() {

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) {
            if (Controller.currentSpeed < Controller.maxSpeed) {
                if (Controller.currentSpeed < 0) {
                    Controller.currentSpeed = Controller.acceleration;
                } else {
                    Controller.currentSpeed += Controller.acceleration;
                }
            }
        } else {
            if (Controller.currentSpeed > 0) {
                Controller.currentSpeed = -Controller.acceleration;
            } else {
                if (Controller.isStarted) {
                    if (Controller.currentSpeed > Controller.maxFallSpeed) {
                        Controller.currentSpeed -= Controller.acceleration;
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            if (this.transform.rotation.eulerAngles.z > 180 + 45 / 2 || this.transform.rotation.eulerAngles.z < 180) {
                this.transform.Rotate(-this.transform.forward, Controller.rotationSpeed, Space.World);
            }
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            if (this.transform.rotation.eulerAngles.z < 180 - 45 / 2 || this.transform.rotation.eulerAngles.z > 180) {
                this.transform.Rotate(this.transform.forward, Controller.rotationSpeed, Space.World);
            }
        }
    }

    void move() {

        if (!isAccelerating()) {
            if (Mathf.Abs(this.transform.rotation.eulerAngles.z) > Controller.rotationFallSpeed) {
                if (this.transform.rotation.z > 0) {
                    this.transform.Rotate(this.transform.forward, -Controller.rotationFallSpeed, Space.World);
                } else if (this.transform.rotation.z < 0) {
                    this.transform.Rotate(this.transform.forward, Controller.rotationFallSpeed, Space.World);
                }
            }
            this.transform.Translate(0, Controller.currentSpeed * Time.deltaTime, 0, Space.World);
        } else {
            if(!Controller.isStarted) {
                Controller.isStarted = true;
            }
            this.transform.Translate(0, Controller.currentSpeed * Time.deltaTime, 0);
        }
    }
}
