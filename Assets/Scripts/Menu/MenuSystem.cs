using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public GameObject instruccionesPanel;

    public void Jugar()
    {
        Debug.Log("Iniciando el juego...");
        SceneManager.LoadScene("Juego");
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void MostrarInstrucciones()
    {
        instruccionesPanel.SetActive(true);
    }

}
