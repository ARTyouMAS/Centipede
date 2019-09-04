using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject gamePanel;
    public GameObject menuPanel;
    public GameObject lossPanel;

    public Canvas canvas;

    public Text scoreText;
    public Text recordText;

    public GameObject[] lifeIcons;

    public GameObject centipedePrefab;
    public GameObject centipedeHeadPrefab;
    public GameObject antPrefab;
    public GameObject popupTextPrefab;

    public GameObject roof; //Объект для изменения направления врага по оси Y.

    public TilemapController tilemapController;

    public bool levelStarted;

    int score;
    int record;
    int crntLevel;

    bool gameOver;

    Coroutine spawnAntCoroutine;

    void Start()
    {
        record = PlayerPrefs.GetInt("record");
        recordText.text = $"Record: {record}";

        crntLevel = 1;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        { 
            Application.Quit();
        }
    }

    public GameObject CreateCentipede()
    {
        GameObject centipede = Instantiate(centipedePrefab); //Создаем врага.
        centipede.transform.position = new Vector3(Random.Range(-4, 20), 0, 0);
        centipede.GetComponentInChildren<CentipedeHead>().offset = 1;

        return centipede;
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
        levelStarted = true;

        spawnAntCoroutine = StartCoroutine(SpawnAnt());

        CreateCentipede();
    }

    IEnumerator SpawnAnt()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f - (crntLevel-1)*0.125f); //Увеличиваем сложность с каждым уровнем.
             if (Random.Range(0, 3) == Random.Range(0, 3))
                Instantiate(antPrefab);
        }
    }

    public IEnumerator EndLevelCheckCour()
    {
        yield return new WaitForSeconds(0.1f);
        if (FindObjectOfType<CentipedeBody>() == null) //Проверяем наличие врагов на сцене, если нет - уровень пройден.
        {
            if (levelStarted)
            {
                LevelControll(true);
            }
        }
    } 

    public void LevelControll(bool nextLevel)
    {
        ShowRoof(false);
        levelStarted = false;

        if (nextLevel)
        {
            crntLevel++;
            AddScore(ScoreType.LevelCompleted, Vector3.zero);
            tilemapController.ChangeTilesColor(); //Меняем цвет блоков на новом уровне.
        }

        gameOver = false;
        SceneClearing();
    }

    public void SceneClearing() //Очистка сцены от врагов и обновление блоков.
    {
        if (spawnAntCoroutine != null)
            StopCoroutine(spawnAntCoroutine);

        var mas1 = FindObjectsOfType<Ant>(); 
        foreach (Ant obj in mas1)
        {
            Destroy(obj.gameObject);
        }

        var mas2 = FindObjectsOfType<CentipedeController>();
        foreach (CentipedeController obj in mas2)
        {
            obj.Destroy();
        }

        tilemapController.UpdateTiles();
    }

    public void LevelControllContinue()
    {
        if (!gameOver)
        {
            CentipedeController centipedeController = CreateCentipede().GetComponentInChildren<CentipedeController>();

            for (int i = 0; i < crntLevel - 1; i++)
            {
                var lastSegment = centipedeController.centBody[centipedeController.centBody.Count - 1]; //Получаем последний сегмент.

                centipedeController.centBody.Remove(lastSegment); //Удаляем из списка.
                Destroy(lastSegment); //Удаляем со сцены.

                GameObject head = Instantiate(centipedeHeadPrefab); //Создаем дополнительную голову.
                head.GetComponentInChildren<CentipedeHead>().speed += (crntLevel - 1) * 0.5f; //За каждый уровень увеличиваем скорость дополнительных голов на 0.5.
                head.transform.position = new Vector3(Random.Range(0, 24), 0, 0);
                head.GetComponentInChildren<CentipedeHead>().offset = 1;

                if (i == 8) //Если оставшихся частей меньше 2 - выход из цикла. 
                    break;
            }

            levelStarted = true;
            spawnAntCoroutine = StartCoroutine(SpawnAnt());
        }
        else
        {
            lossPanel.SetActive(true);
        }
    }

    public void AddScore(ScoreType score, Vector3 pos)
    {
        if(score != ScoreType.LevelCompleted)
        {
            var text = Instantiate(popupTextPrefab, canvas.transform);
            text.transform.position = pos;
            text.transform.GetChild(0).GetComponent<Text>().text = $"+{(int)score*crntLevel}";
            Destroy(text, 1f);
        }

        this.score += (int)score * crntLevel;
        scoreText.text = this.score.ToString();
    }

    public void GameOver()
    {
        levelStarted = false;
        if (score > record)
        {
            record = score;
            PlayerPrefs.SetInt("record", record);
        }

        gameOver = true;
        SceneClearing();
    }

    public void ChangeLifeIcons(int life, bool addLife)
    {
        if (addLife) //Если жизнь добавилась.
        {
            lifeIcons[life - 1].SetActive(true);
        }
        else //Если жизнь отнялась. 
        {
            lifeIcons[life].SetActive(false);
        }

    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ShowRoof(bool show) 
    {
        roof.SetActive(show);
    }
}

public enum ScoreType 
{
    СentipedeHit = 50,
    AntHit = 100,
    LevelCompleted = 250,
    BlockDestruction = 1,
}
