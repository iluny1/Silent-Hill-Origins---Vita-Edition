using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidBall : MonoBehaviour {

    [SerializeField] private float m_Speed = 7f; // Скорость шара (~7 м/с)
    [SerializeField] private float m_Gravity = 9.81f; // Гравитация
    [SerializeField] private float m_Lifetime = 2f; // Время жизни (2 с)
    [SerializeField] private float m_Damage = 10f; // Урон
    [SerializeField] private float m_MaxHeight = 1.8f;

    private GameObject m_Owner;
    private Vector3 m_InitialVelocity; // Начальная скорость
    private float m_ElapsedTime; // Время с момента запуска
    private Vector3 m_StartPosition; // Начальная позиция

    public void Initialize(Vector3 targetPosition, Vector3 startPosition, GameObject owner)
    {
        m_StartPosition = startPosition;
        m_ElapsedTime = 0f;
        m_Owner = owner;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float deltaY = targetPosition.y - startPosition.y;
        float timeToTarget = distance / m_Speed;
        Vector3 direction = (targetPosition - startPosition).normalized;
        float verticalVelocity = (targetPosition.y - startPosition.y + 0.5f * m_Gravity * timeToTarget * timeToTarget) / timeToTarget;
        //m_InitialVelocity = direction * m_Speed + Vector3.up * verticalVelocity;
        float maxVerticalVelocity = Mathf.Sqrt(2 * m_Gravity * (m_MaxHeight - startPosition.y));
        if (verticalVelocity > maxVerticalVelocity)
        {
            verticalVelocity = maxVerticalVelocity;

            // Корректируем timeToTarget, чтобы достичь targetPosition
            // Уравнение: deltaY = vy * t - 0.5 * g * t^2
            // Решаем квадратное уравнение: 0.5 * g * t^2 - vy * t + deltaY = 0
            float a = 0.5f * m_Gravity;
            float b = -verticalVelocity;
            float c = deltaY;
            float discriminant = b * b - 4 * a * c;
            if (discriminant >= 0)
            {
                timeToTarget = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
            }
            else
            {
                timeToTarget = distance / m_Speed; // Фallback
            }
        }

        // Корректируем горизонтальную скорость
        float horizontalSpeed = distance / timeToTarget;
        m_InitialVelocity = direction * horizontalSpeed + Vector3.up * verticalVelocity;
    }

    private void Update()
    {
        m_ElapsedTime += Time.deltaTime;

        // Параболическая траектория: x = x0 + v0*t, y = y0 + vy*t - 0.5*g*t^2
        Vector3 newPosition = m_StartPosition + m_InitialVelocity * m_ElapsedTime;
        newPosition.y -= 0.5f * m_Gravity * m_ElapsedTime * m_ElapsedTime;
        transform.position = newPosition;

        // Уничтожаем шар по истечении времени
        if (m_ElapsedTime >= m_Lifetime)
            AcidBallPool.Instance.ReturnAcidBall(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == m_Owner) return;

        IHealth health = other.GetComponent<IHealth>();

        if (other.transform.parent != null && other.transform.parent.GetComponent<IHealth>() != null)
            health = other.transform.parent.GetComponent<IHealth>();

        if (health != null)
            health.GetDamage(m_Damage, transform.position);

        // Запускаем эффект попадания
        AcidHitEffectPool.Instance.GetAcidHitEffect(transform.position);
        AcidBallPool.Instance.ReturnAcidBall(gameObject);
    }
}
