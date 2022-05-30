using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    Camera Camera;
    Player player;
    [SerializeField] GameObject Flame;
    [SerializeField] GameObject Flame1;
    [SerializeField] GameObject Fire;
    GameObject newFire;
    Vector3 gunPort1;
    Vector3 gunPort2;
    Vector3 temp;
    float timer;
    float cooldown;
    bool alt;
    GameObject Projectiles;
    Vector3 dir;
    int current_weapon;
    GameObject imageObject;
    Image com;

    // Start is called before the first frame update
    void Start()
    {
        current_weapon = 0;
        cooldown = 15;
        Projectiles = GameObject.Find("Projectiles");
        alt = true;
        Camera = Camera.main;
        player = FindObjectOfType<Player>();
        player.GetComponent<Rigidbody>().velocity=new Vector3(0, 0,120);
        Flame=Instantiate(Flame);
        Flame.transform.parent = player.transform;
        Flame.transform.localScale = new Vector3(3, 3, 3);
        Flame1 =Instantiate(Flame1);
        Flame1.transform.parent = player.transform;
        Flame1.transform.localScale = new Vector3(3, 3, 3);
        Flame.transform.position = new Vector3(player.transform.position.x + 6, player.transform.position.y - 2, player.transform.position.z - 13);
        Flame1.transform.position = new Vector3(player.transform.position.x + 2.7f, player.transform.position.y - 2, player.transform.position.z - 13);
        imageObject = GameObject.FindGameObjectWithTag("Fire");
        com = imageObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((CrossPlatformInputManager.GetButton("Fire") || Input.GetButton("Fire1")))
        {
            if (current_weapon == 0 && timer > 0.1)
            {
                if (alt)
                {
                    newFire = Instantiate(Fire, gunPort1, Quaternion.identity);
                    newFire.transform.parent = Projectiles.transform;
                    newFire.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 400);
                    Destroy(newFire, 3);
                    alt = false;
                }
                else
                {
                    newFire = Instantiate(Fire, gunPort2, Quaternion.identity);
                    newFire.transform.parent = Projectiles.transform;
                    newFire.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 400);
                    Destroy(newFire, 3);
                    alt = true;
                }
                timer = 0;
            }
            else
            if (current_weapon == 1 && cooldown > 15)
            {
                StartCoroutine("delay");
                cooldown = 0;
            }
        }
        if ((CrossPlatformInputManager.GetButtonDown("Cycle") || Input.GetKeyDown(KeyCode.Slash)))
        {
            if (current_weapon == 1)
                current_weapon = 0;
            else
                current_weapon = 1;
        }
        Camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y+75, player.transform.position.z - 120);
        if (player.transform.position.y == 200)
        {
            if ((Input.GetAxisRaw("Horizontal") > 0 || CrossPlatformInputManager.GetAxisRaw("Horizontal")>0) && player.transform.position.x < 800 || (Input.GetAxisRaw("Horizontal") < 0 || CrossPlatformInputManager.GetAxisRaw("Horizontal") < 0) && player.transform.position.x > -200 )
            {
                player.transform.position = new Vector3(player.transform.position.x + Input.GetAxisRaw("Horizontal")/2, player.transform.position.y, player.transform.position.z + Input.GetAxisRaw("Vertical")/4);
                player.transform.position = new Vector3(player.transform.position.x + CrossPlatformInputManager.GetAxisRaw("Horizontal")*2, player.transform.position.y, player.transform.position.z + CrossPlatformInputManager.GetAxisRaw("Vertical"));
            }
            Vector3 currentRotation = transform.localRotation.eulerAngles;
            if(CrossPlatformInputManager.GetAxisRaw("Horizontal")!=0)
                currentRotation.x = CrossPlatformInputManager.GetAxisRaw("Horizontal") * 10;
            else
                currentRotation.x = Input.GetAxisRaw("Horizontal") * 10;
            player.transform.localRotation = Quaternion.Euler(currentRotation);
        }
        gunPort1 = new Vector3(player.transform.position.x + 7.5f, player.transform.position.y, player.transform.position.z+20);
        gunPort2 = new Vector3(player.transform.position.x + 0.7f,player.transform.position.y, player.transform.position.z+20);
        timer += Time.deltaTime;
        cooldown += Time.deltaTime;
        if (current_weapon == 1)
            com.fillAmount = cooldown / 15f;
        else
            com.fillAmount = 1;
    }

    IEnumerator delay()
    {
        int s = 0;
        List<GameObject> list = new List<GameObject>();
        foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
        {
            if (go.name == "Enemy" && go.transform.position.y==200)
                list.Add(go);
        }
        if(list.Count!=0)
        {
        int random = list.Count-1;
        GameObject target = list[random];
            for (int i = 0; i < 40; i++)
            {
                yield return new WaitForSeconds(0.1f);
                while (target != null && target.transform.position.y != 200)
                {
                    if (s++ == 10)
                        target = null;
                    if (list[random].transform.position.y == 200)
                        target = list[random];
                    else
                        random = Random.Range(0, list.Count);
                }
                newFire = Instantiate(Fire, gunPort1, Quaternion.identity);
                newFire.transform.parent = Projectiles.transform;
                if (target != null)
                dir = (target.transform.position - transform.position).normalized;
                newFire.GetComponent<Rigidbody>().velocity = new Vector3(dir.x * 3000, dir.y * 3000, dir.z * 3000);
                newFire.transform.LookAt(target.transform);
                Destroy(newFire, 3);
            }
        }
    }
}