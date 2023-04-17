using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamaManagerScript : MonoBehaviour
{
    int[,] map;

    void printArray()
    {
        string debugText = "";
        for(int y=0;y<map.GetLength(y);y++)
        {
            for (int i = 0; i < map.Length; i++)
            {
                debugText += map[y,i].ToString() + ",";
            }
            debugText += "\n";
        }
        Debug.Log(debugText);
    }
    int[] GetPlayerIndex()
    {
        for (int y = 0; y < map.GetLength(y); y++)
        {
            for (int i = 0; i < map.Length; i++)
            {
                if (map[y,i] == 1)
                {
                    return { i,y};
                }
            }
        }
        return -1;
    }

    bool MoveNumber(int number,int[] moveFrom,int[] moveTo)
    {
        if(moveTo[0]<0 || moveTo[0] >= map.GetLength(0) ||
           moveTo[1]<0 || moveTo[1] >= map.Length ||
           moveFrom ==moveTo)
        {
            return false;
        }
        if (map[moveTo[1], moveTo[0]] == 2)
        {
            int[] velocity = { moveTo[0] - moveFrom[0], moveTo[1] - moveFrom[1] };
            int[] nextMoveTo = { moveTo[0] + velocity[0], moveTo[1] + velocity[1] };
            if (!MoveNumber(2,moveTo,nextMoveTo))
            {
                return false;
            }
        }

        map[moveTo[1], moveTo[0]] = number;
        map[moveFrom[1], moveFrom[0]] = 0;
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //   Debug.Log("Hello World");
        map = new int[,]{{ 0, 0, 1},{ 0, 2, 0, }, { 0, 2, 0, } };
        printArray();
        
    }

    // Update is called once per frame
    void Update()
    {
        int playerIndex = GetPlayerIndex();
        int playerMoveX = 0;
        int playerMoveY = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow)) { playerMoveX = 1; } ;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { playerMoveX = -1; };
        if (Input.GetKeyDown(KeyCode.UpArrow)) { };
        if (Input.GetKeyDown(KeyCode.DownArrow)) { };
        if(MoveNumber(1,playerIndex,playerIndex+playerMoveX))
        {
            printArray();
        }
    }
}
