#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterDongZhuoMouOlEntries
{
    internal bool BeginMouDongZhuoOlWine(ProgramSkillFrame f) => _resolve().BeginMouDongZhuoOlWine(f);
    internal bool ContinueMouDongZhuoOlWine(long id, ProgramSimpleCardContinuation c) => _resolve().ContinueMouDongZhuoOlWine(id, c);
    internal bool IsMouDongZhuoOlWine(long id) => _resolve().IsMouDongZhuoOlWine(id);
    internal bool MouDongZhuoOlAdvanceChildren() => _resolve().MouDongZhuoOlAdvanceChildren();
    internal bool MouDongZhuoOlDying() => _resolve().MouDongZhuoOlDying();
    internal bool MouDongZhuoOlEdge(int i) => _resolve().MouDongZhuoOlEdge(i);
    internal bool MouDongZhuoOlFirstChild(ProgramSkillFrame f, ResolutionFrame child) => _resolve().MouDongZhuoOlFirstChild(f, child);
    internal bool MouDongZhuoOlMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation p) => _resolve().MouDongZhuoOlMovement(f, effect, p);
    internal bool MouDongZhuoOlParent(ProgramSkillFrame f) => _resolve().MouDongZhuoOlParent(f);
    internal bool MouDongZhuoOlRecoveryParent(ProgramSkillFrame f, RecoveryReplacementFrame recovery) => _resolve().MouDongZhuoOlRecoveryParent(f, recovery);
    internal ProgramSkillFrame? MouDongZhuoOlRoot() => _resolve().MouDongZhuoOlRoot();
    internal bool MouDongZhuoOlSuffix(int n) => _resolve().MouDongZhuoOlSuffix(n);
    internal void ObserveMouDongZhuoOlUse(IGameEvent payload) => _resolve().ObserveMouDongZhuoOlUse(payload);
    internal void ReturnMouDongZhuoOlWine(CardUseFrame u) => _resolve().ReturnMouDongZhuoOlWine(u);
    internal void ValidateMouDongZhuoOlWine(CardUseFrame u) => _resolve().ValidateMouDongZhuoOlWine(u);
    internal MouDongZhuoOlRuntime MouDongZhuoOl { get => _resolve().MouDongZhuoOl; }
}
