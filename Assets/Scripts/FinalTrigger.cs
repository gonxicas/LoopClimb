using System;
using DG.Tweening;
using LoopClimb.Player;
using UnityEngine;

namespace LoopClimb.Player
{
    public class FinalTrigger : MonoBehaviour
    {
        [SerializeField] private Transform finalPoint;
        [SerializeField] private float cinematicDuration = 1f;
        private GameObject _player;
        private Rigidbody2D _playerRigidbody;
        private PlayerController _playerController;
        private float _playerGravity;

        private void Awake()
        {
            _playerController = FindObjectOfType<PlayerController>();
            _playerRigidbody = _playerController.GetComponentInParent<Rigidbody2D>();
            _player = _playerRigidbody.gameObject;
        }

        private void Start()
        {
            _playerGravity = _playerRigidbody.gravityScale;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            SetPlayerGravity(0);
            PlayCinematic();
        }

        private void PlayCinematic()
        {
            Sequence cinematicSequence = DOTween.Sequence();

            cinematicSequence.AppendCallback(() => _playerController.SetCinematic(true));
            cinematicSequence.Join(_player.transform.DOMove(finalPoint.position, cinematicDuration)
                .SetEase(Ease.OutQuad));
            cinematicSequence.InsertCallback(cinematicDuration, () => _playerController.SetCinematic(false));
            cinematicSequence.InsertCallback(cinematicDuration, () => _playerController.SetCanDoActions(false));
            cinematicSequence.InsertCallback(cinematicDuration, () => _playerController.SetFallAnim(true));
            cinematicSequence.InsertCallback(cinematicDuration, () => SetPlayerGravity(_playerGravity));
            cinematicSequence.Play();

        }

        private void SetPlayerGravity(float value)
        {
            _player.GetComponentInParent<Rigidbody2D>().gravityScale = value;
        }
    }
}