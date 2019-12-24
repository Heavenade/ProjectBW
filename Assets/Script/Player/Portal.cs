﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    // 포탈의 화살표
    private GameObject arrow;
    // 건물 안으로 들어가는 문
    private GameObject door;
    // 각 포탈과 문을 이용해 갈 수 있는 지점
    public GameObject destination;      //포탈 출구

    private Animator Fadeanimator;      //Fadeinout용 애니메이터
    [SerializeField] private GameObject FadeImage;        //Fade용 이미지

    private void Awake() {
        // 포탈 스크립트를 가진 오브젝트는 오직 1개의 오브젝트만 가진다. (화살표 or 문)
        arrow = transform.GetChild(0).gameObject;
        door = arrow;
        arrow.SetActive(false);

        Fadeanimator = GameObject.Find("Fade_Image").transform.GetComponent<Animator>();
        FadeImage = GameObject.Find("Fade_Image");
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.tag == "character") {

            if(arrow != null)
                arrow.SetActive(true);

            if (//조건 시작
                (Input.GetKeyDown(KeyCode.W) && arrow.transform.name == "UpToTake") || (Input.GetKeyDown(KeyCode.S) && arrow.transform.name == "DownToTake")
                || (Input.GetKeyDown(KeyCode.A) && arrow.transform.name == "LeftToTake") || (Input.GetKeyDown(KeyCode.D) && arrow.transform.name == "RightToTake")
                || (Input.GetMouseButtonDown(0) && ( (door.transform.name == "RainaHouseDoor") || (door.transform.name == "BalruaHouseDoor") || (door.transform.name == "ChapterDoor") || (door.transform.name == "ViscountMansionDoor")
                                                       || (door.transform.name == "PresidentMansionDoor") || (door.transform.name == "SalonDoor")))
                //조건 끝
                ) {
                StartCoroutine(FadeWithTakePortal());
            }

            //if (Input.GetKeyDown(KeyCode.S) && arrow.transform.name == "DownToTake") {
            //    StartCoroutine(FadeWithTakePortal());
            //}

            //if (Input.GetKeyDown(KeyCode.A) && arrow.transform.name == "LeftToTake")
            //{
            //    StartCoroutine(FadeWithTakePortal());
            //}

            //if (Input.GetKeyDown(KeyCode.D) && arrow.transform.name == "RightToTake")
            //{
            //    StartCoroutine(FadeWithTakePortal());
            //}
            
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "character") {
            arrow.SetActive(false);
        }
    }

    public void TakePortal() {

        Vector3 tempPosition = PlayerManager.instance.GetPlayerPosition();
        tempPosition.x = destination.transform.position.x;
        tempPosition.y = destination.transform.position.y;
        PlayerManager.instance.SetPlayerPosition(tempPosition);
        string position = destination.transform.parent.parent.parent.name
                           + "_" + destination.transform.parent.parent.name;
        
        //MoveCamera에서 GetCurrentPosition사용
        PlayerManager.instance.SetCurrentPosition(position);

        MiniMapManager.instance.MoveArrowPosition();
        Debug.Log(PlayerManager.instance.GetCurrentPosition() + "으로 이동");
    }

    private IEnumerator FadeWithTakePortal()
    {
        /*페이드 아웃*/
        FadeImage.SetActive(true);
        Fadeanimator.SetBool("isfadeout", true);
        yield return new WaitForSeconds(0.5f);

        /*이동*/
        TakePortal();
        
        /*페이드 인*/
        yield return new WaitForSeconds(1f);
        Fadeanimator.SetBool("isfadeout", false);
    }

}
