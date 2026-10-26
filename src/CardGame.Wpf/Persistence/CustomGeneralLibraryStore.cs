using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CardGame.Content.Standard;

namespace CardGame.Wpf.Persistence;

public interface ICustomGeneralLibraryStore
{
    CustomGeneralLibrary Read();
    void Write(CustomGeneralLibrary library);
}

public sealed class MemoryCustomGeneralLibraryStore : ICustomGeneralLibraryStore
{
    private CustomGeneralLibrary _library;

    public MemoryCustomGeneralLibraryStore(CustomGeneralLibrary? library = null) =>
        _library = CustomGeneralLibraryStorage.Snapshot(library ?? CustomGeneralLibrary.Empty);

    public CustomGeneralLibrary Read() => _library;

    public void Write(CustomGeneralLibrary library)
    {
        var snapshot = CustomGeneralLibraryStorage.Snapshot(library);
        _library = snapshot;
    }
}

/// <summary>Local authoring data, replaced only after a complete validated snapshot is flushed.</summary>
public sealed class FileCustomGeneralLibraryStore : ICustomGeneralLibraryStore
{
    private const long MaximumFileBytes = 4 * 1024 * 1024;
    private readonly string _path;

    public FileCustomGeneralLibraryStore(string? path = null) => _path = Path.GetFullPath(path ??
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CardGame", "custom-generals.json"));

    public CustomGeneralLibrary Read()
    {
        if (!File.Exists(_path)) return CustomGeneralLibrary.Empty;
        using var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (stream.Length > MaximumFileBytes) throw new InvalidDataException("自定义武将库文件过大。");
        try
        {
            var library = JsonSerializer.Deserialize<CustomGeneralLibrary>(stream, CustomGeneralLibraryStorage.JsonOptions)
                ?? throw new InvalidDataException("自定义武将库为空。");
            return CustomGeneralLibraryStorage.Snapshot(library);
        }
        catch (JsonException error)
        {
            throw new InvalidDataException($"自定义武将库格式无效：{error.Message}", error);
        }
    }

    public void Write(CustomGeneralLibrary library)
    {
        var snapshot = CustomGeneralLibraryStorage.Snapshot(library);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(snapshot, CustomGeneralLibraryStorage.JsonOptions);
        if (bytes.LongLength > MaximumFileBytes) throw new InvalidDataException("自定义武将库超过文件大小上限。");
        var directory = Path.GetDirectoryName(_path)!;
        Directory.CreateDirectory(directory);
        var temporary = Path.Combine(directory, $".custom-generals-{Guid.NewGuid():N}.tmp");
        try
        {
            using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                stream.Write(bytes);
                stream.Flush(flushToDisk: true);
            }
            if (File.Exists(_path)) File.Replace(temporary, _path, _path + ".previous", ignoreMetadataErrors: true);
            else File.Move(temporary, _path);
        }
        finally
        {
            if (File.Exists(temporary)) File.Delete(temporary);
        }
    }
}

internal static class CustomGeneralLibraryStorage
{
    internal static JsonSerializerOptions JsonOptions { get; } = CreateJsonOptions();

    private static JsonSerializerOptions CreateJsonOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) }
        };
        options.MakeReadOnly(populateMissingResolver: true);
        return options;
    }

    internal static CustomGeneralLibrary Snapshot(CustomGeneralLibrary library)
    {
        try { return CustomGeneralContent.Snapshot(library); }
        catch (Exception error) when (error is ArgumentException or InvalidOperationException or JsonException)
        {
            throw new InvalidDataException($"自定义武将库无效：{error.Message}", error);
        }
    }
}

/// <summary>Uses the same authoring JSON contract inside the existing save-file envelope.</summary>
public sealed class CustomGeneralLibrarySnapshotConverter : JsonConverter<CustomGeneralLibrary>
{
    public override CustomGeneralLibrary Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // GameSaveFile's init accessor validates and freezes this payload once.
        return JsonSerializer.Deserialize<CustomGeneralLibrary>(ref reader, CustomGeneralLibraryStorage.JsonOptions)
            ?? throw new JsonException("自定义武将快照为空。");
    }

    public override void Write(Utf8JsonWriter writer, CustomGeneralLibrary value, JsonSerializerOptions options) =>
        JsonSerializer.Serialize(writer, value, CustomGeneralLibraryStorage.JsonOptions);
}
