#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineOtherParticipantHealthReplacementRuntime
{
    internal bool TryGetOtherParticipantHpReplacement(global::CardGame.Core.GameEngine.IDamageAttempt attack, out CardUseEffectSource source)
    {
        if (_coreGuanduGeneralQualificationOperations.TryGetGuanduIncomingHpReplacement(attack, out source))
            return true;
        source = null !;
        if (!_contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.OtherParticipantDamageBecomesHpLoss) || attack.IsSourceLess || attack.IsDelayedJudgmentDamage || attack.SourceSeat == attack.TargetSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.TargetSeat) || !_coreMatchParticipantsPlayersAccess._players[attack.TargetSeat].IsAlive || attack.DamageAmount <= 0)
            return false;
        // Card-use base additions (e.g. Alcohol) already belong to the attempt.
        // Replace before damage-only modifiers, prevention, armor and elemental chain issuance.
        foreach (var seat in new[]
        {
            attack.SourceSeat,
            attack.TargetSeat
        }

        )
        {
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                continue;
            var binding = _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramCardPolicyKind.OtherParticipantDamageBecomesHpLoss).FirstOrDefault();
            if (binding.Source is null)
                continue;
            source = new(binding.Source.SkillId, binding.Policy.Id, seat, binding.Source.SkillInstanceId);
            return true;
        }

        return false;
    }
}
