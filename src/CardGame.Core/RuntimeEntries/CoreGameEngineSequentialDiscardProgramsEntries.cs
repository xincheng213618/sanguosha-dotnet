#nullable enable
namespace CardGame.Core.RuntimeEntries;
[global::CardGame.Core.ProgramRuntimeEntry(typeof(global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime))]
internal sealed partial class CoreGameEngineSequentialDiscardProgramsEntries
{
    private readonly global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> _resolve;
    private CoreGameEngineSequentialDiscardProgramsEntries(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialDiscardProgramsEntries Create(global::System.Func<global::CardGame.Core.EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> resolve) => new(resolve);
}
