#nullable enable

namespace CardGame.Core.EngineOwners;

[Core.ProgramCapability(typeof(ProgramSkillChangeRuntime))]
internal sealed class CoreProgramSkillChangesProgramSkillChangeAdapter(Func<ProgramSkillChangeRuntime> targetFactory) : ICoreProgramSkillChangesProgramSkillChangeOperations
{
    private readonly Func<ProgramSkillChangeRuntime> _targetFactory = targetFactory;
    public ProgramSkillChangeRuntime Get() => this._targetFactory();
}
