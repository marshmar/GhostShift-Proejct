using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditorInternal;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.VFX;

public class HeartGenerator : MonoBehaviour
{
    public GameObject enemy_ghost;
    public Transform enemy;
    public GameObject HeartPrefab;//�������� �޾ƿ�
    public GameObject[] PrefabControl;//������ �������� �����ϴ� �迭

    private int randNum;
    bool count; // ������ ������ �����ϱ� ���� ����

    private lastPosition targetObject;
   private Vector2 lastPosition;
    void Start()
    {
        randNum = Random.Range(1, 101); //1���� 100������ ���ڸ� �������� �޾ƿ´�
        Debug.Log(randNum);
        count = true;

        targetObject = FindObjectOfType<lastPosition>();
       
    }
    void lastPos()
    {
        if (enemy_ghost != null)
        {
            Enemy enemyScript = enemy_ghost.GetComponent<Enemy>();
            if (enemyScript.isDied != true)
            {
                // ������Ʈ�� ������ ��, ��ġ�� ���� �� ����            
                lastPosition = enemy_ghost.transform.position;
                Debug.Log("�ٸ� ������Ʈ�� ������ X ��ġ: " + lastPosition);

            }
        }
    }
    // Update is called once per frame
    void Update()
    {


       lastPos();
        if (enemy_ghost == null)
        {

            //20�ۼ�Ʈ Ȯ��

            if (randNum <= 20)
            {


                if (count == true)
                {
                    PrefabControl = GameObject.FindGameObjectsWithTag("Heart");


     
                    Debug.Log(lastPosition);
                    //enemy_ghost�� ���� ��ġ�� �����ͼ� ���ο� �������� ��ġ�� ����
                    Instantiate(HeartPrefab, new Vector2(lastPosition.x, lastPosition.y), Quaternion.identity); //������ ����
                    count = false;
                }


            }
            else
            {
                // Debug.Log("��Ʈ�� �������� ����");
            }

        }
            
        
    }


}
