public interface ISaveLoadService
{
    SaveData SaveData { get; }

    public void RegisterReader<T>(T reader) where T : ISaveDataReader;
    public void RegisterWriter<T>(T writer) where T : ISaveDataWriter;
    void Save();
    void Load();
    void Clear();
}
