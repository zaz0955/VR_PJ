using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BalloonInflator : XRGrabInteractable
{
    [Header("Balloon Data")]
    public Transform attachPoint;
    public Balloon balloonPrefab;

    private Balloon m_BalloonInstance;
    private XRBaseController m_Controller;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Instantiate balloon at attach point
        m_BalloonInstance = Instantiate(balloonPrefab, attachPoint);

        // Get reference to the controller that picked up the inflator
        var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
        m_Controller = controllerInteractor.xrController;

        // Optional: Send haptic feedback when grabbed
        m_Controller.SendHapticImpulse(1, 0.5f);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Destroy balloon when dropped
        Destroy(m_BalloonInstance.gameObject);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        // Only process if selected and controller reference exists
        if (isSelected && m_Controller != null)
        {
            // Scale balloon based on trigger pressure (1.0 to 4.0 scale)
            m_BalloonInstance.transform.localScale = Vector3.one * Mathf.Lerp(1.0f, 4.0f,
                m_Controller.activateInteractionState.value);
        }
    }
}