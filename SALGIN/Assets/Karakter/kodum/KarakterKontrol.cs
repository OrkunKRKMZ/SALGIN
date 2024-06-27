using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarakterKontrol : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    private float karakterHiz;

    private float saglik = 100;
    bool hayattaMi;

    void Start()
    {
        anim = GetComponent<Animator>();
        hayattaMi = true;
    }

    void Update()
    {
        if (saglik <= 0)
        {
            hayattaMi = false;
            anim.SetBool("yasiyorMu", hayattaMi);
        }

        if (hayattaMi)
        {
            Hareket();
        }
    }

    public bool YasiyorMu()
    {
        return hayattaMi;
    }

    public void HasarAl()
    {
        saglik -= Random.Range(5, 15);
    }

    void Hareket()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");
        anim.SetFloat("Horizontal", yatay);
        anim.SetFloat("Vertical", dikey);

        // Karakterin hareketi
        Vector3 hareket = new Vector3(yatay * karakterHiz * Time.deltaTime, 0, dikey * karakterHiz * Time.deltaTime);
        transform.Translate(hareket);
    }
}
