using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class GroundCollision : MonoBehaviour
{
    private PlayerController _player;
    private float _timer = 0;
    private bool _jumping = false;

    [SerializeField] private float landTime = 0.1f;

    private void Awake()
    {
        _player = GetComponentInParent<PlayerController>();
    }

    private void Update()
    {
        if(_jumping)
            AddTime();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var time = GetTime();
        time = time > 0.5f ? landTime : landTime / 2;
        switch (other.tag)
        {
            case "Ground":
                _player.SetTouchingGround(true);
                Invoke(nameof(ResetVelocity),time);
                Invoke(nameof(SetCanDoActionsTrue),time);
                SoundManagaer.instance.PlayFall();
                _player.SetFallAnim(false);
                _jumping = false;
                ResetTimer();
                break;
            case "Slope":
                break;
        }



    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Ground")) return;
        
            _player.SetTouchingGround(false);
            _player.SetCanDoActions(false);
            if (!_player.GetCrouching())
                _player.SetFallAnim(true);
            _jumping = true;
    }

    private void SetCanDoActionsTrue() => _player.SetCanDoActions(true);
    private void ResetVelocity() => _player.ResetVelocity();
    private void ResetTimer() => _timer = 0;
    private float GetTime() => _timer;
    private void AddTime() => _timer += Time.deltaTime;
}
