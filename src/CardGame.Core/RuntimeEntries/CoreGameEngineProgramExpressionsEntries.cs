#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineProgramExpressionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramExpressionsRuntime> _resolve;
    private CoreGameEngineProgramExpressionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramExpressionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramExpressionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramExpressionsRuntime> resolve) => new(resolve);
}
