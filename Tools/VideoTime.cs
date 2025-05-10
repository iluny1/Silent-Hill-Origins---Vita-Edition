using UnityEngine.UI;
using UnityEngine;
public class VideoTime : MonoBehaviour {

    [SerializeField] private VideoFS m_Video;
    [SerializeField] private Text m_VCur;
    [SerializeField] private Text m_VDuration;
    [SerializeField] private Text m_MenuState;

    private void Update()
    {
        SetCurrentText(m_Video.ReturnVideoCurrentMS());
    }

    private void SetCurrentText(float current)
    {
        m_VCur.text = $"Current Time: {(current / 1000f).ToString("#.##")} - ({current} ms)";
    }

    public void SetDurationText(float duration)
    {
        m_VDuration.text = $"Duration: {duration / 1000f} - ({duration} ms)";
    }

    public void SetMenuStateText(string menuStateName)
    {
        m_MenuState.text = $"Menu State: {menuStateName}";
    }
}
