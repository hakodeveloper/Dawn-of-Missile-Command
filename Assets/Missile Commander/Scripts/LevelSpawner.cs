using UnityEngine;
using System.Collections;

public class LevelSpawner : MonoBehaviour {
    public GameObject missile;
    public int missilesToLaunch = 10;
    public float missilesMinSpeed = 1f;
    public float missilesMaxSpeed = 2f;
    public float missilesMinLaunchInterval = 2f;
    public float missilesMaxLaunchInterval = 4f;
    public int difficultyMultiplier = 1;

    private GameMode gameMode;

    public void StartLevel () {
        gameMode = GameObject.Find("Level Script").GetComponent<GameMode>();
        InvokeRepeating("FireMissile", 0, Random.Range(missilesMinLaunchInterval,missilesMaxLaunchInterval));
        missilesMinSpeed = Mathf.Clamp(missilesMinSpeed + (0.1f * difficultyMultiplier), 1f, 2f);
        missilesMaxSpeed = Mathf.Clamp(missilesMaxSpeed + (0.1f * difficultyMultiplier), 2f, 4f);
        missilesMinLaunchInterval = Mathf.Clamp(missilesMinLaunchInterval - (0.1f * difficultyMultiplier), 0.2f, 2f);
        missilesMaxLaunchInterval = Mathf.Clamp(missilesMaxLaunchInterval - (0.1f * difficultyMultiplier), 1f, 4f);
    }

    GameObject SelectTarget() {
        return gameMode.targets[Random.Range(0, 8)];
    }

    Vector3 SelectSpawnPoint() {
        return new Vector3(Random.Range(-8,8),gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
    }
	
    void FireMissile() {
        if (!gameMode.isGameOver) {
            if (missilesToLaunch <= 0) {
                CancelInvoke();
                GameObject.Find("Level Script").GetComponent<GameMode>().waveEnded = true;
            } else {
                GameObject target = SelectTarget();
                Vector3 spawn = SelectSpawnPoint();
                Vector3 direction = target.transform.localPosition - spawn;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                GameObject misil = Instantiate(missile, spawn, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
                Missile parameters = misil.GetComponent<Missile>();
                parameters.speed = Random.Range(missilesMinSpeed, missilesMaxSpeed);
                parameters.target = target.transform.localPosition;
                parameters.targetObject = target;
                missilesToLaunch--;
            }
        }
    }
}
