using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Componet_Click : MonoBehaviour
{
    float speed;
    Quaternion rotation1;
    Quaternion rotation2;
    Vector3 Scale;
    bool Entered = false;
    float Glow_Factor = 0f;
    bool Asc = true;
    bool Enabled = false;
    Animator Anim;

    TextMeshProUGUI Text;
    GameObject BTR112;
    GameObject image;
    GameObject Description;

    private void Start()
    {
        BTR112 = GameObject.Find("Rotator");
        Anim = GameObject.Find("BTR112").GetComponent<Animator>();
        speed = 80;
        rotation1 = Quaternion.Euler(-90, 0, 0);
        Text = FindObjectOfType<TextMeshProUGUI>();
        image = GameObject.Find("Image");
        Text.text = "";
        image.GetComponent<Image>().color = Color.clear;
        Description = GameObject.Find("Text");
        Description.GetComponent<Text>().text = "";
        InvokeRepeating("Glow", 0.1f, 0.1f);

        GameObject.Find("Button").SetActive(false);
        BTR112.GetComponent<Lean.Touch.LeanScale>().enabled = false;
        BTR112.GetComponent<Lean.Touch.LeanRotate>().enabled = false;
        BTR112.GetComponent<Lean.Touch.LeanTranslate>().enabled = false;
    }

    private void Update()
    {

        if (!Anim.GetCurrentAnimatorStateInfo(0).IsTag("1"))
        {
            if (GameObject.Find("BTR112").GetComponent<AudioSource>().isPlaying && !Entered)
            {
                GameObject.Find("BTR112").GetComponents<AudioSource>()[0].Stop();
            }
            if (!Enabled)
            {
                BTR112.GetComponent<Lean.Touch.LeanScale>().enabled = true;
                BTR112.GetComponent<Lean.Touch.LeanRotate>().enabled = true;
                BTR112.GetComponent<Lean.Touch.LeanTranslate>().enabled = true;
                Enabled = true;
            }
            Description.transform.LookAt(2 * Description.transform.position - new Vector3(0, 0, 0));
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 1000) && !Entered)
                {
                    if (hit.transform.gameObject.name != "TextBox" && hit.transform.gameObject.name != "Plane")
                        image.GetComponent<Image>().color = Color.black;
                    if (hit.transform.gameObject.name == "Bumper")
                        Text.text = "Front Armor";
                    else if (hit.transform.gameObject.name == "LC" || hit.transform.gameObject.name == "RC")
                        Text.text = "Cannon";
                    else if (hit.transform.gameObject.name == "Camera")
                        Text.text = "Camera";
                    else if (hit.transform.gameObject.name == "door")
                        Text.text = "Door";
                    else if (hit.transform.gameObject.name == "suspen_L_01" || hit.transform.gameObject.name == "suspen_L_02" || hit.transform.gameObject.name == "suspen_L_03"
                        || hit.transform.gameObject.name == "suspen_R_01" || hit.transform.gameObject.name == "suspen_R_02" || hit.transform.gameObject.name == "suspen_R_03")
                        Text.text = "Wheels";
                    else if (hit.transform.gameObject.name == "TextBox" && image.GetComponent<Image>().color == Color.black)
                    {
                        Description_Handler();
                    }
                    else
                    {
                        Text.text = "";
                        image.GetComponent<Image>().color = Color.clear;
                        if (rotation1 != BTR112.transform.rotation && Entered)
                        {
                            Entered = false;
                            rotation2 = BTR112.transform.rotation;
                            var a = Quaternion.Angle(rotation1, rotation2);
                            StartCoroutine(RotateOverTime(rotation2, rotation1, new Vector3(0.01f, 0.01f, 0.01f), a / speed));
                            Anim.Play("Default");
                            Description.GetComponent<Text>().text = "";
                            GameObject.Find("BTR112").GetComponents<AudioSource>()[0].Stop();
                        }
                    }
                }
                else
                {
                    Text.text = "";
                    image.GetComponent<Image>().color = Color.clear;
                    if (rotation1 != BTR112.transform.rotation && Entered)
                    {
                        Entered = false;
                        rotation2 = BTR112.transform.rotation;
                        var a = Quaternion.Angle(rotation1, rotation2);
                        StartCoroutine(RotateOverTime(rotation2, rotation1, new Vector3(0.01f, 0.01f, 0.01f), a / speed));
                        Anim.Play("Default");
                        Description.GetComponent<Text>().text = "";
                    }
                }
            }
        }
        else
            if (!GameObject.Find("BTR112").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("BTR112").GetComponents<AudioSource>()[0].Play();
        }
    }

    IEnumerator RotateOverTime(Quaternion start, Quaternion end, Vector3 Scale, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            BTR112.transform.rotation = Quaternion.Slerp(start, end, t / dur);
            var newScale = Mathf.Lerp(BTR112.transform.localScale.x, Scale.x, 0.07f);
            BTR112.transform.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
            t += Time.deltaTime;
        }
        BTR112.transform.rotation = end;
        BTR112.transform.localScale = Scale;
        Text.text = "";
        image.GetComponent<Image>().color = Color.clear;
    }

    private void Description_Handler()
    {
        Entered = true;
        if (Text.text == "Front Armor")
        {
            Scale = new Vector3(0.02f, 0.02f, 0.02f);
            rotation2 = Quaternion.Euler(-20, -60, 10);
            Description.GetComponent<Text>().text = "The armour of the BTR-112 comprises welded steel armour plates. The armour can withstand hits from 14.5 mm rounds over the frontal arc." +
            " The side armour can provide protection against large caliber machine gun fire and shrapnel. Active protection methods can also be used, such as explosive reactive armour. These can be added over the existing armour of the vehicle.";
        }
        else if (Text.text == "Cannon")
        {
            Scale = new Vector3(0.02f, 0.02f, 0.02f);
            rotation2 = Quaternion.Euler(-20, -60, 40);
            Description.GetComponent<Text>().text = "The dual 57 mm autocannons used by the BTR-112 Cockroach are nothing new. 57 mm guns have been used for anti-aircraft purposes as far back as the 1950s." +
            " Previous vehicles equipped with these cannons were unsatisfactory due to their poor fire control systems. This is not the case for the BTR-112." +
            " Its modernized 57 mm autocannons are married to a state-of-the-art fire control computer and are deadly accurate against low-flying airborne threats.";
            Anim.Play("Cannon");
        }
        else if (Text.text == "Camera")
        {
            Scale = new Vector3(0.02f, 0.02f, 0.02f);
            rotation2 = Quaternion.Euler(10, -100, 30);
            Anim.Play("Camera");
            Description.GetComponent<Text>().text = "The BTR-112 is equipped with a Multispectral Targeting System camera and sensors which detects movement from far away";
        }
        else if (Text.text == "Door")
        {
            Scale = new Vector3(0.02f, 0.02f, 0.02f);
            rotation2 = Quaternion.Euler(-20, 55, 0);
            Anim.Play("Door");
            Description.GetComponent<Text>().text = "Despite its large guns and vast collection of onboard electronics, the BTR-112 can still comfortably carry a full squad of SGB Bears or Wolves." +
            " Its size and weight makes the Cockroach a bit slower, but its heavy armor makes it remarkably tough. The BTR-112 is built on the same platform as the KV-20 and is roughly the same size.";
        }
        else if (Text.text == "Wheels")
        {
            Scale = new Vector3(0.02f, 0.02f, 0.02f);
            rotation2 = Quaternion.Euler(10, -50, 10);
            Anim.Play("Wheels");
            Description.GetComponent<Text>().text = "The BTR-112 is capable of achieving a maximum speed of 100 km/h, and has cross-country driving ability comparable to that of tracked vehicles, with an average speed of 50 km/h. ";
            GameObject.Find("BTR112").GetComponents<AudioSource>()[0].spatialBlend=0.9f;
            GameObject.Find("BTR112").GetComponents<AudioSource>()[0].Play();
        }
        var a = Quaternion.Angle(rotation1, rotation2);
        StartCoroutine(RotateOverTime(rotation1, rotation2, Scale, a / speed));
    }

    private void Glow()
    {
        Text.fontSharedMaterial.SetFloat("_GlowPower", Glow_Factor);
        if (Asc)
            Glow_Factor += 0.1f;
        else
            Glow_Factor -= 0.1f;
        if (Glow_Factor <= 0)
            Asc = true;
        else if (Glow_Factor >= 1)
            Asc = false;
    }
}
