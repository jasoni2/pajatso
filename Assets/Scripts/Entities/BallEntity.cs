using UnityEngine;
using UnityEngine.Serialization;

public class BallEntity : MonoBehaviour
{
    public float maxSpeed;

    [SerializeField] private Collider2D m_collider;
    [SerializeField] private Rigidbody2D m_rigidbody;

    public Collider2D Collider
    {
        get => m_collider;
    }

    public Rigidbody2D Rigidbody
    {
        get => m_rigidbody;
    }

    private void FixedUpdate()
    {
        if (m_rigidbody.bodyType != RigidbodyType2D.Dynamic) return;

        // Clamps the balls speed to a maximum value.
        // This is to prevent it from exponentially scaling to infinity and
        // teleporting through walls :)
        var direction = m_rigidbody.linearVelocity.normalized;
        m_rigidbody.linearVelocity = direction * Mathf.Clamp(m_rigidbody.linearVelocity.magnitude, -maxSpeed, maxSpeed);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(16.0f, 36.0f, 400.0f, 20.0f), $"Ball speed: ({m_rigidbody.linearVelocity.magnitude})");
    }

    /// <summary>
    /// Launches the ball in the given direction with the given force, enabling
    /// the rigidbody to enable physics on the object.
    /// 
    /// This should only be called at the start of the "round" by whatever is
    /// managing the game flow.
    /// 
    /// This is basically here so that other MonoBehaviours are not directly
    /// accessing the balls rigidbody and manipulating it.
    /// </summary>
    /// <param name="direction">The normalized direction.</param>
    /// <param name="force">The force amount.</param>
    public void Launch(Vector2 direction, float force)
    {
        Debug.Log($"Launching ball with force {force} in direction {direction.x},{direction.y}");
        m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
        m_rigidbody.AddForce(direction * force, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Disables the Rigidbody on the ball to freeze it in place and ignore
    /// gravity/other forces. Should be called when resetting the ball.
    /// 
    /// This is only here so that other MonoBehaviours are not directly
    /// manipulating the rigidbody attached on the ball.
    /// </summary>
    public void MakeInactive()
    {
        Debug.Log("Making ball sleep...");
        m_rigidbody.bodyType = RigidbodyType2D.Static;
    }
}
