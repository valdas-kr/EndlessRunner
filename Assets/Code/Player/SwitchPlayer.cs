using System.Threading.Tasks; using TMPro;
using UnityEngine; using UnityEngine.UI;
using UnityEngine.Localization.Settings;
//LJ - Long Jump (Default), DJ - Double Jump (Frog), TP - Third Player

public class SwitchPlayer : MonoBehaviour {
    //Veikėjų objektai
    public GameObject longJumpBody, doubleJumpBody, thirdPlayerBody;

    //Kuris veikėjas pasirinktas
    public int playerPicked;

    //Veikėjų kainos
    [SerializeField] int price;
    [SerializeField] int TPprice;

    //Vartotojo sąsajos komponentai
    [SerializeField] MenuController mc;
    public GameManager gm;
    public TextMeshProUGUI djButtonText;
    public TextMeshProUGUI tpButtonText;
    [SerializeField] Button ljButton;
    [SerializeField] Button djButton;
    [SerializeField] Button tpButton;

    void Start() {
        //Užkraunamas išsaugotas veikėjo pasirinkimas
        gm = GameManager.Instance;
    }

    public void ChangePlayer(int player) {
        //Keičiamas veikėjas ir aktyvuojamas reikiamas objektas
        playerPicked = player;
        longJumpBody.SetActive(false);
        doubleJumpBody.SetActive(false);
        thirdPlayerBody.SetActive(false);
        if (playerPicked == 1) {
            doubleJumpBody.SetActive(true);
            doubleJumpBody.transform.position = new Vector3(-4.5f, doubleJumpBody.transform.position.y, 0f);
        } else if (playerPicked == 2) {
            thirdPlayerBody.SetActive(true);
            thirdPlayerBody.transform.position = new Vector3(-4.5f, thirdPlayerBody.transform.position.y, 0f);
        } else {
            longJumpBody.SetActive(true);
            longJumpBody.transform.position = new Vector3(-4.5f, longJumpBody.transform.position.y, 0f);
        }
        mc.ClickMainButton();
    }

    public void SelectLongJump() {
        //Pasirenkamas pirmas veikėjas
        ChangePlayer(0);
    }

    async public void BuyFrogBody() {
        //Tikrinama, ar antras veikėjas jau nėra nupirktas
        if (gm.data.frogBodyOwned == false) {
            //Jei nėra, tikrinama, ar užtenka pinigų nupirkti veikėją
            if (gm.data.coins >= price) {
                //Jei užtenka, veikėja nuperkamas, užkraunamas ir išsaugomas
                gm.data.coins -= price;
                gm.totalCoins = gm.data.coins;
                gm.data.frogBodyOwned = true;
                gm.SaveData();
                ChangePlayer(1);
            }
            else {
                //Jei neužtenka pinigų, porai sekundžių įjungiamas pagalbinis tekstas
                if (LocalizationSettings.SelectedLocale.ToString() == "Lithuanian (lt)") {
                    djButtonText.text = "Neužtenka pinigų!";
                } else {
                    djButtonText.text = "Not Enough Coins!";
                }
                await Task.Delay(2000);
                djButtonText.text = "10C";
            }
        } else {
            //Jei yra, jis užkraunamas
            ChangePlayer(1);
        }
    }

    async public void BuyThirdPlayerBody() {
        //Tikrinama, ar trečias veikėjas jau nėra nupirktas
        if (gm.data.thirdPlayerBodyOwned == false) {
            //Jei nėra, tikrinama, ar užtenka pinigų nupirkti veikėją
            if (gm.data.coins >= TPprice) {
                //Jei užtenka, veikėja nuperkamas, užkraunamas ir išsaugomas
                gm.data.coins -= TPprice;
                gm.totalCoins = gm.data.coins;
                gm.data.thirdPlayerBodyOwned = true;
                gm.SaveData();
                ChangePlayer(2);
            }
            else {
                //Jei neužtenka pinigų, porai sekundžių įjungiamas pagalbinis tekstas
                if (LocalizationSettings.SelectedLocale.ToString() == "Lithuanian (lt)") {
                    tpButtonText.text = "Neužtenka pinigų!";
                } else {
                    tpButtonText.text = "Not Enough Coins!";
                }
                await Task.Delay(2000);
                tpButtonText.text = "50C";
            }
        } else {
            //Jei yra, jis užkraunamas
            ChangePlayer(2);
        }
    }
}