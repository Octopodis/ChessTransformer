using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManadger : MonoBehaviour
{
    public Text text;

    private void Start() {
        text = gameObject.GetComponent<Text>();
    }

    public void RedWin() {
        text.text = "GameOver\nRedWin";
        gameObject.SetActive(true);
        Invoke("Off", 8f);
    }

    public void BlackWin() {
        text.text = "GameOver\nBlackWin";
        gameObject.SetActive(true);
        Invoke("Off", 8f);
    }

    public void Introduction() {
        text.text = "Like chess, but pieces are transformed according to the field on which they stand. \nTake care of your oval, it is important here.";
        gameObject.SetActive(true);
        Invoke("Off", 7f);
    }

    private void Off() {
        gameObject.SetActive(false);
    }
}
