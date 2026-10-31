#nullable enable
namespace CardGame.Core.RuntimeEntries;
[Core.ProgramRuntimeEntry(typeof(EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime))]
internal sealed partial class CoreGameEngineDamagePreventionProgramsEntries
{
    private readonly Func<EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> _resolve;
    private CoreGameEngineDamagePreventionProgramsEntries(Func<EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> resolve)
    {
        _resolve = resolve;
    }

    internal static CoreGameEngineDamagePreventionProgramsEntries Create(Func<EngineOwners.CoreGameEngineDamagePreventionProgramsRuntime> resolve) => new(resolve);
}
