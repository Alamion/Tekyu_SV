using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlatforms : MonoBehaviour
{
    public Transform player;
    public GameObject rampPrefab;
    public Transform[] ramps; 
    void Start()
    {
        ramps = new Transform[10];
        ramps[0] = transform.GetChild(0);
        for(int j = 0; j < 9; j++)
        {
            Vector3 prelastRamp = ramps[j].position;
            ramps[j+1] = Instantiate(rampPrefab, new Vector3(prelastRamp.x, prelastRamp.y - 50, prelastRamp.z - 26), new Quaternion(0, 0, 0, 0)).transform;
            ramps[j+1].parent = transform;
        }
    }

    void Update()//обновление под новые данные ramps, проверка, нужно ли создавать новый уровень
    {
        ramps = new Transform[transform.childCount];
        int i = 0;
        foreach (Transform t in transform)
        {
            ramps[i++] = t;
        }
        if (ramps[1].position.y - player.position.y >= 16)
        {
            NewRamp();
        }
    }
    void NewRamp() //удаляем старую деталь, создаём новую уровнем ниже, присваиваем ей родителя
    {
        Destroy(ramps[0].gameObject);
        Vector3 prelastRamp = ramps[ramps.Length - 1].position;
        ramps[ramps.Length - 1] = Instantiate(rampPrefab,new Vector3(prelastRamp.x, prelastRamp.y-50, prelastRamp.z-26), new Quaternion(0, 0, 0, 0)).transform;
        ramps[ramps.Length - 1].parent = transform;
    }
}
