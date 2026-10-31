#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleTargetProtectionEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> _resolve;
    private CoreGameEngineSoleTargetProtectionEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleTargetProtectionEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleTargetProtectionRuntime> resolve) => new(resolve);
}
