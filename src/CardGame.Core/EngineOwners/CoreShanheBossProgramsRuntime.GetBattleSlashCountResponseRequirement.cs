#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreShanheBossProgramsRuntime
{
    internal int GetBattleSlashCountResponseRequirement(CharacterState owner, int sourceSeat, CardKind incomingKind, CardKind requiredKind)
    {
        if (sourceSeat != owner.Seat || !owner.IsAlive || !_coreProgramCardPoliciesOperations.CardPolicies(owner, SkillProgramCardPolicyKind.BattleSlashCountMinimumResponses, incomingKind, requiredKind).Any())
            return 1;
        // Declarations include the currently resolving Slash, native conversions and
        // virtual uses. A Slash merely played as a response has no use declaration.
        // Reading the original facts also preserves the count across actual turns,
        // skill loss/reacquisition, and command replay without a parallel counter.
        var used = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Where(fact => fact.SourceSeat == owner.Seat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(fact.CardKind)).Select(fact => fact.ResolutionId).Distinct().Take(5).Count();
        return Math.Clamp(used, 1, 5);
    }
}
