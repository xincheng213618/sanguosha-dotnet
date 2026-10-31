#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime))]
internal sealed partial class CoreNativeFactionDefenseEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> _resolve;
    private CoreNativeFactionDefenseEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreNativeFactionDefenseEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreNativeFactionDefenseRuntime> resolve) => new(resolve);
}
