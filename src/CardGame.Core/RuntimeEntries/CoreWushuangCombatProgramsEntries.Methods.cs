#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreWushuangCombatProgramsEntries
{
    internal void ChangeWushuangTargetMaximumHp(CharacterState target, int delta, string skill) => _resolve().ChangeWushuangTargetMaximumHp(target, delta, skill);
    internal bool ContinueWushuangCombat(long frameId) => _resolve().ContinueWushuangCombat(frameId);
    internal SkillProgramStepOutcome ExecuteWushuangCombat(ProgramSkillFrame supplied, SkillProgramEffect effect) => _resolve().ExecuteWushuangCombat(supplied, effect);
    internal bool IsWushuangLightningJudgment(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _resolve().IsWushuangLightningJudgment(f, target, reason, bind, visibility);
    internal bool IsWushuangNativeDrawDamageMaterial(ProgramSkillFrame producer, CardMovementBatchContext batch, CardMovementRecord material, int recipient) => _resolve().IsWushuangNativeDrawDamageMaterial(producer, batch, material, recipient);
    internal bool IsWushuangNativeDrawDamageParent(ProgramSkillFrame producer) => _resolve().IsWushuangNativeDrawDamageParent(producer);
    internal bool IsWushuangPaidTargetDrawInvocation(ProgramSkillFrame producer, NativeDrawInvocationRecordedEvent actualDraw, int recipient) => _resolve().IsWushuangPaidTargetDrawInvocation(producer, actualDraw, recipient);
    internal bool RetainWushuangCombat(ProgramSkillFrame f) => _resolve().RetainWushuangCombat(f);
    internal void ReturnWushuangCombatDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome) => _resolve().ReturnWushuangCombatDraw(frame, outcome);
    internal bool TryReturnWushuangLightningJudgment(JudgmentFrame judgment) => _resolve().TryReturnWushuangLightningJudgment(judgment);
    internal bool WushuangNativeDrawObserverEdge(ResolutionFrame parent, ResolutionFrame child) => _resolve().WushuangNativeDrawObserverEdge(parent, child);
}
