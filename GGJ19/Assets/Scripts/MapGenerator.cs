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

    public float childDevAux;

    // Update is called once per frame
    void Start()
    {
        BuildMap();
    }

    public void BuildMap()
    {
        int childDirection = 0;
        //clear the list of nodes
        obstacleList.Clear();

        //set the grid to work with
        mapGrid = new int[GridLength][];
        for (int i = 0; i < mapGrid.Length; i++)
        {
            mapGrid[i] = new int[GridLength];
        }


        //build the map from here
        ////////////////////////////////////
        //build the first Node
        int tries = 20;
        while (obstacleList.Count < maxObstacles && tries>0)
        {
            if (obstacleList.Count == 0)
            {
                float x = Random.Range(0, GridLength - shelterObstacle.size[0]) + (shelterObstacle.size[0]/2);
                float y = Random.Range(0, GridLength - shelterObstacle.size[1]) + (shelterObstacle.size[1] / 2);

                CreateNode(-1, shelterObstacle, null, new Vector3(x, 0, y));
            }
            else
            {
               
                int levelCount = obstacleList.Count;
                for (int i = 0; i < levelCount; i++)
                {
                    if (!obstacleList[i].read && Random.Range(0f, 1f) < childDevAux)
                    {

                        List<int> posibleDirections = new List<int> { 0, 1, 2, 3 };
                        for (int j = 0; j < 4; j++)
                        {
                            childDirection = posibleDirections[Random.Range(0, posibleDirections.Count)];
                            posibleDirections.Remove(childDirection);
                            
                            //Ask if can grow on that direction
                            if (!obstacleList[i].ContainsDirection(childDirection))
                            {
                                Try2GrowNode(childDirection, obstacleList[i]);
                            }
                        }

                    }
                }
            }
            tries--;
        }
        ////////////////////////////////////////////////////////////

        SpawnMap();

    }
    void Try2GrowNode(int direction,  ObstacleNode _father)
    {
        float x = _father.position.x;
        float y = _father.position.z;

        Obstacle aux = obstaclePrefabList[Random.Range(0, obstaclePrefabList.Count)];

        //getting the coordinates of the new obstacle
        switch (direction)
        {
            // growing to the north
            case 0:
                y += (_father.res.size[1]/2) +spawnLength+ (aux.size[1] / 2);
                break;
            // growing to the west
            case 1:
                x += -(_father.res.size[0] / 2) - spawnLength - (aux.size[0] / 2);
                break;
            // growing to the east
            case 2:
                x += (_father.res.size[0] / 2)+ spawnLength+ (aux.size[0] / 2);
                break;
            //growing to the south
            case 3:
                y += -(_father.res.size[1] / 2) - spawnLength - (aux.size[1] / 2);
                break;
            //origin room
            default:
                x = 0f;
                y = 0f;
                break;
        }

        bool canBuild = false;
        bool overlaps = true;

        for (int i = (int)(x - (aux.size[0] / 2)) ; i < x+ (aux.size[0] / 2); i++)
        {
            for (int j = (int)(y - (aux.size[1] / 2)); j < y + (aux.size[1] / 2); j++)
            {
                if (i>0 && i<GridLength && j > 0 && j < GridLength && overlaps)
                {
                    canBuild = true;
                    overlaps = mapGrid[i][j] == 0;
                }
            }
        }

        if (canBuild && overlaps)
        {
            CreateNode(direction,aux,_father,new Vector3(x,0,y));
        }

    }

    void CreateNode(int direction, Obstacle obstacle2spawn, ObstacleNode _father, Vector3 pos)
    {
        for (int i = (int)(pos.x - (obstacle2spawn.size[0] / 2)); i < pos.x + (obstacle2spawn.size[0] / 2); i++)
        {
            for (int j = (int)(pos.z - (obstacle2spawn.size[1] / 2)); j < pos.z + (obstacle2spawn.size[1] / 2); j++)
            {
                if (i > 0 && i < GridLength && j > 0 && j < GridLength )
                {
                    mapGrid[i][j] = 1;
                }
            }
        }
        /*
        for (int i = (int)pos.x; i < (int)pos.x+obstacle2spawn.size[0]; i++)
        {
            for (int j = (int)pos.y; j < (int)pos.y + obstacle2spawn.size[1]; j++)
            {
                
            }
        }*/

        ObstacleNode aux = new ObstacleNode(_father, obstacle2spawn, direction, pos);

        if (_father!= null)
        {
            obstacleList[obstacleList.IndexOf(_father)].children.Add(aux);
        }
        

        obstacleList.Add(aux);

        
    }

    void SpawnMap()
    {
        foreach (ObstacleNode obs in obstacleList)
        {
            Instantiate(obs.res.body,obs.position,Quaternion.identity,transform);
        }
    }

}
[System.Serializable]
public class ObstacleNode
{
    public ObstacleNode father;
    public List<ObstacleNode> children;
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
        children = new List<ObstacleNode>();
    }

    

    public bool ContainsDirection(int _direction)
    {
        bool res = 3-direction == _direction;
        foreach (ObstacleNode obs in children)
        {
            res = res || (obs.direction== _direction);
        }
        return res;
    }
}
