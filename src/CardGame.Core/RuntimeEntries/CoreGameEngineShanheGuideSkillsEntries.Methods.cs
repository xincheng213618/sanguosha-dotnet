#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheGuideSkillsEntries
{
    internal bool CanOfferShanheBuwu(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferShanheBuwu(candidate, trigger, context);
    internal bool IsShanheBuwuDrawMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation movement) => _resolve().IsShanheBuwuDrawMovement(frame, effect, movement);
    internal bool IsShanheFriendlySeat(int owner, int seat) => _resolve().IsShanheFriendlySeat(owner, seat);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveShanheGuideSkill(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ResolveShanheGuideSkill(supplied, effect);
    internal bool ResumeShanheBuwuDraw(long frameId) => _resolve().ResumeShanheBuwuDraw(frameId);
    internal void ReturnShanheBuwuDraw(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnShanheBuwuDraw(supplied, outcome);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> ShanheBuwuDamageBonuses(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().ShanheBuwuDamageBonuses(attack);
}
