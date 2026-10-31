#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineCardRecoveryModifiersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime> _resolve;
    private CoreGameEngineCardRecoveryModifiersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardRecoveryModifiersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineCardRecoveryModifiersRuntime> resolve) => new(resolve);
}
