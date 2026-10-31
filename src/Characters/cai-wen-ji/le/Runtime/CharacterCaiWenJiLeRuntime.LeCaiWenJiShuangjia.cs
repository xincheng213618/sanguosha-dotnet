#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaiWenJiLeRuntime
{
    internal ShuangjiaMarkRuntime ShuangjiaMarks => (ShuangjiaMarkRuntime)_closedShuangjiaMarkRuntimeSource.Get();
    internal bool HasShuangjiaEntityMark(int cardId) => ShuangjiaMarks.HasEntityMark(cardId);
    internal bool IsShuangjiaHandLimitExempt(CharacterState owner, int cardId) => ShuangjiaMarks.IsHandLimitExempt(owner.Seat, cardId);
}
