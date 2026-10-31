#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime))]
internal sealed partial class CoreGameEngineSimultaneousPindianSelectionsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime> _resolve;
    private CoreGameEngineSimultaneousPindianSelectionsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSimultaneousPindianSelectionsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianSelectionsRuntime> resolve) => new(resolve);
}
