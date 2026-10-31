#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime))]
internal sealed partial class CoreGameEngineNonBasicSlashProfileEntries
{
    private readonly Func<EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> _resolve;
    private CoreGameEngineNonBasicSlashProfileEntries(Func<EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineNonBasicSlashProfileEntries Create(Func<EngineOwners.CoreGameEngineNonBasicSlashProfileRuntime> resolve) => new(resolve);
}
