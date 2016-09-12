using UnityEngine;
using System.Collections;

public class WaveAnnouncement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void StartNewWave() {
        Destroy(gameObject);
    }
}
