using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeOut : MonoBehaviour {
    public void LoadGame() {
        SceneManager.LoadScene("GameLevel");
    }
}
