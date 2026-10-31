#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundRankBonusProgramsEntries
{
    internal bool CanRunOtherDyingVictimRecovery(SkillProgramTrigger trigger, ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanRunOtherDyingVictimRecovery(trigger, context, ownerSeat);
    internal SkillProgramStepOutcome ObtainAndArmNextRevealBonus(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().ObtainAndArmNextRevealBonus(frame, effect);
    internal void RecoverOtherDyingVictim(ProgramSkillFrame frame, int hp) => _resolve().RecoverOtherDyingVictim(frame, hp);
    internal void RevealWithNextBooleanBonus(ProgramSkillFrame frame, SkillProgramEffect effect) => _resolve().RevealWithNextBooleanBonus(frame, effect);
    internal bool TryOtherDyingVictim(ProgramSkillWindowContext context, int ownerSeat, scoped out DyingFrame dying) => _resolve().TryOtherDyingVictim(context, ownerSeat, out dying);
}
