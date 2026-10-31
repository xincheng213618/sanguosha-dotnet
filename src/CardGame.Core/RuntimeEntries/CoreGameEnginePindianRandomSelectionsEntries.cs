#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianRandomSelectionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> _resolve;
    private CoreGameEnginePindianRandomSelectionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianRandomSelectionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> resolve) => new(resolve);
}
