﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{

    public static MiniMapManager instance = null;

    public GameObject sectorUI;
    public GameObject miniMapUI;

    public Texture Texture_Village_Street1;

    private GameObject arrow;
    private bool isOpen;
    private bool isZoomIn;

    private Vector3 miniMap_Position_Slum_Street1 = new Vector3(-10.4f, -4.5f, -1f);
    private Vector3 miniMap_Position_Slum_Street2 = new Vector3(-10f, -6.5f, -1f);
    private Vector3 miniMap_Position_Market_Street1 = new Vector3(-10.5f, 2f, -1f);
    private Vector3 miniMap_Position_Market_Street2 = new Vector3(-10.5f, 2f, -1f);
    private Vector3 miniMap_Position_Market_Street3 = new Vector3(-10.5f, 2f, -1f);
    private Vector3 miniMap_Position_Village_Street1 = new Vector3(2.5f, 13.6f, -1f);
    private Vector3 miniMap_Position_Village_Street2 = new Vector3(2.5f, 11.4f, -1f);
    private Vector3 miniMap_Position_Village_Street3 = new Vector3(2.5f, 8.8f, -1f);
    private Vector3 miniMap_Position_Mansion_Street1 = new Vector3(10f, 3.6f, -1f);
    private Vector3 miniMap_Position_Mansion_Street2 = new Vector3(10.5f, 1f, -1f);
    private Vector3 miniMap_Position_Mansion_Street3 = new Vector3(10f, -1f, -1f);
    private Vector3 miniMap_Position_Downtown_Street1 = new Vector3(-0.2f, -0.5f, -1f);

    void Start()
    {
        if (instance == null)
            instance = this;

        isOpen = false;
        isZoomIn = false;

        arrow = transform.GetChild(0).gameObject;
        MoveArrowPosition();
        miniMapUI.SetActive(false);
        sectorUI.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M) && isOpen == false) {
            miniMapUI.SetActive(true);
            isOpen = true;
        }

        else if(Input.GetKeyDown(KeyCode.M) && isOpen == true) {

            if(isZoomIn == true) {
                sectorUI.SetActive(false);
                miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
                isZoomIn = false;
            }

            miniMapUI.SetActive(false);
            isOpen = false;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && isZoomIn == true) {
            sectorUI.SetActive(false);
            miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(true);
            isZoomIn = false;
        }
    }

    public void MoveArrowPosition() {
        if(PlayerManager.instance.GetCurrentPosition() == "Slum_Street1") {
            arrow.transform.localPosition = miniMap_Position_Slum_Street1;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Slum_Street2") {
            arrow.transform.localPosition = miniMap_Position_Slum_Street2;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Market_Street1") {
            arrow.transform.localPosition = miniMap_Position_Market_Street1;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Market_Street2") {
            arrow.transform.localPosition = miniMap_Position_Market_Street2;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Market_Street3") {
            arrow.transform.localPosition = miniMap_Position_Market_Street3;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Village_Street1") {
            arrow.transform.localPosition = miniMap_Position_Village_Street1;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Village_Street2") {
            arrow.transform.localPosition = miniMap_Position_Village_Street2;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Village_Street3") {
            arrow.transform.localPosition = miniMap_Position_Village_Street3;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Mansion_Street1") {
            arrow.transform.localPosition = miniMap_Position_Mansion_Street1;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Mansion_Street2") {
            arrow.transform.localPosition = miniMap_Position_Mansion_Street2;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Mansion_Street3") {
            arrow.transform.localPosition = miniMap_Position_Mansion_Street3;
        }

        else if (PlayerManager.instance.GetCurrentPosition() == "Downtown_Street1") {
            arrow.transform.localPosition = miniMap_Position_Downtown_Street1;
        }
    }

    public void Village_Street1_Button() {
        isZoomIn = true;
        sectorUI.SetActive(true);
        miniMapUI.transform.Find("MiniMapRenderer").gameObject.SetActive(false);
        sectorUI.GetComponent<RawImage>().texture = Texture_Village_Street1;
    }
}