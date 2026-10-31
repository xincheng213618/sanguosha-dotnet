#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJuShouMouEntries
{
    internal void BindMouJsNativeUse(CardUseFrame use) => _resolve().BindMouJsNativeUse(use);
    internal void CaptureMouJsMovementBatch(CardMovementBatchContext batch) => _resolve().CaptureMouJsMovementBatch(batch);
    internal IEnumerable<ProgramTriggerCandidate> CollectMouJsMovementCandidates(CardMovementBatchContext batch) => _resolve().CollectMouJsMovementCandidates(batch);
    internal void CollectWindow_CollectMouJsMovementCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouJsMovementCandidates(batch, result);
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouJsMovementBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouJsMovementBatch(batch, completed);
    internal ProgramSkillWindowContext CreateMouJsMovementContext(CardsMovedTriggerWindowFrame p, ProgramTriggerCandidate c) => _resolve().CreateMouJsMovementContext(p, c);
    internal bool ExcludeOrdinaryMovement_ResolveMouJuShouMuwang(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_ResolveMouJuShouMuwang(candidate, trigger);
    internal bool FinishMouJsAlcoholCost(CardUseFrame use, Card card) => _resolve().FinishMouJsAlcoholCost(use, card);
    internal bool HasMouJsEntityMark(int card) => _resolve().HasMouJsEntityMark(card);
    internal bool IsIssuedMouJsMovementCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedMouJsMovementCandidate(c, w);
    internal bool IsMouJsExactCardQuery(CharacterState actor, int? selected) => _resolve().IsMouJsExactCardQuery(actor, selected);
    internal bool IsMouJsHandLimitExempt(int actor, int card) => _resolve().IsMouJsHandLimitExempt(actor, card);
    internal void IssueMouJsUse(ProgramSkillFrame f) => _resolve().IssueMouJsUse(f);
    internal bool MatchesMouJsDeclaration(ProgramSkillFrame f, CardDeclarationFrame d) => _resolve().MatchesMouJsDeclaration(f, d);
    internal bool MouJsActionBan(ProgramLegalActionQuery q) => _resolve().MouJsActionBan(q);
    internal bool MouJsAdvanceChildren() => _resolve().MouJsAdvanceChildren();
    internal bool MouJsDeclarationEdge(int i) => _resolve().MouJsDeclarationEdge(i);
    internal bool MouJsDying() => _resolve().MouJsDying();
    internal bool MouJsEdge(int i) => _resolve().MouJsEdge(i);
    internal IReadOnlyList<int> MouJsExpectedTargets(MouJsNativeUse n) => _resolve().MouJsExpectedTargets(n);
    internal bool MouJsFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouJsFirstChild(f, child);
    internal bool MouJsHasGrant(int actor, int card) => _resolve().MouJsHasGrant(actor, card);
    internal bool MouJsMaterialBan(ProgramCardMaterialQuery q) => _resolve().MouJsMaterialBan(q);
    internal bool MouJsMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouJsMovement(f, effect, p);
    internal bool MouJsMovementParent(ProgramSkillFrame f) => _resolve().MouJsMovementParent(f);
    internal MouJsMuwangQueuedEvent? MouJsMuwangOpportunity(ProgramSkillFrame f) => _resolve().MouJsMuwangOpportunity(f);
    internal bool MouJsNativeRidesOn(CardUseFrame u, ProgramSkillFrame f) => _resolve().MouJsNativeRidesOn(u, f);
    internal ProgramSkillFrame? MouJsObserverRoot() => _resolve().MouJsObserverRoot();
    internal bool MouJsOffTurnSlash(long id, int actor) => _resolve().MouJsOffTurnSlash(id, actor);
    internal IReadOnlyList<MouJsPlan> MouJsPlans(ProgramSkillFrame f) => _resolve().MouJsPlans(f);
    internal MouJsMuwangQueuedEvent? MouJsQueue(ProgramTriggerCandidate c, CardMovementBatchContext batch) => _resolve().MouJsQueue(c, batch);
    internal bool MouJsReclaimable(MouJsFirstLoss loss, CardMovementRecord discarded) => _resolve().MouJsReclaimable(loss, discarded);
    internal bool MouJsRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame rr) => _resolve().MouJsRecoveryParent(f, rr);
    internal bool MouJsResponseBan(ProgramNativeResponseQuery q) => _resolve().MouJsResponseBan(q);
    internal void MouJsSubmittedUse(ProgramSubmittedUseQuery q) => _resolve().MouJsSubmittedUse(q);
    internal bool MouJsSuffix(int n) => _resolve().MouJsSuffix(n);
    internal bool MouJsUsePurpose(int actor, CardKind kind, bool response, bool dying = false) => _resolve().MouJsUsePurpose(actor, kind, response, dying);
    internal ProgramSkillFrame? MouJsUseQuery(CharacterState actor, int? selected = default) => _resolve().MouJsUseQuery(actor, selected);
    internal bool MouJsViewAsBan(ProgramViewAsLegalityQuery q) => _resolve().MouJsViewAsBan(q);
    internal void ObserveMouJsMovement(CardMovementRecord movement) => _resolve().ObserveMouJsMovement(movement);
    internal void ObserveMouJsUse(IGameEvent payload) => _resolve().ObserveMouJsUse(payload);
    internal void ProjectMouJuShouCardSkillMark(int cardId, ref CardSkillMarkProjection projection) => _resolve().ProjectMouJuShouCardSkillMark(cardId, ref projection);
    internal bool ResumeMouJsUse() => _resolve().ResumeMouJsUse();
    internal bool ReturnMouJsDeclaration(CardDeclarationFrame d, bool succeeded) => _resolve().ReturnMouJsDeclaration(d, succeeded);
    internal bool ReturnMouJsUse(CardUseFrame u) => _resolve().ReturnMouJsUse(u);
    internal ProgramSkillWindowContext? TryCreateMouJsMovementContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate) => _resolve().TryCreateMouJsMovementContext(frame, candidate);
    internal void ValidateMouJsUse(CardUseFrame u) => _resolve().ValidateMouJsUse(u);
    internal MouJuShouRuntime MouJuShou { get => _resolve().MouJuShou; }
}
