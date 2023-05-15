using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GamaManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject wallPrefab;
    public GameObject clearPrefab;

    public GameObject clearText;

    int level;
    int maxLevel = 1;
    int[,,] map;
    GameObject[,] field;
    void printArray()
    {
        /*string debugText = "";
        for(int y=0;y<map.GetLength(0);y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                debugText += map[y,x].ToString() + ",";
            }
            debugText += "\n";
        }
        Debug.Log(debugText);*/
    }
    Vector2Int GetPlayerIndex()
    {
        //Vector2Int result = new Vector2Int(-1,-1);
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] != null &&  field[y,x].tag == "Player")
                {
                    return new Vector2Int(x,y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        if(moveTo.x<0 || moveTo.x >= field.GetLength(1) ||
           moveTo.y<0 || moveTo.y >= field.GetLength(0) ||
           (moveFrom.x ==moveTo.x && moveFrom.y == moveTo.y))
        {
            return false;
        }
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall")
        {
            return false;
        }
            if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            
            Vector2Int velocity = new Vector2Int(moveTo.x-moveFrom.x, moveTo.y - moveFrom.y);
            //int[] nextMoveTo = { moveToX + velocityX, moveToY + velocityY };
            Vector2Int nextMoveTo = new Vector2Int(moveTo.x + velocity.x, moveTo.y + velocity.y);
            if (!MoveNumber("Box",moveTo, nextMoveTo))
            {
                return false;
            }
        }
        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x-2,-moveTo.y+1,0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool IsCleaed()
    {
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (map[level,y,x] == 3)
                {
                    goals.Add(new Vector2Int(x,y));
                }
            }
        }

        for(int i=0;i<goals.Count;i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if(f == null || f.tag != "Box")
            {
                return false;
            }
        }
        if(level< maxLevel)
        {
            level++;

            Reset();
        }
        return true;
    }

    void SetField()
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(2); x++)
            {
                if (map[level, y, x] == 1)
                {
                    field[y, x] = Instantiate(
                    playerPrefab, new Vector3(x - 2, -y + 1, 0), Quaternion.identity);
                }
                if (map[level, y, x] == 2)
                {
                    field[y, x] = Instantiate(
                    boxPrefab, new Vector3(x - 2, -y + 1, 0), Quaternion.identity);
                }
                if (map[level, y, x] == 3)
                {
                    Instantiate(
                    clearPrefab, new Vector3(x - 2, -y + 1, 0.01f), Quaternion.identity);
                }
                if (map[level, y, x] == 4)
                {
                    field[y, x] = Instantiate(
                    wallPrefab, new Vector3(x - 2, -y + 1, 0), Quaternion.identity);
                }
            }

        }
    }

    private void Reset()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if(field[y, x] != null)
                {
                    Destroy(field[y, x]);
                }
            }
        }
        //SetField();
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(2); x++)
            {
                if (map[level, y, x] == 1)
                {
                    field[y, x] = Instantiate(
                    playerPrefab, new Vector3(x - 2, -y + 1, 0), Quaternion.identity);
                }
                if (map[level, y, x] == 2)
                {
                    field[y, x] = Instantiate(
                    boxPrefab, new Vector3(x - 2, -y + 1, 0), Quaternion.identity);
                }
                /*if (map[level, y, x] == 3)
                {
                    Instantiate(
                    clearPrefab, new Vector3(x - 2, -y + 1, 0.01f), Quaternion.identity);
                }*/
                if (map[level, y, x] == 4)
                {
                    field[y, x] = Instantiate(
                    wallPrefab, new Vector3(x - 2, -y + 1, 0), Quaternion.identity);
                }
            }

        }
        clearText.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //   Debug.Log("Hello World");
        level = 0;
        map = new int[,,] {{ { 0, 0, 0, 0, 0 },
                             { 0, 3, 1, 3, 0 },
                             { 0, 0, 2, 0, 0 },
                             { 0, 2, 3, 2, 0 },
                             { 0, 0, 0, 0, 0 }},
                           { { 0, 0, 0, 0, 0 },
                             { 0, 3, 1, 3, 0 },
                             { 0, 4, 2, 0, 0 },
                             { 0, 2, 3, 2, 0 },
                             { 0, 0, 0, 0, 0 }} };
        field = new GameObject[map.GetLength(1),map.GetLength(2)];
        printArray();

        /*Vector2Int playerVector = new Vector2Int();
        playerVector = GetPlayerIndex();
        playerVector.x -= 2;
        playerVector.y *= -1;
        playerVector.y += 1;*/
        SetField();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) { Reset(); };

        /*Vector2Int playerIndex = new Vector2Int();
        playerIndex = GetPlayerIndex();
        int playerIndexX = playerIndex.x;
        int playerIndexY = playerIndex.y;
        //Debug.Log(playerIndexX.ToString());
        //Debug.Log(playerIndexY.ToString());
        */
        Vector2Int playerIndex = new Vector2Int();
        playerIndex = GetPlayerIndex();
        Vector2Int playerMove = new Vector2Int();
        playerMove = playerIndex;
        if(!IsCleaed())
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) { playerMove.x += 1; };
            if (Input.GetKeyDown(KeyCode.LeftArrow)) { playerMove.x -= 1; };
            if (Input.GetKeyDown(KeyCode.UpArrow)) { playerMove.y -= 1; };
            if (Input.GetKeyDown(KeyCode.DownArrow)) { playerMove.y += 1; };
        }
        
        if(MoveNumber("Player", playerIndex,playerMove))
        {
            printArray();
        }
        if(IsCleaed())
        {
            //Debug.Log("Clear!");
            clearText.SetActive(true);
        }
        else
        {
            clearText.SetActive(false);
        }

    }
}
