using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage = 0;

    //サウンド関係
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound; //当たった時の音

    private void Start()
    {
        if (hitSound != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    //ダメージの値更新用
    public void inputDamageAmount(int _damage)
    {
        damage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    //何かに当たって消える際に音を鳴らす
    private void OnDestroy()
    {
        if(audioSource != null)
        {
            Debug.Log("鳴る");
            audioSource.PlayOneShot(hitSound);
        }
    }
}
