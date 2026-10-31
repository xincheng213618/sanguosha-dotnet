#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetQuarantineEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> _resolve;
    private CoreGameEngineSoleTargetQuarantineEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetQuarantineEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetQuarantineRuntime> resolve) => new(resolve);
}
