#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramOwnedCardDistributionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> _resolve;
    private CoreGameEngineProgramOwnedCardDistributionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramOwnedCardDistributionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime> resolve) => new(resolve);
}
