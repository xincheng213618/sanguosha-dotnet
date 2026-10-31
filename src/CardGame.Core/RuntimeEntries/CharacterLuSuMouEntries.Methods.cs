#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterLuSuMouEntries
{
    internal void AppendMouLuSuIssuedDamageCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window, List<DamageTriggerCandidate> result) => _resolve().AppendMouLuSuIssuedDamageCandidates(attack, window, result);
    internal void BeginMouLuSuPlayedCompleted(ProgramSkillFrame f) => _resolve().BeginMouLuSuPlayedCompleted(f);
    internal void BindMouLuSuUse(CardUseDeclaredEvent fact) => _resolve().BindMouLuSuUse(fact);
    internal void CaptureMouLuSuNativeTransferProducer(CardMovementRecord m) => _resolve().CaptureMouLuSuNativeTransferProducer(m);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouLuSuGainCandidates(CardMovementBatchContext batch) => _resolve().CollectMouLuSuGainCandidates(batch);
    internal void CollectWindow_CollectMouLuSuGainCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouLuSuGainCandidates(batch, result);
    internal ProgramSkillWindowContext CreateMouLuSuGainContext(CardsMovedTriggerWindowFrame p, ProgramTriggerCandidate c) => _resolve().CreateMouLuSuGainContext(p, c);
    internal bool ExcludeOrdinaryMovement_IsMouLuSuGainTrigger(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_IsMouLuSuGainTrigger(candidate, trigger);
    internal void FinishMouLuSuPlayedAccepted(ProgramSkillFrame f) => _resolve().FinishMouLuSuPlayedAccepted(f);
    internal void FinishMouLuSuPlayedCompleted(ProgramSkillFrame f) => _resolve().FinishMouLuSuPlayedCompleted(f);
    internal bool IsIssuedMouLuSuDamageCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().IsIssuedMouLuSuDamageCandidate(candidate, context);
    internal bool IsMouLuSuPaymentFrame(ChainedStateBasicFrame p) => _resolve().IsMouLuSuPaymentFrame(p);
    internal bool IsMouLuSuPaymentFrame2(DrawFundedDistinctBasicFrame p) => _resolve().IsMouLuSuPaymentFrame(p);
    internal bool IsMouLuSuPlayedWindow(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().IsMouLuSuPlayedWindow(action, window);
    internal bool IsMouLuSuUseQuery(CharacterState actor) => _resolve().IsMouLuSuUseQuery(actor);
    internal void IssueMouLuSuPlayed(ProgramSkillFrame f) => _resolve().IssueMouLuSuPlayed(f);
    internal void IssueMouLuSuPlayedDeclaration(ProgramSkillFrame f) => _resolve().IssueMouLuSuPlayedDeclaration(f);
    internal void IssueMouLuSuUse(ProgramSkillFrame f) => _resolve().IssueMouLuSuUse(f);
    internal bool MatchesMouLuSuPlayedDeclaration(ProgramSkillFrame f, CardDeclarationFrame d) => _resolve().MatchesMouLuSuPlayedDeclaration(f, d);
    internal bool MouLuSuAdvanceChildren() => _resolve().MouLuSuAdvanceChildren();
    internal bool MouLuSuDying() => _resolve().MouLuSuDying();
    internal bool MouLuSuEdge(int i) => _resolve().MouLuSuEdge(i);
    internal bool MouLuSuFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouLuSuFirstChild(f, child);
    internal int? MouLuSuGainPeer(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().MouLuSuGainPeer(c, w);
    internal (int Peer, int Index)[] MouLuSuGainPeers(CardMovementBatchContext batch, int owner) => _resolve().MouLuSuGainPeers(batch, owner);
    internal bool MouLuSuMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => _resolve().MouLuSuMovement(f, op, p);
    internal bool MouLuSuNativeMatches(CardUseFrame use) => _resolve().MouLuSuNativeMatches(use);
    internal bool MouLuSuNoSlashDebit(long id, int actor) => _resolve().MouLuSuNoSlashDebit(id, actor);
    internal bool MouLuSuPaymentParent(long paymentId, long? parent, long? request, int cursor, CardConversionSource source, CardKind kind, IReadOnlyList<int> targets) => _resolve().MouLuSuPaymentParent(paymentId, parent, request, cursor, source, kind, targets);
    internal bool MouLuSuPlayedDeclarationEdge(int i) => _resolve().MouLuSuPlayedDeclarationEdge(i);
    internal IReadOnlyList<MouLuSuAnniversaryPlayedPlan> MouLuSuPlayedPlans(ProgramSkillFrame f) => _resolve().MouLuSuPlayedPlans(f);
    internal MouLuSuAnniversaryPlayedReturn? MouLuSuPlayedWindowReturn(CardActionContext action, SkillProgramTriggerWindow window) => _resolve().MouLuSuPlayedWindowReturn(action, window);
    internal bool MouLuSuRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame rr) => _resolve().MouLuSuRecoveryParent(f, rr);
    internal bool MouLuSuRequestedQuota(ProgramSlashQuotaQuery q) => _resolve().MouLuSuRequestedQuota(q);
    internal ProgramSkillFrame? MouLuSuRoot() => _resolve().MouLuSuRoot();
    internal ProgramSkillFrame? MouLuSuSlotParent(EquipmentSlotNamedUseDraft slot) => _resolve().MouLuSuSlotParent(slot);
    internal bool MouLuSuSuffix(int n) => _resolve().MouLuSuSuffix(n);
    internal int? MouLuSuTransferSource(CardMovementRecord m) => _resolve().MouLuSuTransferSource(m);
    internal IReadOnlyList<LegalAction> MouLuSuUseActions(ProgramSkillFrame f) => _resolve().MouLuSuUseActions(f);
    internal ProgramSkillFrame? MouLuSuUseQuery(int actor) => _resolve().MouLuSuUseQuery(actor);
    internal bool? ProveMouLuSuMovement(SkillProgramTrigger trigger, ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => _resolve().ProveMouLuSuMovement(trigger, candidate, context);
    internal void RecordMouLuSuOwnedTransferProducer(CardMovementRecord m, int recipient, long producer) => _resolve().RecordMouLuSuOwnedTransferProducer(m, recipient, producer);
    internal bool ReturnMouLuSuAttack() => _resolve().ReturnMouLuSuAttack();
    internal bool ReturnMouLuSuPlayedDeclaration(CardDeclarationFrame d, bool succeeded) => _resolve().ReturnMouLuSuPlayedDeclaration(d, succeeded);
    internal void ReturnMouLuSuPublicReveal(ProgramSkillFrame f) => _resolve().ReturnMouLuSuPublicReveal(f);
    internal void ReturnMouLuSuUse(CardUseFrame use) => _resolve().ReturnMouLuSuUse(use);
    internal void SettleMouLuSuUse(ProgramSkillFrame f) => _resolve().SettleMouLuSuUse(f);
    internal bool TryCancelMouLuSuPayment(long id) => _resolve().TryCancelMouLuSuPayment(id);
    internal ProgramSkillWindowContext? TryCreateMouLuSuGainContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateMouLuSuGainContext(frame, candidate);
    internal bool TryReturnMouLuSuPlayedWindow(ProgramCardTriggerWindowFrame window) => _resolve().TryReturnMouLuSuPlayedWindow(window);
    internal void ValidateMouLuSuUse(CardUseFrame use) => _resolve().ValidateMouLuSuUse(use);
    internal bool HasMouLuSuAnniversary { get => _resolve().HasMouLuSuAnniversary; }
    internal MouLuSuAnniversaryRuntime MouLuSuAnniversary { get => _resolve().MouLuSuAnniversary; }
}
