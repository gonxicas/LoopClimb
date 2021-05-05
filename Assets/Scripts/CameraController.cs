using LoopClimb.Player;
using UnityEngine;

namespace LoopClimb.CameraController
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;
        private Transform _player;
        private float _size;

        private void Awake()
        {
            _camera = Camera.main;
            _player = FindObjectOfType<PlayerController>().gameObject.transform;
        }

        private void Start()
        {
            _size = _camera.orthographicSize;
        }

        private void LateUpdate()
        {
            CheckPlayerPosition();
        }

        //Checks the player's position and move the camera if required.
        private void CheckPlayerPosition()
        {
            var cameraPosition = _camera.transform.position.y;
            var playerPosition = _player.position.y;

            var dir = playerPosition >= cameraPosition + _size ? 1 :
                        (playerPosition <= cameraPosition - _size ? -1 : 0);
            if (dir != 0)
                transform.Translate(Vector3.up * (dir * 2 * _size));
        }
    }
}