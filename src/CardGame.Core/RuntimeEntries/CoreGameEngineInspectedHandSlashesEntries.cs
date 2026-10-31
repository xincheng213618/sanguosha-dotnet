#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime))]
internal sealed partial class CoreGameEngineInspectedHandSlashesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> _resolve;
    private CoreGameEngineInspectedHandSlashesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInspectedHandSlashesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> resolve) => new(resolve);
}
