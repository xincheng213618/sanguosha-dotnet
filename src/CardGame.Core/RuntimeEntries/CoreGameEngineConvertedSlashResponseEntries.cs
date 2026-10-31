#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineConvertedSlashResponseRuntime))]
internal sealed partial class CoreGameEngineConvertedSlashResponseEntries
{
    private readonly Func<EngineOwners.CoreGameEngineConvertedSlashResponseRuntime> _resolve;
    private CoreGameEngineConvertedSlashResponseEntries(Func<EngineOwners.CoreGameEngineConvertedSlashResponseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineConvertedSlashResponseEntries Create(Func<EngineOwners.CoreGameEngineConvertedSlashResponseRuntime> resolve) => new(resolve);
}
