using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapController : MonoBehaviour
{
    public Tilemap tilemap;
    GameController gameController;
    PlayerController playerController;

    public Tile defaultTile, bigTitle, middleTile, smallTile;
    public GameObject bonusPrefab;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerController = FindObjectOfType<PlayerController>();
        ChangeTilesColor(); //Случайный цвет блоков в начале игры.
    }

    public void HitTile(Vector3 tilePosition) //Попадания в блок.
    {
        TileBase tile = tilemap.GetTile(tilemap.WorldToCell(tilePosition));
        if (tile)
        {
            switch (tile.name)//(name) //Взависимости от имени отпределяем следующий спрайт.
            {
                case "defaultMeteor":
                    StartCoroutine(SetTile(tilePosition, bigTitle));
                    break;
                case "bigMeteor":
                    StartCoroutine(SetTile(tilePosition, middleTile));
                    break;
                case "middleMeteor":
                    StartCoroutine(SetTile(tilePosition, smallTile));
                    break;
                case "smallMeteor": //При разрушении блока добавляем очки и вызываем функцию возможного получения бонуса.
                    StartCoroutine(SetTile(tilePosition, null)); //Ставим пустой блок.
                    gameController.AddScore(ScoreType.BlockDestruction, tilePosition);
                    if (Random.Range(0, 12) == Random.Range(0, 12))
                    {
                        Instantiate(bonusPrefab, transform).GetComponent<Transform>().position = tilePosition;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator SetTile(Vector3 tilePosition, Tile tile) //Изменить блок.
    {
        yield return new WaitForEndOfFrame();
        tilemap.SetTile(tilemap.WorldToCell(tilePosition), tile);
    }

    public void SetGroundTile(Vector3 position) //Установить новый блок.
    {
        StartCoroutine(SetTile(position, defaultTile));
    }

    public void ChangeTilesColor()
    {
        tilemap.color = Colors.Get(); //Получаем случайный цвет и устанавливаем его.
    }

    public void UpdateTiles()//Получаем все блоки на сцене.
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);
        List<Vector3Int> pos = new List<Vector3Int>();
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null && tile.name != "defaultMeteor") //Получаем подбитые блоки.
                {
                    pos.Add(new Vector3Int(x-17, y-15,0)); //Добавляем координаты подбитого блока с учетом сдвига.
                }
            }
        }
        StartCoroutine(UpdateTilesCour(pos));
    }

    IEnumerator UpdateTilesCour(List<Vector3Int> pos)
    {
        foreach(Vector3Int tilePos in pos) //Обновляем подбитые блоки. 
        {         
            tilemap.SetTile(tilePos, defaultTile);
            gameController.AddScore(ScoreType.BlockDestruction, tilePos); //Начисляем очки за каждый подбитый блок.
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.5f);
        gameController.LevelControllContinue();//Продолжаем загрузку следующего уровня.
    }
}
