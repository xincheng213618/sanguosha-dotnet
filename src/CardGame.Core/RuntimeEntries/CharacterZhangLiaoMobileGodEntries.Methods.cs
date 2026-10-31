#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLiaoMobileGodEntries
{
    internal void AnswerMobileShenZhangLiao(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMobileShenZhangLiao(choice);
    internal void AssertMobileShenZhangLiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMobileShenZhangLiao(f);
    internal void AssertMobileZhitiContest(global::CardGame.Core.ResolutionFrame parent) => _resolve().AssertMobileZhitiContest(parent);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMobileShenZhangLiao(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffectOp op) => _resolve().BeginMobileShenZhangLiao(supplied, op);
    internal bool BeginMobileZhitiContest(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.PindianResult result, global::CardGame.Core.MobileZhitiContestKind kind) => _resolve().BeginMobileZhitiContest(parent, result, kind);
    internal global::CardGame.Core.PromptChoice ChooseMobileShenZhangLiao(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().ChooseMobileShenZhangLiao(p, f);
    internal bool ContinueMobileZhitiContest(long id) => _resolve().ContinueMobileZhitiContest(id);
    internal bool DrainMobileShenZhangLiao(long id) => _resolve().DrainMobileShenZhangLiao(id);
    internal void ExpireMobileDuorui(global::CardGame.Core.IGameEvent fact) => _resolve().ExpireMobileDuorui(fact);
    internal void FinishMobileShenZhangLiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishMobileShenZhangLiao(f);
    internal bool HasMobileZhitiContestPrompt() => _resolve().HasMobileZhitiContestPrompt();
    internal bool HasMobileZhitiDuelContinuation(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().HasMobileZhitiDuelContinuation(attack);
    internal bool IsMobileDuoruiGrantQualified(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillGrant grant) => _resolve().IsMobileDuoruiGrantQualified(owner, grant);
    internal bool IsMobileZhitiContestSubtree(global::CardGame.Core.ResolutionFrame parent) => _resolve().IsMobileZhitiContestSubtree(parent);
    internal bool MobileDuoruiActive(global::CardGame.Core.MobileDuoruiIssuedEvent e) => _resolve().MobileDuoruiActive(e);
    internal string[] MobileDuoruiSkills(int seat) => _resolve().MobileDuoruiSkills(seat);
    internal global::CardGame.Core.EquipmentSlot[] MobileDuoruiSlots(int seat) => _resolve().MobileDuoruiSlots(seat);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MobileShenZhangLiaoChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MobileShenZhangLiaoChoices(f);
    internal bool MobileShenZhangLiaoEligible(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().MobileShenZhangLiaoEligible(g);
    internal bool MobileZhitiContestCandidateMatches(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().MobileZhitiContestCandidateMatches(candidate, context);
    internal global::CardGame.Core.ProgramSkillWindowContext MobileZhitiContestContext(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().MobileZhitiContestContext(parent, candidate);
    internal bool MobileZhitiOpponent(int owner, int opponent) => _resolve().MobileZhitiOpponent(owner, opponent);
    internal bool MobileZhitiProgramRidesOn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame parent) => _resolve().MobileZhitiProgramRidesOn(f, parent);
    internal global::CardGame.Core.EquipmentSlot[] MobileZhitiSlots(int seat) => _resolve().MobileZhitiSlots(seat);
    internal void PublishMobileShenZhangLiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMobileShenZhangLiao(f);
    internal bool ResumeMobileShenZhangLiao(long id) => _resolve().ResumeMobileShenZhangLiao(id);
    internal bool RetainMobileShenZhangLiao(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainMobileShenZhangLiao(f);
    internal bool ReturnMobileShenZhangLiaoMovement(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ReturnMobileShenZhangLiaoMovement(f);
    internal bool ReturnMobileZhitiContestBinding(global::CardGame.Core.ProgramSkillFrame f, bool completed) => _resolve().ReturnMobileZhitiContestBinding(f, completed);
    internal bool TryBeginMobileZhitiDuelPrograms(global::CardGame.Core.NativeOwners.CardAttackHandle attack) => _resolve().TryBeginMobileZhitiDuelPrograms(attack);
    internal bool TryBeginMobileZhitiPindianPrograms(global::CardGame.Core.PindianFrame parent) => _resolve().TryBeginMobileZhitiPindianPrograms(parent);
    internal bool TryBeginMobileZhitiSimultaneousPrograms(global::CardGame.Core.ProgramSkillFrame parent, global::CardGame.Core.PindianResult result) => _resolve().TryBeginMobileZhitiSimultaneousPrograms(parent, result);
    internal bool TryGetMobileZhitiContestTrigger(scoped out global::CardGame.Core.ProgramTriggerCandidate candidate, scoped out global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().TryGetMobileZhitiContestTrigger(out candidate, out context);
    internal bool TrySkipMobileZhitiContestTrigger(global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TrySkipMobileZhitiContestTrigger(candidate);
    internal bool HasMobileShenZhangLiao { get => _resolve().HasMobileShenZhangLiao; }
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.MobileDuoruiIssuedEvent> MobileDuoruiLeases { get => _resolve().MobileDuoruiLeases; }
}
