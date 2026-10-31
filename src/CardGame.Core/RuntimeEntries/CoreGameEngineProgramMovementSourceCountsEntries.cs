#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramMovementSourceCountsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> _resolve;
    private CoreGameEngineProgramMovementSourceCountsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramMovementSourceCountsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramMovementSourceCountsRuntime> resolve) => new(resolve);
}
