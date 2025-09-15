using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public Animator ani;

    public TypewriterByCharacter textAnimatorPlayer;
    [TextArea(5, 50), SerializeField] //�ν����Ϳ� ���� ����(�ܺ� ��ũ��Ʈ������ �Ұ���)
    string textToShow = " ";
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        
    }
    public void ShowText()
    {
        textAnimatorPlayer.ShowText(textToShow);
    }
    public void message()
    {
        
        ani.SetBool("message", true);
       // Debug.Log("�޽��� ���̱�");
        ShowText(); //�޼��� �ڽ��� ���϶� ���� �ؽ�Ʈ �ִϸ��̼��� ����
    }
    public void messageBehind()
    {
     
        ani.SetBool("message", false);
        
       // Debug.Log("�޽��� ������");
        
    }
    // Update is called once per frame
    public void OnBecameInvisible() //������Ʈ�� ī�޶� �ۿ� �ִ��� üũ
    {
      //  Debug.Log("�Ⱥ���");
      
    }
    public void OnBecameVisible() //������Ʈ�� ī�޶� �ȿ� �ִ��� üũ
    {
     //   Debug.Log("����");
        
    }
}
