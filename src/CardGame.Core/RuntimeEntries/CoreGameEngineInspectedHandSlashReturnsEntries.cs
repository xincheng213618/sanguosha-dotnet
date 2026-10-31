#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineInspectedHandSlashReturnsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime> _resolve;
    private CoreGameEngineInspectedHandSlashReturnsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineInspectedHandSlashReturnsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineInspectedHandSlashReturnsRuntime> resolve) => new(resolve);
}
