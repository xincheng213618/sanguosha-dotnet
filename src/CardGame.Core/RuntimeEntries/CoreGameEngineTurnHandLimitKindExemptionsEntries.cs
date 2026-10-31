#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandLimitKindExemptionsRuntime))]
internal sealed partial class CoreGameEngineTurnHandLimitKindExemptionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandLimitKindExemptionsRuntime> _resolve;
    private CoreGameEngineTurnHandLimitKindExemptionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandLimitKindExemptionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineTurnHandLimitKindExemptionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineTurnHandLimitKindExemptionsRuntime> resolve) => new(resolve);
}
