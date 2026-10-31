#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime))]
internal sealed partial class CoreGameEngineInspectedHandSlashReturnsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime> _resolve;
    private CoreGameEngineInspectedHandSlashReturnsEntries(Func<EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInspectedHandSlashReturnsEntries Create(Func<EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime> resolve) => new(resolve);
}
