using UnityEngine; using Firebase; using TMPro;
using Firebase.Auth; using Firebase.Database;

public class FirebaseManager : MonoBehaviour {
    //Duomenų bazės statusas, autentifikacija, sujungimas ir vartotojas
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public DatabaseReference DBreference;
    public FirebaseUser User;

    //Prisijungimo, atnaujinimo, registracijos, atsijungimo ir vartotojo sąsajos klasės
    public FirebaseLogin login;
    public FirebaseUpdate update;
    public FirebaseRegister register;
    public FirebaseUI ui;
    public FirebaseLogout logout;

    //Vartotojo vardo laukas
    public TMP_InputField usernameField;

    //Ar yra prisijungta
    public bool isLoggedIn;

    private void Awake() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available) {
                auth = FirebaseAuth.DefaultInstance;
                DBreference = FirebaseDatabase.DefaultInstance.RootReference;
                logout.LogOutAccount();
                FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(false);
            }
        });
    }

    public void LoginButton() {
        //Prisijungimo mygtukas
        StartCoroutine(login.Login());
    }

    public void RegisterButton() {
        //registracijos mygtukas
        StartCoroutine(register.Register());
    }

    public void SignOutButton() {
        //Atsijungimo mygtukas
        logout.LogOutAccount();
    }

    public void SaveDataButton() {
        //Duomenų išsaugojimo ir atnaujinimo mygtukas
        StartCoroutine(update.UpdateUsername());
        StartCoroutine(update.UpdateUserStats());
    }

    public void ResetButton() {
        //Profilio šalinimo mygtukas
        logout.DeleteAccount();        
    }
}