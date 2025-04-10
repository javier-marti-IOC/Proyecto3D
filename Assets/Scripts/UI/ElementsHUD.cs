using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementsHUD : MonoBehaviour
{
    // Variables de color para los elementos
    Color32 fireColor = new Color32(194, 83, 83, 255); // #c25353
    Color32 fireColorBlink = new Color32(199, 58, 58, 255); // #c73a3a
    Color32 fireColorActive = new Color32(199, 58, 58, 255); //rgb(216, 37, 37)
    Color32 waterColor = new Color32(63, 130, 194, 255); // #3f82c2
    Color32 waterColorBlink = new Color32(136, 182, 225, 255); // #88b6e1
    Color32 lightningColor = new Color32(209, 181, 74, 255); // #d1b54a
    Color32 lightningColorBlink = new Color32(227, 185, 18, 255); // #e3b912
    Color32 earthColor = new Color32(106, 158, 88, 255); // #6a9e58
    Color32 earthColorBlink = new Color32(87, 128, 73, 255); // #578049

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
        fireElement.fillAmount = fireElement.fillAmount + quantity / maxBarValue;
    }
    // Restar Barra de agua
    public void waterAdd(float quantity) {
        waterElement.fillAmount = waterElement.fillAmount + quantity / maxBarValue;
    }
    // Restar Barra electricidad
    public void lightningAdd(float quantity) {
        lightningElement.fillAmount = lightningElement.fillAmount + quantity / maxBarValue;
    }
    // Restar Barra de tierra
    public void earthAdd(float quantity) {
        earthElement.fillAmount = earthElement.fillAmount + quantity / maxBarValue;
    }
    // Funciones para reducir la imagen de la barra elemental
    // Restar Barra de fuego
    public void fireReduce(float quantity) {
        fireElement.fillAmount = fireElement.fillAmount + quantity / maxBarValue;
        FireStartBlink(); // Llama a la funcion de parpadeo
        fireElementIcon.color = IconColorActive;
        fireElementSelector.gameObject.SetActive(true);
    }
    // Restar Barra de agua
    public void waterReduce(float quantity) {
        waterElement.fillAmount = waterElement.fillAmount + quantity / maxBarValue;
        WaterStartBlink(); // Llama a la funcion de parpadeo
        waterElementIcon.color = IconColorActive;
        waterElementSelector.gameObject.SetActive(true);
    }
    // Restar Barra electricidad
    public void lightningReduce(float quantity) {
        lightningElement.fillAmount = lightningElement.fillAmount + quantity / maxBarValue;
        LightningStartBlink();
        lightningElementIcon.color = IconColorActive;
        lightningElementSelector.gameObject.SetActive(true);
    }
    // Restar Barra de tierra
    public void earthReduce(float quantity) {
        earthElement.fillAmount = earthElement.fillAmount + quantity / maxBarValue;
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
    }

    // Corrutina para el parpadeo de fuego.
    private IEnumerator BlinkFireImage() {
        while (fireBlink) {
            fireElement.color = fireColorBlink;
            yield return new WaitForSeconds(0.5f);
            fireElement.color = fireColor;
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
    }

    // Corrutina para el parpadeo de agua.
    private IEnumerator BlinkWaterImage() {
        while (waterBlink) {
            waterElement.color = waterColorBlink;
            yield return new WaitForSeconds(0.5f);
            waterElement.color = waterColor;
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
    }

    // Corrutina para el parpadeo de electricidad.
    private IEnumerator BlinkLightningImage() {
        while (lightningBlink) {
            lightningElement.color = lightningColorBlink;
            yield return new WaitForSeconds(0.5f);
            lightningElement.color = lightningColor;
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
    }

    // Corrutina para el parpadeo de tierra.
    private IEnumerator BlinkEarthImage() {
        while (earthBlink) {
            earthElement.color = earthColorBlink;
            yield return new WaitForSeconds(0.5f);
            earthElement.color = earthColor;
            yield return new WaitForSeconds(0.5f);
        }
    }


}
