using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
public class Guide : MonoBehaviour
{
   // public GameObject ghost;
    public GameObject goggles_guide;
    public GameObject shield_guide;
    public GameObject cleaner_guide;

    public Image gogglesImage;
    public Image shieldImage;
    public Image cleanerImage;

    
    public Animator ani;

    void Start()
    {
     //   ghost.SetActive(false);
        goggles_guide.SetActive(false);
        shield_guide.SetActive(false);
        cleaner_guide.SetActive(false);

        ani = GetComponent<Animator>();



    }

    // Update is called once per frame
    void Update()
    {
        Transform parentTransform = transform;
        Transform GhostObj = parentTransform.GetChild(0); // Player�� �ڽ� ������Ʈ �� Ghost
        Transform ShieldObj = parentTransform.GetChild(1); // Player�� �ڽ� ������Ʈ �� Shield
        Transform GogglesObj = parentTransform.GetChild(2); // Player�� �ڽ� ������Ʈ �� Goggle
        Transform CleanerObj = parentTransform.GetChild(5); // Player�� �ڽ� ������Ʈ �� Cleaner


        Color alpha_goggles = gogglesImage.color;
        Color alpha_shield = shieldImage.color;
        Color alpha_cleaner = cleanerImage.color;


        if (GhostObj != null && GhostObj.gameObject.activeSelf == true)//GhostObj Ȱ��ȭ ���� 
        {


            goggles_guide.SetActive(false);
            shield_guide.SetActive(false);
            cleaner_guide.SetActive(false);

            //Debug.Log("��Ʈ");

        }
        if (ShieldObj != null && ShieldObj.gameObject.activeSelf == true)
        {
            shield_guide.SetActive(true);

        }

        if (GogglesObj != null && GogglesObj.gameObject.activeSelf == true)
        {

            goggles_guide.SetActive(true);


        }


        if (CleanerObj != null && CleanerObj.gameObject.activeSelf == true)
        {


            cleaner_guide.SetActive(true);

        }

    }
}
   

