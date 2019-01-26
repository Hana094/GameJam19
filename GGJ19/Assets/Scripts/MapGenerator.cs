using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DirectionInt
{
    North, East, West, South
}

public class MapGenerator : MonoBehaviour
{
    int[][] mapGrid;

    public int spawnLength;

    public int maxObstacles;

    public bool untilCantspawnMore;

    public int GridLength;

    List<ObstacleNode> obstacleList = new List<ObstacleNode>();

    public Obstacle shelterObstacle;

    public List<Obstacle> obstaclePrefabList;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildMap()
    {
        //clear the list of nodes
        obstacleList.Clear();
        //set the grid to work with
        mapGrid = new int[GridLength][];
        for (int i = 0; i < mapGrid.Length; i++)
        {
            mapGrid[i] = new int[GridLength];
        }

        //build the first Node
        int x = Random.Range(0,maxObstacles-shelterObstacle.size[0]);
        int y = Random.Range(0, maxObstacles - shelterObstacle.size[1]);



    }
    void Try2GrowNode(int direction, Obstacle obstacle2spawn, ObstacleNode _father, Vector3 pos)
    {

    }

    void CreateNode(int direction, Obstacle obstacle2spawn, ObstacleNode _father, Vector3 pos)
    {
        for (int i = (int)pos.x; i < (int)pos.x+obstacle2spawn.size[0]; i++)
        {
            for (int j = (int)pos.y; j < (int)pos.y + obstacle2spawn.size[1]; j++)
            {
                mapGrid[i][j] = 1;
            }
        }
        obstacleList.Add(new ObstacleNode(_father,obstacle2spawn,direction,pos));
    }

}

public class ObstacleNode
{
    public ObstacleNode father;
    public List<Obstacle> children;
    public Obstacle res;
    public int direction;//from where it comes
    public Vector3 position;
    public bool read;

    public ObstacleNode(ObstacleNode _father, Obstacle _res, int _direction, Vector3 _position)
    {
        read = false;
        father = _father;
        res = _res;
        direction = _direction;
        position = _position;
    }

}
