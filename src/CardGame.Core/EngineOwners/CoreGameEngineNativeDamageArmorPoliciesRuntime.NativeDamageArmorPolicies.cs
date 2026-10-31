#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineNativeDamageArmorPoliciesRuntime
{
    internal bool HasNativeDamageArmorPolicy(int sourceSeat, bool sourceLess, SkillProgramCardPolicyKind kind, IReadOnlySet<string>? visibleSkillIds = null) => !sourceLess && _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) && _coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[sourceSeat], kind).Any(binding => (visibleSkillIds is null || visibleSkillIds.Contains(binding.Source.SkillId)) && (binding.Source.SkillId != "shanhe:duanhun-14079" || _coreDuanhunOperations.IsActiveDuanhunPolicySource(sourceSeat, binding.Source)));
    internal bool BypassesNativeDamageArmor(global::CardGame.Core.GameEngine.IDamageAttempt attack) => _coreWushuangCombatTacticsOperations.WushuangArmorBypass(attack) || _coreDuanhunOperations.DuanhunBypassesNativeDamageArmor(attack.SourceSeat, attack.IsSourceLess) || attack.SourceSeat != attack.TargetSeat && HasNativeDamageArmorPolicy(attack.SourceSeat, attack.IsSourceLess, SkillProgramCardPolicyKind.BypassOtherParticipantArmor);
    internal int NativeDamageArmorMultiplier(global::CardGame.Core.GameEngine.IDamageAttempt attack) => Math.Max(_coreWushuangCombatTacticsOperations.WushuangArmorDamageMultiplier(attack), HasNativeDamageArmorPolicy(attack.SourceSeat, attack.IsSourceLess, SkillProgramCardPolicyKind.DoubleArmorDamage) ? 2 : 1);
}
