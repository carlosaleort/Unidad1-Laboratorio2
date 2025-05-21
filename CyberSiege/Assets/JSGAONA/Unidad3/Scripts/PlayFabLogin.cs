using TMPro;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.JSGAONA.Unidad3.Scripts {

    // Se emplea este script para gestionar un sistema de login con PlayFab
    public class PlayFabLogin : MonoBehaviour {

        [Header("Formulario Inicio Sesion")]
        [SerializeField] private TMP_InputField txtEmailLogin;
        [SerializeField] private TMP_InputField txtPasswordLogin;

        [Header("Formulario Registro")]
        [SerializeField] private TMP_InputField txtNickUserRegister;
        [SerializeField] private TMP_InputField txtEmailRegister;
        [SerializeField] private TMP_InputField txtPasswordRegister;
        [SerializeField] private TMP_InputField txtConfirmPasswordRegister;

        [Header("Paneles del juego")]
        [SerializeField] private GameObject pnlMainMenu;


    #region Inicio de sesion

        // Se emplea este metodo para poder iniciar sesion
        public void Login(){
            string email = txtEmailLogin.text;
            string password = txtPasswordLogin.text;
            // Se valida que el correo y la clave no sean nulos
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password)) {
                LoginWithEmail(email, password);
            }
        }


        // Se emplea este metodo para poder iniciar sesion con correo y contrasena
        private void LoginWithEmail(string email, string password) {
            var request = new LoginWithEmailAddressRequest {
                Email = email,
                Password = password
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }


        // Se emplea este metodo para realizar acciones cuando el inicio de sesion es exitoso
        private void OnLoginSuccess(LoginResult result) {
            Debug.Log("Login con email exitoso.");
            gameObject.SetActive(false);
            pnlMainMenu.SetActive(true);
        }


        // Se emplea este metodo para realizar acciones cuando el inicio de sesion es fallido
        private void OnLoginFailure(PlayFabError error) {
            Debug.LogError("Error al iniciar sesión: " + error.GenerateErrorReport());
        }
    #endregion

        
    #region Registro de usuario
        // Se emplea este metodo para registrar el usuario
        public void Register() {
            // Se obtiene la informacion de los inputs
            string nick = txtNickUserRegister.text;
            string email = txtEmailRegister.text;
            string password = txtPasswordRegister.text;
            string confirmPassword = txtConfirmPasswordRegister.text;

            // Se valida que el correo y la clave no sean nulos
            if(!string.IsNullOrEmpty(nick) && !string.IsNullOrEmpty(email)
                    && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword)) {
                // Se verifica que la clave y repetir clave sean iguales
                if(password.Equals(confirmPassword)){
                    RegisterUser(nick, email, password);
                }
            }
        }


        // Se emplea este metodo para poder registrar un usuario al sistema
        private void RegisterUser(string username, string email, string password) {
            var request = new RegisterPlayFabUserRequest {
                Username = username,
                Email = email,
                Password = password,
                RequireBothUsernameAndEmail = true
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
        }


        // Se emplea este metodo para realizar acciones cuando el registro es exitoso
        private void OnRegisterSuccess(RegisterPlayFabUserResult result) {
            Debug.Log("Registro exitoso.");
            var stats = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = "Score", Value = 0 },
            new StatisticUpdate { StatisticName = "Coins", Value = 0 },
            new StatisticUpdate { StatisticName = "Diamantes", Value = 0 }
            };

            var request = new UpdatePlayerStatisticsRequest {
                Statistics = stats
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request,
                result => Debug.Log("Estadísticas actualizadas con diamantes."),
                error => Debug.LogError("Error al actualizar estadísticas: " + error.GenerateErrorReport()));
        }


        // Se emplea este metodo para realizar acciones cuando el registro es fallido
        private void OnRegisterFailure(PlayFabError error) {
            Debug.LogError("Error en registro: " + error.GenerateErrorReport());
        }
    #endregion
    }
}