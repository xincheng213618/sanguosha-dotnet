#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime))]
internal sealed partial class CoreMatchOutcomesEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime> _resolve;
    private CoreMatchOutcomesEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreMatchOutcomesEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreMatchOutcomesRuntime> resolve) => new(resolve);
}
