#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ParticipantPreventionCapability(Func<RuntimeEntries.CoreGameEngineParticipantPreventionEntries> getCoreGameEngineParticipantPreventionRuntime) : IParticipantPreventionHost
{
    public SkillProgramStepOutcome PreventParticipantDamageDrawActualSource(ProgramSkillFrame f) => getCoreGameEngineParticipantPreventionRuntime().PreventParticipantDamageDrawActualSource(f);
}
