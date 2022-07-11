using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swipeRotate : MonoBehaviour
{
    public Transform player;
    public Transform cam;

    private Vector2 tapPosition;
    private Vector2 swipeDelta;
    private Vector2 swipeTotal;

    public float rigity = 5;

    private bool isSwiping;
    private bool swipingBegan;
    private bool isMobile;
    private float kz = 0;
    private float kx = 0;
    private Vector3 camPos = new Vector3(-7.0f, 9.37f, 0.64f);
    void Start()
    {
        Physics.gravity = new Vector3(0, -1.0f, 0); //строка дл€ рестартов, т.к. гравитаци€ - статична€ переменна€
        swipeTotal = Vector2.zero;
        isMobile = Application.isMobilePlatform;
        cam.LookAt(player);
    }

    
    void Update()
    {
        //изменение положени€ камеры (вращение вокруг шара)
        cam.position = new Vector3(player.position.x + camPos.x, player.position.y + camPos.y, player.position.z + camPos.z);  
        if((Physics.gravity.x<0.98f && swipeDelta.y>0) || (Physics.gravity.x > -0.98f && swipeDelta.y < 0))//ограничени€ дл€ поворота камеры
        {
            cam.RotateAround(player.position, Vector3.forward, 3 * swipeDelta.y / rigity * Time.deltaTime);//Pitch locally, yaw globally не работает?
        }
        if ((Physics.gravity.z < 0.98f && swipeDelta.x < 0) || (Physics.gravity.z > -0.98f && swipeDelta.x > 0))
        {
            cam.Rotate(Vector3.right * 3 * swipeDelta.x / rigity * Time.deltaTime, Space.World);
        }
        camPos = cam.position-player.position;
        if (!isMobile)//если на пк в юнити редакторе
        {
            if (Input.GetMouseButtonDown(0) && !isSwiping)
            {

                isSwiping = true;
                swipingBegan = true;
                tapPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ResetSwipe();
            }
        }
        else //непосредственно в качестве приложени€ на телефоне
        {
            if (Input.touchCount > 0 && !isSwiping)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    swipingBegan = true;
                    tapPosition = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled||Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    ResetSwipe();
                }
            }
            else if (Input.touchCount == 0)
            {
                ResetSwipe();
            }
        }
        CheckSwipe();
        swipingBegan = false;
    }
    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;

        if (isSwiping&&!swipingBegan)
        {
            if (!isMobile && Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - tapPosition;
                SwipeTotalPlus();
                tapPosition = Input.mousePosition;
            }
            else if (Input.touchCount > 0)
            {
                swipeDelta = Input.GetTouch(0).position - tapPosition;
                SwipeTotalPlus();
                tapPosition = Input.GetTouch(0).position;
            }
            kz = swipeTotal.x/400.0f/rigity; //переменные дл€ изменени€ гравитации
            kz = Mathf.Max(Mathf.Min(-kz,1),-1);
            kx = swipeTotal.y / 400.0f / rigity;
            kx = Mathf.Max(Mathf.Min(kx, 1), -1);

            Physics.gravity = new Vector3(kx, -5, kz); //изменение глобальной гравитации объектов


            //неудавша€с€ попытка с RotateAround всей сцены вокруг шара. Ќе удалено, т.к. могут внезапно стать нужны отдельные строки кода


            /*transform.RotateAround(player.position,  -1 * Vector3.forward, rigity  * swipeDelta.y * Time.deltaTime);
            transform.RotateAround(player.position, -1 * Vector3.right, rigity * swipeDelta.x * Time.deltaTime);
            
            float kx = transform.localEulerAngles.x;
            if(kx>30 && kx < 330)
            {
                if(330-kx < kx - 30){kx = 330;}
                else { kx = 30; }
            }
            float kz = transform.localEulerAngles.z;
            if (kz > 30 && kz < 330)
            {
                if (330 - kz < kz - 30) { kz = 330; }
                else { kz = 30; }
            }
            transform.localEulerAngles = new Vector3(kx, 0, kz);*/

            //¬ывод информации (если нужно)

            //print("swipe"+swipeDelta);
            //print("trans"+transform.localEulerAngles);
            //print("grav"+Physics.gravity);
        }
    }
    private void SwipeTotalPlus()//изменение суммы всех свапов с границей в 400
    {
        swipeTotal += swipeDelta;
        swipeTotal.x = Mathf.Max(Mathf.Min(swipeTotal.x, 400*rigity), -400 * rigity);
        swipeTotal.y = Mathf.Max(Mathf.Min(swipeTotal.y, 400 * rigity), -400 * rigity);
    }
    private void ResetSwipe() //обнуление свапа дл€ подготовки к новому
    {
        isSwiping = false;
        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }
}