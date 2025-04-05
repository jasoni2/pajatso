using UnityEngine;

public enum GameplayState
{
    Idling,
    Running,
    Results,
};

public class GameplayManager : MonoBehaviour
{
    private struct ProtoInputState
    {
        public bool active;
        public float inputStart;

        public void Reset()
        {
            active = false;
            inputStart = 0.0f;
        }
    }

    [Header("Values")]
    [Min(0.001f)] public float ballMinLaunchForce;
    [Min(0.001f)] public float ballMaxLaunchForce;
    [Min(0.1f)] public float tempMaxForceTimeSecs;
    
    [Header("References")]
    public Transform ballStart;
    public Transform launchAngleTarget;
    public Transform ballResetPlane;

    [Header("Prefabs")]
    public BallEntity ballPrefab;

    private GameplayState m_gameplayState = GameplayState.Idling;
    private ProtoInputState m_launchState;
    private float m_tempDebugPrevLaunchForce = 0.0f;
    private BallEntity m_ballInstance;

    private void Awake()
    {
        // Instantiate the ball prefab before we start.
        m_ballInstance = Instantiate<BallEntity>(ballPrefab);
        ResetBall();
    }

    private void Start()
    {
        // Make sure we start with a clean slate.
        m_launchState.Reset();
        ResetBall();
        m_gameplayState = GameplayState.Idling;
    }
    private void OnGUI()
    {
        // TEMPORARY DEBUG: Show the launch force on screen
        GUI.Label(new Rect(16.0f, 16.0f, 400.0f, 20.0f), $"<SpaceBar> Launch force: {m_tempDebugPrevLaunchForce}");
    }

    private void Update()
    {
        switch (m_gameplayState)
        {
            case GameplayState.Idling:
                HandleIdleState();
                break;

            case GameplayState.Running:
                HandleRunningState();
                break;

            case GameplayState.Results:
                HandleResultsState();
                break;
        }
    }


    /// <summary>
    /// Handles the Idle state of the gameplay.
    /// 
    /// This is when we are waiting for the player to launch the ball.
    /// </summary>
    private void HandleIdleState()
    {
        bool isLaunchKeyDown = Input.GetKey(KeyCode.Space);

        // Detect Starting the launch...
        if (!m_launchState.active)
        {
            if (!isLaunchKeyDown) return;

            // DEBUG DEBUG DEBUG
            m_tempDebugPrevLaunchForce = 0.0f;
            // DEBUG DEBUG DEBUG

            m_launchState.active = true;
            m_launchState.inputStart = Time.timeSinceLevelLoad;
            return;
        }

        // Figure out if the player is still holding or if they have launched.
        if (isLaunchKeyDown)
        {
            // DEBUG DEBUG DEBUG
            var debug_lengthDown = Time.timeSinceLevelLoad - m_launchState.inputStart;
            var debug_translated = MathUtils.Translate(debug_lengthDown, 0.0f, tempMaxForceTimeSecs, 0.0f, 1.0f);
            var debug_normalized = Mathf.Clamp01(debug_translated);
            m_tempDebugPrevLaunchForce = debug_normalized;
            // DEBUG DEBUG DEBUG

            return;
        }

        // Once the player has released we calculate how long they held down for,
        // in relation to our "max force time". Then we translate that from the
        // 0 - <max force time> range to the 0 - 1 range, so our "force" modifiers
        // can be applied correctly.
        var lengthDown = Time.timeSinceLevelLoad - m_launchState.inputStart;
        var translated = MathUtils.Translate(lengthDown, 0.0f, tempMaxForceTimeSecs, 0.0f, 1.0f);

        m_launchState.Reset();
        LaunchBall(translated);
    }

    /// <summary>
    /// Handles the Running state of the gameplay, when the ball has been
    /// launched and the simulation is running.
    /// </summary>
    private void HandleRunningState()
    {
        // If the ball travels below our "Reset" plane, reset the ball.
        // TODO: This should actually trigger the results state, I believe.
        if (m_ballInstance.transform.position.y < ballResetPlane.position.y)
        {
            // TODO: Should move to results state.
            ResetBall();
            m_gameplayState = GameplayState.Idling;
        }
    }

    /// <summary>
    /// Handles the Results state of the gameplay, after the ball has fallen
    /// out of the game area.
    /// </summary>
    private void HandleResultsState()
    {
        // TODO: Lives logic, score logic, etc. all would trigger here.
        ResetBall();
    }

    /// <summary>
    /// Launches the ball with the given force, starting the main gameplay loop
    /// and triggering the Running gameplay state.
    /// </summary>
    /// <param name="force">The force to launch the ball, normalized to the 0 - 1 range.</param>
    public void LaunchBall(float force)
    {
        // If this gets called when we are not Idling, ignore.
        if (m_gameplayState != GameplayState.Idling) return;

        // Calculate the launch angle. We need to normalize this.
        Vector2 launchAngle = launchAngleTarget.position - ballStart.position;

        // Clamp the given force, then translate it to the min force - max force range.
        // This allows us adjust the buffers of the force applied to the ball, ie:
        // so instead of 0 -> min launch force
        // and instead of 1 -> max launch force
        var normalizedForce = Mathf.Clamp01(force);
        var translated = MathUtils.Translate(normalizedForce, 0, 1, ballMinLaunchForce, ballMaxLaunchForce);

        // Tell the ball to launch, and trigger the Runnig state.
        m_tempDebugPrevLaunchForce = normalizedForce;
        m_ballInstance.Launch(launchAngle.normalized, translated);
        m_gameplayState = GameplayState.Running;
    }

    /// <summary>
    /// Deactivates the ball so it is no longer affected by physics, then reset
    /// it to the "spawn" position.
    /// </summary>
    public void ResetBall()
    {
        m_ballInstance.MakeInactive();
        m_ballInstance.transform.position = ballStart.position;
    }
}
