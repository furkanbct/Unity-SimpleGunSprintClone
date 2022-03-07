using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Pistol : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI tempGoldText;
    int goldAmount;//Collected Golds in Game

    [Header("Fire Rate Options")]
    public float fireRate;
    float nextTimeToFire;
    bool shoot;

    [SerializeField] GameObject bulletPrefab;

    [Header("Force Options")]
    [SerializeField] float forceMultiplier;
    [SerializeField] Transform forcePoint;

    Animator animator;
    AudioSource source;
    Rigidbody rb;
    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        source = gameObject.GetComponent<AudioSource>();
        animator = gameObject.GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        nextTimeToFire = Time.time;
        goldText.text = PlayerPrefs.GetInt("Gold").ToString();//Collected Golds All Time
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + fireRate;
                shoot = true;
            }
        }
    }
    void FixedUpdate()
    {
        if (shoot)
        {
            Shoot();
            shoot = false;
        }
    }
    void Shoot()
    {
        source.Play();
        animator.SetTrigger("Shoot");
        RaycastHit hit;
        if (Physics.Raycast(forcePoint.position, forcePoint.forward, out hit, 50))
        {
            if (hit.collider.gameObject.transform.root.CompareTag("Player"))
            {
                GameObject root = hit.collider.gameObject.transform.root.gameObject;
                root.GetComponent<Collider>().enabled = false;
                root.GetComponent<Animator>().enabled = false;
                root.gameObject.layer = LayerMask.NameToLayer("Bullet");
                root.gameObject.tag = "Untagged";
                foreach (Rigidbody part in root.GetComponentsInChildren<Rigidbody>())
                {
                    part.AddExplosionForce(10, transform.position, 5, 1, ForceMode.Impulse);
                }
                foreach (Collider part in root.GetComponentsInChildren<Collider>())
                {
                    part.gameObject.layer = LayerMask.NameToLayer("Bullet");
                    part.gameObject.tag = "Untagged";
                }
                goldAmount += 10;
                tempGoldText.text = goldAmount.ToString();
            }
            else if (hit.collider.gameObject.transform.root.gameObject.CompareTag("Multiplier"))
            {
                if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
                {
                    Multiplier ml = hit.collider.gameObject.transform.root.gameObject.GetComponent<Multiplier>();
                    ml.mCube.material = new Material(ml.mCube.material);
                    ml.mCube.material.color = Color.white;
                    goldAmount *= ml.multiplyAmount;
                    tempGoldText.text = goldAmount.ToString();
                    StartCoroutine(Finish());
                }
            }
        }
        GameObject bullet = Instantiate(bulletPrefab, forcePoint.position, forcePoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 150, ForceMode.Impulse);
        rb.AddForceAtPosition((transform.right * forceMultiplier), forcePoint.position, ForceMode.Impulse);
    }
    IEnumerator Finish()
    {
        PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") + goldAmount);
        goldText.text = PlayerPrefs.GetInt("Gold").ToString();
        enabled = false;
        yield return new WaitForSecondsRealtime(2);
        gameManager.Finish();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DeadTrigger")
        {
            gameManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
