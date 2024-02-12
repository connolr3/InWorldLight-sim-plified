using UnityEngine;
using System.Collections.Generic;
using System.IO;
//sing bmlTUX;

public class CSVReader : MonoBehaviour
{
     public GameObject [] NiceFemales;
        public GameObject [] NeutralFemales;
            public GameObject [] NiceMales;
        public GameObject [] NeutralMales;
    public ProximityRunner proximityRunner;
    private string[] Names;


    void Start()
    {
        Names = new string[16];
        proximityRunner.allocate();
        ReadCSVFile("Assets/data.csv");

    }
       


    static int IndexToChange = 0;

        public void setAI(string Temperment, string Gender, int index){
           GameObject Character = null; // Initialize with a default value
        if(Gender=="Female"&&Temperment=="Nice"){
                Character=NiceFemales[index];
                }
                else if(Gender=="Male"&&Temperment=="Nice"){
                Character=NiceMales[index];
                }
                else if(Gender=="Female"&&Temperment=="Neutral"){
                Character=NeutralFemales[index];
                }
                else if(Gender=="Male"&&Temperment=="Neutral"){
                Character=NeutralMales[index];
                }
                else{
                    Debug.Log("Error in variable names... no matches found");
                }
        if(Character != null){
        Names[IndexToChange] = Character.name;
                ProximityRunner.names[IndexToChange] = Character.name;
                IndexToChange++;
        }
        else{
            Debug.Log("something has gone terrible wrong");
        }
        }



    void ReadCSVFile(string filePath)
    {
        try
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                // Read the first line to get column names
                string[] columns = reader.ReadLine().Split(',');
                int Trialindex = -1;
                while (!reader.EndOfStream)
                {
                    Trialindex++;
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    // Create a dictionary to store the data with column names as keys
                    Dictionary<string, string> row = new Dictionary<string, string>();

                    for (int i = 0; i < columns.Length; i++)
                    {
                        // Use column names as keys and corresponding values from the line
                        row[columns[i]] = values[i];
                    }
                    // Store the row in the data list
                    data.Add(row);
                    string indexString = row["Index"];
                    // Convert the string index to an integer
                    if (int.TryParse(indexString, out int index))
                    {
                       setAI(row["Temperment"],row["AIGender"],index);
                    }
                    else
                    {
                        // Handle the case where the conversion fails (e.g., invalid index format)
                        Debug.LogError("Error converting index to integer.");
                    }
                }
            }

           /* // uncomment to debug
            foreach (var row in data)
            {
                foreach (var pair in row)
                {
                //    Debug.Log(pair.Key + ": " + pair.Value);
                }
            }*/
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error reading CSV file: " + e.Message);
        }
    }
}
