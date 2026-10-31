#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSimaShiMouEntries
{
    internal bool CanStartMouSimaShiChenlve(global::CardGame.Core.CharacterState owner) => _resolve().CanStartMouSimaShiChenlve(owner);
    internal void CaptureMouSimaShiDiscardBatch(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouSimaShiDiscardBatch(batch);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ActualPhaseEndItem> CollectMouSimaShiPhaseEndCandidates(global::CardGame.Core.ActualDiscardRecoveryPhaseKey phase) => _resolve().CollectMouSimaShiPhaseEndCandidates(phase);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureMouSimaShiDiscardBatch(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouSimaShiDiscardBatch(batch, completed);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeMouSimaShiDeathCandidates(global::CardGame.Core.CharacterState owner, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> existing) => _resolve().IncludeMouSimaShiDeathCandidates(owner, existing);
    internal bool IsIssuedMouSimaShiCandidate(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IsIssuedMouSimaShiCandidate(c, w);
    internal bool MouSimaShiAdvanceChildren() => _resolve().MouSimaShiAdvanceChildren();
    internal string? MouSimaShiCardLabel(int cardId) => _resolve().MouSimaShiCardLabel(cardId);
    internal global::CardGame.Core.MouSimaShiClaimIssuedEvent? MouSimaShiDue(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().MouSimaShiDue(c, w);
    internal bool MouSimaShiDying() => _resolve().MouSimaShiDying();
    internal bool MouSimaShiEdge(int i) => _resolve().MouSimaShiEdge(i);
    internal bool MouSimaShiFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouSimaShiFirstChild(f, child);
    internal bool MouSimaShiMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouSimaShiMovement(f, effect, p);
    internal global::CardGame.Core.CardConversionSource? MouSimaShiNoResponseSource(global::CardGame.Core.CharacterState actor, global::System.Collections.Generic.IReadOnlyList<int> ids) => _resolve().MouSimaShiNoResponseSource(actor, ids);
    internal bool MouSimaShiRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouSimaShiRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouSimaShiRoot() => _resolve().MouSimaShiRoot();
    internal bool MouSimaShiSuffix(int n) => _resolve().MouSimaShiSuffix(n);
    internal void ObserveMouSimaShiAction(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouSimaShiAction(payload);
    internal bool ValidMouSimaShiDebt(global::CardGame.Core.MouSimaShiClaimIssuedEvent d) => _resolve().ValidMouSimaShiDebt(d);
    internal bool HasMouSimaShi { get => _resolve().HasMouSimaShi; }
    internal global::CardGame.Core.MouSimaShiRuntime MouSimaShiRules { get => _resolve().MouSimaShiRules; }
}
