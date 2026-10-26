using System.IO;
using CardGame.Content.Standard;
using CardGame.Core;
using CardGame.Wpf.Persistence;

namespace CardGame.Wpf.ViewModels;

public sealed partial class MainViewModel
{
    private ICustomGeneralLibraryStore _customGeneralStore = null!;
    private CustomGeneralLibrary _customGeneralLibrary = CustomGeneralLibrary.Empty;
    private CustomGeneralLibrary _activeCustomGenerals = CustomGeneralLibrary.Empty;
    private ContentRegistry _nextGameRegistry = null!;
    private string _customGeneralLibraryStatus = string.Empty;

    public string CustomGeneralLibraryStatus
    {
        get => _customGeneralLibraryStatus;
        private set => SetProperty(ref _customGeneralLibraryStatus, value);
    }

    private void InitializeCustomGenerals(ICustomGeneralLibraryStore? store, bool injectedContent)
    {
        _customGeneralStore = store ?? (injectedContent
            ? new MemoryCustomGeneralLibraryStore() : new FileCustomGeneralLibraryStore());
        _nextGameRegistry = _contentRegistry;
        try { ApplyCustomGeneralLibrary(_customGeneralStore.Read()); }
        catch (Exception exception) when (IsCustomGeneralException(exception))
        {
            // Keep the source library intact and make the unavailable content visible.
            CustomGeneralLibraryStatus = $"自定义武将库读取失败：{exception.Message}";
        }
    }

    public CustomGeneralEditorViewModel CreateCustomGeneralEditor() =>
        new(_contentRegistry, _customGeneralStore, ApplyCustomGeneralLibrary);

    private void ApplyCustomGeneralLibrary(CustomGeneralLibrary library)
    {
        var snapshot = CustomGeneralContent.Snapshot(library);
        var registry = CustomGeneralContent.CreateRegistry(_contentRegistry, snapshot);
        _customGeneralLibrary = snapshot;
        _nextGameRegistry = registry;
        var enabled = snapshot.Generals.Count(general => general.Enabled);
        CustomGeneralLibraryStatus = enabled == 0 ? "创建武将，挑选技能或组合自己的技能规则"
            : $"{enabled} 名自定义武将 · 下一局身份模式可选";
        if (OpenGeneralGalleryCommand is not null) RebuildGeneralGallery();
    }

    private static bool HasCustomGenerals(ContentRegistry? registry) =>
        registry?.Packages.Any(package => package.Id == CustomGeneralContent.PackageId) == true;

    private ContentRegistry RegistryForSave(GameSaveFile save)
    {
        var checkpoint = save.Checkpoint;
        if (checkpoint.ContentPackages is null) throw new InvalidDataException("存档的内容包信息不完整。");
        var customPackages = checkpoint.ContentPackages
            .Where(package => package.StartsWith(CustomGeneralContent.PackageId + "@", StringComparison.Ordinal)).ToArray();
        if (save.CustomGenerals is null)
        {
            if (customPackages.Length > 0) throw new InvalidDataException("存档缺少当局自定义武将内容。");
            return RegistryForCheckpoint(checkpoint);
        }
        if (customPackages.Length != 1) throw new InvalidDataException("存档的自定义内容与内容包不匹配。");
        var snapshot = CustomGeneralContent.Snapshot(save.CustomGenerals);
        var basePackages = checkpoint.ContentPackages.Except(customPackages, StringComparer.Ordinal).ToArray();
        var configuredPackages = _contentRegistry.Packages.Select(package => $"{package.Id}@{package.Version}");
        // This modified record only routes to the base-content factory. The original
        // checkpoint is subsequently restored with its complete packages and hash.
        var baseRegistry = basePackages.SequenceEqual(configuredPackages, StringComparer.Ordinal)
            ? _contentRegistry : RegistryForCheckpoint(checkpoint with { ContentPackages = basePackages });
        return CustomGeneralContent.CreateRegistry(baseRegistry, snapshot);
    }

    private static bool IsCustomGeneralException(Exception exception) => exception is
        IOException or UnauthorizedAccessException or System.Text.Json.JsonException or
        ArgumentException or InvalidOperationException or NotSupportedException;
}
