#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanMobileEntries
{
    internal void ApplyNativePeachRecoveryScope(long resolutionId, CharacterState source, CharacterState target, CardKind playedCardKind, ref int recoveryAmount, ref IReadOnlyList<ProgramRecoveryPolicySource> recoveryPolicySources) => _resolve().ApplyNativePeachRecoveryScope(resolutionId, source, target, playedCardKind, ref recoveryAmount, ref recoveryPolicySources);
}
