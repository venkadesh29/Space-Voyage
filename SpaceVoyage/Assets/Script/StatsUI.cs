using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StatsUI : MonoBehaviour
{
    [SerializeField] private Image fuelImage;
    public Button resetart;
    public Button reloadSavePoint;

    public static StatsUI Instance { get; private set; }

    private void Awake()
    {
        resetart.onClick.AddListener(() => SceneManager.LoadScene(0));
        reloadSavePoint.onClick.AddListener(ReloadSavePoint);
    }

    private void Start()
    {
        Instance = this;
        SetGameObjectFalse();
    }

    private void Update()
    {
        fuelImage.fillAmount = Lander.Instance.FuelAmountNormalized();
        fuelImage.color = Color.Lerp(Color.red, Color.green, Lander.Instance.FuelAmountNormalized());
    }

    private void SetGameObjectFalse()
    {
        resetart.gameObject.SetActive(false);
        reloadSavePoint.gameObject.SetActive(false);
    }

    private void ReloadSavePoint()
    {
        SetGameObjectFalse();
        Lander.Instance.rigidbody2D.gravityScale = Lander._GRAVITY;
        Lander.Instance.transform.position = Lander.Instance._savePoint.position;
        Lander.Instance.transform.rotation = Quaternion.identity;
        Lander.Instance.gameObject.SetActive(true);
        Lander.Instance.OnStateChange(Lander.LanderState.WaitingToStart);
    }
}
