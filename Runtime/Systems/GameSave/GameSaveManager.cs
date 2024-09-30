using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;

namespace CupkekGames.Core
{
  public abstract class GameSaveManager<TSaveData> : MonoBehaviour where TSaveData : GameSaveData
  {
    // Settings
    public string FileExtenstion = "save";
    // State
    public GameSaveDataSO<TSaveData> CurrentSave;

    public List<TSaveData> GetAllSave()
    {
      List<string> fileNames = GetAllFileNames();

      List<TSaveData> list = new List<TSaveData>();

      foreach (string fileName in fileNames)
      {
        if (!fileName.Contains(FileExtenstion))
        {
          continue;
        }

        int number = ExtractNumber(fileName);
        if (number == -1)
        {
          continue;
        }
        TSaveData data;
        try
        {
          data = (TSaveData)Activator.CreateInstance(typeof(TSaveData), new object[] { fileName, number });
        }
        catch (Exception e)
        {
          Debug.LogException(e, this);
          continue;
        }
        list.Add(data);
      }

      list.Sort((x, y) => y.SaveDate.CompareTo(x.SaveDate));

      return list;
    }
    public TSaveData GetLastSave()
    {
      List<TSaveData> list = GetAllSave();
      if (list.Count == 0)
      {
        return null;
      }

      TSaveData result = list[0];

      return result;
    }

    protected string GetSaveFileName(int slot)
    {
      return "save" + slot.ToString() + "." + FileExtenstion;
    }

    public int ExtractNumber(string saveString)
    {
      // Regular expression pattern for "save" followed by digits
      string formatPattern = Regex.Escape(FileExtenstion);
      Regex regex = new Regex(@"^save(\d+)\." + formatPattern + "$");
      Match match = regex.Match(saveString);

      if (match.Success)
      {
        string numberString = match.Groups[1].Value;
        return int.Parse(numberString);
      }

      return -1; // Return -1 to indicate an invalid format
    }

    public void SaveToFile(int slot, TSaveData data)
    {
      string fileName = GetSaveFileName(slot);

      OnSaveRequest(fileName, data);
    }

    public void DeleteFile(int slot)
    {
      string fileName = GetSaveFileName(slot);

      OnDeleteRequest(fileName);
    }

    public void CreateNewSave(TSaveData data)
    {
      List<string> fileNames = GetAllFileNames();

      int[] numbers = fileNames.Select(ExtractNumber).ToArray();

      Array.Sort(numbers);

      for (int i = 0; i < numbers.Length; i++)
      {
        if (numbers[i] == -1)
        {
          // ignore invalid file name formats
          continue;
        }

        int current = numbers[i];
        int next = current + 1;

        // Save to first missing number
        // If no missing number, save to next number
        if (i + 1 == numbers.Length || numbers[i + 1] != next)
        {
          // Debug.Log("Saving to slot: " + next);
          SaveToFile(next, data);
          return;
        }
      }

      // if code reached here, there are invalid files only. In that case for loop above doesn't save. So save to first slot here.
      SaveToFile(0, data);
    }
    protected abstract void OnSaveRequest(string fileName, TSaveData data);
    protected abstract void OnDeleteRequest(string fileName);
    protected abstract List<string> GetAllFileNames();
  }
}
