using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class hit : MonoBehaviour
{
    int hits = 0;
    Image HealthBar;
    Image PHealthBar;
    TextMeshProUGUI Score;

    void OnTriggerEnter(Collider collision)
    {
        Score = FindObjectOfType<TextMeshProUGUI>();
        if (!(name == "Enemy" && collision.gameObject.name == "Enemy") && !(name == "Apollo_Fire Variant(Clone)" && collision.gameObject.name == "Enemy") && !(name == "Enemy" && collision.gameObject.name == "Apollo_Fire Variant(Clone)") && !(name == "Tengu_Fire Variant(Clone)" && collision.gameObject.name == "Player") && !(name == "Player" && collision.gameObject.name == "Tengu_Fire Variant(Clone)"))
            hits++;
        if (name == "Player")
        {
            GameObject imageObject = GameObject.FindGameObjectWithTag("PHealth");
            HealthBar = imageObject.GetComponent<Image>();
            HealthBar.fillAmount = 1 - hits / 20f;
        }
        if (name == "Enemy")
        {
            Image imageObject2 = GetComponentInChildren(typeof(Canvas)).GetComponentInChildren(typeof(Image)) as Image;
            PHealthBar = imageObject2.GetComponent<Image>();
            PHealthBar.fillAmount = 1 - hits / 5f;
        }
        if (name == "Enemy" && collision.gameObject.name == "Tengu_Fire Variant(Clone)")
            Score.text = (int.Parse(Score.text) + 1).ToString();
        if (hits >= 5 && name == "Enemy" || hits >= 20 && name == "Player")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
            GetComponent<Rigidbody>().useGravity = true;
            if (Random.Range(0, 2) == 0)
                GetComponent<Rigidbody>().AddTorque(transform.right * Random.Range(0, 51), ForceMode.Acceleration);
            else
                GetComponent<Rigidbody>().AddTorque(-transform.right * Random.Range(0, 51), ForceMode.Acceleration);
            Destroy(gameObject, 10);
        }
        if (!(collision.gameObject.name == "Enemy" && name == "Enemy") && !(collision.gameObject.name == "Apollo_Fire Variant(Clone)" && name == "Enemy") && !(name == "Tengu_Fire Variant(Clone)" && collision.gameObject.name == "Player") && !(name == "Player" && collision.gameObject.name == "Tengu_Fire Variant(Clone)"))
            Destroy(collision.gameObject, 0.1f);
    }
}