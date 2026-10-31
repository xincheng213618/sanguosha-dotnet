#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePaidVirtualSlashTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime> _resolve;
    private CoreGameEnginePaidVirtualSlashTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePaidVirtualSlashTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePaidVirtualSlashTargetsRuntime> resolve) => new(resolve);
}
