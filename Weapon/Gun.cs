using UnityEngine;

public class Gun : WeaponBase {

    Transform m_RightHand;

    [SerializeField] private bool m_Automatic;

    [SerializeField] private int m_AmmoCount;
    [SerializeField] private int m_AmmoCountDefault;

    [SerializeField] private ParticleSystem m_FireFX;
    [SerializeField] private AudioSource m_SFXSource;

    [SerializeField] private float m_RecoilTime;
    [SerializeField] private float m_RecoilTimeDefault;

    [SerializeField] private bool m_ButtonPressed = false;

    [SerializeField] private AudioClip[] m_FireSFX;
    [SerializeField] private AudioClip m_NoAmmoSFX;
    [SerializeField] private AudioClip m_ReloadSFX;

    private void Awake()
    {
        m_AmmoCount = m_AmmoCountDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_ButtonPressed)
        {
            if (m_RecoilTime > 0f) m_RecoilTime -= Time.deltaTime;
            else if (m_AmmoCount > 0) Shoot();
            else ShootNoAmmo();
        }

        if (!m_Automatic && m_RecoilTime > 0f)
        {
            m_RecoilTime -= Time.deltaTime;
        }

        if (Travis_ScriptHandle.Instance.TravisAim.IsAiming)
            Debug.DrawRay(m_FireFX.transform.position, Travis_ScriptHandle.Instance.transform.forward, Color.green, 0.033f);
    }   

    public override void Fire()
    {
        if (!m_Automatic) return;

        m_ButtonPressed = false;
        m_RecoilTime = 0f;        
    }

    public void Shoot()
    {
        if (Travis_ScriptHandle.Instance.TravisAim.Locked && Travis_ScriptHandle.Instance.TravisAim.m_Enemy != null)
            Travis_ScriptHandle.Instance.TravisAim.m_Enemy.GetComponent<IHealth>().GetDamage(m_Damage);
        else
        {
            Ray ray = new Ray(Travis_ScriptHandle.Instance.transform.position + Vector3.up,
                Travis_ScriptHandle.Instance.transform.forward);

            RaycastHit hit;

            if (Physics.SphereCast(ray, 0.25f, out hit, 30f, (1 << 0) | (1 << 10)))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.blue, 2f);
                if (hit.transform.GetComponent<IHealth>() != null)
                    hit.transform.GetComponent<IHealth>().GetDamage(m_Damage, hit.point);
                else if (hit.transform.parent != null &&
                    hit.transform.parent.GetComponent<IHealth>() != null)
                    hit.transform.parent.GetComponent<IHealth>().GetDamage(m_Damage, hit.point);
            }
        }
        PlayShootFX();
        Travis_ScriptHandle.Instance.TravisAnimation.AnimationWeaponFire((int)Type);
       m_RecoilTime = m_RecoilTimeDefault;
        m_AmmoCount--;
    }

    public void ShootNoAmmo()
    {
        //No Ammo SFX
        m_SFXSource.clip = m_NoAmmoSFX;
        PlaySFX();
    }

    public void PlayShootFX()
    {
        m_FireFX.Play();
        m_SFXSource.clip = m_FireSFX.Length == 0 ? null : m_FireSFX[Random.Range(0, m_FireSFX.Length - 1)];
        m_SFXSource.Play();
    }

    public void PlaySFX()
    {
        m_SFXSource.pitch = Random.Range(0.9f, 1.1f);
        m_SFXSource.Play();
    }

    public override void PrepareFire()
    {
        if (m_Automatic)
            m_ButtonPressed = true;
        else
        {
            if (m_RecoilTime <= 0f)
            {
                if (m_AmmoCount > 0)
                    Shoot();
                else
                    ShootNoAmmo();
            }
        }
    }

    public override void Reload()
    {
        Debug.Log("RELOADED GUN!");
        m_Allow = false;
        Travis_ScriptHandle.Instance.TravisAnimation.AnimationWeaponReload();
        m_SFXSource.clip = m_ReloadSFX;
        PlaySFX();

        int ammoInvCount = 999; //Travis_ScriptHandle.Instance.TravisInventory.GetCount(int id)
        if (ammoInvCount <= 0) return;

        int ammoDiff = m_AmmoCountDefault - m_AmmoCount;

        if (ammoInvCount >= ammoDiff)
        {
            m_AmmoCount = m_AmmoCountDefault;
            ammoInvCount =- ammoDiff;
        }
        else
        {
            m_AmmoCount =+ ammoInvCount;
            ammoInvCount = 0;
        }

        //Travis_ScriptHandle.Instance.TravisInventory.SetCount(int id, int count)
        
    }


    public override void WeaponDefaultState()
    {
        m_Allow = true;
    }

    public void WeaponDeny()
    {
        m_Allow = false;
    }

    public override int GetWeaponType()
    {
        return (int)Type;
    }

    public override void ComboColliderActivate() { return; }
    public override void ColliderOff() { return; }

    
}
