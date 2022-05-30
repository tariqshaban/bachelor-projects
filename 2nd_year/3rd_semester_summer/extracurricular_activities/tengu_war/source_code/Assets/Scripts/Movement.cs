using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject Fire;
    [SerializeField] GameObject Flame;
    GameObject newFire;
    Vector3 gunPort1;
    Vector3 gunPort2;
    Vector3 temp;
    float timer;
    bool alt;
    GameObject Projectiles;
    int randomz;

    GameObject Player;
    Vector3 move;
    Vector3 move1;


    void Start()
    {
        Player = GameObject.Find("Player");
        Projectiles = GameObject.Find("Projectiles");
        Flame = Instantiate(Flame);
        Flame.transform.parent = transform;
        Flame.transform.localScale= new Vector3(3,3,3);
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        currentRotation.y = 0;
        Flame.transform.rotation = Quaternion.Euler(currentRotation);
        Flame.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 15);
        alt = true;
        move1 = new Vector3(300, transform.position.y, Player.transform.position.z + 200);
        randomz = Random.Range(200,401);
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector3(transform.position.x, transform.position.y, Player.transform.position.z + randomz);
        move1 = new Vector3(move1.x,move1.y, Player.transform.position.z + randomz);
        move_to_pos();
        engage();
    }

    void move_to_pos()
    {
        if (transform.position.z!=Player.transform.position.z+200 && transform.position.y == 200)
            if(transform.position.z>Player.transform.position.z+2000)
                transform.position = Vector3.MoveTowards(transform.position, move, Time.deltaTime * 800);
            else
            if (transform.position.z > Player.transform.position.z + 1000 && transform.position.z < Player.transform.position.z + 2000)
                transform.position = Vector3.MoveTowards(transform.position, move, Time.deltaTime * 300);
            else
                transform.position = Vector3.MoveTowards(transform.position, move, Time.deltaTime * 160);
    }

    void engage()
    {
        if (transform.position != move1 && transform.position.y == 200 )
            transform.position = Vector3.MoveTowards(transform.position, move1, Time.deltaTime * 30);
        if (transform.position.y == 200 && transform.position.x == move1.x)
        {
            randomz = Random.Range(200, 401);
            move1 = new Vector3(Random.Range(-150, 150)+Player.transform.position.x, transform.position.y, Player.transform.position.z + randomz);
        }

        if (timer > (int)Random.Range(2,60))
        {
            StartCoroutine(waiter());
            timer = 0;
        }
        gunPort1 = new Vector3(transform.position.x - 3, transform.position.y, transform.position.z-25);
        gunPort2 = new Vector3(transform.position.x + 3, transform.position.y, transform.position.z-25);
        timer += Time.deltaTime;
    }

    IEnumerator waiter()
    {
        for (int i = 0; i < 8; i++)
        {
            if (alt)
            {
                newFire = Instantiate(Fire, gunPort1, transform.rotation);
                newFire.transform.parent = Projectiles.transform;
                temp = transform.rotation.eulerAngles;
                temp.y = 0f;
                newFire.transform.rotation = Quaternion.Euler(temp);
                newFire.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -75);
                Destroy(newFire, 3);
                alt = false;
            }
            else
            {
                newFire = Instantiate(Fire, gunPort2, transform.rotation);
                newFire.transform.parent = Projectiles.transform;
                temp = transform.rotation.eulerAngles;
                temp.y = 0f;
                newFire.transform.rotation = Quaternion.Euler(temp);
                newFire.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -75);
                Destroy(newFire, 3);
                alt = true;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}