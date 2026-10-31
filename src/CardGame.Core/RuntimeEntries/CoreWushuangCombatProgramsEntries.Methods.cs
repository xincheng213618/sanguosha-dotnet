#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreWushuangCombatProgramsEntries
{
    internal void ChangeWushuangTargetMaximumHp(global::CardGame.Core.CharacterState target, int delta, string skill) => _resolve().ChangeWushuangTargetMaximumHp(target, delta, skill);
    internal bool ContinueWushuangCombat(long frameId) => _resolve().ContinueWushuangCombat(frameId);
    internal global::CardGame.Core.SkillProgramStepOutcome ExecuteWushuangCombat(global::CardGame.Core.ProgramSkillFrame supplied, global::CardGame.Core.SkillProgramEffect effect) => _resolve().ExecuteWushuangCombat(supplied, effect);
    internal bool IsWushuangLightningJudgment(global::CardGame.Core.ProgramSkillFrame f, int target, string reason, string bind, global::CardGame.Core.SkillProgramCardSetVisibility visibility) => _resolve().IsWushuangLightningJudgment(f, target, reason, bind, visibility);
    internal bool IsWushuangNativeDrawDamageMaterial(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.CardMovementBatchContext batch, global::CardGame.Core.CardMovementRecord material, int recipient) => _resolve().IsWushuangNativeDrawDamageMaterial(producer, batch, material, recipient);
    internal bool IsWushuangNativeDrawDamageParent(global::CardGame.Core.ProgramSkillFrame producer) => _resolve().IsWushuangNativeDrawDamageParent(producer);
    internal bool IsWushuangPaidTargetDrawInvocation(global::CardGame.Core.ProgramSkillFrame producer, global::CardGame.Core.NativeDrawInvocationRecordedEvent actualDraw, int recipient) => _resolve().IsWushuangPaidTargetDrawInvocation(producer, actualDraw, recipient);
    internal bool RetainWushuangCombat(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RetainWushuangCombat(f);
    internal void ReturnWushuangCombatDraw(global::CardGame.Core.ProgramSkillFrame frame, global::CardGame.Core.NativeDrawOutcome outcome) => _resolve().ReturnWushuangCombatDraw(frame, outcome);
    internal bool TryReturnWushuangLightningJudgment(global::CardGame.Core.JudgmentFrame judgment) => _resolve().TryReturnWushuangLightningJudgment(judgment);
    internal bool WushuangNativeDrawObserverEdge(global::CardGame.Core.ResolutionFrame parent, global::CardGame.Core.ResolutionFrame child) => _resolve().WushuangNativeDrawObserverEdge(parent, child);
}
