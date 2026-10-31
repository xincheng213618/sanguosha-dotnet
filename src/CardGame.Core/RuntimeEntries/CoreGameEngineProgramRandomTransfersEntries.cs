#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramRandomTransfersEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRandomTransfersRuntime> _resolve;
    private CoreGameEngineProgramRandomTransfersEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRandomTransfersRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRandomTransfersEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRandomTransfersRuntime> resolve) => new(resolve);
}
