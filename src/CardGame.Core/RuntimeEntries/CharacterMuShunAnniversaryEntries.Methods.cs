#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterMuShunAnniversaryEntries
{
    internal void AfterMuShunChain(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().AfterMuShunChain(attack);
    internal void AnswerMuShun(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerMuShun(choice);
    internal void AssertMuShun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertMuShun(f);
    internal void BeforeMuShunChain(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().BeforeMuShunChain(attack);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginMuShunJinjian(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().BeginMuShunJinjian(supplied);
    internal bool CanMuShunContest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanMuShunContest(f);
    internal void CloseMuShunWithoutContest(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CloseMuShunWithoutContest(f);
    internal void ConsumeMuShunDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ConsumeMuShunDamage(payload);
    internal bool ContinueIssuedMuShun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedMuShun(f);
    internal bool HasAnniversaryMuShunPrompt() => _resolve().HasAnniversaryMuShunPrompt();
    internal bool IsMuShunPindianSettlementBatch(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().IsMuShunPindianSettlementBatch(f, batch);
    internal global::CardGame.Core.PromptChoice MuShunAi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MuShunAi(p, f);
    internal global::CardGame.Core.ProgramTriggerCandidate MuShunCandidate(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MuShunCandidate(f);
    internal int MuShunChainCursor(global::CardGame.Core.GameEngine.IDamageAttempt a) => _resolve().MuShunChainCursor(a);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> MuShunChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MuShunChoices(f);
    internal global::CardGame.Core.MuShunShizhaoIssuedEvent? MuShunDamageArm(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MuShunDamageArm(attack);
    internal global::CardGame.Core.MuShunDamageReceipt? MuShunDamageState(long id) => _resolve().MuShunDamageState(id);
    internal int MuShunJin(int seat) => _resolve().MuShunJin(seat);
    internal bool MuShunLossCandidate(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramSkillWindowContext c, bool issued) => _resolve().MuShunLossCandidate(candidate, c, issued);
    internal global::System.Collections.Generic.IEnumerable<(global::CardGame.Core.CardUseEffectSource Source, int Amount)> MuShunNextReceivedDamage(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _resolve().MuShunNextReceivedDamage(attack);
    internal bool MuShunPindianStructuralEdge(global::CardGame.Core.ResolutionFrame child, global::CardGame.Core.ResolutionFrame parent) => _resolve().MuShunPindianStructuralEdge(child, parent);
    internal void MuShunRange(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.List<global::CardGame.Core.RuleQueryContribution> contributions) => _resolve().MuShunRange(actor, contributions);
    internal void PublishMuShun(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishMuShun(f);
    internal bool QualifyMuShun(global::CardGame.Core.ProgramTriggerLegalityContext g) => _resolve().QualifyMuShun(g);
    internal global::CardGame.Core.SkillProgramStepOutcome ResolveMuShunShizhao(global::CardGame.Core.ProgramSkillFrame supplied) => _resolve().ResolveMuShunShizhao(supplied);
    internal bool ResumeMuShunContest(long id) => _resolve().ResumeMuShunContest(id);
    internal void SetMuShunDamageState(long id, global::CardGame.Core.MuShunDamageReceipt? r) => _resolve().SetMuShunDamageState(id, r);
    internal void SetMuShunJin(global::CardGame.Core.ProgramSkillFrame f, int count) => _resolve().SetMuShunJin(f, count);
    internal bool ValidMuShunJinjian(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMuShunJinjian(f);
    internal bool WindowSettlement_IsMuShunPindianSettlementBatch(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().WindowSettlement_IsMuShunPindianSettlementBatch(frame, batch);
    internal bool HasMuShunCapability { get => _resolve().HasMuShunCapability; }
    internal bool HasMuShunShizhaoCapability { get => _resolve().HasMuShunShizhaoCapability; }
}
