#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramDodgeResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> _resolve;
    private CoreGameEngineProgramDodgeResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramDodgeResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramDodgeResponsesRuntime> resolve) => new(resolve);
}
