#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineDamageSourceHandChoiceEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> _resolve;
    private CoreGameEngineDamageSourceHandChoiceEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageSourceHandChoiceEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineDamageSourceHandChoiceRuntime> resolve) => new(resolve);
}
