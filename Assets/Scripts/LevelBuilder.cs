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

    void Awake()
    {
        BuildIfNeeded();
    }

    void OnEnable()
    {
        BuildIfNeeded();
    }

    void BuildIfNeeded()
    {
        if (built) return;
        if (transform.childCount > 0) { built = true; return; }

        BuildQuadrant(1, 1);
        BuildQuadrant(-1, 1);
        BuildQuadrant(1, -1);
        BuildQuadrant(-1, -1);
        built = true;
    }

    void BuildQuadrant(float flipX, float flipY)
    {
        int rows = levelMap.GetLength(0);
        int cols = levelMap.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int value = levelMap[row, col];
                GameObject prefab = GetPrefab(value);
                if (prefab == null) continue;

                float x = col * tileSize * flipX;
                float y = -row * tileSize * flipY;

                Vector3 pos = new Vector3(x, y, 0);
                GameObject piece = Instantiate(prefab, pos, Quaternion.identity, transform);

                Vector3 scale = piece.transform.localScale;
                scale.x *= flipX;
                scale.y *= flipY;
                piece.transform.localScale = scale;
            }
        }
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