using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SaveData (Highscore highscore)
	{
		string path = Directory.GetCurrentDirectory() + "/ranking.sav";	// Location of the Save
		BinaryFormatter formatter = new BinaryFormatter();		

		

		List<Highscore> ls = LoadScore();
		if (ls == null)		// If no Save File is Detected
		{
			List<Highscore> newHsList = new List<Highscore>();	// Create a new List<Highscore>
			newHsList.Add(highscore);	// Add new highscore to List

			FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);	// Save a new File
			Debug.Log("Saving to New Save File");
			formatter.Serialize(stream, newHsList);
			stream.Close();
		} else	// If a Save File is Detected
		{
			List<Highscore> hsList = LoadScore();	// Load the Save File
			hsList.Add(highscore);  // Add new highscore to Save File list
			FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);	// Overwrite Save File
			Debug.Log("Overwriting to Save File");
			formatter.Serialize(stream, hsList);
			stream.Close();
		}
	}

	public static List<Highscore> LoadScore()
	{
		string path = Directory.GetCurrentDirectory() + "/ranking.sav";
		if (File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

			List<Highscore> data = formatter.Deserialize(stream) as List<Highscore>;
			stream.Close();

			return data;
		} else
		{
			Debug.Log("Save File not found in " + path);
			return null;
		}
	}
}
