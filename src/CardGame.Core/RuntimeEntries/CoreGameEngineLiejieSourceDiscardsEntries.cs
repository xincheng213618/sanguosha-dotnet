#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime))]
internal sealed partial class CoreGameEngineLiejieSourceDiscardsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> _resolve;
    private CoreGameEngineLiejieSourceDiscardsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineLiejieSourceDiscardsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineLiejieSourceDiscardsRuntime> resolve) => new(resolve);
}
