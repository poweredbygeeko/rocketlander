using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {

    Vector3 speed = Vector3.zero;

    void Start() {
        speed = getSpeed();
        this.transform.localScale = getSize();
        this.transform.position = getPosition();
    }

    void Update() {

        move();

    }

    Vector3 getPosition() {
        float yPos = this.transform.localPosition.y + this.transform.GetComponent<Renderer>().bounds.size.y / 2;
        float zPos = Random.Range(-this.GetComponent<Renderer>().bounds.size.z, this.GetComponent<Renderer>().bounds.size.z);
        return new Vector3(this.transform.position.x, yPos, zPos);
    }

    Vector3 getSize() {

        float scale = (Resources.Load("Prefabs/Flame") as GameObject).GetComponent<Renderer>().bounds.size.y;
        scale *= Random.Range(.5f, 2);
        scale /= this.GetComponent<Renderer>().bounds.size.y;
        return new Vector3(scale, scale, scale);

    }

    Vector3 getSpeed() {

        float speedAmount = (Resources.Load("Prefabs/Flame") as GameObject).GetComponent<Renderer>().bounds.size.y / 5;
        speedAmount *= Random.Range(.3f, 2);

        float xSpeed = speedAmount * Random.Range(-5, 5);
        float ySpeed = speedAmount * Random.Range(2.5f, 5);

        return new Vector3(xSpeed, ySpeed, 0);
    }

    void move() {
        this.transform.Translate(speed * Time.deltaTime);
    }

}
