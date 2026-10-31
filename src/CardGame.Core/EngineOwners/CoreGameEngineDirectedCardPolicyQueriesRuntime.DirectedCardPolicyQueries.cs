#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineDirectedCardPolicyQueriesRuntime
{
    internal bool HasCardDistanceExemption(CharacterState actor, CharacterState target, CardKind kind, long? cardUseFrameId = null, int? excludedEquipmentId = null, EquipmentSlot? excludedEquipmentSlot = null, IReadOnlyList<int>? physicalCardIds = null) => _coreProgramDistanceRulesOperations.DistanceRules.HasCardUseExemption(new(actor, target, kind, cardUseFrameId, excludedEquipmentId, excludedEquipmentSlot) { PhysicalCardIds = physicalCardIds });
    internal bool HasCardArmorBypass(CharacterState actor, CharacterState target, CardKind kind) => global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _characterHuaXiongMouMobileOperations.HasMouHuaXiongWei(actor.Seat) || global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && _coreProgramCardPoliciesOperations.HasCardPolicy(actor, SkillProgramCardPolicyKind.HellSlashIgnoreArmor, kind) || _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(actor) || _coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(target) || _coreDirectedTurnCardPoliciesOperations.HasDirectedTurnCardPolicy(actor.Seat, target.Seat, kind, DirectedTurnCardPolicyEffect.IgnoreArmor) || _characterGaoShunBoundaryMobileOperations.HasBoundaryGaoShunArmorBypass(actor.Seat, target.Seat, kind);
    internal bool HasYuanchouCardPolicy => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasCardPolicy(SkillProgramCardPolicyKind.BlackSlashIgnoresArmorBothDirections);

    internal bool HasDirectedCardArmorBypass(long resolutionId, int targetSeat)
    {
        var action = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == resolutionId)?.Action;
        // Damage sources can change (for example in a duel). The policy belongs
        // to the original card actor and its actual declared targets.
        return _characterFanJiangZhangDaOlOperations.HasYuanchouArmorBypass(resolutionId, targetSeat) || _coreOriginalTargetAdditionsOperations.HasIssuedOriginalTargetArmorBypass(resolutionId, targetSeat) || action is not null && action.TargetSeats.Contains(targetSeat) && _coreDirectedTurnCardPoliciesOperations.HasDirectedTurnCardPolicy(action.ActorSeat, targetSeat, action.EffectiveKind, DirectedTurnCardPolicyEffect.IgnoreArmor);
    }

    internal bool HasSlashAllowanceForAnyTarget(CharacterState actor, IEnumerable<CardKind>? kinds = null) => _coreMatchParticipantsPlayersAccess._players.Any(target => target.IsAlive && target.Seat != actor.Seat && (kinds ?? global::CardGame.Core.EngineOwners.CoreGameEngineDirectedCardPolicyQueriesDefinitions.SlashKinds).Any(kind => !_coreCardUseModulesOperations.IsCardUseForbidden(actor.Seat, kind, CardActionType.Use) && !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target.Seat, kind) && (_coreRuleQueriesOperations.CanSpendSlashUse(actor, target, ignoresCount: false, kind) || _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Any(card => _coreProgramAllHandBonusOperationOperations.BypassesSlashLimitBySuit(actor, card, kind) || _coreJudgedRankSlashPoliciesOperations.HasJudgedRankSlashQuota(actor.Seat, kind, _coreFinalTargetSlashOperations.SpecificSlashRank(actor, card, kind))))));
    internal bool CanUseGlobalCard(CharacterState actor, CardKind kind, bool excludeOwner = false) => GetDeclaredGlobalCardTargets(actor, kind).Where(target => !excludeOwner || target != actor.Seat).All(target => !_coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(actor.Seat, target, kind));
    internal IEnumerable<int> GetDeclaredGlobalCardTargets(CharacterState actor, CardKind kind) => _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive && !_coreProgramTargetRulesOperations.TargetRules.IsDeclaredGlobalProhibited(new(actor.Seat, player.Seat, kind)) && (kind is CardKind.PeachGarden or CardKind.FiveGrains or CardKind.AllianceFeast || player.Seat != actor.Seat)).Select(player => player.Seat);
    internal IEnumerable<int> GetDeclaredCardTargets(CharacterState actor, LegalActionKind actionKind, IReadOnlyList<int> targets) => actionKind switch
    {
        LegalActionKind.BarbarianAssault => GetDeclaredGlobalCardTargets(actor, CardKind.BarbarianAssault),
        LegalActionKind.ArrowBarrage => GetDeclaredGlobalCardTargets(actor, CardKind.ArrowBarrage),
        LegalActionKind.PeachGarden => GetDeclaredGlobalCardTargets(actor, CardKind.PeachGarden),
        LegalActionKind.FiveGrains => GetDeclaredGlobalCardTargets(actor, CardKind.FiveGrains),
        LegalActionKind.BorrowedSword => targets.Take(1),
        _ => targets
    };
    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 200)]
    internal bool DistancePhysicalSlash200HasCardDistanceExemption(ProgramSlashDistanceQuery q) => HasCardDistanceExemption(q.Actor, q.Target, q.Kind, q.ExistingUseFrameId);
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 100)]
    [ProgramDistanceRule(ProgramDistanceStage.ProvidedSlash, 80)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedSlash, 160)]
    [ProgramDistanceRule(ProgramDistanceStage.BorrowedPhysicalSlash, 100)]
    internal bool DistanceVirtualSlash100HasCardDistanceExemption(ProgramSlashDistanceQuery q) => HasCardDistanceExemption(q.Actor, q.Target, q.Kind);
}
