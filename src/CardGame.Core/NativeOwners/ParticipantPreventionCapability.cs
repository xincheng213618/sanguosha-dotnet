#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ParticipantPreventionCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineParticipantPreventionRuntime>> getCoreGameEngineParticipantPreventionRuntime) : IParticipantPreventionHost
{
    public SkillProgramStepOutcome PreventParticipantDamageDrawActualSource(ProgramSkillFrame f) => getCoreGameEngineParticipantPreventionRuntime().Value.PreventParticipantDamageDrawActualSource(f);
}
