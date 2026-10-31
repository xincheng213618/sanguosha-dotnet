#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineCardMovementProgramsRuntime))]
internal sealed partial class CoreGameEngineCardMovementProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineCardMovementProgramsRuntime> _resolve;
    private CoreGameEngineCardMovementProgramsEntries(Func<EngineOwners.CoreGameEngineCardMovementProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineCardMovementProgramsEntries Create(Func<EngineOwners.CoreGameEngineCardMovementProgramsRuntime> resolve) => new(resolve);
}
