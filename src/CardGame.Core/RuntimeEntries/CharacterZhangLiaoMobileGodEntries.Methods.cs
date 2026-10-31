#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangLiaoMobileGodEntries
{
    internal void AnswerMobileShenZhangLiao(PromptChoice choice) => _resolve().AnswerMobileShenZhangLiao(choice);
    internal void AssertMobileZhitiContest(ResolutionFrame parent) => _resolve().AssertMobileZhitiContest(parent);
    internal SkillProgramStepOutcome BeginMobileShenZhangLiao(ProgramSkillFrame supplied, SkillProgramEffectOp op) => _resolve().BeginMobileShenZhangLiao(supplied, op);
    internal bool BeginMobileZhitiContest(ResolutionFrame parent, PindianResult result, MobileZhitiContestKind kind) => _resolve().BeginMobileZhitiContest(parent, result, kind);
    internal PromptChoice ChooseMobileShenZhangLiao(PendingDecision p, ProgramSkillFrame f) => _resolve().ChooseMobileShenZhangLiao(p, f);
    internal bool ContinueMobileZhitiContest(long id) => _resolve().ContinueMobileZhitiContest(id);
    internal bool DrainMobileShenZhangLiao(long id) => _resolve().DrainMobileShenZhangLiao(id);
    internal void ExpireMobileDuorui(IGameEvent fact) => _resolve().ExpireMobileDuorui(fact);
    internal void FinishMobileShenZhangLiao(ProgramSkillFrame f) => _resolve().FinishMobileShenZhangLiao(f);
    internal bool IsMobileDuoruiGrantQualified(CharacterState owner, SkillGrant grant) => _resolve().IsMobileDuoruiGrantQualified(owner, grant);
    internal bool IsMobileZhitiContestSubtree(ResolutionFrame parent) => _resolve().IsMobileZhitiContestSubtree(parent);
    internal bool MobileDuoruiActive(MobileDuoruiIssuedEvent e) => _resolve().MobileDuoruiActive(e);
    internal string[] MobileDuoruiSkills(int seat) => _resolve().MobileDuoruiSkills(seat);
    internal EquipmentSlot[] MobileDuoruiSlots(int seat) => _resolve().MobileDuoruiSlots(seat);
    internal IReadOnlyList<PromptChoice> MobileShenZhangLiaoChoices(ProgramSkillFrame f) => _resolve().MobileShenZhangLiaoChoices(f);
    internal bool MobileShenZhangLiaoEligible(ProgramTriggerLegalityContext g) => _resolve().MobileShenZhangLiaoEligible(g);
    internal bool MobileZhitiContestCandidateMatches(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().MobileZhitiContestCandidateMatches(candidate, context);
    internal ProgramSkillWindowContext MobileZhitiContestContext(ResolutionFrame parent, ProgramTriggerCandidate candidate) => _resolve().MobileZhitiContestContext(parent, candidate);
    internal bool MobileZhitiOpponent(int owner, int opponent) => _resolve().MobileZhitiOpponent(owner, opponent);
    internal bool MobileZhitiProgramRidesOn(ProgramSkillFrame f, ResolutionFrame parent) => _resolve().MobileZhitiProgramRidesOn(f, parent);
    internal EquipmentSlot[] MobileZhitiSlots(int seat) => _resolve().MobileZhitiSlots(seat);
    internal void PublishMobileShenZhangLiao(ProgramSkillFrame f) => _resolve().PublishMobileShenZhangLiao(f);
    internal bool ResumeMobileShenZhangLiao(long id) => _resolve().ResumeMobileShenZhangLiao(id);
    internal bool RetainMobileShenZhangLiao(ProgramSkillFrame f) => _resolve().RetainMobileShenZhangLiao(f);
    internal bool ReturnMobileShenZhangLiaoMovement(ProgramSkillFrame f) => _resolve().ReturnMobileShenZhangLiaoMovement(f);
    internal bool ReturnMobileZhitiContestBinding(ProgramSkillFrame f, bool completed) => _resolve().ReturnMobileZhitiContestBinding(f, completed);
    internal bool TryBeginMobileZhitiDuelPrograms(NativeOwners.CardAttackHandle attack) => _resolve().TryBeginMobileZhitiDuelPrograms(attack);
    internal bool TryBeginMobileZhitiPindianPrograms(PindianFrame parent) => _resolve().TryBeginMobileZhitiPindianPrograms(parent);
    internal bool TryBeginMobileZhitiSimultaneousPrograms(ProgramSkillFrame parent, PindianResult result) => _resolve().TryBeginMobileZhitiSimultaneousPrograms(parent, result);
    internal bool TryGetMobileZhitiContestTrigger(scoped out ProgramTriggerCandidate candidate, scoped out ProgramSkillWindowContext context) => _resolve().TryGetMobileZhitiContestTrigger(out candidate, out context);
    internal bool TrySkipMobileZhitiContestTrigger(ProgramTriggerCandidate candidate) => _resolve().TrySkipMobileZhitiContestTrigger(candidate);
    internal bool HasMobileShenZhangLiao { get => _resolve().HasMobileShenZhangLiao; }
    internal IEnumerable<MobileDuoruiIssuedEvent> MobileDuoruiLeases { get => _resolve().MobileDuoruiLeases; }
}
