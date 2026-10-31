#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineRoundFirstSlashTargetsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundFirstSlashTargetsRuntime> _resolve;
    private CoreGameEngineRoundFirstSlashTargetsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundFirstSlashTargetsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineRoundFirstSlashTargetsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineRoundFirstSlashTargetsRuntime> resolve) => new(resolve);
}
