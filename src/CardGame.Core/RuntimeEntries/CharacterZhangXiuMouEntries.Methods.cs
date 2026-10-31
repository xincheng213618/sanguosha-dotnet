#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangXiuMouEntries
{
    internal void BindMouZhangXiuAnniversarySlashUse(CardUseFrame supplied) => _resolve().BindMouZhangXiuAnniversarySlashUse(supplied);
    internal bool CanMouZhangXiuAnniversarySlash(ProgramSkillFrame f) => _resolve().CanMouZhangXiuAnniversarySlash(f);
    internal void CaptureMouZhangXiuAnniversaryDiscards(CardMovementBatchContext batch) => _resolve().CaptureMouZhangXiuAnniversaryDiscards(batch);
    internal CardMovementBatchContext CompleteMovementBatchCaptureMouZhangXiuAnniversaryDiscards(CardMovementBatchBuilder batch, CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouZhangXiuAnniversaryDiscards(batch, completed);
    internal void ContinueMouZhangXiuAnniversary(long id) => _resolve().ContinueMouZhangXiuAnniversary(id);
    internal bool FinishMouZhangXiuAnniversaryAttack(NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouZhangXiuAnniversaryAttack(attack, out paused);
    internal bool IsIssuedMouZhangXiuAnniversaryUse(CardUseFrame use) => _resolve().IsIssuedMouZhangXiuAnniversaryUse(use);
    internal bool IsMouZhangXiuAnniversaryUse(long id) => _resolve().IsMouZhangXiuAnniversaryUse(id);
    internal void IssueMouZhangXiuAnniversarySlash(ProgramSkillFrame f) => _resolve().IssueMouZhangXiuAnniversarySlash(f);
    internal bool MouZhangXiuAnniversaryAdvanceChildren() => _resolve().MouZhangXiuAnniversaryAdvanceChildren();
    internal bool MouZhangXiuAnniversaryDying() => _resolve().MouZhangXiuAnniversaryDying();
    internal bool MouZhangXiuAnniversaryEdge(int i) => _resolve().MouZhangXiuAnniversaryEdge(i);
    internal bool MouZhangXiuAnniversaryFinalizedDamageUse(ProgramTargetCardWindowQuery query) => _resolve().MouZhangXiuAnniversaryFinalizedDamageUse(query);
    internal bool MouZhangXiuAnniversaryFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouZhangXiuAnniversaryFirstChild(f, child);
    internal bool MouZhangXiuAnniversaryMovement(ProgramSkillFrame f, SkillProgramEffect? op, ProgramMovementContinuation p) => _resolve().MouZhangXiuAnniversaryMovement(f, op, p);
    internal bool MouZhangXiuAnniversaryRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouZhangXiuAnniversaryRecoveryParent(f, recovery);
    internal Card MouZhangXiuAnniversaryRepresentation(long id) => _resolve().MouZhangXiuAnniversaryRepresentation(id);
    internal ProgramSkillFrame? MouZhangXiuAnniversaryRoot() => _resolve().MouZhangXiuAnniversaryRoot();
    internal bool MouZhangXiuAnniversarySuffix(int n) => _resolve().MouZhangXiuAnniversarySuffix(n);
    internal void ObserveMouZhangXiuAnniversaryDamage(IGameEvent payload) => _resolve().ObserveMouZhangXiuAnniversaryDamage(payload);
    internal CardActionContext PrepareMouZhangXiuAnniversarySlashAction(CardActionContext action) => _resolve().PrepareMouZhangXiuAnniversarySlashAction(action);
    internal bool ReturnMouZhangXiuAnniversaryAttack(GameEngine.AttackCompletionReceipt c) => _resolve().ReturnMouZhangXiuAnniversaryAttack(c);
    internal void ReturnMouZhangXiuAnniversaryNative(CardUseFrame use) => _resolve().ReturnMouZhangXiuAnniversaryNative(use);
    internal void SettleMouZhangXiuAnniversaryNative(MouZhangXiuAnniversarySlashOrigin o) => _resolve().SettleMouZhangXiuAnniversaryNative(o);
    internal bool ValidMouZhangXiuAnniversary(ProgramSkillFrame f) => _resolve().ValidMouZhangXiuAnniversary(f);
    internal void ValidateMouZhangXiuAnniversaryNative(CardUseFrame use) => _resolve().ValidateMouZhangXiuAnniversaryNative(use);
    internal bool HasMouZhangXiuAnniversary { get => _resolve().HasMouZhangXiuAnniversary; }
    internal MouZhangXiuAnniversaryRuntime MouZhangXiuAnniversaryRules { get => _resolve().MouZhangXiuAnniversaryRules; }
}
