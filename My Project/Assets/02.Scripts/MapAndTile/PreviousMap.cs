using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreviousMap : MonoBehaviour
{
    public SceneManagerEx.Scenes scene;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
           // Debug.Log("back��ũ��Ʈ return map �Լ� ����");
            GameObject.Find("fadeout").GetComponent<SceneFadeInOut>().ReturnMap();


        }
    }

    // Start is called before the first frame update
}

