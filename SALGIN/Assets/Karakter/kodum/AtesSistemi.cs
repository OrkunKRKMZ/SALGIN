using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtesSistemi : MonoBehaviour
{
    public Camera kamera;
    public LayerMask zombiKatman;
    KarakterKontrol hpKontrol;
    public ParticleSystem muzzleFlash; // Muzzle flash efekti
    private Animator anim; // Animatör bileþeni

    private float sarjor = 25;
    private float cephane = 50;
    private float sarjorKapasitesi = 25; // Sarjör kapasitesini 25'ten 5'e deðiþtirdim, sarjor deðiþkeni ile uyumlu hale getirmek için

    void Start()
    {
        if (kamera == null)
        {
            kamera = Camera.main;
        }
        hpKontrol = this.gameObject.GetComponent<KarakterKontrol>(); // KarakterKontrol scriptine eriþim
        anim = GetComponent<Animator>(); // Animator bileþenine eriþim

        if (anim == null)
        {
            Debug.LogError("Animator bileþeni eksik!");
        }

        if (muzzleFlash == null)
        {
            muzzleFlash = GetComponentInChildren<ParticleSystem>();
            if (muzzleFlash == null)
            {
                Debug.LogError("Muzzle flash partikül sistemi bulunamadý!");
            }
        }
    }

    void Update()
    {
        if (hpKontrol.YasiyorMu())
        {
            if (Input.GetButtonDown("Fire1")) // Varsayýlan ateþ tuþu sol fare butonudur
            {
                if (sarjor > 0)
                {
                    AtesEtme();
                }
                else if (sarjor <= 0 && cephane > 0)
                {
                    anim.SetBool("sarjorDegistirme", true);
                    StartCoroutine(SarjorDegistirme());
                }
            }
        }
    }

    IEnumerator SarjorDegistirme()
    {
        // Sarjör deðiþtirme animasyonunun süresini bekleyin
        yield return new WaitForSeconds(2f); // Bu süreyi animasyon sürenize göre ayarlayýn

        float eksikMermi = sarjorKapasitesi - sarjor;
        if (cephane >= eksikMermi)
        {
            cephane -= eksikMermi;
            sarjor = sarjorKapasitesi;
        }
        else
        {
            sarjor += cephane;
            cephane = 0;
        }

        anim.SetBool("sarjorDegistirme", false);
    }

    void AtesEtme()
    {
        sarjor--;

        muzzleFlash.Play(); // Ateþ efektini oynat

        Ray ray = kamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, zombiKatman))
        {
            Debug.Log("Zombi vuruldu: " + hit.collider.gameObject.name);
            Zombi zombi = hit.collider.gameObject.GetComponent<Zombi>();
            if (zombi != null)
            {
                zombi.HasarAl();
            }
            else
            {
                Debug.LogWarning("Zombi scripti bulunamadý: " + hit.collider.gameObject.name);
            }
        }
    }
}
