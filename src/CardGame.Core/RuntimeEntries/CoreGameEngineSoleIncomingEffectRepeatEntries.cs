#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineSoleIncomingEffectRepeatEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> _resolve;
    private CoreGameEngineSoleIncomingEffectRepeatEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSoleIncomingEffectRepeatEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSoleIncomingEffectRepeatRuntime> resolve) => new(resolve);
}
