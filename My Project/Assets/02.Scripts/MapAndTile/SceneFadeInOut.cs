using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEditor.SearchService;
//using UnityEditorInternal;

public class SceneFadeInOut : MonoBehaviour
{

    private GameObject playerObj;
    private Health healthScr;
    public Image Panel;
    float time = 0f;
    float frameTime = 1f;

    public static SceneFadeInOut instance = null;

    bool coroutineControl=false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
                Destroy(this.gameObject);
        }
        playerObj = GameObject.Find("Player");
        healthScr = playerObj.GetComponent<Health>();
    }

    void Start()
    {
        
/*        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;

        }
        instance = this;
        DontDestroyOnLoad(gameObject);*/
    }

    public void Fade()
    {
        coroutineControl = true;
        StartCoroutine(FadeFlow());

    }
    public void Fade2()
    {
        coroutineControl = true;
        StartCoroutine(FadeFlow2());

    }
    public void ReturnMap()
    {
        coroutineControl = true;
        StartCoroutine(FadeFlowReturnMap());

    }
    public void ReturnMap2()
    {
        coroutineControl = true;
        StartCoroutine(FadeFlowReturnMap2());

    }
    IEnumerator FadeFlow()
    {
        Debug.Log("������");
        healthScr.isInvincible = true;
        //NextMap�� �ڽ� ������Ʈ Gate ã��
        GameObject otherGameObject = GameObject.Find("NextMap");
        Transform GridTransform = otherGameObject.transform.Find("Grid");
        Transform GateTransform = GridTransform.transform.Find("Gate");


        //Gate ������Ʈ���� NextMap ��ũ��Ʈ���� scene ���� ����
        NextMap NextMapScript = GateTransform.GetComponent<NextMap>();
        SceneManagerEx.Scenes sceneName = NextMapScript.scene;

        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);

        //���� �� �ε�
        if (coroutineControl == true)
        {
            SceneManagerEx.Instance.LoadScene(sceneName);
        }
        yield return new WaitForSeconds(0.5f);

        //���� ���� ����Ʈ ���� ���ٸ� �ش�Ǵ� ��ġ�� �÷��̾ �̵�
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Stage1_Map2"|| currentSceneName == "Stage1_Map4")
        {

           // Debug.Log("���� ���� map2�� �̵��մϴ�.");
            playerObj.transform.position = new Vector2(-16, -3);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map3")
        {
            Debug.Log("���� ���� map3�� �̵��մϴ�.");
            playerObj.transform.position = new Vector2(-16, 5.4f);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map5")
        {
            playerObj.transform.position = new Vector2(-16, -4);
            coroutineControl = false;
        }
        if ( currentSceneName == "Stage1_Map7" || currentSceneName == "Stage1_Map8")
        {
            playerObj.transform.position = new Vector2(-16, -4);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map6")
        {

            playerObj.transform.position = new Vector2(-16, 5);
            coroutineControl = false;
        }
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
        healthScr.isInvincible = false;


        yield return null;
    }

    //Stage1_Map4���� �������� ó���ϱ�����
    IEnumerator FadeFlow2()
    {
        healthScr.isInvincible = true;
        //SceneChangeAreaTwo�� �ڽ� ������Ʈ Gate ã��
        GameObject otherGameObject = GameObject.Find("NextMap_gate2");
        Transform GridTransform = otherGameObject.transform.Find("Grid");
        Transform GateTransform = GridTransform.transform.Find("Gate");


        //Gate ������Ʈ���� SceneChangeArea ��ũ��Ʈ���� scene ���� ����
        NextMap_gate2 NextMap_gate2Script = GateTransform.GetComponent<NextMap_gate2>();
        SceneManagerEx.Scenes sceneName = NextMap_gate2Script.scene;

        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);

        //�� �ε�
        if (coroutineControl == true)
        {
            SceneManagerEx.Instance.LoadScene(sceneName);
            foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
            {
                if (o.CompareTag("Bullet"))
                {
                    Destroy(o);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);

        //���� ���� ����Ʈ ���� ���ٸ� �ش�Ǵ� ��ġ�� �÷��̾ �̵�
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Stage1_Map2" || currentSceneName == "Stage1_Map4")
        {


            playerObj.transform.position = new Vector2(-16, -3);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map3" || currentSceneName == "Stage1_Map6")
        {

            playerObj.transform.position = new Vector2(-16, 5);
            coroutineControl = false;
        }
        if(currentSceneName == "Stage1_Map7")
        {

            playerObj.transform.position = new Vector2(-16, 7);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map5" ||  currentSceneName == "Stage1_Map8")
        {
            playerObj.transform.position = new Vector2(-16, -4);
            coroutineControl = false;
        }
     
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);


        healthScr.isInvincible = false;
        yield return null;
    }
    
    
    //Stage1_Map7���� �������� ó���ϱ� ����

    IEnumerator FadeFlowReturnMap()

    {
        healthScr.isInvincible = true;
        Debug.Log("������");
        //ReturnMap�� �ڽ� ������Ʈ Gate ã��
        GameObject otherGameObject = GameObject.Find("PreviousMap");
        Transform GridTransform = otherGameObject.transform.Find("Grid");
        Transform GateTransform = GridTransform.transform.Find("Gate");


        //Gate ������Ʈ���� NextMap ��ũ��Ʈ���� scene ���� ����
        PreviousMap PreviousMapScript = GateTransform.GetComponent<PreviousMap>();
        SceneManagerEx.Scenes pre_sceneName = PreviousMapScript.scene;

        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);

        //�� �ε�
        if (coroutineControl == true)
        {
            SceneManagerEx.Instance.LoadScene(pre_sceneName);
            foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
            {
                if (o.CompareTag("Bullet"))
                {
                    Destroy(o);
                }
            }
        }
        Debug.Log("�� �ε�");
        yield return new WaitForSeconds(0.5f);
       
        //���� ���� ����Ʈ ���� ���ٸ� �ش�Ǵ� ��ġ�� �÷��̾ �̵�
        string currentSceneName = SceneManager.GetActiveScene().name;
      
            if (currentSceneName == "Stage1_Map1")
            {
                playerObj.transform.position = new Vector2(10, -3);
                Debug.Log("�������� map1�� ���ư��ϴ�.");
                coroutineControl = false;
             }
            if (currentSceneName == "Stage1_Map2")
            {
                playerObj.transform.position = new Vector2(10, 3f);
                Debug.Log("�������� map2�� ���ư��ϴ�.");
                coroutineControl = false;
            }
            if (currentSceneName == "Stage1_Map3" || currentSceneName == "Stage1_Map7")
            {

                playerObj.transform.position = new Vector2(10, -3.7f);
                Debug.Log("�������� map3�� ���ư��ϴ�.");
                coroutineControl = false;
             }
            if (currentSceneName == "Stage1_Map4")//map4 down gate
            {

                playerObj.transform.position = new Vector2(20, -3f);
                Debug.Log("�������� map4�� ���ư��ϴ�." + currentSceneName);
                coroutineControl = false;

        }
            if (currentSceneName == "Stage1_Map5")
            {
                playerObj.transform.position = new Vector2(37, -3f);
                coroutineControl = false;
            }

        

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);


        healthScr.isInvincible = false;
        yield return null;
    }
    IEnumerator FadeFlowReturnMap2()
    {
        healthScr.isInvincible = true;
        Debug.Log("������");
        //ReturnMap�� �ڽ� ������Ʈ Gate ã��
        GameObject otherGameObject = GameObject.Find("PreviousMap_gate2");
        Transform GridTransform = otherGameObject.transform.Find("Grid");
        Transform GateTransform = GridTransform.transform.Find("Gate");


        //Gate ������Ʈ���� NextMap ��ũ��Ʈ���� scene ���� ����
        PreviousMap_gate2 PreviousMap_gate2Script = GateTransform.GetComponent<PreviousMap_gate2>();
        SceneManagerEx.Scenes pre_sceneName = PreviousMap_gate2Script.scene;

        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        time = 0f;
        yield return new WaitForSeconds(0.5f);

        //�� �ε�
        if (coroutineControl == true)
        {
            SceneManagerEx.Instance.LoadScene(pre_sceneName);
        }
        yield return new WaitForSeconds(0.5f);

        //���� ���� ����Ʈ ���� ���ٸ� �ش�Ǵ� ��ġ�� �÷��̾ �̵�
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Stage1_Map4")
        {
            playerObj.transform.position = new Vector2(20, 8.5f);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map6")
        {

            playerObj.transform.position = new Vector2(22, 1.3f);
            coroutineControl = false;
        }
        if (currentSceneName == "Stage1_Map7")
        {
            playerObj.transform.position = new Vector2(10, -3.5f);
            coroutineControl = false;
        }

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / frameTime;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);


        healthScr.isInvincible = false;
        yield return null;
    }
}
