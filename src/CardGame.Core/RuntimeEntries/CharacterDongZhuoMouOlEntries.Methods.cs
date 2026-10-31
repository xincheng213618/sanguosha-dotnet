#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongZhuoMouOlEntries
{
    internal bool BeginMouDongZhuoOlWine(global::CardGame.Core.ProgramSkillFrame f) => _resolve().BeginMouDongZhuoOlWine(f);
    internal bool ContinueMouDongZhuoOlWine(long id, global::CardGame.Core.ProgramSimpleCardContinuation c) => _resolve().ContinueMouDongZhuoOlWine(id, c);
    internal bool IsMouDongZhuoOlWine(long id) => _resolve().IsMouDongZhuoOlWine(id);
    internal bool MouDongZhuoOlAdvanceChildren() => _resolve().MouDongZhuoOlAdvanceChildren();
    internal bool MouDongZhuoOlDying() => _resolve().MouDongZhuoOlDying();
    internal bool MouDongZhuoOlEdge(int i) => _resolve().MouDongZhuoOlEdge(i);
    internal bool MouDongZhuoOlFirstChild(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.ResolutionFrame child) => _resolve().MouDongZhuoOlFirstChild(f, child);
    internal bool MouDongZhuoOlMovement(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.SkillProgramEffect? effect, global::CardGame.Core.ProgramMovementContinuation p) => _resolve().MouDongZhuoOlMovement(f, effect, p);
    internal bool MouDongZhuoOlParent(global::CardGame.Core.ProgramSkillFrame f) => _resolve().MouDongZhuoOlParent(f);
    internal bool MouDongZhuoOlRecoveryParent(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.RecoveryReplacementFrame recovery) => _resolve().MouDongZhuoOlRecoveryParent(f, recovery);
    internal global::CardGame.Core.ProgramSkillFrame? MouDongZhuoOlRoot() => _resolve().MouDongZhuoOlRoot();
    internal bool MouDongZhuoOlSuffix(int n) => _resolve().MouDongZhuoOlSuffix(n);
    internal void ObserveMouDongZhuoOlUse(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveMouDongZhuoOlUse(payload);
    internal void ReturnMouDongZhuoOlWine(global::CardGame.Core.CardUseFrame u) => _resolve().ReturnMouDongZhuoOlWine(u);
    internal void ValidateMouDongZhuoOlWine(global::CardGame.Core.CardUseFrame u) => _resolve().ValidateMouDongZhuoOlWine(u);
    internal global::CardGame.Core.MouDongZhuoOlRuntime MouDongZhuoOl { get => _resolve().MouDongZhuoOl; }
}
