#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineBoundRankBonusProgramsEntries
{
    internal bool CanRunOtherDyingVictimRecovery(global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat) => _resolve().CanRunOtherDyingVictimRecovery(trigger, context, ownerSeat);
    internal global::CardGame.Core.SkillProgramStepOutcome ObtainAndArmNextRevealBonus(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ObtainAndArmNextRevealBonus(frame, effect);
    internal void RecoverOtherDyingVictim(global::CardGame.Core.ProgramSkillFrame frame, int hp) => _resolve().RecoverOtherDyingVictim(frame, hp);
    internal void RevealWithNextBooleanBonus(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect effect) => _resolve().RevealWithNextBooleanBonus(frame, effect);
    internal bool TryOtherDyingVictim(global::CardGame.Core.ProgramSkillWindowContext context, int ownerSeat, scoped out global::CardGame.Core.DyingFrame dying) => _resolve().TryOtherDyingVictim(context, ownerSeat, out dying);
}
