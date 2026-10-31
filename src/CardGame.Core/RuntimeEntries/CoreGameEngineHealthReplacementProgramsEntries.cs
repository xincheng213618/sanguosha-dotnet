#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime))]
internal sealed partial class CoreGameEngineHealthReplacementProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime> _resolve;
    private CoreGameEngineHealthReplacementProgramsEntries(Func<EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineHealthReplacementProgramsEntries Create(Func<EngineOwners.CoreGameEngineHealthReplacementProgramsRuntime> resolve) => new(resolve);
}
