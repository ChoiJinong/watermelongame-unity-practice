using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> planets = new List<GameObject>();
    public GameObject nextObject;
    public GameObject clone;
    float timeCount = 0.0f;
    void Start()
    {
        SelectNextObject();
        clone = Instantiate(nextObject);
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clone.transform.position = new Vector2(Mathf.Clamp(mousePos.x, -4f, 4f), 4.2f);

        if (Input.GetMouseButtonDown(0) && timeCount >= 0.7f)
        {
            timeCount = 0;
            clone.GetComponent<CircleCollider2D>().enabled = true;
            clone.GetComponent<Rigidbody2D>().gravityScale = 1;
            SelectNextObject();
            clone = Instantiate(nextObject);
        }
    }
    void SelectNextObject()
    {
        int n = Random.Range(0, 3);
        nextObject = planets[n];
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
