#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuanGongZhuOlEntries
{
    internal bool CanContinueIssuedXuanGongZhu(ProgramSkillFrame f) => _resolve().CanContinueIssuedXuanGongZhu(f);
    internal bool TryAdvanceXuanGongZhuSubtree() => _resolve().TryAdvanceXuanGongZhuSubtree();
    internal bool XuanGongZhuFirstChild(ProgramSkillFrame root, ResolutionFrame child) => _resolve().XuanGongZhuFirstChild(root, child);
    internal ProgramSkillFrame? XuanGongZhuObserverRoot() => _resolve().XuanGongZhuObserverRoot();
}
