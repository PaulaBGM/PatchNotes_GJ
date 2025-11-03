using System;
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
        // Configurar autom�ticamente seg�n el nivel
        if (LevelManager.Instance != null)
        {
            // En nivel Hard fuego instant�neo
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

        SpriteRenderer playerVisual = other.gameObject.GetComponentInChildren<SpriteRenderer>();
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Collider2D collider = other.GetComponent<Collider2D>();

        if (playerVisual == null) return;

        // Detenemos el movimiento físico del jugador
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;       // Detiene el movimiento
            rb.bodyType = RigidbodyType2D.Kinematic;            // Desactiva las físicas
        }

        if (collider != null)
        {
            collider.enabled = false;         // Evita que siga activando cosas
        }

        switch (trapMode)
        {
            case FireTrapMode.Instant:
                ActivateFire();
                StartCoroutine(FadeOutPlayer(playerVisual));
                LevelManager.Instance?.PlayerDefeated("El jugador fue quemado por fuego instantáneo");
                break;

            case FireTrapMode.Timed:
                if (isActive)
                {
                    StartCoroutine(FadeOutPlayer(playerVisual));
                    LevelManager.Instance?.PlayerDefeated("El jugador fue quemado por fuego con temporizador");
                }
                break;
        }
    }
    
    private IEnumerator FadeOutPlayer(SpriteRenderer playerVisual)
    {
        float fadeDuration = 1f; // tiempo del desvanecido
        float elapsed = 0f;
        Color color = playerVisual.color;
        float startAlpha = color.a;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            playerVisual.color = color;
            yield return null;
        }

        // finalmente desactiva el sprite
        playerVisual.enabled = false;
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
