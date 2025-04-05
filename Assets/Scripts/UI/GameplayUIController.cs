using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class GameplayUIController : MonoBehaviour
{
    private static string k_LaunchButtonName = "LaunchButton";
    private static string k_LaunchForceBarName = "ForceAmount";

    [SerializeField] private GameplayManager m_gameplayManager;
    private Button m_launchButton;
    private VisualElement m_forceBar;

    private float m_chargeStart = -1.0f;

    private void OnEnable()
    {
        // Cache important elements and register event callbacks.
        var uiDoc = GetComponent<UIDocument>();

        m_launchButton = uiDoc.rootVisualElement.Q<Button>(k_LaunchButtonName);
        m_forceBar = uiDoc.rootVisualElement.Q(k_LaunchForceBarName);

        // Note: trickle down added to workaround clickables not bubbling the click event.
        // See: https://discussions.unity.com/t/adding-onpointerdownevent-to-button-not-work/896035/7
        m_launchButton.RegisterCallback<PointerDownEvent>(OnLaunchClicked, TrickleDown.TrickleDown);
        m_launchButton.RegisterCallback<PointerUpEvent>(OnLaunchReleased);
        ResetChargebar();
    }

    private void Update()
    {
        if (m_gameplayManager.GetGameplayState() != GameplayState.Idling)
            return;

        UpdateChargeBar();
    }

    /// <summary>
    /// Gets the current normalized charge amount, from the time that the player
    /// clicked the launch button to now. Converts seconds to relative time given
    /// the "max force time".
    /// </summary>
    /// <returns>The normalized (0 - 1) charge amount.</returns>
    private float GetCurrentChargeAmount()
    {
        if (m_chargeStart < 0.0f)
            return 0.0f;

        // TODO: move max force value to a game config scriptable object.
        var maxTime = m_gameplayManager.tempMaxForceTimeSecs;
        var chargeTime = Mathf.Clamp(Time.timeSinceLevelLoad - m_chargeStart, 0.0f, maxTime);
        return MathUtils.Translate(chargeTime, 0.0f, maxTime, 0.0f, 1.0f);
    }

    /// <summary>
    /// Updates the height of the charge bar UI with the current charge amount.
    /// </summary>
    private void UpdateChargeBar()
    {
        var amount = GetCurrentChargeAmount();
        m_forceBar.style.height = new Length(amount * 100.0f, LengthUnit.Percent);
    }

    /// <summary>
    /// Resets the height of the charge bar UI to 0%.
    /// </summary>
    private void ResetChargebar()
    {
        m_forceBar.style.height = new Length(0.0f, LengthUnit.Percent);
    }

    /// <summary>
    /// Event handler for when the player clicks the launch button.
    /// 
    /// Initializes the charging.
    /// </summary>
    /// <param name="evt">The pointer down event.</param>
    private void OnLaunchClicked(PointerDownEvent evt)
    {
        if (m_gameplayManager.GetGameplayState() != GameplayState.Idling)
            return;

        m_chargeStart = Time.timeSinceLevelLoad;
    }

    // TODO: Add event so this can subscribe to when the ball resets.
    /// <summary>
    /// Event handler for when the player releases the launch button.
    /// 
    /// Determines the strength and triggers the ball to launch.
    /// </summary>
    /// <param name="evt">The pointer up event.</param>
    private void OnLaunchReleased(PointerUpEvent evt)
    {
        var finalAmount = GetCurrentChargeAmount();

        // Reset charge start to prevent race conditions and update the charge bar to the final amount.
        m_chargeStart = -1.0f;
        m_forceBar.style.height = new Length(finalAmount * 100.0f, LengthUnit.Percent);

        m_gameplayManager.LaunchBall(finalAmount);
    }
}
