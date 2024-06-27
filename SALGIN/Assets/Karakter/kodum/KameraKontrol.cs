using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraKontrol : MonoBehaviour
{
    public Transform hedef; // Takip edilecek karakter
    public Vector3 hedefMesafe = new Vector3(0, 2, -4); // Karaktere göre kameranýn baþlangýç konumu
    public float sensitivity = 2f; // Fare duyarlýlýðý
    public Transform karakterVucut; // Karakterin vücut transformu

    private float mouseX = 0f;
    private float mouseY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Fareyi ekranýn ortasýna kilitle
        Cursor.visible = false; // Fareyi görünmez yap
    }

    void Update()
    {
        KamerayiDon();
    }

    void KamerayiDon()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;

        // Y ekseninde bakýþ limitlerini ayarla
        mouseY = Mathf.Clamp(mouseY, -35f, 60f);

        // Kamerayý döndür
        transform.localRotation = Quaternion.Euler(mouseY, mouseX, 0);

        // Karakterin sadece yatay eksende dönmesini saðla
        hedef.rotation = Quaternion.Euler(0, mouseX, 0);

        // Karakterin vücut rotasyonunu güncelle
        karakterVucut.localRotation = Quaternion.Euler(0, mouseX, 0);
    }

    void LateUpdate()
    {
        // Kamera pozisyonunu hedefin pozisyonuna ve hedef mesafesine göre güncelle
        Vector3 hedefKonum = hedef.position + hedefMesafe;
        transform.position = Vector3.Lerp(transform.position, hedefKonum, Time.deltaTime * 10);

        // Kamera hedefe bakar
        transform.LookAt(hedef.position + Vector3.up * 2); // Karakterin baþ hizasýna bakmasý için ayarlama
    }
}
