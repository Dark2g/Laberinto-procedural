using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Maze : MonoBehaviour
{
    [Header("Variables laberinto")]
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f; //Este es el tamaño de la celda. OJO! Debe coincidir con la escala

    [Header("Variables prefabs")]
    [SerializeField] private GameObject _cellPrefab, _keyPrefab, _exitPrefab, _playerPrefab; //El objeto con las cuatro paredes y el resto de prefabs...


    private MazeCell[,] grid; //Matriz de una clase para los datos y las paredes de cada celda individual

    void Start()
    {
        //1 Inicializa
        InitialGrid();
        //2 Ejecutamos el algoritmo
        GenerateMaze(0, 0);
        //3 Colocamos los distintos elementos (prefabs)
        placeItems();
        //4 Pondríamos la cámara en su sitio para que se vea la escena aérea completa
        placeCamera();
    }

    /// <summary>
    /// Inicia la rejilla entera
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void InitialGrid()
    {
        grid = new MazeCell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3(x * cellSize, 0, y * cellSize);
                GameObject newCell = Instantiate(_cellPrefab, position, Quaternion.identity, transform);
                grid[x, y] = new MazeCell(x, y, newCell); //Metemos la nueva celda
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void GenerateMaze(int x, int y)
    {
        grid[x, y].isVisited = true;

        //Creamos una lista en la que ponemos las paredes que debemos romper para su salida
        List<(int dx, int dy, string wallA, string wallB)> directions = new List<(int dx, int dy, string, string)>
     {
         (0,1,"Wall_N","Wall_S"),
         (0,-1,"Wall_S","Wall_N"),
         (1,0,"Wall_E","Wall_W"),
         (-1,0,"Wall_W","Wall_E")
     };

        //Direcciones aleatorias. 
        for (int i = 0; i < directions.Count; i++)
        {
            var temp = directions[i];
            int r = Random.Range(i, directions.Count);
            directions[i] = directions[r];
            directions[r] = temp;
        }

        foreach (var direction in directions)
        {
            int nx = x + direction.dx;
            int ny = y + direction.dy;

            if (nx >= 0 && ny >= 0 && nx < width && ny < height && !grid[nx, ny].isVisited)
            {
                grid[x, y].RemoveWall(direction.wallA);
                grid[nx, ny].RemoveWall(direction.wallB);
                GenerateMaze(nx, ny);
            }
        }
    }

    private void placeItems()
    {
        //Instancio la salida arriba derecha y el candado
        Vector3 exitPosition = new Vector3((width - 1) * cellSize, 0.5f, (height - 1) * cellSize);
        Instantiate(_exitPrefab, exitPosition, Quaternion.Euler(90f, 45f, 0f));

        //Instanciamos la llave
        int keyXPosition, keyYPosition;
        do
        {
            keyXPosition = Random.Range(0, width);
            keyYPosition = Random.Range(0, height);
        }
        while (keyXPosition == 0 && keyYPosition == 0);

        Vector3 keyPosition = new Vector3(keyXPosition * cellSize, 0.5f, keyYPosition * cellSize);
        Instantiate(_keyPrefab, keyPosition, Quaternion.identity);

        //Instanciamos al player
        Vector3 playerPosition = new Vector3(0, 1f, 0);
        Instantiate(_playerPrefab, playerPosition, Quaternion.Euler(0f, 180f, 0f));

    }

    private void placeCamera()
    {
        Camera cam = Camera.main;

        //Calculamos la posición
        float centroX = (width * cellSize) / 2f - (cellSize / 2f);
        float centroZ = (height * cellSize) / 2f - (cellSize / 2f);

        //Calculamos la altura
        float elevation = Mathf.Max(width, height) * cellSize * 0.6f;
        cam.transform.position = new Vector3(centroX, elevation, centroZ);

        //Importante, girar la cámara hacia abajo
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    public void ResetMaze()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}