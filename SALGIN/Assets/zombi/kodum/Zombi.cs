using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombi : MonoBehaviour
{
    public float zombiHP = 100;
    Animator zombiAnim;
    bool zombiOlu;
    public float kovalamaMesafesi;
    public float saldirmaMesafesi;
    NavMeshAgent zombiNavMesh;
    float mesafe;
    GameObject hedefOyuncu;

    void Start()
    {
        zombiAnim = GetComponent<Animator>();
        hedefOyuncu = GameObject.Find("savasci");
        zombiNavMesh = GetComponent<NavMeshAgent>();

        if (zombiNavMesh == null)
        {
            Debug.LogError("NavMeshAgent bileþeni eksik!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (zombiHP <= 0 && !zombiOlu)
        {
            zombiOlu = true;
            zombiAnim.SetBool("oldu", true);
            StartCoroutine(YokOl());
        }

        if (zombiOlu) return;

        mesafe = Vector3.Distance(this.transform.position, hedefOyuncu.transform.position);

        if (mesafe < kovalamaMesafesi && mesafe >= saldirmaMesafesi)
        {
            zombiNavMesh.isStopped = false;
            zombiNavMesh.SetDestination(hedefOyuncu.transform.position);
            zombiAnim.SetBool("yuruyor", true);
            zombiAnim.SetBool("saldiriyor", false);
        }
        else if (mesafe < saldirmaMesafesi)
        {
            zombiNavMesh.isStopped = true;
            zombiAnim.SetBool("yuruyor", false);
            zombiAnim.SetBool("saldiriyor", true);
            transform.LookAt(hedefOyuncu.transform.position);
        }
        else
        {
            zombiNavMesh.isStopped = true;
            zombiAnim.SetBool("yuruyor", false);
            zombiAnim.SetBool("saldiriyor", false);
        }
    }

    public void HasarVer()
    {
        hedefOyuncu.GetComponent<KarakterKontrol>().HasarAl();
    }

    IEnumerator YokOl()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public void HasarAl()
    {
        zombiHP -= Random.Range(15, 30);
        Debug.Log("Zombi hasar aldý, kalan HP: " + zombiHP);

        if (zombiHP <= 0 && !zombiOlu)
        {
            zombiHP = 0;
            zombiOlu = true;
            Debug.Log("Zombi öldü!");
            zombiAnim.SetBool("oldu", true);
            StartCoroutine(YokOl());
        }
    }
}

