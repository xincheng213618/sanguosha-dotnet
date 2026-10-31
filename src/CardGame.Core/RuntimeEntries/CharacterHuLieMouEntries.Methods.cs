#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterHuLieMouEntries
{
    internal void AppendMouHuLieDamageCandidates(GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window, List<DamageTriggerCandidate> candidates) => _resolve().AppendMouHuLieDamageCandidates(attack, window, candidates);
    internal void BindMouHuLieSlashUse(CardUseFrame supplied) => _resolve().BindMouHuLieSlashUse(supplied);
    internal bool CanMouHuLieSlash(ProgramSkillFrame f) => _resolve().CanMouHuLieSlash(f);
    internal void CaptureMouHuLieUsedDiscard(CardMovementBatchContext batch) => _resolve().CaptureMouHuLieUsedDiscard(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouHuLieUsedDiscard(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouHuLieUsedDiscard(batch, completed);
    internal MouHuLieYu? CurrentMouHuLieYu(int owner, string instance) => _resolve().CurrentMouHuLieYu(owner, instance);
    internal bool FinishMouHuLieAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouHuLieAttack(attack, out paused);
    internal IReadOnlyList<ProgramTriggerCandidate> IncludeMouHuLieRoundDebt(CharacterState owner, SkillProgramTriggerWindow window, IReadOnlyList<ProgramTriggerCandidate> ordinary) => _resolve().IncludeMouHuLieRoundDebt(owner, window, ordinary);
    internal bool IsIssuedMouHuLieUse(CardUseFrame use) => _resolve().IsIssuedMouHuLieUse(use);
    internal bool IsMouHuLieUse(long id) => _resolve().IsMouHuLieUse(id);
    internal bool IsMouHuLieUsedDiscard(CardMovementBatchContext batch, CardMovementRecord move, MouHuLieYu yu) => _resolve().IsMouHuLieUsedDiscard(batch, move, yu);
    internal void IssueMouHuLieSlash(ProgramSkillFrame f) => _resolve().IssueMouHuLieSlash(f);
    internal bool IssuedMouHuLieDamage(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().IssuedMouHuLieDamage(c, w);
    internal bool MouHuLieAdvanceChildren() => _resolve().MouHuLieAdvanceChildren();
    internal string? MouHuLieCardLabel(int cardId) => _resolve().MouHuLieCardLabel(cardId);
    internal MouHuLieDamageEvent? MouHuLieDamageReceipt(ProgramTriggerCandidate c, ProgramSkillWindowContext w) => _resolve().MouHuLieDamageReceipt(c, w);
    internal bool MouHuLieDying() => _resolve().MouHuLieDying();
    internal bool MouHuLieEdge(int i) => _resolve().MouHuLieEdge(i);
    internal bool MouHuLieFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouHuLieFirstChild(f, child);
    internal bool MouHuLieMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => _resolve().MouHuLieMovement(f, op, p);
    internal bool MouHuLieRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouHuLieRecoveryParent(f, recovery);
    internal Card MouHuLieRepresentation(long id) => _resolve().MouHuLieRepresentation(id);
    internal ProgramSkillFrame? MouHuLieRoot() => _resolve().MouHuLieRoot();
    internal bool MouHuLieSuffix(int n) => _resolve().MouHuLieSuffix(n);
    internal int[]? MouHuLieUsedIndexes(CardMovementBatchContext batch, ProgramTriggerCandidate candidate, ProgramInstructionFeatures features) => _resolve().MouHuLieUsedIndexes(batch, candidate, features);
    internal void ObserveMouHuLieYitouDamage(IGameEvent payload) => _resolve().ObserveMouHuLieYitouDamage(payload);
    internal CardActionContext PrepareMouHuLieSlashAction(CardActionContext action) => _resolve().PrepareMouHuLieSlashAction(action);
    internal bool ReturnMouHuLieAttack(GameEngine.AttackCompletionReceipt c) => _resolve().ReturnMouHuLieAttack(c);
    internal void ReturnMouHuLieNative(CardUseFrame use) => _resolve().ReturnMouHuLieNative(use);
    internal void SettleMouHuLieNative(MouHuLieSlashOrigin o) => _resolve().SettleMouHuLieNative(o);
    internal void ValidateMouHuLieNative(CardUseFrame use) => _resolve().ValidateMouHuLieNative(use);
    internal bool HasMouHuLie { get => _resolve().HasMouHuLie; }
    internal MouHuLieRuntime MouHuLieRules { get => _resolve().MouHuLieRules; }
}
