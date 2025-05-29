using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
using TMPro;
using System.Collections;

public class RopeSimulator : MonoBehaviour
{
    public float rotationSpeed = 70f;
    public float acceleration = 10f;
    public float maxSpeed = 350f;

    public List<MonoBehaviour> skeletonControllers; 
    private HashSet<MonoBehaviour> eliminated = new HashSet<MonoBehaviour>();
    private List<MonoBehaviour> aliveControllers = new List<MonoBehaviour>();

    private float currentAngle = 0f;
    private bool checkWindowActive = false;

    private bool gameEnded = false;
    public TextMeshProUGUI winText;
    public float delayBeforeReturn = 2f; // Segundos antes de permitir volver al menú
    private bool canReturnToMenu = false;

    public TextMeshProUGUI saltoCounterText;
    private int saltoCount = 0;
    private int playearsDead = 0;

    void Update()
    {
        // Acelera la cuerda
        rotationSpeed = Mathf.Min(rotationSpeed + acceleration * Time.deltaTime, maxSpeed);
        float deltaRotation = rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.right * deltaRotation);

        currentAngle = transform.localEulerAngles.x;
        if (currentAngle > 360f) currentAngle -= 360f;

        // Verifica la zona de golpe
        if (currentAngle > 80f && currentAngle < 100f)
        {
            if (!checkWindowActive && !playearsDead.Equals(skeletonControllers.Count))
            {
                checkWindowActive = true;
                saltoCount++;
                UpdateSaltoText();
                CheckSkeletons();
            }
        }
        else
        {
            checkWindowActive = false;
        }

        if (canReturnToMenu && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void CheckSkeletons()
    {
        foreach (MonoBehaviour controller in skeletonControllers)
        {
            if (eliminated.Contains(controller))
                continue; // Ya eliminado

            var method = controller.GetType().GetMethod("IsGrounded");
            if (method != null)
            {
                bool isGrounded = (bool)method.Invoke(controller, null);
                if (isGrounded)
                {
                    Debug.Log("💀 " + controller.gameObject.name + " fue golpeado por la cuerda");
                    playearsDead++;

                    Rigidbody rb = controller.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 forceDir = Vector3.right * 20f + Vector3.up * 3f; // X+ y un poco arriba
                        rb.linearVelocity = Vector3.zero; // Resetea velocidad antes
                        rb.AddForce(forceDir, ForceMode.Impulse);
                    }
                    eliminated.Add(controller);
                    checkWinner();
                }
            }
        }
    }

    void checkWinner()
    {
        if (gameEnded) return;

        var alive = new List<MonoBehaviour>();
        foreach (var skeleton in skeletonControllers)
        {
            if (!eliminated.Contains(skeleton))
            {
                alive.Add(skeleton);
            }
        }

        if (alive.Count == 1)
        {
            gameEnded = true;
            string winnerName = alive[0].gameObject.name;
            Debug.Log("🏆 ¡Ganó el jugador: " + winnerName + "!");

            if (winText != null)
            {
                winText.text = "¡Ganó el jugador: " + winnerName + "!\nPresiona ESPACIO para volver al menú";
                winText.enabled = true;
            }

            StartCoroutine(EnableReturnToMenu());
        }
    }

    IEnumerator EnableReturnToMenu()
    {
        yield return new WaitForSeconds(delayBeforeReturn);
        canReturnToMenu = true;
    }

    void UpdateSaltoText()
    {
        if (saltoCounterText != null)
        {
            saltoCounterText.text = saltoCount.ToString();
        }
    }
}