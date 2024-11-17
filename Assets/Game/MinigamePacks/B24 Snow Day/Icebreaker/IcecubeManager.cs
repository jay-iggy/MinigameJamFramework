using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcecubeManager : MonoBehaviour
{
    public GameObject template;
    public new Camera camera;
    public static int width = 10;
    public static int height = 6;
    public int suddenDeathTime = 90;

    private float lastMelted = 0;
    private float timer = 0;
    private GameObject[] grid = new GameObject[width * height];
    private System.Random random = new System.Random();
    private bool hasIncreasedPitch = false;

    // Start is called before the first frame update
    void Start()
    {
        float xOffset = transform.position.x - ((width - 1) * template.transform.localScale.x / 2);
        float zOffset = transform.position.z - ((height - 1) * template.transform.localScale.z / 2);

        for (int i = 0; i < width * height; i++) {
            int row = i / width;
            int column = i % width;

            Vector3 newPos = new Vector3(xOffset + column * template.transform.localScale.x, template.transform.position.y, zOffset + row * template.transform.localScale.z);
            GameObject clone = Instantiate(template, newPos, template.transform.rotation);
            clone.name = template.name + (i + 1);
            clone.transform.parent = transform;
            clone.transform.position = newPos;
            clone.SetActive(true);

            grid[i] = clone;

            if (column > 0) {
                clone.GetComponent<IcecubeController>().neighborLeft = grid[i - 1].GetComponent<IcecubeController>();
                grid[i - 1].GetComponent<IcecubeController>().neighborRight = clone.GetComponent<IcecubeController>();
            }

            if (row > 0) {
                clone.GetComponent<IcecubeController>().neighborDown = grid[i - width].GetComponent<IcecubeController>();
                grid[i - width].GetComponent<IcecubeController>().neighborUp = clone.GetComponent<IcecubeController>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        lastMelted += Time.deltaTime;

        if (lastMelted > 2) {
            lastMelted -= 2;
            if (timer > suddenDeathTime) {
                StartCoroutine(grid[random.Next(0, width * height - 1)].GetComponent<IcecubeController>().Flash(1, 3));

                if(!hasIncreasedPitch){
                    hasIncreasedPitch = true;
                    camera.GetComponent<SoundEffectController>().GetSource("Theme").pitch = 1.4f;
                }
            } else if (random.Next(0, 5) == 0) {
                int row = random.Next(0, height);
                for (int i = 0; i < width; i++) {
                    StartCoroutine(grid[row * width + i].GetComponent<IcecubeController>().StartFreeze(false, 2f));
                }
            } else if (random.Next(0, 4) == 0) {
                int column = random.Next(0, width);
                for (int i = 0; i < height; i++) {
                    StartCoroutine(grid[i * width + column].GetComponent<IcecubeController>().StartFreeze(false, 2f));
                }
            } else {
                StartCoroutine(grid[random.Next(0, width * height - 1)].GetComponent<IcecubeController>().StartFreeze(true, 2f));
            }
        }
    }


}
