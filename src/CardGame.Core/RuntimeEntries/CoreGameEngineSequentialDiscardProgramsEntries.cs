#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime))]
internal sealed partial class CoreGameEngineSequentialDiscardProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> _resolve;
    private CoreGameEngineSequentialDiscardProgramsEntries(Func<EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineSequentialDiscardProgramsEntries Create(Func<EngineOwners.CoreGameEngineSequentialDiscardProgramsRuntime> resolve) => new(resolve);
}
