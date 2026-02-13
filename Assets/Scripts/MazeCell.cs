using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell
{
    //Variables
    public bool isVisited = false;
    public GameObject cellObj;

    public MazeCell(int x, int y, GameObject obj) //Contructor
    {
        cellObj = obj;
    }

    public void RemoveWall(string wallName)
    {
        Transform wall = cellObj.transform.Find(wallName);
        if (wall != null)
            wall.gameObject.SetActive(false); //Se desactiva
    }

}