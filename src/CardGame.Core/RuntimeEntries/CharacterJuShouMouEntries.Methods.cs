#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterJuShouMouEntries
{
    internal void BindMouJsNativeUse(global::CardGame.Core.CardUseFrame use) => _resolve().BindMouJsNativeUse(use);
    internal void CaptureMouJsMovementBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouJsMovementBatch(batch);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.ProgramTriggerCandidate> CollectMouJsMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CollectMouJsMovementCandidates(batch);
    internal void CollectWindow_CollectMouJsMovementCandidates(global::CardGame.Core.CardMovementBatchContext batch, global::System.Collections.Generic.List<global::CardGame.Core.ProgramTriggerCandidate> result) => _resolve().CollectWindow_CollectMouJsMovementCandidates(batch, result);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureMouJsMovementBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouJsMovementBatch(batch, completed);
    internal global::CardGame.Core.ProgramSkillWindowContext CreateMouJsMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame p, global::CardGame.Core.ProgramTriggerCandidate c) => _resolve().CreateMouJsMovementContext(p, c);
    internal bool ExcludeOrdinaryMovement_ResolveMouJuShouMuwang(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger) => _resolve().ExcludeOrdinaryMovement_ResolveMouJuShouMuwang(candidate, trigger);
    internal bool FinishMouJsAlcoholCost(global::CardGame.Core.CardUseFrame use, global::CardGame.Core.Card card) => _resolve().FinishMouJsAlcoholCost(use, card);
    internal bool HasMouJsEntityMark(int card) => _resolve().HasMouJsEntityMark(card);
    internal bool IsIssuedMouJsMovementCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedMouJsMovementCandidate(c, w);
    internal bool IsMouJsExactCardQuery(global::CardGame.Core.CharacterState actor, int? selected) => _resolve().IsMouJsExactCardQuery(actor, selected);
    internal bool IsMouJsHandLimitExempt(int actor, int card) => _resolve().IsMouJsHandLimitExempt(actor, card);
    internal void IssueMouJsUse(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouJsUse(f);
    internal bool MatchesMouJsDeclaration(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.CardDeclarationFrame d) => _resolve().MatchesMouJsDeclaration(f, d);
    internal bool MouJsActionBan(global::CardGame.Core.ProgramLegalActionQuery q) => _resolve().MouJsActionBan(q);
    internal bool MouJsAdvanceChildren() => _resolve().MouJsAdvanceChildren();
    internal bool MouJsBeforeDamage(long id) => _resolve().MouJsBeforeDamage(id);
    internal bool MouJsDamageWindow(long id) => _resolve().MouJsDamageWindow(id);
    internal bool MouJsDeclarationEdge(int i) => _resolve().MouJsDeclarationEdge(i);
    internal bool MouJsDying() => _resolve().MouJsDying();
    internal bool MouJsEdge(int i) => _resolve().MouJsEdge(i);
    internal global::System.Collections.Generic.IReadOnlyList<int> MouJsExpectedTargets(global::CardGame.Core.MouJsNativeUse n) => _resolve().MouJsExpectedTargets(n);
    internal bool MouJsFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouJsFirstChild(f, child);
    internal bool MouJsHasGrant(int actor, int card) => _resolve().MouJsHasGrant(actor, card);
    internal bool MouJsMaterialBan(global::CardGame.Core.ProgramCardMaterialQuery q) => _resolve().MouJsMaterialBan(q);
    internal bool MouJsMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouJsMovement(f, effect, p);
    internal bool MouJsMovementParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJsMovementParent(f);
    internal global::CardGame.Core.MouJsMuwangQueuedEvent? MouJsMuwangOpportunity(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJsMuwangOpportunity(f);
    internal bool MouJsNativeRidesOn(global::CardGame.Core.CardUseFrame u, global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJsNativeRidesOn(u, f);
    internal global::CardGame.Core.ProgramSkillFrame? MouJsObserverRoot() => _resolve().MouJsObserverRoot();
    internal bool MouJsOffTurnSlash(long id, int actor) => _resolve().MouJsOffTurnSlash(id, actor);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.MouJsPlan> MouJsPlans(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouJsPlans(f);
    internal global::CardGame.Core.MouJsMuwangQueuedEvent? MouJsQueue(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.CardMovementBatchContext batch) => _resolve().MouJsQueue(c, batch);
    internal bool MouJsReclaimable(global::CardGame.Core.MouJsFirstLoss loss, global::CardGame.Core.CardMovementRecord discarded) => _resolve().MouJsReclaimable(loss, discarded);
    internal bool MouJsRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame rr) => _resolve().MouJsRecoveryParent(f, rr);
    internal bool MouJsResponseBan(global::CardGame.Core.ProgramNativeResponseQuery q) => _resolve().MouJsResponseBan(q);
    internal void MouJsSubmittedUse(global::CardGame.Core.ProgramSubmittedUseQuery q) => _resolve().MouJsSubmittedUse(q);
    internal bool MouJsSuffix(int n) => _resolve().MouJsSuffix(n);
    internal bool MouJsUsePurpose(int actor, global::CardGame.Core.CardKind kind, bool response, bool dying = false) => _resolve().MouJsUsePurpose(actor, kind, response, dying);
    internal global::CardGame.Core.ProgramSkillFrame? MouJsUseQuery(global::CardGame.Core.CharacterState actor, int? selected = default) => _resolve().MouJsUseQuery(actor, selected);
    internal bool MouJsViewAsBan(global::CardGame.Core.ProgramViewAsLegalityQuery q) => _resolve().MouJsViewAsBan(q);
    internal void ObserveMouJsMovement(global::CardGame.Core.CardMovementRecord movement) => _resolve().ObserveMouJsMovement(movement);
    internal void ObserveMouJsUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouJsUse(payload);
    internal void ProjectMouJuShouCardSkillMark(int cardId, ref global::CardGame.Core.CardSkillMarkProjection projection) => _resolve().ProjectMouJuShouCardSkillMark(cardId, ref projection);
    internal bool ResumeMouJsUse() => _resolve().ResumeMouJsUse();
    internal bool ReturnMouJsDeclaration(global::CardGame.Core.CardDeclarationFrame d, bool succeeded) => _resolve().ReturnMouJsDeclaration(d, succeeded);
    internal bool ReturnMouJsUse(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnMouJsUse(u);
    internal global::CardGame.Core.ProgramSkillWindowContext? TryCreateMouJsMovementContext(global::CardGame.Core.CardsMovedTriggerWindowFrame frame, global::CardGame.Core.ProgramTriggerCandidate candidate) => _resolve().TryCreateMouJsMovementContext(frame, candidate);
    internal void ValidateMouJsUse(global::CardGame.Core.CardUseFrame u) => _resolve().ValidateMouJsUse(u);
    internal global::CardGame.Core.MouJuShouRuntime MouJuShou { get => _resolve().MouJuShou; }
}
