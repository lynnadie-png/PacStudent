using UnityEngine;

[ExecuteInEditMode]
public class LevelBuilder : MonoBehaviour
{
    public GameObject outsideWallPrefab;
    public GameObject outsideCornerPrefab;
    public GameObject insideWallPrefab;
    public GameObject insideCornerPrefab;
    public GameObject pelletPrefab;
    public GameObject powerPelletPrefab;
    public GameObject tJunctionPrefab;
    public GameObject ghostExitWallPrefab;

    public float tileSize = 1f;
    private bool built = false;

    private int[,] levelMap = new int[,]
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,8},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };

    void Awake() { BuildIfNeeded(); }
    void OnEnable() { BuildIfNeeded(); }

    void BuildIfNeeded()
    {
        if (built) return;
        if (transform.childCount > 0) { built = true; return; }

        BuildQuadrant(levelMap, 1, 1);
        BuildQuadrant(MirrorHorizontal(levelMap), -1, 1);
        int[,] trimmed = TrimLastRow(levelMap);
        BuildQuadrant(MirrorVertical(trimmed), 1, -1);
        BuildQuadrant(MirrorVertical(MirrorHorizontal(trimmed)), -1, -1);

        built = true;
    }

    bool IsWallType(int value)
    {
        return value == 1 || value == 2 || value == 3 || value == 4 || value == 7 || value == 8;
    }

    int[,] MirrorHorizontal(int[,] map)
    {
        int rows = map.GetLength(0), cols = map.GetLength(1);
        int[,] result = new int[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                result[r, c] = map[r, cols - 1 - c];
        return result;
    }

    int[,] MirrorVertical(int[,] map)
    {
        int rows = map.GetLength(0), cols = map.GetLength(1);
        int[,] result = new int[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                result[r, c] = map[rows - 1 - r, c];
        return result;
    }

    int[,] TrimLastRow(int[,] map)
    {
        int rows = map.GetLength(0), cols = map.GetLength(1);
        int[,] result = new int[rows - 1, cols];
        for (int r = 0; r < rows - 1; r++)
            for (int c = 0; c < cols; c++)
                result[r, c] = map[r, c];
        return result;
    }

    void BuildQuadrant(int[,] map, float signX, float signY)
    {
        int rows = map.GetLength(0);
        int cols = map.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int value = map[row, col];
                GameObject prefab = GetPrefab(value);
                if (prefab == null) continue;

                float x = col * tileSize * signX;
                float y = -row * tileSize * signY;
                Vector3 pos = new Vector3(x, y, 0);

                float rotationZ = 0f;
                if (value == 1 || value == 2 || value == 3 || value == 4 || value == 7)
                {
                    rotationZ = GetRotation(map, row, col, value);
                }

                GameObject piece = Instantiate(prefab, pos, Quaternion.Euler(0, 0, rotationZ), transform);
            }
        }
    }


    float GetRotation(int[,] map, int row, int col, int value)
    {
        int rows = map.GetLength(0), cols = map.GetLength(1);

        bool up = row > 0 && IsWallType(map[row - 1, col]);
        bool down = row < rows - 1 && IsWallType(map[row + 1, col]);
        bool left = col > 0 && IsWallType(map[row, col - 1]);
        bool right = col < cols - 1 && IsWallType(map[row, col + 1]);

        if (value == 2 || value == 4) 
        {
       
            if (left || right) return 0f;
            if (up || down) return 90f;
            return 0f;
        }

        if (value == 1 || value == 3) 
        {
            if (right && down) return 0f;
            if (left && down) return 90f;
            if (left && up) return 180f;
            if (right && up) return 270f;
            return 0f;
        }

        if (value == 7) 
        {
            if (up && left && right) return 0f;
            if (up && down && right) return 90f;
            if (down && left && right) return 180f;
            if (up && down && left) return 270f;
            return 0f;
        }

        return 0f;
    }

    GameObject GetPrefab(int value)
    {
        switch (value)
        {
            case 1: return outsideCornerPrefab;
            case 2: return outsideWallPrefab;
            case 3: return insideCornerPrefab;
            case 4: return insideWallPrefab;
            case 5: return pelletPrefab;
            case 6: return powerPelletPrefab;
            case 7: return tJunctionPrefab;
            case 8: return ghostExitWallPrefab;
            default: return null;
        }
    }
}