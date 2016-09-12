using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
    public GameObject explosion;
    public bool isEnemy;
    public float speed;
    public Vector3 target;
    public GameObject targetObject;
    public int scoreValue = 20;

	void Start () {
    }

	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (transform.position == target) {
            Instantiate(explosion, new Vector3(target.x,target.y+0.3f,target.z), Quaternion.identity);
            if (isEnemy) {
                if (targetObject.gameObject.name.Equals("City 1")) GameObject.Find("Level Script").GetComponent<GameMode>().citiesDestroyed[0] = true;
                if (targetObject.gameObject.name.Equals("City 2")) GameObject.Find("Level Script").GetComponent<GameMode>().citiesDestroyed[1] = true;
                if (targetObject.gameObject.name.Equals("City 3")) GameObject.Find("Level Script").GetComponent<GameMode>().citiesDestroyed[2] = true;
                if (targetObject.gameObject.name.Equals("City 4")) GameObject.Find("Level Script").GetComponent<GameMode>().citiesDestroyed[3] = true;
                if (targetObject.gameObject.name.Equals("City 5")) GameObject.Find("Level Script").GetComponent<GameMode>().citiesDestroyed[4] = true;
                if (targetObject.gameObject.name.Equals("City 6")) GameObject.Find("Level Script").GetComponent<GameMode>().citiesDestroyed[5] = true;
                if (targetObject.gameObject.name.Equals("Launcher Left")) GameObject.Find("Level Script").GetComponent<GameMode>().launchersDestroyed[0] = true;
                if (targetObject.gameObject.name.Equals("Launcher Central")) GameObject.Find("Level Script").GetComponent<GameMode>().launchersDestroyed[1] = true;
                if (targetObject.gameObject.name.Equals("Launcher Right")) GameObject.Find("Level Script").GetComponent<GameMode>().launchersDestroyed[2] = true;
            }
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D otherObject) {
        if (otherObject.gameObject.name.Equals("Air Explosion(Clone)")) {
            Destroy(gameObject);
            GameObject.Find("Level Script").GetComponent<GameMode>().score += scoreValue;
        }
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }

    public void DestroyMissile() {
        Destroy(gameObject);
    }
}
