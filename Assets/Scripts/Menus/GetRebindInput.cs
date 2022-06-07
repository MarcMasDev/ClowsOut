using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
public class GetRebindInput : MonoBehaviour
{

    public InputActionReference m_Input;
    public GameObject m_WaitInput, m_Button;
    public TMP_Text m_Text;

    private void Start()
    {
        int l_BindingIndex = m_Input.action.GetBindingIndexForControl(m_Input.action.controls[0]);
        m_Text.text = InputControlPath.ToHumanReadableString(m_Input.action.bindings[l_BindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}
