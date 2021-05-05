using LoopClimb.Player;
using Unity.Mathematics;
using UnityEngine;

namespace LoopClimb.Grid
{
    public class MaskGrid : MonoBehaviour
    {
        [SerializeField] private GameObject maskPrefab;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 cellSize = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 bottomLeft = new Vector2(0, 0);
        [SerializeField] private float lightedTime = 180f;


        private Transform _player;
        private CapsuleCollider2D _playerCollider2D;
        private Vector2 _playerOffsetPosition;
        private Cell[,] _grid;


        private int _rows, _columns;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>().gameObject.transform;
            _playerCollider2D = _player.gameObject.GetComponent<CapsuleCollider2D>();
            maskPrefab.transform.localScale = new Vector3(cellSize.x, cellSize.y, 1);
        }

        private void Start()
        {
            _playerOffsetPosition = _playerCollider2D.bounds.extents;
            CreateTheGrid();
        }

        private void Update()
        {
            CheckPlayerPosition();
        }

        private void CreateTheGrid()
        {
            _rows = (int) Mathf.Ceil((float) gridSize.x / (float) cellSize.x);
            _columns = (int) Mathf.Ceil((float) gridSize.y / (float) cellSize.y);

            _grid = new Cell[_rows, _columns];

            for (int i = 0; i < _rows; i++)
            for (int j = 0; j < _columns; j++)
            {
                Vector3 cellPosition = bottomLeft
                                       + Vector2.right * (i * cellSize.x + cellSize.x / 2)
                                       + Vector2.up * (j * cellSize.y + cellSize.y / 2);
                var currentCell = Instantiate(maskPrefab, cellPosition, quaternion.identity, transform);
                currentCell.name = "Cell" + i + "," + j;
                _grid[i, j] = new Cell(currentCell, cellPosition, i, j);
                currentCell.SetActive(false);
            }
        }
        //It checks in which cell the player is and reveals it.
        private void CheckPlayerPosition()
        {
            //Checks the cell in which the player is.
            var playerPosition = _player.transform.position;
            var iMinCell =
                (int) Mathf.Floor((playerPosition.x - bottomLeft.x - _playerOffsetPosition.x) / cellSize.x);
            var jMinCell =
                (int) Mathf.Floor((playerPosition.y - bottomLeft.y - _playerOffsetPosition.y) / cellSize.y);
            var iMaxCell =
                (int) Mathf.Floor((playerPosition.x - bottomLeft.x + _playerOffsetPosition.x) / cellSize.x);
            var jMaxCell =
                (int) Mathf.Floor((playerPosition.y - bottomLeft.y + _playerOffsetPosition.y) / cellSize.y);
            //Reveals the cell in which the player is.
            for (int i = 0; i < _rows; i++)
            for (int j = 0; j < _columns; j++)
            {
                if (i < iMinCell || i > iMaxCell || j < jMinCell || j > jMaxCell)
                    _grid[i, j].IsPlayerInside = false;
                else
                {
                    _grid[i, j].IsPlayerInside = true;
                    _grid[i, j].ShowPixels();
                    if (!_grid[i, j].CoroutineWorking)
                        StartCoroutine(_grid[i, j].HidePixels(lightedTime));
                }
            }
        }
    }
}