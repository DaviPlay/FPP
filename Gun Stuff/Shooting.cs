using System;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private int selectedWeapon = 0;

    public static Action semiShootInput;
    public static Action autoShootInput;
    public static Action reloadInput;
    public static Action inspectInput;

    public static Action updateText;

    private static IWeapon weapon;
    private static IData data;

    void Start()
    {
        foreach (Transform gunContainer in transform)
            foreach (Transform gunParent in gunContainer)
                foreach (Transform gun in gunParent)
                    if (gun.gameObject.activeSelf)
                    {
                        weapon = gun.gameObject.GetComponent<Gun>() != null ? gun.gameObject.GetComponent<Gun>() : gun.gameObject.GetComponent<Melee>();
                        data = weapon is Gun ? gun.gameObject.GetComponent<Gun>().GetData() : gun.gameObject.GetComponent<Melee>().GetData();
                    }

        updateText?.Invoke();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            autoShootInput?.Invoke();
            updateText?.Invoke();
        }
        
        if (Input.GetButtonDown("Fire1"))
        {
            semiShootInput?.Invoke();
            updateText?.Invoke();
        }

        if (Input.GetButtonDown("Reload"))
        {
            reloadInput?.Invoke();
            updateText?.Invoke();
        }

        if (Input.GetButtonDown("Inspect"))
            inspectInput?.Invoke();

        SwitchWeapon();
    }

    private void SwitchWeapon()
    {
        if (MenuFunctions.isGamePaused) return;
        int previousWeapon = selectedWeapon;

        if (Input.GetAxis("Scroll") > 0)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Scroll") < 0)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (previousWeapon != selectedWeapon)
            SelectWeapon();

        updateText?.Invoke();
    }

    public void SelectWeapon()
    {
        int i = 0;
        foreach (Transform gunContainer in transform)
            foreach (Transform gunParent in gunContainer)
                foreach (Transform gun in gunParent)
                {
                    if (i == selectedWeapon)
                    {
                        gun.gameObject.SetActive(true);
                        weapon = gun.gameObject.GetComponent<Gun>() != null ? gun.gameObject.GetComponent<Gun>() : gun.gameObject.GetComponent<Melee>();
                        data = weapon is Gun ? gun.gameObject.GetComponent<Gun>().GetData() : gun.gameObject.GetComponent<Melee>().GetData();
                    }
                    else
                        gun.gameObject.SetActive(false);

                    i++;
                }
    }

    public static IData GetData()
    {
        return data;
    }
}
