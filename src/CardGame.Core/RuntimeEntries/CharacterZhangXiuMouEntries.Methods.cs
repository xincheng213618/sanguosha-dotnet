#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangXiuMouEntries
{
    internal void BindMouZhangXiuAnniversarySlashUse(global::CardGame.Core.CardUseFrame supplied) => _resolve().BindMouZhangXiuAnniversarySlashUse(supplied);
    internal bool CanMouZhangXiuAnniversarySlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanMouZhangXiuAnniversarySlash(f);
    internal void CaptureMouZhangXiuAnniversaryDiscards(global::CardGame.Core.CardMovementBatchContext batch) => _resolve().CaptureMouZhangXiuAnniversaryDiscards(batch);
    internal global::CardGame.Core.CardMovementBatchContext CompleteMovementBatchCaptureMouZhangXiuAnniversaryDiscards(global::CardGame.Core.CardMovementBatchBuilder batch, global::CardGame.Core.CardMovementBatchContext completed) => _resolve().CompleteMovementBatchCaptureMouZhangXiuAnniversaryDiscards(batch, completed);
    internal void ContinueMouZhangXiuAnniversary(long id) => _resolve().ContinueMouZhangXiuAnniversary(id);
    internal bool FinishMouZhangXiuAnniversaryAttack(global::CardGame.Core.NativeOwners.CardAttackHandle attack, scoped out bool paused) => _resolve().FinishMouZhangXiuAnniversaryAttack(attack, out paused);
    internal bool IsIssuedMouZhangXiuAnniversaryUse(global::CardGame.Core.CardUseFrame use) => _resolve().IsIssuedMouZhangXiuAnniversaryUse(use);
    internal bool IsMouZhangXiuAnniversaryUse(long id) => _resolve().IsMouZhangXiuAnniversaryUse(id);
    internal void IssueMouZhangXiuAnniversarySlash(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IssueMouZhangXiuAnniversarySlash(f);
    internal bool MouZhangXiuAnniversaryAdvanceChildren() => _resolve().MouZhangXiuAnniversaryAdvanceChildren();
    internal bool MouZhangXiuAnniversaryBeforeDamage(long id) => _resolve().MouZhangXiuAnniversaryBeforeDamage(id);
    internal bool MouZhangXiuAnniversaryDamageWindow(long id) => _resolve().MouZhangXiuAnniversaryDamageWindow(id);
    internal bool MouZhangXiuAnniversaryDying() => _resolve().MouZhangXiuAnniversaryDying();
    internal bool MouZhangXiuAnniversaryEdge(int i) => _resolve().MouZhangXiuAnniversaryEdge(i);
    internal bool MouZhangXiuAnniversaryFinalizedDamageUse(global::CardGame.Core.ProgramTargetCardWindowQuery query) => _resolve().MouZhangXiuAnniversaryFinalizedDamageUse(query);
    internal bool MouZhangXiuAnniversaryFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouZhangXiuAnniversaryFirstChild(f, child);
    internal bool MouZhangXiuAnniversaryMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? op, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouZhangXiuAnniversaryMovement(f, op, p);
    internal bool MouZhangXiuAnniversaryRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouZhangXiuAnniversaryRecoveryParent(f, recovery);
    internal global::CardGame.Core.Card MouZhangXiuAnniversaryRepresentation(long id) => _resolve().MouZhangXiuAnniversaryRepresentation(id);
    internal global::CardGame.Core.ProgramSkillFrame? MouZhangXiuAnniversaryRoot() => _resolve().MouZhangXiuAnniversaryRoot();
    internal bool MouZhangXiuAnniversarySuffix(int n) => _resolve().MouZhangXiuAnniversarySuffix(n);
    internal void ObserveMouZhangXiuAnniversaryDamage(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouZhangXiuAnniversaryDamage(payload);
    internal global::CardGame.Core.CardActionContext PrepareMouZhangXiuAnniversarySlashAction(global::CardGame.Core.CardActionContext action) => _resolve().PrepareMouZhangXiuAnniversarySlashAction(action);
    internal bool ReturnMouZhangXiuAnniversaryAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt c) => _resolve().ReturnMouZhangXiuAnniversaryAttack(c);
    internal void ReturnMouZhangXiuAnniversaryNative(global::CardGame.Core.CardUseFrame use) => _resolve().ReturnMouZhangXiuAnniversaryNative(use);
    internal void SettleMouZhangXiuAnniversaryNative(global::CardGame.Core.MouZhangXiuAnniversarySlashOrigin o) => _resolve().SettleMouZhangXiuAnniversaryNative(o);
    internal bool ValidMouZhangXiuAnniversary(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ValidMouZhangXiuAnniversary(f);
    internal void ValidateMouZhangXiuAnniversaryNative(global::CardGame.Core.CardUseFrame use) => _resolve().ValidateMouZhangXiuAnniversaryNative(use);
    internal bool HasMouZhangXiuAnniversary { get => _resolve().HasMouZhangXiuAnniversary; }
    internal global::CardGame.Core.MouZhangXiuAnniversaryRuntime MouZhangXiuAnniversaryRules { get => _resolve().MouZhangXiuAnniversaryRules; }
}
