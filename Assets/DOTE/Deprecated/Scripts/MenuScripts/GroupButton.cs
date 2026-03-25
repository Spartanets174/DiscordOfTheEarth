using UnityEngine;
using UnityEngine.UI;

public class GroupButton : MonoBehaviour
{
    protected Button m_button;
    public Button Button => m_button;

    protected Toggle m_toggle;
    public Toggle Toggle => m_toggle;

    private bool m_isEnabled = false;

    public bool IsEnabled
    {
        get
        {
            return m_isEnabled;
        }
        set
        {
            m_isEnabled = value;
            m_toggle.isOn = IsEnabled;
        }
    }

    public virtual void Init()
    {
        m_button = GetComponent<Button>();
        m_toggle = GetComponentInChildren<Toggle>();
        IsEnabled = false;
    }

    protected virtual void OnCLick()
    {
        IsEnabled = !IsEnabled;

    }


    private void OnDestroy()
    {
        Button.onClick?.RemoveAllListeners();
    }
}
