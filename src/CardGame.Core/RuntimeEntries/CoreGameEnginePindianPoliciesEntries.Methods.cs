#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianPoliciesEntries
{
    internal bool CanOfferPindianTopChoice(CharacterState owner) => _resolve().CanOfferPindianTopChoice(owner);
    internal int[] CurrentPindianProcessingIds(PindianFrame frame) => _resolve().CurrentPindianProcessingIds(frame);
    internal int EffectivePindianRank(CharacterState owner, Card card) => _resolve().EffectivePindianRank(owner, card);
    internal bool HasCurrentPindianSourceCard(PindianFrame frame) => _resolve().HasCurrentPindianSourceCard(frame);
    internal int ReserveOrReadPindianTop(CharacterState owner, bool reserve) => _resolve().ReserveOrReadPindianTop(owner, reserve);
}
