#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePairedColorResponsesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime> _resolve;
    private CoreGameEnginePairedColorResponsesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePairedColorResponsesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePairedColorResponsesRuntime> resolve) => new(resolve);
}
