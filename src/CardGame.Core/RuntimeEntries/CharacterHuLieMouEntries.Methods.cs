#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuLieMouEntries
{
    internal void AppendMouHuLieDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.List<global::CardGame.Core.DamageTriggerCandidate> candidates) => _resolve().AppendMouHuLieDamageCandidates(attack, window, candidates);
    internal void BindMouHuLieSlashUse(global::CardGame.Core.CardUseFrame supplied) => _resolve().BindMouHuLieSlashUse(supplied);
    internal bool CanMouHuLieSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanMouHuLieSlash(f);
    internal void CaptureMouHuLieUsedDiscard(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouHuLieUsedDiscard(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureMouHuLieUsedDiscard(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouHuLieUsedDiscard(batch, completed);
    internal global::CardGame.Core.MouHuLieYu? CurrentMouHuLieYu(int owner, string instance) => _resolve().CurrentMouHuLieYu(owner, instance);
    internal bool FinishMouHuLieAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouHuLieAttack(attack, out paused);
    internal global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> IncludeMouHuLieRoundDebt(global::CardGame.Core.CharacterState owner, global::CardGame.Core.SkillProgramTriggerWindow window, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramTriggerCandidate> ordinary) => _resolve().IncludeMouHuLieRoundDebt(owner, window, ordinary);
    internal bool IsIssuedMouHuLieUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsIssuedMouHuLieUse(use);
    internal bool IsMouHuLieUse(long id) => _resolve().IsMouHuLieUse(id);
    internal bool IsMouHuLieUsedDiscard(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CardMovementRecord move, global::CardGame.Core.MouHuLieYu yu) => _resolve().IsMouHuLieUsedDiscard(batch, move, yu);
    internal void IssueMouHuLieSlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouHuLieSlash(f);
    internal bool IssuedMouHuLieDamage(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().IssuedMouHuLieDamage(c, w);
    internal bool MouHuLieAdvanceChildren() => _resolve().MouHuLieAdvanceChildren();
    internal string? MouHuLieCardLabel(int cardId) => _resolve().MouHuLieCardLabel(cardId);
    internal global::CardGame.Core.MouHuLieDamageEvent? MouHuLieDamageReceipt(global::CardGame.Core.ProgramTriggerCandidate c, global::CardGame.Core.ProgramSkillWindowContext w) => _resolve().MouHuLieDamageReceipt(c, w);
    internal bool MouHuLieDying() => _resolve().MouHuLieDying();
    internal bool MouHuLieEdge(int i) => _resolve().MouHuLieEdge(i);
    internal bool MouHuLieFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouHuLieFirstChild(f, child);
    internal bool MouHuLieMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouHuLieMovement(f, op, p);
    internal bool MouHuLieRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouHuLieRecoveryParent(f, recovery);
    internal global::CardGame.Core.Card MouHuLieRepresentation(long id) => _resolve().MouHuLieRepresentation(id);
    internal global::CardGame.Core.ProgramSkillFrame? MouHuLieRoot() => _resolve().MouHuLieRoot();
    internal bool MouHuLieSuffix(int n) => _resolve().MouHuLieSuffix(n);
    internal int[]? MouHuLieUsedIndexes(global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.ProgramTriggerCandidate candidate, global::CardGame.Core.ProgramInstructionFeatures features) => _resolve().MouHuLieUsedIndexes(batch, candidate, features);
    internal void ObserveMouHuLieYitouDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouHuLieYitouDamage(payload);
    internal global::CardGame.Core.CardActionContext PrepareMouHuLieSlashAction(global::CardGame.Core.CardActionContext action) => _resolve().PrepareMouHuLieSlashAction(action);
    internal bool ReturnMouHuLieAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt c) => _resolve().ReturnMouHuLieAttack(c);
    internal void ReturnMouHuLieNative(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouHuLieNative(use);
    internal void SettleMouHuLieNative(global::CardGame.Core.MouHuLieSlashOrigin o) => _resolve().SettleMouHuLieNative(o);
    internal void ValidateMouHuLieNative(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMouHuLieNative(use);
    internal bool HasMouHuLie { get => _resolve().HasMouHuLie; }
    internal global::CardGame.Core.MouHuLieRuntime MouHuLieRules { get => _resolve().MouHuLieRules; }
}
