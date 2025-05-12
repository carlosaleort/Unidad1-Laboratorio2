using TMPro;
using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Assets.JSGAONA.Unidad3.Scripts
{

    public class playFablogin : MonoBehaviour
    {
        [Header("Formulario Inicio Sesion")]
        [SerializeField] private TMP_InputField txtEmailLogin;
        [SerializeField] private TMP_InputField txtPasswordLogin;

        [Header("Formulario Registro")]
        [SerializeField] private TMP_InputField txtNickUserRegister;
        [SerializeField] private TMP_InputField txtEmailRegister;
        [SerializeField] private TMP_InputField txtPasswordRegister;
        [SerializeField] private TMP_InputField txtConfirmPasswordRegister;
        [SerializeField] private Toggle toggleRememberMe;

        [Header("Paneles del juego")]
        [SerializeField] private GameObject pnlMainMenu;


        #region Inicio de sesion
        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
                PlayFabSettings.TitleId = "TU_TITLE_ID"; 

            if (PlayerPrefs.GetInt("RememberMe", 0) == 1)
            {
                string savedUser = PlayerPrefs.GetString("SavedUser", "");
                string savedPass = PlayerPrefs.GetString("SavedPass", "");

                txtEmailLogin.text = savedUser;
                txtPasswordLogin.text = savedPass;
                if (toggleRememberMe != null) toggleRememberMe.isOn = true;

            }
        }


        
        public void Login()
        {
            string input = txtEmailLogin.text;
            string password = txtPasswordLogin.text;

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(password)) return;

            // Guardar credenciales si "Recordar usuario" está activo
            if (toggleRememberMe != null && toggleRememberMe.isOn)
            {
                PlayerPrefs.SetString("SavedUser", input);
                PlayerPrefs.SetString("SavedPass", password);
                PlayerPrefs.SetInt("RememberMe", 1);
            }
            else
            {
                PlayerPrefs.DeleteKey("SavedUser");
                PlayerPrefs.DeleteKey("SavedPass");
                PlayerPrefs.SetInt("RememberMe", 0);
            }

            // Detectar si es email o username
            if (input.Contains("@"))
            {
                LoginWithEmail(input, password);
            }
            else
            {
                LoginWithUsername(input, password);
            }
        }
        private void LoginWithUsername(string username, string password)
        {
            var request = new LoginWithPlayFabRequest
            {
                Username = username,
                Password = password
            };
            PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
        }
        private void LoginWithEmail(string email, string password)
        {
            var request = new LoginWithEmailAddressRequest
            {
                Email = email,
                Password = password
            };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }


        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log("Login con email exitoso.");
            gameObject.SetActive(false);
            pnlMainMenu.SetActive(true);
        }


        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError("Error al iniciar sesión: " + error.GenerateErrorReport());
        }
        #endregion


        #region Registro de usuario
        public void Register()
        {
            string nick = txtNickUserRegister.text;
            string email = txtEmailRegister.text;
            string password = txtPasswordRegister.text;
            string confirmPassword = txtConfirmPasswordRegister.text;

            Debug.Log($"Intentando registrar: {nick} / {email}");

            if (!string.IsNullOrEmpty(nick) && !string.IsNullOrEmpty(email)
                    && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(confirmPassword))
            {
                if (password.Equals(confirmPassword))
                {
                    RegisterUser(nick, email, password);
                }
            }
        }


        private void RegisterUser(string username, string email, string password)
        {
            var request = new RegisterPlayFabUserRequest
            {
                Username = username,
                Email = email,
                Password = password,
                RequireBothUsernameAndEmail = true
            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
        }


        private void OnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log("Registro exitoso. Verificando cuenta...");
            VerificarCuenta(txtEmailRegister.text);
            var stats = new List<StatisticUpdate> {
            new StatisticUpdate { StatisticName = "Score", Value = 0 },
            new StatisticUpdate { StatisticName = "Coins", Value = 0 },
            new StatisticUpdate { StatisticName = "Diamantes", Value = 0 }
            };

            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = stats
            };

            PlayFabClientAPI.UpdatePlayerStatistics(request,
                result => Debug.Log("Estadísticas actualizadas con diamantes."),
                error => Debug.LogError("Error al actualizar estadísticas: " + error.GenerateErrorReport()));
        }


        private void OnRegisterFailure(PlayFabError error)
        {
            Debug.LogError("Error en registro: " + error.GenerateErrorReport());
        }
        #endregion
        private void VerificarCuenta(string email)
        {
            var request = new GetAccountInfoRequest
            {
                Email = email
            };

            PlayFabClientAPI.GetAccountInfo(request,
                result => {
                    Debug.Log("Cuenta encontrada: " + result.AccountInfo.Username + " | PlayFab ID: " + result.AccountInfo.PlayFabId);
                },
                error => {
                    Debug.LogError("No se encontró la cuenta: " + error.GenerateErrorReport());
                });
        }

    }
}