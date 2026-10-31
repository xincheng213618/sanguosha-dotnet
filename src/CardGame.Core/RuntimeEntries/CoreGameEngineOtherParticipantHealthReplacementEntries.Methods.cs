#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineOtherParticipantHealthReplacementEntries
{
    internal bool TryGetOtherParticipantHpReplacement(GameEngine.IDamageAttempt attack, scoped out CardUseEffectSource source) => _resolve().TryGetOtherParticipantHpReplacement(attack, out source);
}
