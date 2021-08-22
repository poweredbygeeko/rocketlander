using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour {

    Vector3 position = Vector3.zero;

    void Start() {
        init();
    }

    void Update() {
        GameObject.Find("Progress Bar").transform.position = position + new Vector3(Controller.xOffset, Controller.yOffset, 0);
        updateProgress();
    }

    void init() {

        GameObject leftCircle = GameObject.Find("White_Gray").transform.Find("Circle_Left").gameObject;
        GameObject rightCircle = GameObject.Find("White_Gray").transform.Find("Circle_Right").gameObject;
        GameObject middle = GameObject.Find("White_Gray").transform.Find("Middle").gameObject;

        float xPos = (Controller.screenSize.x / 2) / 10;
        xPos = -(Controller.screenSize.x / 2) + xPos;
        float yPos = (Controller.screenSize.y / 2) / 5;
        yPos = (Controller.screenSize.y / 2) - yPos;
        float zPos = 0;

        leftCircle.transform.position = new Vector3(xPos, yPos, zPos);
        rightCircle.transform.position = new Vector3(-xPos, yPos, zPos);

        middle.transform.position = new Vector3(0, yPos, zPos);

        float heightScale = (Controller.screenSize.y / 20) / middle.GetComponent<Renderer>().bounds.size.y;
        leftCircle.transform.localScale = new Vector3(heightScale, heightScale, heightScale);
        rightCircle.transform.localScale = new Vector3(heightScale, heightScale, heightScale);

        float length = ((rightCircle.transform.position.x - leftCircle.transform.position.x) - leftCircle.GetComponent<Renderer>().bounds.size.x) / middle.GetComponent<Renderer>().bounds.size.x;

        middle.transform.localScale = new Vector3(1, heightScale, 1);
        Controller.Sizes.ProgressBar.middle = middle.GetComponent<Renderer>().bounds.size;
        print(middle.GetComponent<Renderer>().bounds.size);
        middle.transform.localScale = new Vector3(length, heightScale, 1);
        Controller.Sizes.ProgressBar.middleStretched = middle.GetComponent<Renderer>().bounds.size;

        GameObject.Find("White_Gray").transform.parent = null;

        GameObject.Find("Progress Bar").transform.position = middle.transform.position;
        GameObject.Find("White_Gray").transform.parent = GameObject.Find("Progress Bar").transform;

        position = GameObject.Find("Progress Bar").transform.position;



    }

    void updateProgress() {
        GameObject leftCircle = GameObject.Find("Green").transform.Find("Circle_Left").gameObject;
        GameObject rightCircle = GameObject.Find("Green").transform.Find("Circle_Right").gameObject;
        GameObject middle = GameObject.Find("Green").transform.Find("Middle").gameObject;

        leftCircle.transform.position = GameObject.Find("White_Gray").transform.Find("Circle_Left").transform.position;
        float xPos = leftCircle.GetComponent<Renderer>().bounds.size.x;
        rightCircle.transform.position = leftCircle.transform.position + new Vector3(xPos, 0, 0);

        Vector3 size = GameObject.Find("White_Gray").transform.Find("Circle_Left").localScale;
        leftCircle.transform.localScale = size;
        rightCircle.transform.localScale = size;

        float totalDistance = GameObject.FindGameObjectWithTag("Platform").transform.position.x - Controller.screenSize.x / 2;
        float distancePercent = Controller.xOffset / totalDistance;

        if(distancePercent > 1) {
            distancePercent = 1;
        } else if(distancePercent < 0) {
            distancePercent = 0;
        }

        float width = distancePercent * (Controller.Sizes.ProgressBar.middleStretched.x + rightCircle.GetComponent<Renderer>().bounds.size.x);
        width = width / Controller.Sizes.ProgressBar.middle.x;

        middle.transform.localScale = new Vector3(width, size.y, size.z);
      
        float yPos = leftCircle.transform.position.y;
        float zPos = leftCircle.transform.position.z;

        xPos = leftCircle.transform.position.x + leftCircle.GetComponent<Renderer>().bounds.size.x / 2;

        middle.transform.position = new Vector3(xPos, yPos, zPos);

        xPos = middle.GetComponent<Renderer>().bounds.max.x;

        rightCircle.transform.position = new Vector3(xPos, yPos, zPos);

    }
}

