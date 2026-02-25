using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadService : ISaveLoadService
{
    private static readonly string SavePath = $"{Application.persistentDataPath}/save.json";

    public SaveData SaveData { get; private set; } = new();

    private readonly List<ISaveDataWriter> _writers = new();
    private readonly List<ISaveDataReader> _readers = new();

    public void RegisterReader<T>(T reader) where T : ISaveDataReader =>
        _readers.Add(reader);

    public void RegisterWriter<T>(T writer) where T : ISaveDataWriter =>
        _writers.Add(writer);

    public void Save()
    {
        try
        {
            foreach (ISaveDataWriter writer in _writers)
                writer.Write(SaveData);

            string json = JsonUtility.ToJson(SaveData, true);
            File.WriteAllText(SavePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"Cannot save data: {e.Message}");
        }
    }

    public void Load()
    {
        if (!File.Exists(SavePath))
            return;

        try
        {
            string json = File.ReadAllText(SavePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            SaveData = saveData ?? new SaveData();
        }
        catch (Exception e)
        {
            Debug.LogError($"Cannot load data: {e.Message}");
            SaveData = new SaveData();
        }

        foreach (var reader in _readers)
            reader.Read(SaveData);
    }

    public void Clear()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}