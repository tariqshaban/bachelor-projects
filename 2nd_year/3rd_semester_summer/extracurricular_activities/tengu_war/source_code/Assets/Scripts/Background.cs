using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Player player;
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Flame;
    GameObject tempEnemy;
    GameObject tempFlame;
    [SerializeField] GameObject Ally;
    [SerializeField] GameObject AllyFlame;
    [SerializeField] GameObject AllyFlame1;
    GameObject tempAlly;
    GameObject tempFlame1;
    GameObject tempFlame2;
    GameObject Back;
    Vector3 pos;
    Vector3[] Formation;
    Vector3 random;

    // Start is called before the first frame update
    void Start()
    {
        Formation= new[] { new Vector3(0f, 0f, 0f), new Vector3(50f, 1f, -50f), new Vector3(-50f, 0f, -50f), new Vector3(100f, 1f, -100f), new Vector3(-100f, 0f, -100f), new Vector3(150f, 1f, -150f), new Vector3(-150f, 0f, -150f) };
        Back = GameObject.Find("Background");
        player = FindObjectOfType<Player>();
        InvokeRepeating("E", 0, Random.Range(9, 11));
        InvokeRepeating("P", 0, Random.Range(9, 11));
    }

    void E()
    {
        random = new Vector3(Random.Range(-1000, 1001), Random.Range(-200, 201), Random.Range(3000, 4001));
        for (int i = 0; i < Random.Range(2, 7); i++)
        {
            pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z)-Formation[i]+random;
            tempEnemy = Instantiate(Enemy, pos, Quaternion.identity);
            Vector3 currentRotation = Enemy.transform.localRotation.eulerAngles;
            currentRotation.y = 270;
            tempEnemy.transform.rotation = Quaternion.Euler(currentRotation);
            tempEnemy.transform.parent = Back.transform;
            tempEnemy.name = "Background_Enemy";
            tempEnemy.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, -600);
            Destroy(tempEnemy, 8);
            tempFlame = Instantiate(Flame);
            tempFlame.transform.parent = tempEnemy.transform;
            tempFlame.transform.localScale = new Vector3(3, 3, 3);
            Vector3 currentRotation1 = tempEnemy.transform.localRotation.eulerAngles;
            currentRotation1.y = 0;
            tempFlame.transform.rotation = Quaternion.Euler(currentRotation1);
            tempFlame.transform.position = new Vector3(tempEnemy.transform.position.x, tempEnemy.transform.position.y, tempEnemy.transform.position.z + 15);
        }
    }
    void P()
    {
        random = new Vector3(Random.Range(-1000, 1001), Random.Range(-200, 201), Random.Range(-500, -1001));
        for (int i = 0; i < Random.Range(2, 7); i++)
        {
            pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) + Formation[i] + random;
            tempAlly = Instantiate(Ally, pos, Quaternion.identity);
            tempAlly.transform.parent = Back.transform;
            Vector3 currentRotation = tempAlly.transform.localRotation.eulerAngles;
            currentRotation.y = 90;
            tempAlly.transform.rotation = Quaternion.Euler(currentRotation);
            tempAlly.transform.parent = Back.transform;
            tempAlly.name = "Background_Ally";
            tempAlly.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 600);
            Destroy(tempAlly, 8);
            tempFlame1 = Instantiate(AllyFlame);
            tempFlame1.transform.parent = tempAlly.transform;
            tempFlame1.transform.localScale = new Vector3(3, 3, 3);
            tempFlame1.transform.position = new Vector3(tempAlly.transform.position.x + 6, tempAlly.transform.position.y - 2, tempAlly.transform.position.z - 13);
            tempFlame2 = Instantiate(AllyFlame1);
            tempFlame2.transform.parent = tempAlly.transform;
            tempFlame2.transform.localScale = new Vector3(3, 3, 3);
            tempFlame2.transform.position = new Vector3(tempAlly.transform.position.x + 2.7f, tempAlly.transform.position.y - 2, tempAlly.transform.position.z - 13);
        }
    }
}
