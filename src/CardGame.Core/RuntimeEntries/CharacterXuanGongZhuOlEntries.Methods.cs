#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuanGongZhuOlEntries
{
    internal bool CanContinueIssuedXuanGongZhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedXuanGongZhu(f);
    internal bool TryAdvanceXuanGongZhuSubtree() => _resolve().TryAdvanceXuanGongZhuSubtree();
    internal bool XuanGongZhuFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().XuanGongZhuFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? XuanGongZhuObserverRoot() => _resolve().XuanGongZhuObserverRoot();
}
