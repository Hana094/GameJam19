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

    public List<GameObject> ResourcesPrefabList;
    public List<GameObject> RefugeesPrefabList;

    public int refuggesTries = 5;

    public int resourcesTries = 25;

    public float childDevAux;

    // Update is called once per frame
    void Start()
    {
        //BuildMap();
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
                float x = Random.Range(0, GridLength - (int)shelterObstacle.size[0]) + (shelterObstacle.size[0]/2);
                float y = Random.Range(0, GridLength - (int)shelterObstacle.size[1]) + (shelterObstacle.size[1] / 2);

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
            obstacleList[obstacleList.IndexOf(_father)].read = true;
        }
        

        obstacleList.Add(aux);

        
    }

    void SpawnMap()
    {
        foreach (ObstacleNode obs in obstacleList)
        {
            //Instantiate(obs.res.body, obs.position, Quaternion.identity, transform);
            Instantiate(obs.res.body,obs.position,((obs.IsSimetrical() && Random.Range(0f, 1f) < 0.5f) ? Quaternion.Euler(0,90,0) : Quaternion.identity),transform);
        }
        int refGen = 0;
        int resGen = 0;
        int x = 0;
        int y = 0;
        while (refGen<refuggesTries)
        {
            x = Random.Range(0,GridLength);
            y = Random.Range(0, GridLength);
            if (mapGrid[x][y]==0)
            {
                Instantiate( RefugeesPrefabList[Random.Range(0, RefugeesPrefabList.Count)] , new Vector3(x,0,y), Quaternion.Euler(0, 90* Random.Range(0, 4), 0) , transform);
                mapGrid[x][y] = 1;

                refGen++;
            }
        }
        int iterations = 0;

        while (refGen < resourcesTries && iterations < 100)
        {
            x = Random.Range(0, GridLength);
            y = Random.Range(0, GridLength);
            if (mapGrid[x][y] == 0)
            {
                Instantiate( ResourcesPrefabList[Random.Range(0, ResourcesPrefabList.Count)], new Vector3(x, 0, y), Quaternion.Euler(0, 90 * Random.Range(0, 4), 0), transform);
                mapGrid[x][y] = 1;
                refGen++;
            }
            iterations++;
        }

        
    }

    public void SetPlayersPos()
    {
        float s = -0.25f;
        foreach (Player p in GameManager.instance.players)
        {
            print(obstacleList[0].position);
            p.gameObject.transform.position = new Vector3(obstacleList[0].position.x - s, 1, obstacleList[0].position.z - s);
            s++;
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

    public bool IsSimetrical()
    {
        return res.size[0] == res.size[1];
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
