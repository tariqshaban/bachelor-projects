using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject Enemy;
    GameObject Player;
    GameObject New_Enemy;
    GameObject Enemies;
    Vector3 Spawn_Position;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        Enemies = GameObject.Find("Enemies");
        InvokeRepeating("Waves",0,Random.Range(6,11));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Waves()
    {
        for (int i = 0; i < Random.Range(2,7); i++)
        {
            Spawn_Position = new Vector3(Player.transform.position.x + Random.Range(-250, 250), Player.transform.position.y, Player.transform.position.z + Random.Range(4000, 6000));
            New_Enemy = Instantiate(Enemy, Spawn_Position, Quaternion.identity);
            New_Enemy.transform.parent = transform;
            Vector3 currentRotation = transform.localRotation.eulerAngles;
            currentRotation.y = 270;
            New_Enemy.transform.rotation = Quaternion.Euler(currentRotation);
            New_Enemy.transform.parent = Enemies.transform;
            New_Enemy.name = "Enemy";
        }
    }
}