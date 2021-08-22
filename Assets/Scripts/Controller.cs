using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    public static Vector3 initRocketPos = Vector3.zero;

    private static Vector3 currentArrayScreenPos = Vector3.zero;

    private static List<Vector3> arrayScreens = new List<Vector3>();

    public static Vector3 screenSize = Vector3.zero;

    public static bool isStarted = false;

    public static bool isGameOver = false;

    public static float currentSpeed = 0;
    public static float maxSpeed = 50f;
    public static float maxFallSpeed = -50f;
    public static float acceleration = 20f;

    public static float xOffset = 0;
    public static float yOffset = 0;

    public static float rotationSpeed = 1f;
    public static float rotationFallSpeed = .75f;

    void Awake() {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -Camera.main.transform.position.z)) * 2;
        initSizes();
        initRocketPos = GameObject.Find("Rocket").transform.position;
        initScene(Vector3.zero);
        initPlatform();
    }

    void Update() {
        if (!isGameOver) {
            updateCamera();
            checkCurrentArrayScreen();
            initClouds();
        }
        initComets();
    }

    void initSizes() {

        GameObject cloud = Resources.Load("Prefabs/Clouds/cloud_1") as GameObject;
        float scale = (screenSize.x / 3.5f) / cloud.GetComponent<SpriteRenderer>().bounds.size.x;

        foreach (GameObject c in Resources.LoadAll<GameObject>("Prefabs/Clouds")) {
            c.transform.localScale = new Vector3(scale, scale, scale);
        }

        scale = (cloud.GetComponent<Renderer>().bounds.size.x / 6) / (GameObject.Find("Mid").GetComponent<Renderer>().bounds.size.x);
        scale /= 2;

        GameObject.Find("Rocket").transform.localScale = new Vector3(scale, scale, scale);
        Sizes.rocket = Rocket.getSize();

        (Resources.Load("Prefabs/Flame") as GameObject).transform.localScale = new Vector3(scale, scale, scale);

        scale = (cloud.GetComponent<Renderer>().bounds.size.x / 10) / (Resources.Load("Prefabs/Star") as GameObject).GetComponent<Renderer>().bounds.size.x;

        (Resources.Load("Prefabs/Star") as GameObject).transform.localScale = new Vector3(scale, scale, scale);

        GameObject floor = GameObject.Find("Floor");
        floor.transform.localScale = new Vector3(screenSize.x * 3, screenSize.y / 10 + (Sizes.rocket.y * 2), Sizes.rocket.z * 3);
        floor.transform.localPosition = new Vector3(0, screenSize.y / -2, 0);

        GameObject.Find("Rocket").transform.localPosition = new Vector3(0, floor.transform.localPosition.y + floor.transform.localScale.y / 2, 0);
    }

    void initPlatform() {
        GameObject platform = Instantiate(Resources.Load("Prefabs/Platforms/Platform_" + Random.Range(1, 4)) as GameObject);
        GameObject floor = GameObject.Find("Floor");

        float height = platform.transform.Find("Platform").GetComponent<Renderer>().bounds.size.y;

        float xPos = screenSize.x * Random.Range(2, 4);
        float yPos = floor.transform.position.y + floor.GetComponent<Renderer>().bounds.size.y/2;
        platform.transform.position = new Vector3(xPos, yPos, 0);
    }

    void checkCurrentArrayScreen() {

        foreach (Vector3 arrayScreen in arrayScreens) {

            float distanceX = arrayScreen.x - getScreenCentre().x;
            float distanceY = arrayScreen.y - getScreenCentre().y;

            if (arrayScreen.x - currentArrayScreenPos.x == 0) {
                if (Mathf.Abs(distanceY) <= screenSize.y / 2) {
                    currentArrayScreenPos = arrayScreen;
                }
            }

            if (arrayScreen.y - currentArrayScreenPos.y == 0) {
                if (Mathf.Abs(distanceX) <= screenSize.x / 2) {
                    currentArrayScreenPos = arrayScreen;
                }
            }

        }

    }

    void initClouds() {

        Vector3 newArrayScreenPos = currentArrayScreenPos + new Vector3(0, screenSize.y, 0);
        if (newArrayScreenPos.y - getScreenCentre().y < screenSize.y * 1.5f) {
            checkInitArrayScreen(newArrayScreenPos);
        }

        newArrayScreenPos = currentArrayScreenPos + new Vector3(0, -screenSize.y, 0);
        if (newArrayScreenPos.y - getScreenCentre().y > screenSize.y * -1.5f) {
            checkInitArrayScreen(newArrayScreenPos);
        }

        newArrayScreenPos = currentArrayScreenPos + new Vector3(-screenSize.x, 0, 0);
        if (newArrayScreenPos.x - getScreenCentre().x > screenSize.x * -1.5f) {
            checkInitArrayScreen(newArrayScreenPos);
        }

        newArrayScreenPos = currentArrayScreenPos + new Vector3(screenSize.x, 0, 0);
        if (newArrayScreenPos.x - getScreenCentre().x < screenSize.x * 1.5f) {
            checkInitArrayScreen(newArrayScreenPos);
        }

        newArrayScreenPos = currentArrayScreenPos + new Vector3(-screenSize.x, screenSize.y, 0);
        if (newArrayScreenPos.x - getScreenCentre().x > screenSize.x * -1.5f
            || newArrayScreenPos.y - getScreenCentre().y < screenSize.y * -1.5f) {
            checkInitArrayScreen(newArrayScreenPos);
        }

        newArrayScreenPos = currentArrayScreenPos + new Vector3(screenSize.x, screenSize.y, 0);
        if (newArrayScreenPos.x - getScreenCentre().x < screenSize.x * 1.5f
            || newArrayScreenPos.y - getScreenCentre().y < screenSize.y * 1.5f) {
            checkInitArrayScreen(newArrayScreenPos);
        }

        newArrayScreenPos = currentArrayScreenPos + new Vector3(screenSize.x, -screenSize.y, 0);
        if (newArrayScreenPos.x - getScreenCentre().x < screenSize.x * 1.5f
            || newArrayScreenPos.y - getScreenCentre().y > screenSize.y * -1.5f) {
            checkInitArrayScreen(newArrayScreenPos);
        }

        newArrayScreenPos = currentArrayScreenPos + new Vector3(-screenSize.x, -screenSize.y, 0);
        if (newArrayScreenPos.x - getScreenCentre().x > screenSize.x * -1.5f
            || newArrayScreenPos.y - getScreenCentre().y > screenSize.y * -1.5f) {
            checkInitArrayScreen(newArrayScreenPos);
        }


    }

    void checkInitArrayScreen(Vector3 pos) {
        if (!arrayScreens.Contains(pos)) {
            initScene(pos);
            arrayScreens.Add(pos);
        }
    }

    void initScene(Vector3 centre) {

        int amount = Random.Range(2, 5);

        for (int i = 1; i <= amount; i++) {
            float xPos = Random.Range(centre.x - screenSize.x / 2, centre.x + screenSize.x / 2);
            float yPos = Random.Range(centre.y - screenSize.y / 2, centre.y + screenSize.y / 2);
            float zPos = Controller.Sizes.rocket.z * Random.Range(-3, 3);

            initCloud(new Vector3(xPos, yPos, zPos));
        }

        arrayScreens.Add(new Vector3(0, 0, 0));

    }

    void updateCamera() {

            Vector3 rocketPos = GameObject.Find("Rocket").transform.position;

            float deltaX = rocketPos.x - initRocketPos.x;
            float deltaY = rocketPos.y - initRocketPos.y;

            float zPos = Camera.main.transform.position.z;

            Camera.main.transform.position = new Vector3(deltaX, deltaY, zPos);

            xOffset = deltaX;
            yOffset = deltaY;

    }

    private static Vector3 getScreenCentre() {
        float centreX = xOffset;
        float centreY = yOffset;
        float centreZ = GameObject.Find("Rocket").transform.position.z;
        return new Vector3(centreX, centreY, centreZ);
    }

    private static Vector3 getDistanceFromCentre(GameObject o) {

        float centreX = xOffset;
        float centreY = yOffset;
        float centreZ = GameObject.Find("Rocket").transform.position.z;

        float distanceX = o.transform.position.x - centreX;
        float distanceY = o.transform.position.y - centreY;
        float distanceZ = o.transform.position.z - centreZ;

        return new Vector3(distanceX, distanceY, distanceZ);

    }

    private static GameObject initCloud(Vector3 pos) {

        GameObject[] clouds = Resources.LoadAll<GameObject>("Prefabs/Clouds");
        GameObject cloud = Instantiate(Resources.Load("Prefabs/Clouds/" + clouds[Random.Range(0, clouds.Length)].name) as GameObject);

        
        float scale = Random.Range(.5f, 1.5f);

        cloud.transform.localScale = new Vector3(scale, scale, scale);
        cloud.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(.1f, .9f));
        cloud.transform.parent = GameObject.Find("Background").transform;
        cloud.transform.position = pos;

        initStars(cloud);

        return cloud;
    }


    private static void initStars(GameObject cloud) {

        for (int i = 0; i <= Random.Range(2, 5); i++) {

            float xPos = cloud.transform.position.x + (cloud.GetComponent<Renderer>().bounds.size.x * Random.Range(-1.5f, 1.5f));
            float yPos = cloud.transform.position.y + (cloud.GetComponent<Renderer>().bounds.size.y * Random.Range(-1.5f, 1.5f));

            float scale = Random.Range(.1f, 3.5f);

            GameObject star = Instantiate(Resources.Load("Prefabs/star") as GameObject);
            star.transform.position = new Vector3(xPos, yPos, 0);
            star.transform.localScale = new Vector3(scale, scale, scale);
            star.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, Random.Range(.1f, 1));
            star.transform.parent = GameObject.Find("Background").transform;

        }

    }

    private static float lastTime = 0;

    private static void initComets() {

        if(Time.time - lastTime > Random.Range(1, 4)) {
            
            if (Random.Range(0, 1000) > 950) {
                GameObject comet = Instantiate(Resources.Load("Prefabs/Comet_" + Random.Range(1, 3)) as GameObject);
                comet.transform.parent = GameObject.Find("Comets").transform;
                lastTime = Time.time;
            }
        }
    }

    public static void gameOver(Vector3 point) {
        isGameOver = true;
        Destroy(GameObject.Find("Rocket"));
        for (int i = 0; i < Random.Range(5, 10); i++) {
            GameObject smoke = Instantiate(Resources.Load("Prefabs/Smoke_" + Random.Range(1, 3)) as GameObject);
            smoke.transform.position = point;
        }
    }

    public class Sizes {

        public static Vector3 flame = Vector3.zero;
        public static Vector3 rocket = Vector3.zero;

        public class ProgressBar {

            public static Vector3 middle = Vector3.zero;
            public static Vector3 middleStretched = Vector3.zero;

        }

    }


}
