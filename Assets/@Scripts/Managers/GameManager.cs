using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> planets = new List<GameObject>();
    public List<Sprite> planetImages = new List<Sprite>();

    public GameObject nextObject;
    public GameObject clone;

    public GameObject gameOver;
    public TMP_Text scoreText;
    public Image nextImage;

    [SerializeField]
    float timeCount = 0.0f;
    [SerializeField]
    int score = 0;

    void Start()
    {
        SelectNextObject();
        clone = Instantiate(nextObject);
        clone.GetComponent<Planet>().gameManager = this;

        scoreText.text = "0";
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clone.transform.position = new Vector2(Mathf.Clamp(mousePos.x, -4.0f, 4.0f), 4.0f);

        if (Input.GetMouseButtonDown(0) && timeCount >= 0.7f)
        {
            timeCount = 0;
            clone.GetComponent<CircleCollider2D>().enabled = true;
            clone.GetComponent<Rigidbody2D>().gravityScale = 1;
            SelectNextObject();
            clone = Instantiate(nextObject);
            clone.GetComponent<Planet>().gameManager = this;
        }
    }
    void SelectNextObject()
    {
        int n = Random.Range(0, 3);
        nextObject = planets[n];
    }
    public void AddScore(int level)
    {
        score += (int)Mathf.Pow(2, level);
        scoreText.text = score.ToString();
        Debug.Log("현재 점수 : " + score);
    }
    public void GameOver()
    {
        gameOver.SetActive(true);
    }
    public void OnClicked_ReTry_Button()
    {
        SceneManager.LoadScene(0);
    }
}
