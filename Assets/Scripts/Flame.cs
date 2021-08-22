using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {

    float maxScale = 1.75f;
    float scaleIncrement = .1f;

    void Start() {

    }

    void Update() {
        if (!Controller.isGameOver) {
            setSize();
            setPosition();
        }
    }

   

    private void setPosition() {

        this.transform.rotation = GameObject.Find("Rocket").transform.rotation;

        Vector3 rocketPos = GameObject.Find("Rocket").transform.position;
        float differenceY = Controller.Sizes.flame.y / 3;
        Vector3 difference = GameObject.Find("Rocket").transform.up * differenceY;

        this.transform.localPosition = new Vector3(rocketPos.x - difference.x, rocketPos.y - difference.y, rocketPos.z - difference.z);
    }

    private void setSize() {

        float currentScale = this.transform.localScale.x;
        float scale = currentScale + scaleIncrement;

        if (Rocket.isAccelerating()) {
            scale = currentScale + scaleIncrement;
        } else {
            if (this.transform.localScale.x > 0) {
                scale = currentScale - scaleIncrement;
            } else {
                scale = 0;
            }
        }

        if (Rocket.isAccelerating() && currentScale <= maxScale) {
            this.transform.localScale = new Vector3(scale, scale, scale);
        } else if(!Rocket.isAccelerating() && currentScale > 0) {
            this.transform.localScale = new Vector3(scale, scale, scale);
        }

        Controller.Sizes.flame = (Resources.Load("Prefabs/Flame") as GameObject).GetComponent<Renderer>().bounds.size * this.transform.localScale.x; 

    }

    

}
