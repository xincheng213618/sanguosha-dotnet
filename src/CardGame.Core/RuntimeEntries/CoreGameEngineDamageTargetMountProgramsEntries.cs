#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime))]
internal sealed partial class CoreGameEngineDamageTargetMountProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime> _resolve;
    private CoreGameEngineDamageTargetMountProgramsEntries(Func<EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetMountProgramsEntries Create(Func<EngineOwners.CoreGameEngineDamageTargetMountProgramsRuntime> resolve) => new(resolve);
}
