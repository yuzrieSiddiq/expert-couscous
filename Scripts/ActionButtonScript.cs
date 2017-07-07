using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonScript : MonoBehaviour {

    // set from inspector
    public Material green_light_on;
    public Material green_light_off;
    public Material red_light_on;
    public Material red_light_off;
    public Material blue_light_on;
    public Material blue_light_off;

    // set from script
    private int green_number = 0;
    private int red_number = 0;
    private int blue_number = 0;
    private int total_number = 0;

    /**
     * EventTrigger: on button press
     **/
    public void OnPressReset () {
        Debug.Log ("Test Reset Button");
    }

    public void OnPressGreenUp () {
        if (green_number < 9) {
            /** 1. update the current number **/
            green_number++;

            /**
         * 2. get the "Sa" GameObject and all the children except "Title"
         * loop through each children
         * */
            Transform Sa = GameObject.Find ("Sa").transform;
            int numbers = Sa.childCount - 1;

            for (int i = 0; i < numbers; i++) {
                Transform number = Sa.GetChild (i);

                /** 3. updating colors until the number = current number **/
                if (i == green_number)
                    break;

                /** 4. change green color from off to on **/
                MeshRenderer green_lightbulb = number.GetChild (1).gameObject.GetComponent<MeshRenderer>();
                green_lightbulb.material.CopyPropertiesFromMaterial (green_light_on);
            }

            /** 5. lastly calculate total**/
            CalculateTotal ();
        }
    }


    public void OnPressGreenDown () {
        // pre-check: lowerbound
        if (green_number > 0) {
            /** 3. update the current number **/
            green_number--;

            /** 1. get the latest green number **/
            Transform Sa = GameObject.Find ("Sa").transform;
            Transform number = Sa.GetChild (green_number);

            /** 2. revert back the color **/
            MeshRenderer green_lightbulb = number.GetChild (1).gameObject.GetComponent<MeshRenderer>();
            green_lightbulb.material.CopyPropertiesFromMaterial (green_light_off);

            /** 4. lastly calculate total**/
            CalculateTotal ();
        }
    }

    public void OnPressRedUp () {
        if (red_number < 9) {
            /** 1. update the current number **/
            red_number++;

            /**
         * 2. get the "Sa" GameObject and all the children except "Title"
         * loop through each children
         * */
            Transform Sa = GameObject.Find ("Sa").transform;
            int numbers = Sa.childCount - 1;

            for (int i = 0; i < numbers; i++) {
                Transform number = Sa.GetChild (i);

                /** 3. updating colors until the number = current number **/
                if (i == red_number)
                    break;

                /** 4. change green color from off to on **/
                MeshRenderer red_lightbulb = number.GetChild (2).gameObject.GetComponent<MeshRenderer>();
                red_lightbulb.material.CopyPropertiesFromMaterial (red_light_on);
            }

            /** 5. lastly calculate total**/
            CalculateTotal ();
        }
    }

    public void OnPressRedDown () {
        // pre-check: lowerbound
        if (red_number > 0) {
            /** 3. update the current number **/
            red_number--;

            /** 1. get the latest green number **/
            Transform Sa = GameObject.Find ("Sa").transform;
            Transform number = Sa.GetChild (red_number);

            /** 2. revert back the color **/
            MeshRenderer red_lightbulb = number.GetChild (2).gameObject.GetComponent<MeshRenderer>();
            red_lightbulb.material.CopyPropertiesFromMaterial (red_light_off);

            /** 4. lastly calculate total**/
            CalculateTotal ();
        }
    }

    /**
     * Other checks
     * the input uses global variables
     * */
    private bool CheckIsTens () {
        return false;
    }

    private void CalculateTotal () {
        total_number = green_number + red_number;

        Text value_textbox = GameObject.FindGameObjectWithTag ("total").GetComponent<Text>();
        value_textbox.text = total_number.ToString ();
    }
}
