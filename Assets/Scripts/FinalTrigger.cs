using DG.Tweening;
using UnityEngine;

public class FinalTrigger : MonoBehaviour
{
    [SerializeField] private Transform finalPoint;
    [SerializeField] private float cinematicDuration=1f;
    private GameObject _player;
    private PlayerController _playerController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _player = other.attachedRigidbody.gameObject;
        _playerController = other.gameObject.GetComponentInChildren<PlayerController>();
        SetPlayerGravity(0);
        PlayCinematic();
    }

    private void PlayCinematic()
    {
        Sequence cinematicSequence = DOTween.Sequence();

        cinematicSequence.AppendCallback(() => _playerController.SetCinematic(true));
        cinematicSequence.Join(_player.transform.DOMove(finalPoint.position, cinematicDuration).SetEase(Ease.OutQuad));
        cinematicSequence.InsertCallback(cinematicDuration,() => _playerController.SetCinematic(false));
        cinematicSequence.InsertCallback(cinematicDuration,() => _playerController.SetCanDoActions(false));
        cinematicSequence.InsertCallback(cinematicDuration,() => _playerController.SetFallAnim(true));
        cinematicSequence.InsertCallback(cinematicDuration,() => SetPlayerGravity(2));
        cinematicSequence.Play();

    }

    private void SetPlayerGravity(float value)
    {
        _player.GetComponentInParent<Rigidbody2D>().gravityScale = value;
    }
}
