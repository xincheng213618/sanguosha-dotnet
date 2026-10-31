#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class RandomPhaseSkillCapability(Func<RuntimeEntries.CoreGameEngineSharedPhaseSkillsEntries> getCoreGameEngineSharedPhaseSkillsRuntime) : IRandomPhaseSkillHost
{
    public SkillProgramStepOutcome Offer(ProgramSkillFrame frame, SkillProgramEffect effect, int recipientSeat) => getCoreGameEngineSharedPhaseSkillsRuntime().OfferRandomPhaseSkill(frame, effect, recipientSeat);
}
