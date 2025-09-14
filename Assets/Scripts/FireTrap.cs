using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class FireTrap : MonoBehaviour
{
    public enum FireTrapMode { Instant, Timed }

    [Header("General Settings")]
    [SerializeField] private FireTrapMode trapMode = FireTrapMode.Instant;
    [SerializeField] private Animator fireAnimator;
    [SerializeField] private string fireTriggerName = "Activate";

    [Header("Timed Settings")]
    [SerializeField] private float activeTime = 1f;   // Tiempo encendido
    [SerializeField] private float cooldownTime = 2f; // Tiempo apagado

    private bool isActive = false; // Solo se usa en modo Timed
    private Coroutine cycleRoutine;

    private void Reset()
    {
        if (fireAnimator == null)
            fireAnimator = GetComponentInChildren<Animator>();

        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void Start()
    {
        // Configurar automáticamente según el nivel
        if (LevelManager.Instance != null)
        {
            // En nivel Hard fuego instantáneo
            if (!LevelManager.Instance.IsGoodLevel && LevelManager.Instance.BrokenLevelIndex == 0)
                trapMode = FireTrapMode.Instant;
            else
                trapMode = FireTrapMode.Timed;
        }

        if (trapMode == FireTrapMode.Timed)
            cycleRoutine = StartCoroutine(FireCycle());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        switch (trapMode)
        {
            case FireTrapMode.Instant:
                // Mata siempre
                ActivateFire();
                LevelManager.Instance?.PlayerDefeated("El jugador fue quemado por fuego instantáneo");
                break;

            case FireTrapMode.Timed:
                if (isActive)
                {
                    LevelManager.Instance?.PlayerDefeated("El jugador fue quemado por fuego con temporizador");
                }
                break;
        }
    }

    private void ActivateFire()
    {
        if (fireAnimator != null && !string.IsNullOrEmpty(fireTriggerName))
            fireAnimator.SetTrigger(fireTriggerName);
    }

    private IEnumerator FireCycle()
    {
        while (true)
        {
            // Encender fuego
            isActive = true;
            ActivateFire();
            yield return new WaitForSeconds(activeTime);

            // Apagar fuego
            isActive = false;
            yield return new WaitForSeconds(cooldownTime);
        }
    }
}
