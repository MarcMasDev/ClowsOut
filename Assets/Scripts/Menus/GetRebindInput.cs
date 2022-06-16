using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class GetRebindInput : MonoBehaviour
{

    public InputActionReference m_Input;
    public GameObject m_WaitInput, m_Button;
    public TMP_Text m_Text;
    public int m_Index;

    private void Start()
    {
        m_WaitInput.SetActive(false);
        int l_BindingIndex = m_Input.action.GetBindingIndexForControl(m_Input.action.controls[m_Index]);
        m_Text.text = InputControlPath.ToHumanReadableString(m_Input.action.bindings[l_BindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}
