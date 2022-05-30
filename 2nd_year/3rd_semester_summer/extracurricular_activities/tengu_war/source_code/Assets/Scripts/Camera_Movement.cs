using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    /*
    Destroyer Destroyer;
    Terrain Terrain;
    Terrain Terrain1;
    //Terrain Terrain2;
    Terrain Terrain3;
    //Terrain Terrain4;
    Vector3 new_pos1;
    //Vector3 new_pos2;
    Vector3 new_pos3;
    //Vector3 new_pos4;
    GameObject Terrains_list;
    int multi;
    bool available;
    bool available1;
    bool available2;
    [SerializeField] TerrainData[] Terrains = new TerrainData[3];

    // Start is called before the first frame update
    void Start()
    {
        Terrains_list = GameObject.Find("Terrains");
        available = true;
        available1 = true;
        available2 = true;
        multi = 2000;
        Terrain = FindObjectOfType<Terrain>();
        Destroyer = FindObjectOfType<Destroyer>();
        new_pos1 = new Vector3(Terrain.transform.position.x + 600, Terrain.transform.position.y, Terrain.transform.position.z);
        //new_pos2 = new Vector3(Terrain.transform.position.x + 1200, Terrain.transform.position.y, Terrain.transform.position.z);
        new_pos3 = new Vector3(Terrain.transform.position.x - 600, Terrain.transform.position.y, Terrain.transform.position.z);
        //new_pos4 = new Vector3(Terrain.transform.position.x - 1200, Terrain.transform.position.y, Terrain.transform.position.z);
        for (int i = 0; i < 2; i++)
        {
            Terrain1 = Instantiate(Terrain, new_pos1, Quaternion.identity);
            Terrain1.transform.parent = Terrains_list.transform;
            Terrain1.terrainData = Terrains[Random.Range(0, 3)];
            //Terrain2 = Instantiate(Terrain, new_pos2, Quaternion.identity);
            // Terrain2.transform.parent = Terrains_list.transform;
            //Terrain2.terrainData = Terrains[Random.Range(0, 3)];
            Terrain3 = Instantiate(Terrain, new_pos3, Quaternion.identity);
            Terrain3.transform.parent = Terrains_list.transform;
            Terrain3.terrainData = Terrains[Random.Range(0, 3)];
            //Terrain4 = Instantiate(Terrain, new_pos4, Quaternion.identity);
            //Terrain4.transform.parent = Terrains_list.transform;
            //Terrain4.terrainData = Terrains[Random.Range(0, 3)];
            new_pos1 = new Vector3(Terrain1.transform.position.x, Terrain1.transform.position.y, Terrain1.transform.position.z + 2000);
            //new_pos2 = new Vector3(Terrain2.transform.position.x, Terrain2.transform.position.y, Terrain2.transform.position.z + 2000);
            new_pos3 = new Vector3(Terrain3.transform.position.x, Terrain3.transform.position.y, Terrain3.transform.position.z + 2000);
            //new_pos4 = new Vector3(Terrain4.transform.position.x, Terrain4.transform.position.y, Terrain4.transform.position.z + 2000);
        }
        var new_pos = new Vector3(Terrain.transform.position.x, Terrain.transform.position.y, Terrain.transform.position.z + 2000);
        Terrain = Instantiate(Terrain, new_pos, Quaternion.identity);
        Terrain.transform.parent = Terrains_list.transform;
        Terrain.terrainData = Terrains[Random.Range(0, 3)];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * 80);
        Destroyer.transform.position = new Vector3(transform.position.x, 100f, transform.position.z);
        if (transform.position.z < multi + 100 && transform.position.z > multi && available == true)
        {
            var new_pos = new Vector3(Terrain.transform.position.x, Terrain.transform.position.y, Terrain.transform.position.z + 2000);
            Terrain = Instantiate(Terrain, new_pos, Quaternion.identity);
            Terrain.transform.parent = Terrains_list.transform;
            Terrain.terrainData = Terrains[Random.Range(0, 3)];
            //new_pos2 = new Vector3(Terrain2.transform.position.x, Terrain2.transform.position.y, Terrain2.transform.position.z + 2000);
            //new_pos4 = new Vector3(Terrain4.transform.position.x, Terrain4.transform.position.y, Terrain4.transform.position.z + 2000);
            //Terrain2 = Instantiate(Terrain, new_pos2, Quaternion.identity);
            //Terrain2.transform.parent = Terrains_list.transform;
            //Terrain2.terrainData = Terrains[Random.Range(0, 3)];
            //Terrain4 = Instantiate(Terrain, new_pos4, Quaternion.identity);
            //Terrain4.transform.parent = Terrains_list.transform;
            //Terrain4.terrainData = Terrains[Random.Range(0, 3)];
            available = false;
        }
        if (transform.position.z < multi + 200 && transform.position.z > multi && available1 == true)
        {
            var new_pos = new Vector3(Terrain.transform.position.x, Terrain.transform.position.y, Terrain.transform.position.z + 2000);
            new_pos1 = new Vector3(Terrain1.transform.position.x, Terrain1.transform.position.y, Terrain1.transform.position.z + 2000);
            Terrain1 = Instantiate(Terrain, new_pos1, Quaternion.identity);
            Terrain1.transform.parent = Terrains_list.transform;
            Terrain1.terrainData = Terrains[Random.Range(0, 3)];
            available1 = false;
        }
        if (transform.position.z < multi + 300 && transform.position.z > multi && available2 == true)
        {
            var new_pos = new Vector3(Terrain.transform.position.x, Terrain.transform.position.y, Terrain.transform.position.z + 2000);
            new_pos3 = new Vector3(Terrain3.transform.position.x, Terrain3.transform.position.y, Terrain3.transform.position.z + 2000);
            Terrain3 = Instantiate(Terrain, new_pos3, Quaternion.identity);
            Terrain3.transform.parent = Terrains_list.transform;
            Terrain3.terrainData = Terrains[Random.Range(0, 3)];
            available2 = false;
            multi += 2000;
        }
        if (transform.position.z >= multi)
        {
            available = true;
            available1 = true;
            available2 = true;
        }
    }
    */
}