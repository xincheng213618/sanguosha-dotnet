#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanMobileRuntime
{
    internal void ApplyNativePeachRecoveryScope(long resolutionId, CharacterState source, CharacterState target, CardKind playedCardKind, ref int recoveryAmount, ref IReadOnlyList<ProgramRecoveryPolicySource> recoveryPolicySources)
    {
        if (playedCardKind != CardKind.Peach || source.Seat == target.Seat || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Id == resolutionId)is not { CardKind: CardKind.Peach, Action: { Type: CardActionType.Use, EffectiveKind: CardKind.Peach } action } use || use.SourceSeat != source.Seat || action.ActorSeat != source.Seat || !use.TargetSeats.Contains(target.Seat) || !action.EffectiveDesignatedTargetSeats.Contains(target.Seat))
            return;
        var faction = _coreMatchSetupDefinitionsOperations.GetEffectiveFactionId(source);
        var existingSources = recoveryPolicySources;
        var extra = _coreProgramCardPoliciesOperations.CardPolicies(target, SkillProgramCardPolicyKind.RescueRecoveryBonus, CardKind.Peach).Where(item => item.Policy.PeachRecoveryScope == SkillProgramPeachRecoveryScope.NativePeach && string.Equals(faction, item.Policy.FactionId, StringComparison.Ordinal)).Where(item => !existingSources.Any(existing => existing.SkillId == item.Source.SkillId && existing.PolicyId == item.Policy.Id)).ToArray();
        if (extra.Length == 0)
            return;
        recoveryAmount = checked(recoveryAmount + extra.Sum(item => item.Policy.Value));
        recoveryPolicySources = Array.AsReadOnly(recoveryPolicySources.Concat(extra.Select(item => new ProgramRecoveryPolicySource(item.Source.SkillId, item.Policy.Id))).Distinct().ToArray());
    }
}
