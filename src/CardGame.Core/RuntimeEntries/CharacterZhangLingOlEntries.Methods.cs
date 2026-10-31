#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLingOlEntries
{
    internal bool CanOfferCategoryTalismanRelease(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferCategoryTalismanRelease(candidate, trigger, context);
    internal bool CanReleaseCategoryTalisman(int holder, ProgramSkillWindowContext context) => _resolve().CanReleaseCategoryTalisman(holder, context);
    internal bool ContinueIssuedZhangLingBinding(ProgramSkillFrame frame) => _resolve().ContinueIssuedZhangLingBinding(frame);
    internal bool IsCategoryTalismanUseForbidden(int actorSeat, CardKind effectiveKind, CardActionType type) => _resolve().IsCategoryTalismanUseForbidden(actorSeat, effectiveKind, type);
    internal SkillProgramStepOutcome IssueHuqiSlash(ProgramSkillFrame frame, string bind) => _resolve().IssueHuqiSlash(frame, bind);
    internal CategoryTalismanPlacedEvent? LiveCategoryTalisman(int seat) => _resolve().LiveCategoryTalisman(seat);
    internal void ObserveZhangLingNativeFacts(IGameEvent payload) => _resolve().ObserveZhangLingNativeFacts(payload);
    internal SkillProgramStepOutcome PlaceCategoryTalisman(ProgramSkillFrame supplied, string bind) => _resolve().PlaceCategoryTalisman(supplied, bind);
    internal SkillProgramStepOutcome ReleaseCategoryTalisman(ProgramSkillFrame supplied) => _resolve().ReleaseCategoryTalisman(supplied);
    internal bool RestoreHuqiSuspendedAttack(GameEngine.AttackCompletionReceipt completion) => _resolve().RestoreHuqiSuspendedAttack(completion);
    internal bool ResumeCategoryTalismanMovement(long id) => _resolve().ResumeCategoryTalismanMovement(id);
    internal bool ReturnCategoryTalismanMovement(ProgramSkillFrame frame) => _resolve().ReturnCategoryTalismanMovement(frame);
    internal void SynchronizeCategoryTalismanGrants() => _resolve().SynchronizeCategoryTalismanGrants();
    internal SkillProgramStepOutcome UseHuqiSlash(ProgramSkillFrame supplied, string bind) => _resolve().UseHuqiSlash(supplied, bind);
    internal bool TracksCategoryTalismans { get => _resolve().TracksCategoryTalismans; }
}
