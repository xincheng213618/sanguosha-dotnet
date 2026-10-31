#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime))]
internal sealed partial class CoreGameEngineDamageTargetObtainProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> _resolve;
    private CoreGameEngineDamageTargetObtainProgramsEntries(Func<EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamageTargetObtainProgramsEntries Create(Func<EngineOwners.CoreGameEngineDamageTargetObtainProgramsRuntime> resolve) => new(resolve);
}
