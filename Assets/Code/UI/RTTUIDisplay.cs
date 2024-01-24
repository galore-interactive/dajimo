using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class RTTUIDisplay : MonoBehaviour
{
    [SerializeField] private float _refreshRate = 0.2f;
    private TextMeshProUGUI _textComponent;
    private NetworkController _networkController;
    private float _timeSinceLastRefresh = 0f;
    private ulong _accumulatedRTT;
    private uint _samplesReceivedSinceLastRefresh;

    private void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        _networkController = FindObjectOfType<NetworkController>();
    }

    private void Update()
    {
        _timeSinceLastRefresh += Time.deltaTime;
        if (_timeSinceLastRefresh >= _refreshRate)
        {
            UpdateText(GetAverageRTT());
            _timeSinceLastRefresh = 0f;
        }
    }

    private void FixedUpdate()
    {
        _accumulatedRTT += _networkController.GetLocalClientRTT();
        _samplesReceivedSinceLastRefresh++;
    }

    private void UpdateText(ulong rtt)
    {
        const string RTT = "RTT: ";

        if (_networkController.Client_IsConnected())
        {
            const string MS = "ms";
            _textComponent.text = string.Concat(RTT, rtt, MS);
        }
        else
        {
            const string NOT_CONNECTED = "<NO CXN>";
            _textComponent.text = string.Concat(RTT, NOT_CONNECTED);
        }
    }

    private ulong GetAverageRTT()
    {
        if (_samplesReceivedSinceLastRefresh == 0)
        {
            return 0;
        }

        ulong result = _accumulatedRTT / _samplesReceivedSinceLastRefresh;
        _samplesReceivedSinceLastRefresh = 0;
        _accumulatedRTT = 0;
        return result;
    }
}
