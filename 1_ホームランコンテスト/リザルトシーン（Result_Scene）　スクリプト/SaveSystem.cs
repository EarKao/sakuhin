using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
	public static void SaveData (Highscore highscore)
	{
		string path = Directory.GetCurrentDirectory() + "/ranking.sav";	// �Z�[�u�ʒu�@Location of the Save
		BinaryFormatter formatter = new BinaryFormatter();		

		

		List<Highscore> ls = LoadScore();
		if (ls == null)     // �Z�[�u�Ȃ��̏ꍇ�@If no Save File is Detected
		{
			List<Highscore> newHsList = new List<Highscore>();	// List<Highscore>�����@Create a new List<Highscore>
			newHsList.Add(highscore);	// �n�C�X�R�A��ǉ�����@Add new highscore to List

			FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);	// �Z�[�u����@Save a new File
			Debug.Log("Saving to New Save File");
			formatter.Serialize(stream, newHsList);
			stream.Close();
		} else	// �Z�[�u�f�[�^����̏ꍇ�@If a Save File is Detected
		{
			List<Highscore> hsList = LoadScore();	// �Z�[�u�����[�h����@Load the Save File
			hsList.Add(highscore);  // �n�C�X�R�A��ǉ�����@Add new highscore to Save File list
			FileStream stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);	// �㏑���@Overwrite Save File
			Debug.Log("Overwriting to Save File");
			formatter.Serialize(stream, hsList);
			stream.Close();
		}
	}
	
	// �n�C�X�R�A�����[�h����
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
