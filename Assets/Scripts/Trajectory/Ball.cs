using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    private bool _isGhost;

    public void Init(Vector3 velocity, bool isGhost)
    {
        _isGhost = isGhost;
        _rb.AddForce(velocity, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision col)
    {
        if (_isGhost) return;
    }
}
