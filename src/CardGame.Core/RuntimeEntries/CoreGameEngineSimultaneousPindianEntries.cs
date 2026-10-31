#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianRuntime))]
internal sealed partial class CoreGameEngineSimultaneousPindianEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianRuntime> _resolve;
    private CoreGameEngineSimultaneousPindianEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSimultaneousPindianEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSimultaneousPindianRuntime> resolve) => new(resolve);
}
