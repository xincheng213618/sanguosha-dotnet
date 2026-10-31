#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class RandomPhaseSkillCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineSharedPhaseSkillsEntries> getCoreGameEngineSharedPhaseSkillsRuntime) : IRandomPhaseSkillHost
{
    public SkillProgramStepOutcome Offer(ProgramSkillFrame frame, SkillProgramEffect effect, int recipientSeat) => getCoreGameEngineSharedPhaseSkillsRuntime().OfferRandomPhaseSkill(frame, effect, recipientSeat);
}
