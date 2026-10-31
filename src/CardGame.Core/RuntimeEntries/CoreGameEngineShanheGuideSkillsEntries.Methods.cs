#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuideSkillsEntries
{
    internal bool CanOfferShanheBuwu(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferShanheBuwu(candidate, trigger, context);
    internal bool IsShanheFriendlySeat(int owner, int seat) => _resolve().IsShanheFriendlySeat(owner, seat);
    internal SkillProgramStepOutcome ResolveShanheGuideSkill(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ResolveShanheGuideSkill(supplied, effect);
    internal bool ResumeShanheBuwuDraw(long frameId) => _resolve().ResumeShanheBuwuDraw(frameId);
    internal void ReturnShanheBuwuDraw(ProgramSkillFrame supplied, NativeDrawOutcome outcome) => _resolve().ReturnShanheBuwuDraw(supplied, outcome);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> ShanheBuwuDamageBonuses(GameEngine.IDamageAttempt attack) => _resolve().ShanheBuwuDamageBonuses(attack);
}
