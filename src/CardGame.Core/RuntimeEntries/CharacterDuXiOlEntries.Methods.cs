#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDuXiOlEntries
{
    internal bool AdvanceDuXiSubtree() => _resolve().AdvanceDuXiSubtree();
    internal PromptChoice AiDuXi(PendingDecision p, ProgramSkillFrame f) => _resolve().AiDuXi(p, f);
    internal void AnswerDuXi(PromptChoice choice) => _resolve().AnswerDuXi(choice);
    internal SkillProgramStepOutcome BeginDuXi(ProgramSkillFrame input, SkillProgramEffectOp operation) => _resolve().BeginDuXi(input, operation);
    internal bool CanOfferDuXi(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => _resolve().CanOfferDuXi(candidate, trigger, context);
    internal void CaptureDuXiBixiongBatch(CardMovementBatchContext batch) => _resolve().CaptureDuXiBixiongBatch(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureDuXiBixiongBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureDuXiBixiongBatch(batch, completed);
    internal void ContinueDuXi(long id) => _resolve().ContinueDuXi(id);
    internal bool ContinueIssuedDuXi(ProgramSkillFrame f) => _resolve().ContinueIssuedDuXi(f);
    internal bool DrainDuXiChildren(long id) => _resolve().DrainDuXiChildren(id);
    internal IEnumerable<CardMovementRecord> DuXiBixiongMovements(CardMovementBatchContext batch, int owner) => _resolve().DuXiBixiongMovements(batch, owner);
    internal IReadOnlyList<PromptChoice> DuXiChoices(ProgramSkillFrame f) => _resolve().DuXiChoices(f);
    internal DuXiQuxiMarkerMovedEvent[] DuXiCurrentMarkers() => _resolve().DuXiCurrentMarkers();
    internal int DuXiDrawAdjustment(CharacterState player) => _resolve().DuXiDrawAdjustment(player);
    internal bool DuXiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().DuXiFirstChild(f, child);
    internal bool DuXiHasPair(int owner) => _resolve().DuXiHasPair(owner);
    internal int? DuXiHolder(CardConversionSource source, PlayerMarkerKind marker) => _resolve().DuXiHolder(source, marker);
    internal CardConversionSource DuXiIssuer(ProgramSkillFrame f) => _resolve().DuXiIssuer(f);
    internal ProgramSkillFrame? DuXiObserverRoot() => _resolve().DuXiObserverRoot();
    internal bool DuXiObserverSuffix(int rootIndex) => _resolve().DuXiObserverSuffix(rootIndex);
    internal IReadOnlyDictionary<int, int> DuXiSuitProjection() => _resolve().DuXiSuitProjection();
    internal void ExpireDuXiBixiong(IGameEvent payload) => _resolve().ExpireDuXiBixiong(payload);
    internal void FinishDuXi(ProgramSkillFrame f) => _resolve().FinishDuXi(f);
    internal bool HasAnyDuXiSuitProhibition() => _resolve().HasAnyDuXiSuitProhibition();
    internal bool IsDuXiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().IsDuXiRecoveryParent(f, recovery);
    internal bool IsDuXiSuitTargetProhibited(int sourceSeat, int targetSeat, Suit effectiveSuit) => _resolve().IsDuXiSuitTargetProhibited(sourceSeat, targetSeat, effectiveSuit);
    internal void PublishDuXi(ProgramSkillFrame f) => _resolve().PublishDuXi(f);
    internal bool ResumeDuXi(long id) => _resolve().ResumeDuXi(id);
    internal void SetDuXiMarker(ProgramSkillFrame f, PlayerMarkerKind marker, int holder) => _resolve().SetDuXiMarker(f, marker, holder);
    internal bool TrySkipDuXiDiscardPhase(CharacterState player) => _resolve().TrySkipDuXiDiscardPhase(player);
    internal bool HasDuXiCapability { get => _resolve().HasDuXiCapability; }
}
