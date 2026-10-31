#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime))]
internal sealed partial class CoreGameEngineActualUseTargetProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> _resolve;
    private CoreGameEngineActualUseTargetProgramsEntries(Func<EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineActualUseTargetProgramsEntries Create(Func<EngineOwners.CoreGameEngineActualUseTargetProgramsRuntime> resolve) => new(resolve);
}
