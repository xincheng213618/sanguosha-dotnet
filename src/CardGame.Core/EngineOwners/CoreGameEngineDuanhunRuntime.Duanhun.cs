#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDuanhunRuntime
{
    internal const string DuanhunSkillId = "shanhe:duanhun-14079";
    internal bool DuanhunBypassesNativeDamageArmor(int sourceSeat, bool sourceLess, IReadOnlySet<string>? visibleSkillIds = null) => !sourceLess && _coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat) && _coreMatchParticipantsPlayersAccess._players[sourceSeat].IsAlive && _coreProgramCardPoliciesOperations.CardPolicies(_coreMatchParticipantsPlayersAccess._players[sourceSeat], SkillProgramCardPolicyKind.BypassOtherParticipantArmor).Any(binding => binding.Source.SkillId == DuanhunSkillId && (visibleSkillIds is null || visibleSkillIds.Contains(binding.Source.SkillId)) && IsActiveDuanhunPolicySource(sourceSeat, binding.Source));
    internal bool IsActiveDuanhunPolicySource(int ownerSeat, IndexedSkillProgramInstance source) => source.SkillId == DuanhunSkillId && _coreTableSeatQueriesOperations.IsValidPlayerSeat(ownerSeat) && source.SkillInstanceId is { } instance && _coreMatchParticipantsPlayersAccess._players[ownerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[ownerSeat], source.SkillId, instance) && _characterHuCheErMouOperations.KongwuEffectiveEquipment(_coreMatchParticipantsPlayersAccess._players[ownerSeat]).Any(card => card.Id > 0 && !card.IsGeneralWeapon && card.Kind == CardKind.Duanhun && _cardZonesOperations._cardZones.GetLocation(card.Id) == CardLocation.Equipment(ownerSeat) && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(ownerSeat, source.SkillId, instance, card));
}
