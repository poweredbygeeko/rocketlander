using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comet : MonoBehaviour {

    Vector3 speed = Vector3.zero;

    void Start() {
        this.transform.localScale = getSize();
        this.transform.localPosition = getPosition();
        speed = getSpeed();
    }

    void Update() {
        this.transform.Translate(speed * Time.deltaTime);
    }

    Vector3 getSpeed() {
        float xSpeed = Random.Range(-2, -7) * Controller.Sizes.rocket.x / 2;
        
        return new Vector3(xSpeed, 0, 0);
    }

    Vector3 getSize() {
        float scale = Controller.Sizes.rocket.y * Random.Range(.15f, 1.5f);
        scale /= this.GetComponent<Renderer>().bounds.size.y;
        return new Vector3(scale, scale, scale);
    }

    Vector3 getPosition() {

        float xPos = Controller.xOffset + Controller.screenSize.x / 2;
        xPos += this.GetComponent<Renderer>().bounds.size.x / 2;
        float yPos = Controller.yOffset + Random.Range(-Controller.screenSize.y / 2, Controller.screenSize.y / 2);

        return new Vector3(xPos, yPos, 0);

    }

    public void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.transform.parent != null) {
            if (collision.gameObject.transform.parent.name.Equals("Rocket") || collision.gameObject.transform.parent.parent.name.Equals("Rocket")) {
                Controller.gameOver(this.GetComponent<Collider>().ClosestPoint(collision.gameObject.transform.position));
            }
        }
    }
}
