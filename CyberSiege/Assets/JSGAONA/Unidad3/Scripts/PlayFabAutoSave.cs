using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;
using System.Collections.Generic;

namespace Assets.JSGAONA.Unidad3.Scripts {

    public class PlayFabAutoSave : MonoBehaviour {

    [SerializeField] private int score = 100;
    [SerializeField] private int coins = 2;
    [SerializeField] private int diamons = 20;



        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Player")) VerificarYGuardarMejorScore();
        }

        public void SaveScore() {
            var request = new UpdatePlayerStatisticsRequest {
                Statistics = new List<StatisticUpdate> {
                    new StatisticUpdate {
                        StatisticName = "Score",
                        Value = score
                    },
                    new StatisticUpdate {
                        StatisticName = "Coins",
                        Value = coins
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request,
                result => Debug.Log("Score guardado correctamente en el punto de guardado."),
                error => Debug.LogError("Error al guardar score: " + error.GenerateErrorReport()));
        }


        public void VerificarYGuardarMejorScore() {
            PlayFabClientAPI.GetPlayerStatistics(new GetPlayerStatisticsRequest(), result => {
                int scoreActual = 0;
                foreach (var stat in result.Statistics) {
                    if (stat.StatisticName == "Score") {
                        scoreActual = stat.Value;
                        break;
                    }
                }
                // El score actual es mayor al obtenido de la DB
                if(score > scoreActual){
                    // Guardar el nuevo score
                    SaveScore();
                }else{
                    // no hacer nada
                }
            }, error => {
                Debug.LogError("Error al obtener estadísticas: " + error.GenerateErrorReport());
            });
        }
    }
}