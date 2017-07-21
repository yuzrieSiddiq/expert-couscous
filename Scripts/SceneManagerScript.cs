using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    public int nextSceneIndex;
    public GameObject fadingInObject;
    public GameObject fadingOutObject;
    public Animator fadingInAnimator;
    public Animator fadingOutAnimator;

    private Image fadingInImage;
    private Image fadingOutImage;

    void Start() {
        Screen.fullScreen = !Screen.fullScreen;
        Screen.SetResolution(600, 1024, true);

        fadingOutObject.SetActive (true);
        fadingOutImage = fadingOutObject.GetComponent<Image> ();

        fadingInObject.SetActive (true);
        fadingInImage = fadingInObject.GetComponent<Image> ();

        fadingOutObject.SetActive (false);

        StartCoroutine ( FadingIn() );
    }

    /** Trigger Change Scene via OnClick button **/
    public void ChangeScene() {
        fadingInObject.SetActive (false);
        fadingOutObject.SetActive (true);
        StartCoroutine ( FadingOut() );
        SceneManager.LoadScene (nextSceneIndex);
    }

    public void ExitGame() {
        Application.Quit();
    }

    /**
     * IEnumerator for Coroutine (something like invokeWS in async task)
     * Fade in/out to black to next scene in Build Settings
     **/

    IEnumerator FadingOut() {
        fadingOutAnimator.SetBool ("FadeOut", true);
        yield return new WaitForSeconds ( 1.25f );

        fadingOutAnimator.SetBool ("FadeOut", false);
        fadingOutObject.SetActive (false);
    }

    IEnumerator FadingIn() {
        fadingInObject.SetActive (true);
        fadingInAnimator.SetBool ("FadeIn", true);
        yield return new WaitForSeconds ( 1.25f );

        fadingInAnimator.SetBool ("FadeIn", false);
        fadingInObject.SetActive (false);
    }
}
