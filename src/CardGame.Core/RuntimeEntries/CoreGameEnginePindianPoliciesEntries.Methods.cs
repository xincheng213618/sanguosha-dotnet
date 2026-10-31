#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianPoliciesEntries
{
    internal bool CanOfferPindianTopChoice(global::CardGame.Core.CharacterState owner) => _resolve().CanOfferPindianTopChoice(owner);
    internal int[] CurrentPindianProcessingIds(global::CardGame.Core.PindianFrame frame) => _resolve().CurrentPindianProcessingIds(frame);
    internal int EffectivePindianRank(global::CardGame.Core.CharacterState owner, global::CardGame.Core.Card card) => _resolve().EffectivePindianRank(owner, card);
    internal bool HasCurrentPindianSourceCard(global::CardGame.Core.PindianFrame frame) => _resolve().HasCurrentPindianSourceCard(frame);
    internal int ReserveOrReadPindianTop(global::CardGame.Core.CharacterState owner, bool reserve) => _resolve().ReserveOrReadPindianTop(owner, reserve);
}
