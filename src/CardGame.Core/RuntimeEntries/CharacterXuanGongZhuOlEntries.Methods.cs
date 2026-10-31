#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterXuanGongZhuOlEntries
{
    internal void AssertRegisteredIssuedXuanGongZhu(global::CardGame.Core.ProgramSkillFrame frame) => _resolve().AssertRegisteredIssuedXuanGongZhu(frame);
    internal bool CanContinueIssuedXuanGongZhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().CanContinueIssuedXuanGongZhu(f);
    internal bool HasXuanGongZhuDamageObserver(long id) => _resolve().HasXuanGongZhuDamageObserver(id);
    internal bool IsXuanGongZhuDying() => _resolve().IsXuanGongZhuDying();
    internal void RequireIssuedXuanGongZhu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequireIssuedXuanGongZhu(f);
    internal bool TryAdvanceXuanGongZhuSubtree() => _resolve().TryAdvanceXuanGongZhuSubtree();
    internal bool XuanGongZhuFirstChild(global::CardGame.Core.ProgramSkillFrame root, global::CardGame.Core.ResolutionFrame child) => _resolve().XuanGongZhuFirstChild(root, child);
    internal global::CardGame.Core.ProgramSkillFrame? XuanGongZhuObserverRoot() => _resolve().XuanGongZhuObserverRoot();
}
