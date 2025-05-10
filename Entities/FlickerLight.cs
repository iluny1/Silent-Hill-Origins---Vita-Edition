using UnityEngine;

[ExecuteInEditMode]
public class FlickerLight : MonoBehaviour {


    [SerializeField] private string m_Sequence;
    private Enums.LightMultiplier[] m_Multipliers;
    [SerializeField] private Enums.LightFlickerHz m_Hz = Enums.LightFlickerHz.TenPerSecond; //10Hz (Like Half-Life)
    private int m_CurrentIndex = 0;

    private Light m_Light;
    [SerializeField] private float m_InitialIntensity;
    
    [SerializeField] private float m_FlickerTimer;
    [SerializeField]  private float m_FlickerTimerCurrent = 0;     
	
	void Awake()
    {
        m_Light = GetComponent<Light>();
        m_FlickerTimer = 1f / (int)m_Hz;
        CreateArray();
	}

    private void OnEnable()
    {
        m_FlickerTimer = 1f / (int)m_Hz;
        CreateArray();
    }

    // Update is called once per frame
    void Update ()
    {
        if (m_FlickerTimerCurrent > 0) m_FlickerTimerCurrent -= Time.deltaTime;
        else Flick();
	}

    private void CreateArray()
    {
        char[] chars = m_Sequence.ToCharArray();
        m_Multipliers = new Enums.LightMultiplier[chars.Length];

        for (int i = 0; i < chars.Length; i++)
        {
            switch (chars[i])
            {
                case 'a': m_Multipliers[i] = Enums.LightMultiplier.a; break;
                case 'b': m_Multipliers[i] = Enums.LightMultiplier.b; break;
                case 'c': m_Multipliers[i] = Enums.LightMultiplier.c; break;
                case 'd': m_Multipliers[i] = Enums.LightMultiplier.d; break;
                case 'e': m_Multipliers[i] = Enums.LightMultiplier.e; break;
                case 'f': m_Multipliers[i] = Enums.LightMultiplier.f; break;
                case 'g': m_Multipliers[i] = Enums.LightMultiplier.g; break;
                case 'h': m_Multipliers[i] = Enums.LightMultiplier.h; break;
                case 'i': m_Multipliers[i] = Enums.LightMultiplier.i; break;
                case 'j': m_Multipliers[i] = Enums.LightMultiplier.j; break;
                case 'k': m_Multipliers[i] = Enums.LightMultiplier.k; break;
                case 'l': m_Multipliers[i] = Enums.LightMultiplier.l; break;
                case 'm': m_Multipliers[i] = Enums.LightMultiplier.m; break;
                case 'n': m_Multipliers[i] = Enums.LightMultiplier.n; break;
                case 'o': m_Multipliers[i] = Enums.LightMultiplier.o; break;
                case 'p': m_Multipliers[i] = Enums.LightMultiplier.p; break;
                case 'q': m_Multipliers[i] = Enums.LightMultiplier.q; break;
                case 'r': m_Multipliers[i] = Enums.LightMultiplier.r; break;
                case 's': m_Multipliers[i] = Enums.LightMultiplier.s; break;
                case 't': m_Multipliers[i] = Enums.LightMultiplier.t; break;
                case 'u': m_Multipliers[i] = Enums.LightMultiplier.u; break;
                case 'v': m_Multipliers[i] = Enums.LightMultiplier.v; break;
                case 'w': m_Multipliers[i] = Enums.LightMultiplier.w; break;
                case 'x': m_Multipliers[i] = Enums.LightMultiplier.x; break;
                case 'y': m_Multipliers[i] = Enums.LightMultiplier.y; break;
                case 'z': m_Multipliers[i] = Enums.LightMultiplier.z; break;
                default: m_Multipliers[i] = Enums.LightMultiplier.m; break;
            }
        }
    }

    private void Flick()
    {
        m_Light.intensity = m_InitialIntensity * ((int)m_Multipliers[m_CurrentIndex] / 100f);

        if (m_CurrentIndex == m_Multipliers.Length - 1)
            m_CurrentIndex = 0;
        else
            m_CurrentIndex++;

        m_FlickerTimerCurrent = m_FlickerTimer;
    }
}
