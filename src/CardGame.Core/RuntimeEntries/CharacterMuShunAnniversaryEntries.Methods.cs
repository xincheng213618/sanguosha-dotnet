#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMuShunAnniversaryEntries
{
    internal void AfterMuShunChain(GameEngine.IDamageAttempt attack) => _resolve().AfterMuShunChain(attack);
    internal void AnswerMuShun(PromptChoice choice) => _resolve().AnswerMuShun(choice);
    internal void BeforeMuShunChain(GameEngine.IDamageAttempt attack) => _resolve().BeforeMuShunChain(attack);
    internal SkillProgramStepOutcome BeginMuShunJinjian(ProgramSkillFrame supplied) => _resolve().BeginMuShunJinjian(supplied);
    internal bool CanMuShunContest(ProgramSkillFrame f) => _resolve().CanMuShunContest(f);
    internal void CloseMuShunWithoutContest(ProgramSkillFrame f) => _resolve().CloseMuShunWithoutContest(f);
    internal void ConsumeMuShunDamage(IGameEvent payload) => _resolve().ConsumeMuShunDamage(payload);
    internal bool ContinueIssuedMuShun(ProgramSkillFrame f) => _resolve().ContinueIssuedMuShun(f);
    internal bool IsMuShunPindianSettlementBatch(ProgramSkillFrame f, CardMovementBatchContext batch) => _resolve().IsMuShunPindianSettlementBatch(f, batch);
    internal PromptChoice MuShunAi(PendingDecision p, ProgramSkillFrame f) => _resolve().MuShunAi(p, f);
    internal ProgramTriggerCandidate MuShunCandidate(ProgramSkillFrame f) => _resolve().MuShunCandidate(f);
    internal int MuShunChainCursor(GameEngine.IDamageAttempt a) => _resolve().MuShunChainCursor(a);
    internal IReadOnlyList<PromptChoice> MuShunChoices(ProgramSkillFrame f) => _resolve().MuShunChoices(f);
    internal MuShunShizhaoIssuedEvent? MuShunDamageArm(GameEngine.IDamageAttempt attack) => _resolve().MuShunDamageArm(attack);
    internal MuShunDamageReceipt? MuShunDamageState(long id) => _resolve().MuShunDamageState(id);
    internal int MuShunJin(int seat) => _resolve().MuShunJin(seat);
    internal bool MuShunLossCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext c, bool issued) => _resolve().MuShunLossCandidate(candidate, c, issued);
    internal IEnumerable<(CardUseEffectSource Source, int Amount)> MuShunNextReceivedDamage(GameEngine.IDamageAttempt attack) => _resolve().MuShunNextReceivedDamage(attack);
    internal bool MuShunPindianStructuralEdge(ResolutionFrame child, ResolutionFrame parent) => _resolve().MuShunPindianStructuralEdge(child, parent);
    internal void MuShunRange(CharacterState actor, List<RuleQueryContribution> contributions) => _resolve().MuShunRange(actor, contributions);
    internal void PublishMuShun(ProgramSkillFrame f) => _resolve().PublishMuShun(f);
    internal bool QualifyMuShun(ProgramTriggerLegalityContext g) => _resolve().QualifyMuShun(g);
    internal SkillProgramStepOutcome ResolveMuShunShizhao(ProgramSkillFrame supplied) => _resolve().ResolveMuShunShizhao(supplied);
    internal bool ResumeMuShunContest(long id) => _resolve().ResumeMuShunContest(id);
    internal void SetMuShunDamageState(long id, MuShunDamageReceipt? r) => _resolve().SetMuShunDamageState(id, r);
    internal void SetMuShunJin(ProgramSkillFrame f, int count) => _resolve().SetMuShunJin(f, count);
    internal bool ValidMuShunJinjian(ProgramSkillFrame f) => _resolve().ValidMuShunJinjian(f);
    internal bool WindowSettlement_IsMuShunPindianSettlementBatch(ProgramSkillFrame frame, CardMovementBatchContext batch) => _resolve().WindowSettlement_IsMuShunPindianSettlementBatch(frame, batch);
    internal bool HasMuShunCapability { get => _resolve().HasMuShunCapability; }
    internal bool HasMuShunShizhaoCapability { get => _resolve().HasMuShunShizhaoCapability; }
}
