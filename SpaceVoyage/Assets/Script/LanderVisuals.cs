using UnityEngine;

public class LanderVisuals : MonoBehaviour
{
    [SerializeField] private ParticleSystem thruster;

    [SerializeField] private GameObject landerExplosive;

    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();

        lander.on_up_Force += Lander_on_up_Force;
        lander.on_Before_Force += Lander_on_before_Force;
    }

    private void Start()
    {
        lander.on_Landing += Lander_on_Landing;

        ParticleSystem.EmissionModule emissionModule = thruster.emission;

        emissionModule.enabled = false;

    }

    private void Lander_on_Landing(object sender, Lander.OnLandingEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.LandingType.TooSteep:
            case Lander.LandingType.Hard:
                {
                    Instantiate(landerExplosive, transform.position, Quaternion.identity);
                    gameObject.SetActive(false);
                    StatsUI.Instance.resetart.gameObject.SetActive(true);
                    StatsUI.Instance.reloadSavePoint.gameObject.SetActive(true);
                }
                break;
        }
    }

    private void Lander_on_before_Force(object sender, System.EventArgs e)
    {
        SetEnableParticleSystem(thruster, false);
    }
    private void Lander_on_up_Force(object sender, System.EventArgs e)
    {
        SetEnableParticleSystem(thruster, true);
    }

    private void SetEnableParticleSystem(ParticleSystem partice, bool enable)
    {
        ParticleSystem.EmissionModule emissionModule = partice.emission;
        emissionModule.enabled = enable;

    }
}
