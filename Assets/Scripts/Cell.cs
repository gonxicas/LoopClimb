using System.Collections;
using LoopClimb.Others;
using UnityEngine;

namespace LoopClimb.Grid
{
    public class Cell
    {
        public int GridX { get; }
        public int GridY { get; }
        public bool IsActive { get; set; }
        public Vector3 Position { get; }
        public GameObject Mask { get; }

        public bool IsPlayerInside { get; set; }
        public bool CoroutineWorking { get; private set; }

        public Cell(GameObject mask, Vector3 pos, int i, int j)
        {
            Mask = mask;
            IsActive = false;
            Position = pos;
            GridX = i;
            GridY = j;
        }

        //It makes this cell invisible.
        public IEnumerator HidePixels(float seconds)
        {
            CoroutineWorking = true;
            //The double while IsPLayerInside makes sure that the cell doesn't become invisible while
            //the player is inside it.
            while (IsPlayerInside)
            {
                while (IsPlayerInside)
                {
                    yield return true;
                }

                yield return new WaitForSecondsRealtime(seconds);
            }
            //If the game is paused, the player will be able to see the map, but when he resumes the game,
            //the map that should be hidden will be hidden.
            yield return new WaitWhile(CheckGamePaused);

            Mask.SetActive(false);
            IsActive = false;
            CoroutineWorking = false;
        }
        private bool CheckGamePaused()
        {
            return InGameOptions.Instance.PausePanel.activeInHierarchy;
        }
        //It makes this cell visible.
        public void ShowPixels()
        {
            if (IsActive) return;
            Mask.SetActive(true);
            IsActive = true;
        }
    }
}