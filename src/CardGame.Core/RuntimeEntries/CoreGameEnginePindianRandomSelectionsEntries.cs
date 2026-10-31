#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime))]
internal sealed partial class CoreGameEnginePindianRandomSelectionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> _resolve;
    private CoreGameEnginePindianRandomSelectionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEnginePindianRandomSelectionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEnginePindianRandomSelectionsRuntime> resolve) => new(resolve);
}
