using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtesSistemi : MonoBehaviour
{
    public Camera kamera;
    public LayerMask zombiKatman;
    KarakterKontrol hpKontrol;
    public ParticleSystem muzzleFlash; // Muzzle flash efekti
    private Animator anim; // Animat�r bile�eni

    private float sarjor = 25;
    private float cephane = 50;
    private float sarjorKapasitesi = 25; // Sarj�r kapasitesini 25'ten 5'e de�i�tirdim, sarjor de�i�keni ile uyumlu hale getirmek i�in

    void Start()
    {
        if (kamera == null)
        {
            kamera = Camera.main;
        }
        hpKontrol = this.gameObject.GetComponent<KarakterKontrol>(); // KarakterKontrol scriptine eri�im
        anim = GetComponent<Animator>(); // Animator bile�enine eri�im

        if (anim == null)
        {
            Debug.LogError("Animator bile�eni eksik!");
        }

        if (muzzleFlash == null)
        {
            muzzleFlash = GetComponentInChildren<ParticleSystem>();
            if (muzzleFlash == null)
            {
                Debug.LogError("Muzzle flash partik�l sistemi bulunamad�!");
            }
        }
    }

    void Update()
    {
        if (hpKontrol.YasiyorMu())
        {
            if (Input.GetButtonDown("Fire1")) // Varsay�lan ate� tu�u sol fare butonudur
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
        // Sarj�r de�i�tirme animasyonunun s�resini bekleyin
        yield return new WaitForSeconds(2f); // Bu s�reyi animasyon s�renize g�re ayarlay�n

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

        muzzleFlash.Play(); // Ate� efektini oynat

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
                Debug.LogWarning("Zombi scripti bulunamad�: " + hit.collider.gameObject.name);
            }
        }
    }
}
