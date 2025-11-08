using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    [SerializeField] private Image _fillImage;


    private void Update()
    {
        _fillImage.fillAmount = _timer.TimeRemaining / _timer.TotalTime;
    }
}
