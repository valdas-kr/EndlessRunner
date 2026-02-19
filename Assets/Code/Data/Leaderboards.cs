using System; using System.Collections; using System.Collections.Generic;
using System.Linq; using System.Threading; using TMPro; using UnityEngine;

public class Leaderboards : MonoBehaviour {
    [SerializeField] FirebaseManager firebaseManager;
    private SynchronizationContext context;

    //Geriausių rezultatų atvaizdavimo laukai
    public TMP_Text[] leaderboards;

    private void Awake() {
        context = SynchronizationContext.Current;
    }

    public IEnumerator LoadScoreboardData() {
        //Atnaujinami geriausi visų lygių rezultatai
        for(int i = 1; i <= 3; i++) {
            int index = i;
            firebaseManager.DBreference.Child("users").OrderByChild("highscore" + index).LimitToLast(10).GetValueAsync().ContinueWith(task => {
                if (task.IsCompleted) {
                    //Neapdoroti duomenys
                    Dictionary<string, object> unsortedLeaderboard = (Dictionary<string, object>)task.Result.Value;

                    //Gaunami atskiri vartotojų vardų ir jų rezultatų duomenys
                    string[] names = unsortedLeaderboard.Values.Select(value => (Dictionary<string, object>)value).
                    Select(d => d["username"].ToString()).ToArray();
                    int[] scores = unsortedLeaderboard.Values.Select(value => (Dictionary<string, object>)value).
                    Select(d => Convert.ToInt32(d["highscore" + index])).ToArray();

                    //Gauti duomenys sujungiami į Tuple tipą
                    Tuple<string, int>[] userScores = new Tuple<string, int>[scores.Length];
                    for (int j = 0; j < scores.Length; j++) {
                        userScores[j] = new(names[j], scores[j]);
                    }

                    //Tuple tipas surūšiuoja pagal rezultatus
                    Tuple<string, int>[] sortedScores = userScores.OrderByDescending(tuple => tuple.Item2).ToArray();
                    string leaderboard = "";
                    for (int l = 0; l < sortedScores.Length; l++) {
                        //Rezultatai atvaizduojami
                        leaderboard += l + 1 + ". " + sortedScores[l].Item1 + " - " + sortedScores[l].Item2 + "\n";
                    }

                    context.Post(_ => {
                        //Teksto atnaujinimas perkeliamas į pagrindinę giją, vartotojas mato rezultatus
                        leaderboards[index-1].text = leaderboard;
                    }, null);
                }
            });
        }

        yield return null;
    }

    public void ClearScoreboardData() {
        context.Post(_ => {
            //Teksto atnaujinimas perkeliamas į pagrindinę giją, vartotojas mato rezultatus
            leaderboards[0].text = "";
            leaderboards[1].text = "";
            leaderboards[2].text = "";
        }, null);
    }
}