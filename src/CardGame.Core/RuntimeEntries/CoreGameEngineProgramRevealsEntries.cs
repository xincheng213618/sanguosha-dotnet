#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime))]
internal sealed partial class CoreGameEngineProgramRevealsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime> _resolve;
    private CoreGameEngineProgramRevealsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineProgramRevealsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineProgramRevealsRuntime> resolve) => new(resolve);
}
