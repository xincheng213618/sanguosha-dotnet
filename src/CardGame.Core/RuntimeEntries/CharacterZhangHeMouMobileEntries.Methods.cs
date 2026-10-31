#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterZhangHeMouMobileEntries
{
    internal bool BeginMouQiaobianExtraPlay(ProgramSkillFrame f) => _resolve().BeginMouQiaobianExtraPlay(f);
    internal bool MouQiaobianAdvanceChildren() => _resolve().MouQiaobianAdvanceChildren();
    internal bool MouQiaobianDying() => _resolve().MouQiaobianDying();
    internal bool MouQiaobianFirstNativeChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouQiaobianFirstNativeChild(f, child);
    internal bool MouQiaobianNativeEdge(int i) => _resolve().MouQiaobianNativeEdge(i);
    internal ProgramSkillFrame? MouQiaobianNativeRoot() => _resolve().MouQiaobianNativeRoot();
    internal bool MouQiaobianNativeSuffix(int n) => _resolve().MouQiaobianNativeSuffix(n);
    internal bool MouQiaobianRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouQiaobianRecoveryParent(f, recovery);
    internal void ReturnMouQiaobianExtraPlay(ProgramPhaseSchedule s, bool cancel) => _resolve().ReturnMouQiaobianExtraPlay(s, cancel);
    internal MouQiaobianRuntime MouQiaobian { get => _resolve().MouQiaobian; }
}
