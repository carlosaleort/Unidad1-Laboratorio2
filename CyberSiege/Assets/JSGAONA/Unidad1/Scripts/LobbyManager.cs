using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
  
        public void CambiarASiguienteEscena()
        {
            int escenaActual = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(escenaActual + 1);
        }


    public void CerrarAplicacion()
    {
        Application.Quit();
        Debug.Log("Aplicación cerrada"); 
    }
}
