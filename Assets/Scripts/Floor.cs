using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

    private float extendAmount = 0;

    void Start() {
    }

    void Update() {
        if (!Controller.isGameOver) {
            setFloorSize();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.transform.parent != null) {
            if (collision.gameObject.transform.parent.name.Equals("Rocket") || collision.gameObject.transform.parent.parent.name.Equals("Rocket")) {
                if (Controller.isStarted) {
                    Controller.gameOver(this.transform.GetComponent<Collider>().ClosestPoint(collision.transform.position));
                }
            }
        }
    }

    void setFloorSize() {
        float deltaX = Mathf.Abs(GameObject.Find("Rocket").transform.position.x) / 4;
        if(deltaX > extendAmount) {
            this.transform.localScale = new Vector3(this.transform.localScale.x + deltaX, this.transform.localScale.y, this.transform.localScale.z);
            extendAmount = deltaX;
        }
    }

}
