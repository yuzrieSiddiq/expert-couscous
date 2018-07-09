using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinusActionButtonScript : MonoBehaviour {

    // signatures static variables
    private static string SIGNATURE_PLUS = "PLUS";
    private static string SIGNATURE_MINUS = "MINUS";

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
    private string signature;

    /**
     * EventTrigger: when black/yellow button is pressed
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

    /**
     * Logic on when plus is
     * 1. Add the number (0 becomes 1) on sa column
     * 2. Light up the number (1) on sa column
     * 3. When reach 10, light up blue button on puluh column
     * 4. when reach 11, (turn off all light bulbs); then turn on light bulb 1 on sa column
     * 5. when reach 100, disable green button
     * 
     * - every time press a button, update all the numbers in the UI to match latest numbers stored in program
     * */
    public void OnPressGreenUp () {
        /** 1. getting the assigned variables and light up the bulb */
        signature = SIGNATURE_PLUS;
        SaLightBulbOn(green_value);

        /** 2. update the current "sa" number **/
        sa_value++;
        green_value++;

        /** 3. increment/calculate total **/
        CalculateTotal();

        /** 4. update ui to match latest variables value */
        UpdateCalculationTexts();
    }

    public void OnPressRedUp () {
        // TODO: logic on press minus button
        signature = SIGNATURE_MINUS;
        Transform Sa = GameObject.Find("Sa").transform;
        Transform Puluh = GameObject.Find("Puluh").transform;
        int sa_numbers = Sa.childCount - 2;
        int puluh_numbers = Puluh.childCount - 1;
        int check_number = total_number - (puluh_value * 10);
    }

    private void UpdateCalculationTexts()
    {
        /** 1. Get bentuk lazim and bentuk biasa, and update the text **/
        Text bentuk_lazim_green = GameObject.FindGameObjectWithTag("bentuk-lazim").transform.Find("Green").GetComponent<Text>();
        Text bentuk_lazim_red = GameObject.FindGameObjectWithTag("bentuk-lazim").transform.Find("Red").GetComponent<Text>();

        Text bentuk_biasa_green = GameObject.FindGameObjectWithTag("bentuk-biasa").transform.Find("Green").GetComponent<Text>();
        Text bentuk_biasa_red = GameObject.FindGameObjectWithTag("bentuk-biasa").transform.Find("Red").GetComponent<Text>();

        bentuk_lazim_green.text = green_value.ToString();
        bentuk_lazim_red.text = red_value.ToString();

        bentuk_biasa_green.text = green_value.ToString();
        bentuk_biasa_red.text = red_value.ToString();
    }

    /**
     * This function keep tracks of whether the question is in "tens" or not
     * i.e: 10, 20, 30, ...
     * @return boolean
     * */
    private bool isTens(int number) {
        if (number % 10 == 0)
            return true;

        return false;
    }

    private bool isMoreThanTen(int value)
    {
        if (value > 10)
            return true;

        return false;
    }

    /**
     * This function checks if the value is back to zero
     * NOTE: only check on sa column; NOT total and not puluh
     * */
    private bool isZero(int number)
    {
        if (number == 0)
            return true;

        return false;
    }

    private void CalculateTotal () {
        /** 1. increment the total number **/
        total_number = sa_value + (puluh_value * 10);

        /** 2. Update numbers in bentuk lazim **/
        Text bentuk_lazim_total= GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Total").GetComponent<Text>();
        Text bentuk_biasa_total = GameObject.FindGameObjectWithTag("bentuk-biasa").transform.Find("Total").GetComponent<Text>();
        bentuk_lazim_total.text= total_number.ToString ();
        bentuk_biasa_total.text= total_number.ToString ();

        /** 3. Get the gameobjects of sa and puluh columns and get their counts **/
        Transform Sa = GameObject.Find("Sa").transform;
        Transform Puluh = GameObject.Find("Puluh").transform;
        int sa_numbers = Sa.childCount - 2;
        int puluh_numbers = Puluh.childCount - 1;

        if (isTens(total_number))
        {
            puluh_value = total_number / 10;
            sa_value = 0;
            
            // TODO: Calculation to get the total
        } else {
            
        }
    }

    /**
     * After CheckIsTens() returns true
     * within 1 second, disable buttons and reset numbers in bentuk lazim and biasa
     * 
     * param: light_bulb ref "10" in sa
     * */
    //IEnumerator LightBulb_Off(MeshRenderer light_bulb) {
    //    // TODO: turn off a selected bulb
    //}

    /**
     * Turn on a sselected bulb
     * */
    private void SaLightBulbOn(int value)
    {
        Transform Sa = GameObject.Find("Sa").transform;
        Transform Puluh = GameObject.Find("Puluh").transform;
        int sa_numbers = Sa.childCount - 1;
        //int puluh_numbers = Puluh.childCount - 1;

        int check_number = value;
        if (isMoreThanTen(value)) {
            check_number = value - (puluh_value * 10);
            Debug.Log("returned to : " + check_number);
        }

        // go through each sa column
        for (int i = 0; i < sa_numbers; i++)
        {
            Transform number = Sa.GetChild(i);
            if (i == check_number)
            {
                MeshRenderer green_lightbulb = number.GetChild(1).gameObject.GetComponent<MeshRenderer>();
                green_lightbulb.material.CopyPropertiesFromMaterial(green_light_on);
            }
        }
    }
}
