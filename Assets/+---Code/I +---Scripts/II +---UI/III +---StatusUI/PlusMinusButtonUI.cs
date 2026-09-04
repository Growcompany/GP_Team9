using UnityEngine;
using UnityEngine.UI;

public class PlusMinusButtonUI : MonoBehaviour
{
    [SerializeField] Button minus;
    [SerializeField] Button plus;

    public CurrentStatusUI currentStatus;

    [Header("PlayerMovementStats 참고")]
    [SerializeField] private int min;
    [SerializeField] private int max;

    private AudioSource m_audioSource;

    private void Awake()
    {
        if(minus == null)
            minus = transform.Find("Minus").GetComponent<Button>();

        if(plus == null)
            plus = transform.Find("Plus").GetComponent<Button>();

        if(currentStatus == null)
            currentStatus = transform.parent.Find("CurrentState").gameObject.GetComponent<CurrentStatusUI>();

        minus.onClick.AddListener(Minus);
        plus.onClick.AddListener(Plus);

        //CheckMinMax();

        m_audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        minus.onClick.RemoveListener(Minus);
        plus.onClick.RemoveListener(Plus);
    }

    void Minus()
    {
        // plus 버튼 enable
        if(!plus.enabled)
            plus.enabled = true;

        // min값보다 작으면 minus 버튼 disable
        CheckMinMax(false);

        // Current Status 감소
        if (GameManager.instance.currentUsedStatusPoint > 0)
        {
            GameManager.instance.currentUsedStatusPoint--;
            currentStatus.onStatusChanged.Invoke(false);
            m_audioSource.Play();
        }
    }

    void Plus()
    {
        // minus 버튼 enable
        if(!minus.enabled)
            minus.enabled = true;

        // max값보다 크면 plus 버튼 disable
        CheckMinMax(true);

        // Current Status 증가
        if (GameManager.instance.availablePoint > GameManager.instance.currentUsedStatusPoint)
        {
            GameManager.instance.currentUsedStatusPoint++;
            currentStatus.onStatusChanged.Invoke(true);
            m_audioSource.Play();
        }
    }

    // flag = true ==> Plus 확인
    // flag = false ==> Minus 확인
    void CheckMinMax(bool flag)
    {
        int nextValue = flag ? currentStatus.currentValue + 1 : currentStatus.currentValue - 1;
        if(nextValue >= max)
        {
            plus.enabled = false;
        }
        else if(nextValue <= min)
        {
            minus.enabled = false;
        }
    }

    void CheckMinMax()
    {
        CheckMinMax(true);
        CheckMinMax(false);
    }
}
