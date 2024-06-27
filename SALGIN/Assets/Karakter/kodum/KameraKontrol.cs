using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KameraKontrol : MonoBehaviour
{
    public Transform hedef; // Takip edilecek karakter
    public Vector3 hedefMesafe = new Vector3(0, 2, -4); // Karaktere g�re kameran�n ba�lang�� konumu
    public float sensitivity = 2f; // Fare duyarl�l���
    public Transform karakterVucut; // Karakterin v�cut transformu

    private float mouseX = 0f;
    private float mouseY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Fareyi ekran�n ortas�na kilitle
        Cursor.visible = false; // Fareyi g�r�nmez yap
    }

    void Update()
    {
        KamerayiDon();
    }

    void KamerayiDon()
    {
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * sensitivity;

        // Y ekseninde bak�� limitlerini ayarla
        mouseY = Mathf.Clamp(mouseY, -35f, 60f);

        // Kameray� d�nd�r
        transform.localRotation = Quaternion.Euler(mouseY, mouseX, 0);

        // Karakterin sadece yatay eksende d�nmesini sa�la
        hedef.rotation = Quaternion.Euler(0, mouseX, 0);

        // Karakterin v�cut rotasyonunu g�ncelle
        karakterVucut.localRotation = Quaternion.Euler(0, mouseX, 0);
    }

    void LateUpdate()
    {
        // Kamera pozisyonunu hedefin pozisyonuna ve hedef mesafesine g�re g�ncelle
        Vector3 hedefKonum = hedef.position + hedefMesafe;
        transform.position = Vector3.Lerp(transform.position, hedefKonum, Time.deltaTime * 10);

        // Kamera hedefe bakar
        transform.LookAt(hedef.position + Vector3.up * 2); // Karakterin ba� hizas�na bakmas� i�in ayarlama
    }
}
