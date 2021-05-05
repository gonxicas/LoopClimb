using System.Collections;
using UnityEngine;

public class Cell
{
    public int GridX { get; set; }
    public int GridY { get; set; }
    public bool IsActive { get; set; }
    public Vector3 Position { get; set; }
    public GameObject Mask { get; set; }

    public bool IsPlayerInside { get; set; }
    public bool CoroutineWorking { get; set; }

    public Cell(GameObject mask, Vector3 pos, int i, int j)
    {
        Mask = mask;
        IsActive = false;
        Position = pos;
        GridX = i;
        GridY = j;
    }

    public IEnumerator HidePixels(float seconds)
    {
        CoroutineWorking = true;

        while (IsPlayerInside)
        {
            while (IsPlayerInside)
            {
                yield return true;
            }

            yield return new WaitForSecondsRealtime(seconds);
        }

        while (InGameOptions.Instance.PausePanel.activeInHierarchy)
        {
            yield return true;
        }

        Mask.SetActive(false);
        IsActive = false;
        CoroutineWorking = false;
    }

    public void ShowPixels()
    {
        if (IsActive) return;
        Mask.SetActive(true);
        IsActive = true;
    }
}