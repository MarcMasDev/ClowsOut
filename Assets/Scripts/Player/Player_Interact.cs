using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interact : MonoBehaviour
{
    private IInteractable m_CurrentInteractable = null;
    private GameObject m_CurrentInteractableGO = null;
    private Player_Blackboard m_Blackboard;

    private void Start()
    {
        m_Blackboard = GetComponent<Player_Blackboard>();
    }
    private void OnEnable()
    {
        InputManager.Instance.OnStartInteracting += StartInteracting;
    }
    private void OnDisable()
    {
        InputManager.Instance.OnStartInteracting -= StartInteracting;
    }
    void Update()
    {
        RaycastHit l_Hit;
        if (Physics.Raycast(m_Blackboard.m_Camera.transform.position, m_Blackboard.m_Camera.transform.forward, 
            out l_Hit, m_Blackboard.m_InteractDistance, m_Blackboard.m_InteractLayers))
        {
            IInteractable l_Interactable = l_Hit.collider.GetComponent<IInteractable>();
            if (l_Interactable != null)
            {
               // Debug.Log("Interact");
                GameObject l_InteractableGO = l_Hit.collider.gameObject;
                if (m_CurrentInteractable != null && m_CurrentInteractableGO.GetInstanceID() != l_InteractableGO.GetInstanceID())
                {
                    m_CurrentInteractable.StopPointing();
                }
                m_CurrentInteractableGO = l_InteractableGO;
                m_CurrentInteractable = l_Interactable;
                m_CurrentInteractable.StartPointing();
            }
            else
            {
                if (m_CurrentInteractable != null)
                {
                    m_CurrentInteractable.StopPointing();
                    m_CurrentInteractable = null;
                }
            }
        }
        else
        {
            if (m_CurrentInteractable != null)
            {
                m_CurrentInteractable.StopPointing();
                m_CurrentInteractable = null;
            }
        }
    }
    private void StartInteracting()
    {
        if (m_CurrentInteractable != null)
        {
            m_CurrentInteractable.Interact();
        }
    }
}
