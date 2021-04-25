using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorGenerator
{
    // size is the dimensions of the sector in meters (making it a square
    // cellSize is the size in meters of each individual grid unit
    // density is the chance for a POI. Nothing is represented by a negative number, otherwise a valid array index is used
    // maxIndex is the highest index for the array containing POIs to generate from
    public static int[,] GenerateSectorGrid(SectorData sector)
    {
        int gridDimensions = (int)(sector.TotalSize / sector.GridSize);
        Debug.Log("Generating sector with grid dimensions " + gridDimensions + "x" + gridDimensions);
        int[,] grid = new int[gridDimensions, gridDimensions];
        for(int i = 0; i < gridDimensions; i++)
        {
            for(int j = 0;  j < gridDimensions; j++)
            {
                if(Random.value * 100f <= sector.Density)
                {
                    grid[i, j] = Random.Range(0, sector.POIList.Count);
                }
                else
                {
                    grid[i, j] = -1;
                }
            }
        }
        return grid;
    }
}
