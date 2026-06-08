using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject playerBullet;
    public AudioClip parryingSfx;
    private new AudioSource audio;
    public bool isParrying;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponentInParent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Bullet")
        {
            audio.clip = parryingSfx;
            audio.Play();
            GameObject hitflash = Instantiate(hitEffect, transform.position, transform.rotation);
            Destroy(hitflash, 0.2f);
            CameraShake.Instance.OnShakeCamera();
            if (isParrying)
            {
                Debug.Log("Æ¨°Ü³½ ÃÑ¾Ë »ý¼º");
                GameObject bullet = Instantiate(playerBullet, transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Sign(collider.GetComponent<Rigidbody2D>().velocity.x) * -150.0f, 0));
                isParrying = false;
            }
            Destroy(collider.gameObject);
        }
    }
}
