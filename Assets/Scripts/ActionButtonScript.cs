using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonScript : MonoBehaviour {

    // set from inspector
    public Material green_light_on;
    public Material red_light_on;
    public Material blue_light_on;
    public Material white_bulb;
    public Sprite action_button;
    public AudioClip up_button_sound;
    public AudioClip blue_button_sound;
    public AudioClip reset_button_sound;

    // set from script
    private int green_value = 0;
    private int red_value = 0;
    private int sa_value = 0;
    private int puluh_value = 0;
    private int total_number = 0;

    /**
     * EventTrigger: when reset button is pressed
     **/
    public void OnPressReset () {
        /** 1. play the reset sound **/
        GameObject audioSourceObject = GameObject.FindGameObjectWithTag ("audio-bell");
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource> ();
        audioSource.clip = reset_button_sound;
        audioSource.Play ();

        /** 2. get all red, green and blue numbers and turn off all their lights **/
        Transform Sa = GameObject.Find ("Sa").transform;
        Transform Puluh = GameObject.Find ("Puluh").transform;
        int numbers = Sa.childCount - 2;

        for (int i = 0; i < numbers; i++) {
            Transform sa_number = Sa.GetChild (i);
            Transform puluh_number = Puluh.GetChild (i);

            MeshRenderer green_lightbulb = sa_number.GetChild (1).gameObject.GetComponent<MeshRenderer>();
            MeshRenderer red_lightbulb = sa_number.GetChild (2).gameObject.GetComponent<MeshRenderer>();
            MeshRenderer blue_lightbulb = puluh_number.GetChild (1).gameObject.GetComponent<MeshRenderer>();

            green_lightbulb.material.CopyPropertiesFromMaterial (white_bulb);
            red_lightbulb.material.CopyPropertiesFromMaterial (white_bulb);
            blue_lightbulb.material.CopyPropertiesFromMaterial (white_bulb);
        }

        /** 3. special for 10 cannot be included by above because different layouts **/
        MeshRenderer sa_ten = Sa.GetChild (9).GetChild(1).gameObject.GetComponent<MeshRenderer>();
        sa_ten.material.CopyPropertiesFromMaterial (white_bulb);

        MeshRenderer puluh_ten = Puluh.GetChild (9).GetChild(1).gameObject.GetComponent<MeshRenderer>();
        puluh_ten.material.CopyPropertiesFromMaterial (white_bulb);

        /** 4. reset all the numbers **/
        total_number = 0;
        puluh_value = 0;
        sa_value = 0;
        green_value = 0;
        red_value = 0;

        // bentuk lazim
        Text bentuk_lazim_blue = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Blue").GetComponent<Text>();
        Text bentuk_lazim_green = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Green").GetComponent<Text>();
        Text bentuk_lazim_red = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Red").GetComponent<Text>();
        Text bentuk_lazim_total = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Total").GetComponent<Text>();

        bentuk_lazim_blue.text = puluh_value.ToString ();
        bentuk_lazim_green.text = green_value.ToString ();
        bentuk_lazim_red.text= red_value.ToString ();
        bentuk_lazim_total.text= total_number.ToString ();

        // bentuk biasa
        Text bentuk_biasa_blue = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Blue").GetComponent<Text>();
        Text bentuk_biasa_green = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Green").GetComponent<Text>();
        Text bentuk_biasa_red = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Red").GetComponent<Text>();
        Text bentuk_biasa_total = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Total").GetComponent<Text>();

        bentuk_biasa_blue.text = puluh_value.ToString ();
        bentuk_biasa_green.text = green_value.ToString ();
        bentuk_biasa_red.text= red_value.ToString ();
        bentuk_biasa_total.text= total_number.ToString ();

        /** 5. re-enable the buttons (when the number reach 100, they turn white) **/
        GameObject[] action_buttons = GameObject.FindGameObjectsWithTag ("action-up");
        foreach (GameObject action_button in action_buttons) {
            action_button.GetComponent<Button> ().enabled = true;
        }
    }

    public void OnPressGreenUp () {
        /** 1. set maximum number condition: 100 **/
        if (total_number < 100) {
        
            /** 1a. set maximum number for the sa column (right) **/
            if (green_value < 9 && sa_value < 10) {
                /**
                 * 2. get the "Sa" GameObject and all the children except "Title"
                 * loop through each children
                 * */
                Transform Sa = GameObject.Find ("Sa").transform;
                int numbers = Sa.childCount - 2;

                for (int i = 0; i < numbers; i++) {
                    Transform number = Sa.GetChild (i);

                    /** 3. light up the bulb for current number **/
                    if (i == sa_value) {
                        MeshRenderer green_lightbulb = number.GetChild (1).gameObject.GetComponent<MeshRenderer> ();
                        green_lightbulb.material.CopyPropertiesFromMaterial (green_light_on);
                    }
                }

                /** 4. update the current "sa" number **/
                sa_value++;
                green_value++;

                /** 5. increment/calculate total **/
                CalculateTotal ();

                /** 6. Update numbers in bentuk lazim **/
                Text bentuk_lazim_green= GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Green").GetComponent<Text>();
                bentuk_lazim_green.text = green_value.ToString ();

                Text bentuk_biasa_green= GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Green").GetComponent<Text>();
                bentuk_biasa_green.text = green_value.ToString ();
            }
        } else {
            
            /** 1b. if total already reach 100, disable both buttons - become white **/
            GameObject[] action_buttons = GameObject.FindGameObjectsWithTag ("action-up");
            foreach (GameObject action_button in action_buttons) {
                action_button.GetComponent<Button> ().enabled = false;
            }
        }
    }

    public void OnPressRedUp () {
        /** 1. set maximum number condition: 100 **/
        if (total_number < 100) {

            /** 1a. set maximum number for the sa column (right) **/
            if (red_value < 9 && sa_value < 10) {
                /**
                 * 2. get the "Sa" GameObject and all the children except "Title"
                 * loop through each children
                 * */
                Transform Sa = GameObject.Find ("Sa").transform;
                int numbers = Sa.childCount - 2;

                for (int i = 0; i < numbers; i++) {
                    Transform number = Sa.GetChild (i);

                    /** 3. light up the bulb for current number **/
                    if (i == sa_value) {
                        MeshRenderer red_lightbulb = number.GetChild (2).gameObject.GetComponent<MeshRenderer> ();
                        red_lightbulb.material.CopyPropertiesFromMaterial (red_light_on);
                    }
                }

                /** 4. update the current "sa" number **/
                sa_value++;
                red_value++;

                /** 5. increment/calculate total **/
                CalculateTotal ();

                /** 6. Update numbers in bentuk lazim **/
                Text bentuk_lazim_red= GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Red").GetComponent<Text>();
                bentuk_lazim_red.text = red_value.ToString ();

                Text bentuk_biasa_red= GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Red").GetComponent<Text>();
                bentuk_biasa_red.text = red_value.ToString ();
            }
        } else {
            
            /** 1b. if total already reach 100, disable both buttons - become white **/
            GameObject[] action_buttons = GameObject.FindGameObjectsWithTag ("action-up");
            foreach (GameObject action_button in action_buttons) {
                action_button.GetComponent<Button> ().enabled = false;
            }
        }
    }

    /**
     * This function keep tracks of whether the question is in "tens" or not
     * i.e: 10, 20, 30, ...
     * */
    private bool CheckIsTens () {
        /** 1. get the number 10 in "sa" **/
        Transform Sa = GameObject.Find ("Sa").transform;
        MeshRenderer sa_ten = Sa.GetChild (9).GetChild(1).gameObject.GetComponent<MeshRenderer>();

        /** 2. set the sound to ready (only set, no play) **/
        GameObject audioSourceObject = GameObject.FindGameObjectWithTag ("audio-bell");
        AudioSource audioSource = audioSourceObject.GetComponent<AudioSource> ();
        audioSource.clip = blue_button_sound;

        /** 3. if the number is a "tens" **/
        if (total_number % 10 == 0 && sa_value != 0) {
            // 4a. light up the 10 in "sa" and ring the bell
            sa_ten.material.CopyPropertiesFromMaterial (blue_light_on);
            audioSource.Play ();
            StartCoroutine ( LightBulb_Off(sa_ten) );

            return true;

        } else {
            // 4b. light off the 10 in "sa"
            sa_ten.material.CopyPropertiesFromMaterial (white_bulb);

            return false;
        }
    }

    private void CalculateTotal () {
        /** 1. increment the total number **/
        total_number = sa_value + (puluh_value * 10);

        /** 2. Update numbers in bentuk lazim **/
        Text bentuk_lazim_total= GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Total").GetComponent<Text>();
        bentuk_lazim_total.text= total_number.ToString ();

        Text bentuk_biasa_total= GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Total").GetComponent<Text>();
        bentuk_biasa_total.text= total_number.ToString ();

        /** 3. reset all red and green button to off and plus 1 blue **/
        if (CheckIsTens ()) {
            puluh_value = total_number / 10;
            sa_value = 0;

            /** 4. check what is the number and assign to blue number **/
            Transform Sa = GameObject.Find ("Sa").transform;
            int sa_numbers = Sa.childCount - 2;

            /** 5. go through every "sa" light bulb **/
            for (int i = 0; i < sa_numbers; i++) {
                Transform number = Sa.GetChild (i);

                /** 6. turn off all red and green lights **/
                MeshRenderer red_lightbulb = number.GetChild (2).gameObject.GetComponent<MeshRenderer> ();
                red_lightbulb.material.CopyPropertiesFromMaterial (white_bulb);

                MeshRenderer green_lightbulb = number.GetChild (1).gameObject.GetComponent<MeshRenderer> ();
                green_lightbulb.material.CopyPropertiesFromMaterial (white_bulb);
            }

            /** 7. turn on blue light **/
            Transform Puluh = GameObject.Find ("Puluh").transform;
            int puluh_numbers = Puluh.childCount - 1;

            /** 8. go through every "puluh" light bulb **/
            for (int i = 0; i < puluh_numbers; i++) {
                Transform puluh_number = Puluh.GetChild (i);

                /** 9. exit the loop **/
                if (i == puluh_value)
                    break;

                /** 10. light up where current blue light supposed to light up **/
                MeshRenderer blue_lightbulb = puluh_number.GetChild (1).gameObject.GetComponent<MeshRenderer> ();
                blue_lightbulb.material.CopyPropertiesFromMaterial (blue_light_on);
            }
        } else {
            GameObject audioSourceObject = GameObject.FindGameObjectWithTag ("audio-bell");
            AudioSource audioSource = audioSourceObject.GetComponent<AudioSource> ();
            audioSource.PlayOneShot(up_button_sound);
        }
    }

    /**
     * After CheckIsTens() returns true
     * within 1 second, disable buttons and reset numbers in bentuk lazim and biasa
     * 
     * param: light_bulb ref "10" in sa
     * */
    IEnumerator LightBulb_Off(MeshRenderer light_bulb) {
        /** 1. get the buttons and put them as disabled **/
        GameObject[] action_buttons = GameObject.FindGameObjectsWithTag ("action-up");
        foreach (GameObject action_button in action_buttons) {
            action_button.GetComponent<Button> ().enabled = false;
        }

        /** 2. wait 1 second before proceeding with the next part **/
        yield return new WaitForSeconds ( 1 );

        /** 3. after waiting 1 second, if number still not one hundred, re-enable buttons **/
        if (total_number < 100) {
            /** 4. get the buttons and put them as enabled **/
            action_buttons = GameObject.FindGameObjectsWithTag ("action-up");
            foreach (GameObject action_button in action_buttons) {
                action_button.GetComponent<Button> ().enabled = true;
            }
        }

        /** 5. reset the color of light_bulb "10" in sa **/
        light_bulb.material.CopyPropertiesFromMaterial (white_bulb);

        /** 6. Update numbers in bentuk lazim and bentuk biasa **/
        green_value = 0;
        red_value = 0;

        Text bentuk_lazim_blue = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Blue").GetComponent<Text>();
        Text bentuk_lazim_green = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Green").GetComponent<Text>();
        Text bentuk_lazim_red = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Red").GetComponent<Text>();

        bentuk_lazim_blue.text = puluh_value.ToString ();
        bentuk_lazim_green.text = green_value.ToString ();
        bentuk_lazim_red.text= red_value.ToString ();

        Text bentuk_biasa_blue = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Blue").GetComponent<Text>();
        Text bentuk_biasa_green = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Green").GetComponent<Text>();
        Text bentuk_biasa_red = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Red").GetComponent<Text>();

        bentuk_biasa_blue.text = puluh_value.ToString ();
        bentuk_biasa_green.text = green_value.ToString ();
        bentuk_biasa_red.text= red_value.ToString ();

        /** 7. only show the blue number if its not 0 (will look weird to the kids) **/
        if (puluh_value > 0) {
            GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find ("Blue").gameObject.SetActive (true);
            GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find ("Blue").gameObject.SetActive (true);
        } else {
            GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find ("Blue").gameObject.SetActive (false);
            GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find ("Blue").gameObject.SetActive (false);
        }
    }
}
