using UnityEngine;
using System.Collections;

public class LauncherController : MonoBehaviour {
    public GameObject[] launchers = new GameObject[3];
    public GameObject missile;
    public int cannonsMaxAngle = 90;
    public float cannonsMaxHeat = 50f;
    public float missileMinSpeed = 2f;
    public float missileMaxSpeed = 15f;
    public float shootHeatAmount = 12f;
    public float heatCooldownAmount = 0.15f;

    private GameObject[] cannons;
    private GameObject[] fireSockets;
    private GameObject[] heatMeter;
    private Vector3 mousePosition;
    private Vector3 cannonPosition;

    private float heatMeterDiv;
    private int[] shootPriority;
    private float[] distanceToTarget;
    private float[] cannonsHeat;
    private bool[] cannonsCanShoot = new bool[3];

    void Start () {
        cannons = new GameObject[3];
        fireSockets = new GameObject[3];
        heatMeter = new GameObject[3];
        cannonsHeat = new float[3];
        heatMeterDiv = cannonsMaxHeat / 0.25f;
        for (int i=0; i<3; i++) {
            cannons[i]= launchers[i].transform.Find("Launcher Cannon").gameObject;
        }
        for (int i = 0; i < 3; i++) {
            fireSockets[i] = cannons[i].transform.Find("Launcher Socket").gameObject;
        }
        for (int i = 0; i < 3; i++) {
            heatMeter[i] = cannons[i].transform.Find("Heat Meter").gameObject;
            heatMeter[i].GetComponent<SpriteRenderer>().color = new Vector4(0.05f, 0.1f, 0.65f, 0.6f);
        }
        shootPriority = new int[3];
	}

    void Update () {
        AimCannons();
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
        for (int i = 0; i < 3; i++) {
            cannonsHeat[i] = Mathf.Clamp(cannonsHeat[i]-heatCooldownAmount,0,cannonsMaxHeat);
            if (!GameObject.Find("Level Script").GetComponent<GameMode>().launchersDestroyed[i]) {
                if (cannonsHeat[i] == 0 && cannonsCanShoot[i] == false) {
                    cannonsCanShoot[i] = true;
                    heatMeter[i].GetComponent<SpriteRenderer>().color = new Vector4(0.05f, 0.1f, 0.65f, 0.6f);
                }
            } else {
                cannonsCanShoot[i] = false;
            }
            heatMeter[i].transform.localScale = new Vector3(0.035f,Mathf.Clamp(cannonsHeat[i]/heatMeterDiv, 0,0.25f),0f);
        }
    }

    void AimCannons(){
        foreach (GameObject cannon in cannons) {
            mousePosition = Input.mousePosition;
            cannonPosition = Camera.main.WorldToScreenPoint(cannon.transform.position);
            mousePosition.x = mousePosition.x - cannonPosition.x;
            mousePosition.y = mousePosition.y - cannonPosition.y;
            float angle = (Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg) - 90;
            if (angle > (cannonsMaxAngle * -1) && angle < cannonsMaxAngle) {
                cannon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
        }
    }

    void Shoot() {
        distanceToTarget = new float[3];
        for (int i = 0; i < 3; i++) {
            mousePosition = Input.mousePosition;
            cannonPosition = Camera.main.WorldToScreenPoint(cannons[i].transform.position);
            distanceToTarget[i] = Vector3.Distance(mousePosition, cannonPosition);
        }
        shootPriority = SpeedSort(distanceToTarget);
        if (cannonsCanShoot[shootPriority[0]]) {
            FireMissile(shootPriority[0]);
        } else if (cannonsCanShoot[shootPriority[1]]) {
            FireMissile(shootPriority[1]);
        } else if (cannonsCanShoot[shootPriority[2]]) {
            FireMissile(shootPriority[2]);
        }
    }

    void FireMissile(int cannonID) {
        cannons[cannonID].GetComponent<AudioSource>().Play();
        cannonsHeat[cannonID] = cannonsHeat[cannonID] + shootHeatAmount;
        if (cannonsHeat[cannonID] > cannonsMaxHeat) {
            cannonsCanShoot[cannonID] = false;
            heatMeter[cannonID].GetComponent<SpriteRenderer>().color = new Vector4(0.78f,0,0,0.6f);
        }
        Vector3 playerTargetPoint = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        GameObject misil = Instantiate(missile, fireSockets[cannonID].transform.position, cannons[cannonID].transform.rotation * Quaternion.Euler(0,0,90)) as GameObject;
        Missile parameters = misil.GetComponent<Missile>();
        parameters.speed = Mathf.Clamp(Vector3.Distance(playerTargetPoint, fireSockets[cannonID].transform.position)*2, missileMinSpeed, missileMaxSpeed);
        parameters.target = playerTargetPoint;
    }

    //Ordenamiento por fuerza bruta para evitar la notacion Big O(N^2)
    int[] SpeedSort(float[] array) {
        int[] order = new int[] { 0, 1, 2 };
        if (array[0] < array[1]) {
            if (array[0] < array[2]) {
                if (array[1] < array[2]) {
                    order = new int[] { 0, 1, 2 };
                } else {
                    order = new int[] { 0, 2, 1 };
                }
            }
        }
        if (array[1] < array[0]) {
            if (array[1] < array[2]) {
                if (array[0] < array[2]) {
                    order = new int[] { 1, 0, 2 };
                } else {
                    order = new int[] { 1, 2, 0 };
                }
            }
        }
        if (array[2] < array[0]) {
            if (array[2] < array[1]) {
                if (array[0] < array[1]) {
                    order = new int[] { 2, 0, 1 };
                } else {
                    order = new int[] { 2, 1, 0 };
                }
            }
        }
        return order;
    }
}
