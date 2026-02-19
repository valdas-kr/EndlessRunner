using UnityEngine; using System.IO; 
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem {
    //Duomenų saugojimo katalogas vartotojo kompiuteryje
    public static readonly string SAVE_FOLDER = Application.persistentDataPath + "/saves/";

    public static void Save(string fileName, Data dataToSave) {
        //Jei nėra saugojimo katalogo, sukuriamas naujas katalogas
        if (!Directory.Exists(SAVE_FOLDER)) {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        //Įrašomi duomenys į failą
        FileStream dataStream = new(SAVE_FOLDER + fileName, FileMode.Create);
        BinaryFormatter converter = new();
        converter.Serialize(dataStream, dataToSave);
        dataStream.Close();
    }

    public static Data Load(string fileName) {
        //Duomenų saugojimo failo vieta
        string fileLoc = SAVE_FOLDER + fileName;

        //Jei failas egzistuoja, yra įkeliami duomenys
        if(File.Exists(fileLoc)) {
            FileStream dataStream = new(fileLoc, FileMode.Open);
            BinaryFormatter converter = new();
            Data saveData = converter.Deserialize(dataStream) as Data;
            dataStream.Close();
            return saveData;
        } else {
            return null;
        }
    }
}