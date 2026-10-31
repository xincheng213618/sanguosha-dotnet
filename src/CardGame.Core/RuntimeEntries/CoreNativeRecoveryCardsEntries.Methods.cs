#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreNativeRecoveryCardsEntries
{
    internal void CompleteRecoveryCardUse(CharacterState source, CharacterState target, Card card, long resolutionId, CardKind playedCardKind, int recoveryAmount, IReadOnlyList<ProgramRecoveryPolicySource> recoveryPolicySources) => _resolve().CompleteRecoveryCardUse(source, target, card, resolutionId, playedCardKind, recoveryAmount, recoveryPolicySources);
    internal void ResolveDyingAlcohol(CharacterState source, CharacterState target, Card alcohol, CardConversionSource? conversionSource = default, DyingResponseEvent? dyingResponse = default) => _resolve().ResolveDyingAlcohol(source, target, alcohol, conversionSource, dyingResponse);
    internal void ResolvePeach(CharacterState player, Card peach) => _resolve().ResolvePeach(player, peach);
    internal void ResolvePeach2(CharacterState source, CharacterState target, Card peach, bool allowDying, CardConversionSource? conversionSource = default, DyingResponseEvent? dyingResponse = default) => _resolve().ResolvePeach(source, target, peach, allowDying, conversionSource, dyingResponse);
    internal void ResolveRecoveryCard(CharacterState source, CharacterState target, Card card, string cardName, CardKind? playedCardKind = default, int recoveryAmount = 1, IReadOnlyList<(string SkillId, string PolicyId)>? recoveryPolicySources = default, CardConversionSource? conversionSource = default, DyingResponseEvent? dyingResponse = default, IReadOnlyList<Card>? physicalCards = default, DrawFundedDistinctBasicPayment? drawFundedPayment = default, HuanpeiPaidOrigin? huanpeiOrigin = default) => _resolve().ResolveRecoveryCard(source, target, card, cardName, playedCardKind, recoveryAmount, recoveryPolicySources, conversionSource, dyingResponse, physicalCards, drawFundedPayment, huanpeiOrigin);
}
