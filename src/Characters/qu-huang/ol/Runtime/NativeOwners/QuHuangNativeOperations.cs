#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class QuHuangNativeOperations(global::CardGame.Core.NativeOwners.KernelAdvanceRulesAndPublishStateCall advanceRulesAndPublishState, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeCardUseEntries> getCoreNativeCardUseRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeNullificationEntries> getCoreNativeNullificationRuntime, global::System.Func<global::CardGame.Core.RuntimeEntries.CoreNativeTrickEffectsEntries> getCoreNativeTrickEffectsRuntime, global::CardGame.Core.EngineStates.CoreMatchParticipantsState matchParticipantsState) : IQuHuangNativeOperations
{
    public void PublishState() => advanceRulesAndPublishState();
    public long BeginCardUse(Card card, int actor, IReadOnlyList<int> targets, CardKind kind, CardConversionSource source, QuHuangNishouUseOrigin origin) => getCoreNativeCardUseRuntime().BeginCardUse(card, actor, targets, kind, conversionSource: source, quHuangNishou: origin);
    public void BeginTrickWindow(long id, Card card, int actor, IReadOnlyList<int> targets, LegalActionKind kind, CardKind playedKind) => getCoreNativeNullificationRuntime().BeginJizhiOrNullificationWindow(id, card, actor, targets, kind, playedCardKind: playedKind);
    public void SkipUnavailableTarget(NullificationWindowFrame window, int target, CardEffectSkipReason reason) => getCoreNativeTrickEffectsRuntime().SkipUnavailableTargetCardEffect(window, matchParticipantsState.ReferenceToPlayers[target], reason);
}
