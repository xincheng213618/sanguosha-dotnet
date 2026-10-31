#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOtherParticipantHealthReplacementEntries
{
    internal bool TryGetOtherParticipantHpReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, scoped out global::CardGame.Core.CardUseEffectSource source) => _resolve().TryGetOtherParticipantHpReplacement(attack, out source);
}
