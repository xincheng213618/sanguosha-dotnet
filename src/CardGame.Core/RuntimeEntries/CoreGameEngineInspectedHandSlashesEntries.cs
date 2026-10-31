#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> _resolve;
    private CoreGameEngineInspectedHandSlashesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInspectedHandSlashesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashesRuntime> resolve) => new(resolve);
}
