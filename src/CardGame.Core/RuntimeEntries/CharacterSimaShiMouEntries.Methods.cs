#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaShiMouEntries
{
    internal bool CanStartMouSimaShiChenlve(CharacterState owner) => _resolve().CanStartMouSimaShiChenlve(owner);
    internal void CaptureMouSimaShiDiscardBatch(CardMovementBatchContext batch) => _resolve().CaptureMouSimaShiDiscardBatch(batch);
    internal IReadOnlyList<ActualPhaseEndItem> CollectMouSimaShiPhaseEndCandidates(ActualDiscardRecoveryPhaseKey phase) => _resolve().CollectMouSimaShiPhaseEndCandidates(phase);
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouSimaShiDiscardBatch(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouSimaShiDiscardBatch(batch, completed);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMouSimaShiDeathCandidates(CharacterState owner, IReadOnlyList<ProgramTriggerCandidate> existing) => _resolve().IncludeMouSimaShiDeathCandidates(owner, existing);
    internal bool IsIssuedMouSimaShiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IsIssuedMouSimaShiCandidate(c, w);
    internal bool MouSimaShiAdvanceChildren() => _resolve().MouSimaShiAdvanceChildren();
    internal string? MouSimaShiCardLabel(int cardId) => _resolve().MouSimaShiCardLabel(cardId);
    internal MouSimaShiClaimIssuedEvent? MouSimaShiDue(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().MouSimaShiDue(c, w);
    internal bool MouSimaShiDying() => _resolve().MouSimaShiDying();
    internal bool MouSimaShiEdge(int i) => _resolve().MouSimaShiEdge(i);
    internal bool MouSimaShiFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouSimaShiFirstChild(f, child);
    internal bool MouSimaShiMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouSimaShiMovement(f, effect, p);
    internal CardConversionSource? MouSimaShiNoResponseSource(CharacterState actor, IReadOnlyList<int> ids) => _resolve().MouSimaShiNoResponseSource(actor, ids);
    internal bool MouSimaShiRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouSimaShiRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouSimaShiRoot() => _resolve().MouSimaShiRoot();
    internal bool MouSimaShiSuffix(int n) => _resolve().MouSimaShiSuffix(n);
    internal void ObserveMouSimaShiAction(IGameEvent payload) => _resolve().ObserveMouSimaShiAction(payload);
    internal bool ValidMouSimaShiDebt(MouSimaShiClaimIssuedEvent d) => _resolve().ValidMouSimaShiDebt(d);
    internal bool HasMouSimaShi { get => _resolve().HasMouSimaShi; }
    internal MouSimaShiRuntime MouSimaShiRules { get => _resolve().MouSimaShiRules; }
}
