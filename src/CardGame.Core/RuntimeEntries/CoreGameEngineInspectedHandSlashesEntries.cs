#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineInspectedHandSlashesRuntime))]
internal sealed partial class CoreGameEngineInspectedHandSlashesEntries
{
    private readonly Func<EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> _resolve;
    private CoreGameEngineInspectedHandSlashesEntries(Func<EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInspectedHandSlashesEntries Create(Func<EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> resolve) => new(resolve);
}
