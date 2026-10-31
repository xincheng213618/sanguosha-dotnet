#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class RandomPhaseSkillCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineSharedPhaseSkillsRuntime>> getCoreGameEngineSharedPhaseSkillsRuntime) : IRandomPhaseSkillHost
{
    public SkillProgramStepOutcome Offer(ProgramSkillFrame frame, SkillProgramEffect effect, int recipientSeat) => getCoreGameEngineSharedPhaseSkillsRuntime().Value.OfferRandomPhaseSkill(frame, effect, recipientSeat);
}
