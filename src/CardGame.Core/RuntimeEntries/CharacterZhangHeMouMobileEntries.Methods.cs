#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHeMouMobileEntries
{
    internal bool BeginMouQiaobianExtraPlay(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMouQiaobianExtraPlay(f);
    internal bool MouQiaobianAdvanceChildren() => _resolve().MouQiaobianAdvanceChildren();
    internal bool MouQiaobianBeforeDamage(long id) => _resolve().MouQiaobianBeforeDamage(id);
    internal bool MouQiaobianDamage(long id) => _resolve().MouQiaobianDamage(id);
    internal bool MouQiaobianDying() => _resolve().MouQiaobianDying();
    internal bool MouQiaobianFirstNativeChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouQiaobianFirstNativeChild(f, child);
    internal bool MouQiaobianMovementReturn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouQiaobianMovementReturn(f, effect, p);
    internal bool MouQiaobianNativeEdge(int i) => _resolve().MouQiaobianNativeEdge(i);
    internal global::CardGame.Core.ProgramSkillFrame? MouQiaobianNativeRoot() => _resolve().MouQiaobianNativeRoot();
    internal bool MouQiaobianNativeSuffix(int n) => _resolve().MouQiaobianNativeSuffix(n);
    internal bool MouQiaobianRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouQiaobianRecoveryParent(f, recovery);
    internal void ReturnMouQiaobianExtraPlay(global::CardGame.Core.ProgramPhaseSchedule s, bool cancel) => _resolve().ReturnMouQiaobianExtraPlay(s, cancel);
    internal global::CardGame.Core.MouQiaobianRuntime MouQiaobian { get => _resolve().MouQiaobian; }
}
