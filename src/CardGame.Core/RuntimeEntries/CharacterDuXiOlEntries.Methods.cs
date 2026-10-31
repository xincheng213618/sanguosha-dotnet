#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuXiOlEntries
{
    internal bool AdvanceDuXiSubtree() => _resolve().AdvanceDuXiSubtree();
    internal global::CardGame.Core.PromptChoice AiDuXi(global::CardGame.Core.PendingDecision p, global::CardGame.Core.ProgramSkillFrame f) => _resolve().AiDuXi(p, f);
    internal void AnswerDuXi(global::CardGame.Core.PromptChoice choice) => _resolve().AnswerDuXi(choice);
    internal global::CardGame.Core.SkillProgramStepOutcome BeginDuXi(global::CardGame.Core.ProgramSkillFrame input, global::CardGame.Core.SkillProgramEffectOp operation) => _resolve().BeginDuXi(input, operation);
    internal bool CanOfferDuXi(global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.SkillProgramTrigger trigger, global::CardGame.Core.ProgramSkillWindowContext context) => _resolve().CanOfferDuXi(candidate, trigger, context);
    internal void CaptureDuXiBixiongBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureDuXiBixiongBatch(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureDuXiBixiongBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureDuXiBixiongBatch(batch, completed);
    internal void ContinueDuXi(long id) => _resolve().ContinueDuXi(id);
    internal bool ContinueIssuedDuXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueIssuedDuXi(f);
    internal bool DrainDuXiChildren(long id) => _resolve().DrainDuXiChildren(id);
    internal global::System.Collections.Generic.IEnumerable<global::CardGame.Core.CardMovementRecord> DuXiBixiongMovements(global::CardGame.Core.CardMovementBatchContext batch, int owner) => _resolve().DuXiBixiongMovements(batch, owner);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.PromptChoice> DuXiChoices(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DuXiChoices(f);
    internal global::CardGame.Core.DuXiQuxiMarkerMovedEvent[] DuXiCurrentMarkers() => _resolve().DuXiCurrentMarkers();
    internal int DuXiDrawAdjustment(global::CardGame.Core.CharacterState player) => _resolve().DuXiDrawAdjustment(player);
    internal bool DuXiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().DuXiFirstChild(f, child);
    internal bool DuXiHasPair(int owner) => _resolve().DuXiHasPair(owner);
    internal int? DuXiHolder(global::CardGame.Core.CardConversionSource source, global::CardGame.Core.PlayerMarkerKind marker) => _resolve().DuXiHolder(source, marker);
    internal global::CardGame.Core.CardConversionSource DuXiIssuer(global::CardGame.Core.ProgramSkillFrame f) => _resolve().DuXiIssuer(f);
    internal global::CardGame.Core.ProgramSkillFrame? DuXiObserverRoot() => _resolve().DuXiObserverRoot();
    internal bool DuXiObserverSuffix(int rootIndex) => _resolve().DuXiObserverSuffix(rootIndex);
    internal global::System.Collections.Generic.IReadOnlyDictionary<int, int> DuXiSuitProjection() => _resolve().DuXiSuitProjection();
    internal void ExpireDuXiBixiong(global::CardGame.Core.IGameEvent payload) => _resolve().ExpireDuXiBixiong(payload);
    internal void FinishDuXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().FinishDuXi(f);
    internal bool HasAnyDuXiSuitProhibition() => _resolve().HasAnyDuXiSuitProhibition();
    internal bool IsDuXiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().IsDuXiRecoveryParent(f, recovery);
    internal bool IsDuXiSuitTargetProhibited(int sourceSeat, int targetSeat, global::CardGame.Core.Suit effectiveSuit) => _resolve().IsDuXiSuitTargetProhibited(sourceSeat, targetSeat, effectiveSuit);
    internal void PublishDuXi(global::CardGame.Core.ProgramSkillFrame f) => _resolve().PublishDuXi(f);
    internal bool ResumeDuXi(long id) => _resolve().ResumeDuXi(id);
    internal void SetDuXiMarker(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PlayerMarkerKind marker, int holder) => _resolve().SetDuXiMarker(f, marker, holder);
    internal bool TrySkipDuXiDiscardPhase(global::CardGame.Core.CharacterState player) => _resolve().TrySkipDuXiDiscardPhase(player);
    internal bool HasDuXiCapability { get => _resolve().HasDuXiCapability; }
}
