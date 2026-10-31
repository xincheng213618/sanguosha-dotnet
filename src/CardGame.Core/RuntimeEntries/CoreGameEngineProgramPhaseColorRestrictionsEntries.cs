#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramPhaseColorRestrictionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime> _resolve;
    private CoreGameEngineProgramPhaseColorRestrictionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramPhaseColorRestrictionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramPhaseColorRestrictionsRuntime> resolve) => new(resolve);
}
