#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLingOlEntries
{
    internal bool CanOfferCategoryTalismanRelease(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferCategoryTalismanRelease(candidate, trigger, context);
    internal bool CanReleaseCategoryTalisman(int holder, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanReleaseCategoryTalisman(holder, context);
    internal bool ContinueIssuedZhangLingBinding(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ContinueIssuedZhangLingBinding(frame);
    internal bool IsCategoryTalismanMovement(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation pending) => _resolve().IsCategoryTalismanMovement(frame, effect, pending);
    internal bool IsCategoryTalismanUseForbidden(int actorSeat, global::CardGame.Core.CardKind effectiveKind, global::CardGame.Core.CardActionType type) => _resolve().IsCategoryTalismanUseForbidden(actorSeat, effectiveKind, type);
    internal global::CardGame.Core.SkillProgramStepOutcome IssueHuqiSlash(global::CardGame.Core.ProgramSkillFrame frame, string bind) => _resolve().IssueHuqiSlash(frame, bind);
    internal global::CardGame.Core.CategoryTalismanPlacedEvent? LiveCategoryTalisman(int seat) => _resolve().LiveCategoryTalisman(seat);
    internal void ObserveZhangLingNativeFacts(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveZhangLingNativeFacts(payload);
    internal global::CardGame.Core.SkillProgramStepOutcome PlaceCategoryTalisman(global::CardGame.Core.ProgramSkillFrame supplied, string bind) => _resolve().PlaceCategoryTalisman(supplied, bind);
    internal global::CardGame.Core.SkillProgramStepOutcome ReleaseCategoryTalisman(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ReleaseCategoryTalisman(supplied);
    internal bool RestoreHuqiSuspendedAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion) => _resolve().RestoreHuqiSuspendedAttack(completion);
    internal bool ResumeCategoryTalismanMovement(long id) => _resolve().ResumeCategoryTalismanMovement(id);
    internal bool ReturnCategoryTalismanMovement(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().ReturnCategoryTalismanMovement(frame);
    internal void SynchronizeCategoryTalismanGrants() => _resolve().SynchronizeCategoryTalismanGrants();
    internal global::CardGame.Core.SkillProgramStepOutcome UseHuqiSlash(global::CardGame.Core.ProgramSkillFrame supplied, string bind) => _resolve().UseHuqiSlash(supplied, bind);
    internal bool TracksCategoryTalismans { get => _resolve().TracksCategoryTalismans; }
}
