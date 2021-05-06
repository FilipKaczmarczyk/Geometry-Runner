using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  

public class GameController : MonoBehaviour
{
    public GameObject barrier;

    public GameObject player;

    public TrailRenderer trailRenderer;

    public ParticleSystem deathEffect;

    public GameObject startScreen;
    public GameObject gameplayScreen;
    public GameObject gameOverScreen;
    public GameObject highScoreScreen;
    public Image FadeOffImage;

    public float barrierStack = 5;

    public float barrierSpawnY;

    public float barrierSapwnXMin = -1.8f;
    public float barrierSapwnXMax = 1.8f;

    public float barrierSpawnYDelay;

    public Color[] colors;

    public Text scoreText;
    public Text yourScore;
    public Text highScoreText;

    private int score = 0;
    private int highScore = 0;

    private bool readytoReload = false;

    public float fadeTime = 2f;

    private void Awake()
    {
        FadeOffImage.canvasRenderer.SetAlpha(1.0f);
        FadeOffImage.CrossFadeAlpha(0, fadeTime, false);
        highScore = PlayerPrefs.GetInt("highScore");
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    private void Update()
    {
        if (!readytoReload)
        {
            
        }
        else
        {
            /*if (Input.GetMouseButton(0))
            {
                readytoReload = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }*/

            if (Input.touchCount > 0)
            {
                readytoReload = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void StartGame()
    {
        for (int i = 0; i < barrierStack; i++)
        {
            SpawnObstacle();
        }

        scoreText.text = score.ToString();
        startScreen.SetActive(false);
        gameplayScreen.SetActive(true);
    }

    public void SpawnObstacle()
    {
        GameObject newBarrier = Instantiate(barrier, new Vector3(Random.Range(barrierSapwnXMin, barrierSapwnXMax), barrierSpawnY, 1f), Quaternion.identity);

        SpriteRenderer spriteRenderer = newBarrier.GetComponent<SpriteRenderer>();
        Color newColor = colors[Random.Range(0, colors.Length)];
        spriteRenderer.color = newColor;
        
        barrierSpawnY += barrierSpawnYDelay;
    }

    public void Scored(GameObject go)
    {
        SpawnObstacle();

        Color newColor = go.transform.parent.gameObject.GetComponent<SpriteRenderer>().color;
        player.GetComponent<SpriteRenderer>().color = newColor;
        trailRenderer.material.color = newColor;

        score++;
        scoreText.text = score.ToString();

        StartCoroutine(DestroyAfterTime(go.gameObject));
    }

    IEnumerator DestroyAfterTime(GameObject go)
    {
        yield return new WaitForSeconds(3f);
        Destroy(go.transform.parent.gameObject);
    }

    public void GameOver()
    {
        deathEffect.Play();

        FadeOffImage.CrossFadeAlpha(1, fadeTime, false);

        StartCoroutine(ShowScore());
    }

    IEnumerator ShowScore()
    {
        yourScore.text = "Your Score: " + score.ToString();
        yield return new WaitForSeconds(fadeTime);
        gameOverScreen.SetActive(true);

        if (score > highScore)
        {
            highScoreScreen.SetActive(true);
            highScore = score;
        }

        PlayerPrefs.SetInt("highScore", highScore);
        readytoReload = true;
    }
}
