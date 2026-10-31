#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeSlashTargetDuelEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> _resolve;
    private CoreNativeSlashTargetDuelEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeSlashTargetDuelEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeSlashTargetDuelRuntime> resolve) => new(resolve);
}
