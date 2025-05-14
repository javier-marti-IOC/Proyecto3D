using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementsHUD : MonoBehaviour
{
    // Variables de color para los elementos
    Color32 fireColor = new Color32(194, 83, 83, 255); // #c25353
    Color32 fireColorBlink = new Color32(191, 68, 72, 255); // #bf4448
    Color32 waterColor = new Color32(62, 124, 178, 255); // #3e7cb2
    Color32 waterColorBlink = new Color32(15, 101, 175, 255); // #0f65af
    Color32 lightningColor = new Color32(223, 215, 100, 255); // #dfd764
    Color32 lightningColorBlink = new Color32(221, 209, 47, 255); // #ddd12f
    Color32 earthColor = new Color32(107, 172, 102, 255); //rgb(107, 172, 102)
    Color32 earthColorBlink = new Color32(80, 127, 76, 255); //rgb(80, 127, 76)

    // Color para opacidad de los iconos
    Color32 IconColorActive = new Color32(255,255,255,255);
    Color32 IconColorInactive = new Color32(255,255,255,215);

    // Color para opacidad de los bordes
    Color32 BorderColorActive = new Color32(255,255,255,255);
    Color32 BorderColorInactive = new Color32(255,255,255,150);

    // Imagenes que se utilizan para rellenar el elemento
    public Image fireElement; // Imagen de la barra de relleno de fuego
    public Image waterElement; // Imagen de la barra de relleno de agua
    public Image lightningElement; // Imagen de la barra de relleno de electricidad
    public Image earthElement; // Imagen de la barra de relleno de tierra

    // Imagenes de los iconos de los elementos.
    public Image fireElementIcon; // Icono de fuego
    public Image waterElementIcon; // Icono de agua
    public Image lightningElementIcon; // Icono de rayo
    public Image earthElementIcon; // Icono de tierra

    // Imagenes de los Selectores de los elementos.
    public Image fireElementSelector; // Selector de el fuego
    public Image waterElementSelector; // Selector de el agua
    public Image lightningElementSelector; //Selector de el rayo
    public Image earthElementSelector; // Selector de la tierra

    // Booleanas que se activan si esta parpadeando el color del elemento, para saber si esta activo o no.
    private bool fireBlink; // Parpadeo Fuego
    private bool waterBlink; // Parpadeo Agua
    private bool lightningBlink; // Parpadeo Electrico
    private bool earthBlink; // Parpadeo Tierra

    private float maxBarValue = 100; // Maximo valor de una barra elemental

    // Funciones para aumentar la imagen de la barra elemental. 
     public void fireAdd(float quantity) {
        fireElement.fillAmount = quantity / maxBarValue;

        if (fireElement.fillAmount >= 1f) // Si se llena el color del elemento cambia al color blink
        {
            fireElement.color = fireColorBlink;
        }
    }
    // Restar Barra de agua
    public void waterAdd(float quantity) {
        waterElement.fillAmount = quantity / maxBarValue;

        if (waterElement.fillAmount >= 1f) // Si se llena el color del elemento cambia al color blink
        {
            waterElement.color = waterColorBlink;
        }
    }
    // Restar Barra electricidad
    public void lightningAdd(float quantity) {
        lightningElement.fillAmount = quantity / maxBarValue;

        if (lightningElement.fillAmount >= 1f) // Si se llena el color del elemento cambia al color blink
        {
            lightningElement.color = lightningColorBlink;
        }
    }
    // Restar Barra de tierra
    public void earthAdd(float quantity) {
        earthElement.fillAmount = quantity / maxBarValue;

        if (earthElement.fillAmount >= 1f) // Si se llena el color del elemento cambia al color blink
        {
            earthElement.color = earthColorBlink;
        }
    }
    // Funciones para reducir la imagen de la barra elemental
    // Restar Barra de fuego
    public void fireReduce(float quantity) {
        fireElement.fillAmount = quantity / maxBarValue;
        FireStartBlink(); // Llama a la funcion de parpadeo
        fireElementIcon.color = IconColorActive;
        fireElementSelector.gameObject.SetActive(true);
    }
    // Restar Barra de agua
    public void waterReduce(float quantity) {
        waterElement.fillAmount = quantity / maxBarValue;
        WaterStartBlink(); // Llama a la funcion de parpadeo
        waterElementIcon.color = IconColorActive;
        waterElementSelector.gameObject.SetActive(true);
    }
    // Restar Barra electricidad
    public void lightningReduce(float quantity) {
        lightningElement.fillAmount = quantity / maxBarValue;
        LightningStartBlink();
        lightningElementIcon.color = IconColorActive;
        lightningElementSelector.gameObject.SetActive(true);
    }
    // Restar Barra de tierra
    public void earthReduce(float quantity) {
        earthElement.fillAmount = quantity / maxBarValue;
        EarthStartBlink();
        earthElementIcon.color = IconColorActive;
        earthElementSelector.gameObject.SetActive(true);
    }

    // Funciones para iniciar/desactivar parpadeo en barra.
    // Iniciar y detener parpadeo de fuego
    public void FireStartBlink() {
        if (!fireBlink) {
            fireBlink = true;
            StartCoroutine(BlinkFireImage());
        }
    }
    // Parar el parpadeo de fuego.
    public void FireStopBlink() {
        fireBlink = false;
        fireElementIcon.color = IconColorInactive;
        fireElementSelector.gameObject.SetActive(false);
        fireElement.color = fireColor;
    }

    public void ElementBlink(Element element) {

    }

    // Corrutina para el parpadeo de fuego.
    private IEnumerator BlinkFireImage() {
        while (fireBlink) {
            //fireElement.color = fireColorBlink;
            fireElementSelector.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            //fireElement.color = fireColor;
            fireElementSelector.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Iniciar y detener parpadeo de agua
    public void WaterStartBlink() {
        if (!waterBlink) {
            waterBlink = true;
            StartCoroutine(BlinkWaterImage());
        }
    }
    // Parar el parpadeo de agua.
    public void WaterStopBlink() {
        waterBlink = false;
        waterElementIcon.color = IconColorInactive;
        waterElementSelector.gameObject.SetActive(false);
        waterElement.color = waterColor;
    }

    // Corrutina para el parpadeo de agua.
    private IEnumerator BlinkWaterImage() {
        while (waterBlink) {
            //waterElement.color = waterColorBlink;
            waterElementSelector.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            //waterElement.color = waterColor;
            waterElementSelector.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Iniciar y detener parpadeo de electricidad
    public void LightningStartBlink() {
        if (!lightningBlink) {
            lightningBlink = true;
            StartCoroutine(BlinkLightningImage());
        }
    }
    // Parar el parpadeo de electricidad.
    public void LightningStopBlink() {
        lightningBlink = false;
        lightningElementIcon.color = IconColorInactive;
        lightningElementSelector.gameObject.SetActive(false);
        lightningElement.color = lightningColor;
    }

    // Corrutina para el parpadeo de electricidad.
    private IEnumerator BlinkLightningImage() {
        while (lightningBlink) {
            //lightningElement.color = lightningColorBlink;
            lightningElementSelector.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            //lightningElement.color = lightningColor;
            lightningElementSelector.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Iniciar y detener parpadeo de tierra
    public void EarthStartBlink() {
        if (!earthBlink) {
            earthBlink = true;
            StartCoroutine(BlinkEarthImage());
        }
    }
    // Parar el parpadeo de tierra.
    public void EarthStopBlink() {
        earthBlink = false;
        earthElementIcon.color = IconColorInactive;
        earthElementSelector.gameObject.SetActive(false);
        earthElement.color = earthColor;
    }

    // Corrutina para el parpadeo de tierra.
    private IEnumerator BlinkEarthImage() {
        while (earthBlink) {
            //earthElement.color = earthColorBlink;
            earthElementSelector.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            //earthElement.color = earthColor;
            earthElementSelector.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }


}
