using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public int depth = 113;

    public int scale = 113;

    public float offsetX = 100f;
    public float offsetY = 100f;

    // Start is called before the first frame update
    void Update()
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    TerrainData GenerateTerrainData(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GeneratedHeights());

        return terrainData;
    }

    float[,] GeneratedHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = calcHeight(x, y);
            }
        }
        return heights;
    }

    float calcHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

}