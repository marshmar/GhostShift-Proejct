using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextMap : MonoBehaviour
{
    public SceneManagerEx.Scenes scene;
  
    
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            Debug.Log("next��ũ��Ʈ fadeflow �Լ� ����");
            GameObject.Find("fadeout").GetComponent<SceneFadeInOut>().Fade();
           
            
        }
    }
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
