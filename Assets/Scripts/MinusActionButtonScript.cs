using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinusActionButtonScript : MonoBehaviour {

    // signatures static variables
    private static string SIGNATURE_PLUS = "PLUS";
    private static string SIGNATURE_MINUS = "MINUS";
    private static string COLUMN_SA = "Sa";
    private static string COLUMN_PULUH = "Puluh";
    private static int UNCOUNTED_CHILD = 1;
    private static int UNCOUNTED_CHILD_INCL_10 = 2;

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
        Text bentuk_lazim_green = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Green").GetComponent<Text>();
        Text bentuk_lazim_red = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Red").GetComponent<Text>();
        Text bentuk_lazim_total = GameObject.FindGameObjectWithTag ("bentuk-lazim").transform.Find("Total").GetComponent<Text>();
        
        bentuk_lazim_green.text = green_value.ToString ();
        bentuk_lazim_red.text= red_value.ToString ();
        bentuk_lazim_total.text= total_number.ToString ();

        // bentuk biasa
        Text bentuk_biasa_green = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Green").GetComponent<Text>();
        Text bentuk_biasa_red = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Red").GetComponent<Text>();
        Text bentuk_biasa_total = GameObject.FindGameObjectWithTag ("bentuk-biasa").transform.Find("Total").GetComponent<Text>();
        
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
     * Logic on when PLUS is
     * 
     * 1. Add the number (0 becomes 1) on sa column
     * 2. Light up the number (1) on sa column
     * 3. When reach 10, light up blue button on puluh column
     * 4. when reach 11, (turn off all light bulbs); then turn on light bulb 1 on sa column
     * 5. when reach 100, disable green button
     * 
     * - every time press a button, update all the numbers in the UI to match latest numbers stored in program
     * */
    public void OnPressGreenUp () {
        signature = SIGNATURE_PLUS;

        if (total_number < 100)
        {
            LightBulbOn(COLUMN_SA, green_value);
            if (isTens(total_number))
                LightBulbOn(COLUMN_PULUH, puluh_value - 1);

            /** 1. update the current "sa" number **/
            sa_value++;
            green_value++;

            /** 2. increment/calculate total **/
            CalculateTotal();

            /** 3. update ui to match latest variables value */
            UpdateCalculationTexts();
        }
    }

    /**
     * Logic on when MINUS is
     * 
     * 1. Minus the number (1 becomes 0) on sa column
     * 2. Light off the number (1) on sa column
     * 3. When reach 10, light off blue button on puluh column, light app all sa buttons
     * 4. when reach 0, disable red button
     * 5. when first time press red button, green button is then disabled
     * 
     * - every time press a button, update all the numbers in the UI to match latest numbers stored in program
     * */
    public void OnPressRedUp () {
        signature = SIGNATURE_MINUS;

        if (total_number >= 0)
        {
            /** start disabling the green button */
            DisableAdding();

            /** 2. update the current "sa" number **/
            sa_value--;
            red_value++;

            if (isTens(total_number))
            {
                puluh_value--;
                LightBulbOff(COLUMN_PULUH, puluh_value);
                RefillColumns(COLUMN_SA, UNCOUNTED_CHILD_INCL_10);
            }

            LightBulbOff(COLUMN_SA, sa_value);

            /** 3. increment/calculate total **/
            CalculateTotal();
            if (total_number <= 0) DisableMinus();

            /** 4. update ui to match latest variables value */
            UpdateCalculationTexts();
        }

        //Debug.Log("green value: " + green_value);
        //Debug.Log("red value: " + red_value);
        //Debug.Log("sa value: " + sa_value);
        //Debug.Log("puluh value: " + puluh_value);
        //Debug.Log("total number: " + total_number);
        //Debug.Log("-------------------------------");
    }

    private void UpdateCalculationTexts()
    {
        /** 1. Get bentuk lazim and bentuk biasa, and update the text **/
        Text bentuk_lazim_green = GameObject.FindGameObjectWithTag("bentuk-lazim").transform.Find("Green").GetComponent<Text>();
        Text bentuk_lazim_red = GameObject.FindGameObjectWithTag("bentuk-lazim").transform.Find("Red").GetComponent<Text>();
        Text bentuk_lazim_total = GameObject.FindGameObjectWithTag("bentuk-lazim").transform.Find("Total").GetComponent<Text>();
        
        Text bentuk_biasa_green = GameObject.FindGameObjectWithTag("bentuk-biasa").transform.Find("Green").GetComponent<Text>();
        Text bentuk_biasa_red = GameObject.FindGameObjectWithTag("bentuk-biasa").transform.Find("Red").GetComponent<Text>();
        Text bentuk_biasa_total = GameObject.FindGameObjectWithTag("bentuk-biasa").transform.Find("Total").GetComponent<Text>();

        bentuk_lazim_green.text = green_value.ToString();
        bentuk_lazim_red.text = red_value.ToString();
        bentuk_lazim_total.text = total_number.ToString();

        bentuk_biasa_green.text = green_value.ToString();
        bentuk_biasa_red.text = red_value.ToString();
        bentuk_biasa_total.text = total_number.ToString();
    }

    /**
     * Turn off a selected bulb
     * 
     * string   column  "Sa"
     * int      value   any value 0 - 10
     * */
    private void LightBulbOff(string column, int value)
    {
        Transform Columns = GameObject.Find(column).transform;
        int columnLength = Columns.childCount - 1;
        int check_number = getCheckNumber(value);
        Debug.Log(column);

        // for puluh column, no need extra check because the numbers only go from 1 - 10
        if (column == COLUMN_PULUH)
            check_number = value;
        
        // turn on the light bulbs to start minusing again from above
        if (isTens(value) && !isZero(value) && column == COLUMN_SA)
            RefillColumns(COLUMN_SA, UNCOUNTED_CHILD);

        // mechanism to turn off the light
        for (int i = 0; i < columnLength; i++)
        {
            Transform number = Columns.GetChild(i);
            if (i == check_number)
            {
                MeshRenderer green_lightbulb = number.GetChild(1).gameObject.GetComponent<MeshRenderer>();
                green_lightbulb.material.CopyPropertiesFromMaterial(white_bulb);
            }
        }
    }

    /**
     * Turn on a selected bulb
     * 
     * string   column  "Sa"
     * int      value   any value 0 - 10
     * */
    private void LightBulbOn(string column, int value)
    {
        Transform Columns = GameObject.Find(column).transform;
        int columnLength = Columns.childCount - 1;
        int check_number = getCheckNumber(value);

        // clear the column if more than 10
        if (isMoreThanTen(value) && isTens(value)) {
            ClearColumns(COLUMN_SA);
        }

        // because the check number always just fall from 0 - 10; it just keep on lighting up these bulbs (will not overflow)
        for (int i = 0; i < columnLength; i++)
        {
            Transform number = Columns.GetChild(i);
            if (i == check_number)
            {
                MeshRenderer green_lightbulb = number.GetChild(1).gameObject.GetComponent<MeshRenderer>();
                green_lightbulb.material.CopyPropertiesFromMaterial(green_light_on);
            }
        }
    }

    /**
     * Clear whole column (selected column "Sa" or "Puluh")
     * */
    private void ClearColumns(string column)
    {
        Transform Columns = GameObject.Find(column).transform;
        int columnLength = Columns.childCount - 1; // don't include "Title"

        for (int i = 0; i < columnLength; i++)
        {
            Transform number = Columns.GetChild(i);
            MeshRenderer lightbulb = number.GetChild(1).gameObject.GetComponent<MeshRenderer>();
            lightbulb.material.CopyPropertiesFromMaterial(white_bulb);
        }
    }

    /**
     * Clear a lightbulb in specific column
     * */
    private void ClearColumn(string column, int value)
    {
        Transform Columns = GameObject.Find(column).transform;
        int columnLength = Columns.childCount - 1;
        int check_number = getCheckNumber(value);

        for (int i = 0; i < columnLength; i++)
        {
            Transform number = Columns.GetChild(i);
            if (i == check_number)
            {
                MeshRenderer green_lightbulb = number.GetChild(1).gameObject.GetComponent<MeshRenderer>();
                green_lightbulb.material.CopyPropertiesFromMaterial(white_bulb);
            }
        }
    }

    /**
     * Refill whole columns with light bulbs (selected column "Sa" or "Puluh")
     * */
    private void RefillColumns(string column, int uncounted_child)
    {
        Transform Columns = GameObject.Find(column).transform;
        int columnLength = Columns.childCount - uncounted_child;

        for (int i = 0; i < columnLength; i++)
        {
            Transform number = Columns.GetChild(i);
            MeshRenderer lightbulb = number.GetChild(1).gameObject.GetComponent<MeshRenderer>();
            lightbulb.material.CopyPropertiesFromMaterial(green_light_on);
        }
    }

    private void DisableAdding()
    {
        GameObject.Find("UpButton").GetComponent<Button>().enabled = false;
    }

    private void DisableMinus()
    {
        GameObject.Find("DownButton").GetComponent<Button>().enabled = false;
    }

    // utility functions goes here

    /**
     * This function keep tracks of whether the question is in "tens" or not
     * i.e: 10, 20, 30, ...
     * @return boolean
     * */
    private bool isTens(int number)
    {
        if (number % 10 == 0)
            return true;

        return false;
    }

    private bool isMoreThanTen(int value)
    {
        if (value >= 10)
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

    private int getCheckNumber(int value)
    {
        int check_number = value;
        int tens = puluh_value * 10;

        Debug.Log("check number before " + value);
        Debug.Log("tens " + tens);
        if (isMoreThanTen(value))
        {
            check_number = value - tens;
        }
        Debug.Log("check number after " + check_number);
        Debug.Log("-----------------------");

        return check_number;
    }

    private void CalculateTotal()
    {
        /** 1. Calculation to get the total **/
        total_number = green_value - red_value;

        if (isTens(total_number))
        {
            puluh_value = total_number / 10;
        }
    }
}
