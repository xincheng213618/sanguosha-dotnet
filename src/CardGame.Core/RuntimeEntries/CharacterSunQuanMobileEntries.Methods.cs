#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CharacterSunQuanMobileEntries
{
    internal void ApplyNativePeachRecoveryScope(long resolutionId, global::CardGame.Core.CharacterState source, global::CardGame.Core.CharacterState target, global::CardGame.Core.CardKind playedCardKind, ref int recoveryAmount, ref global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.ProgramRecoveryPolicySource> recoveryPolicySources) => _resolve().ApplyNativePeachRecoveryScope(resolutionId, source, target, playedCardKind, ref recoveryAmount, ref recoveryPolicySources);
}
