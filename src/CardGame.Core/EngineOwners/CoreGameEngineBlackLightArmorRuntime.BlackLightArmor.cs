#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineBlackLightArmorRuntime
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62051)]
    internal bool CanOfferBlackLightArmor(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BlackLightArmorContract.Skill)
            return true;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity, TargetSeat: { } target } || target != candidate.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(target) || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || _coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(_coreMatchParticipantsPlayersAccess._players[target]) || _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target]).SingleOrDefault(card => card.Kind == CardKind.BlackLightArmor && _coreEquipmentSkillsOperations.IsActiveProgramSourceEquipmentCard(target, candidate.SkillId, candidate.SkillInstanceId, card))is null || _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(frame => frame.Id == context.ParentFrameId)is not { } window || window.ParentFrameId != identity.ParentCardUseFrameId || window.Action.ActionId != identity.CardActionId || _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == window.ParentFrameId)is not { Action: { Type: CardActionType.Use } action } use || use.SourceSeat != action.ActorSeat || action.ActorSeat != identity.ActorSeat || action.EffectiveKind != identity.EffectiveKind || action.ActionId != identity.CardActionId || window.Action.ActionId != action.ActionId || !window.Action.TargetSeats.SequenceEqual(use.TargetSeats) || use.IneffectiveTargetSeats?.Contains(target) == true)
            return false;
        var primary = use.CardKind == CardKind.BorrowedSword ? use.TargetSeats.Where((_, index) => index % 2 == 0).Distinct().ToArray() : use.TargetSeats.Distinct().ToArray();
        if (primary.Length <= 1 || !primary.Contains(target) || CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && _coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[action.ActorSeat], _coreMatchParticipantsPlayersAccess._players[target], use.CardKind) || _coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(use.Id, target) || _coreCurrentCardEnhancementsOperations.HasCurrentCardEnhancement(use.Id, CurrentCardEnhancement.IgnoreArmor) || use.IgnoresArmor || use.CardAttack is { IgnoresArmor: true } attack && attack.TargetSeat == target)
            return false;
        var trick = CardCatalog.Get(use.CardKind).CategoryName == "锦囊牌";
        return CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || trick && (RuntimeSkills.RuntimeRules_Core_MouLuMeng.IsDamageCardKind(use.CardKind) || use.CardKind == CardKind.UnexpectedAssault || action.EffectiveIsRed == false || action.EffectiveIsRed is null && action.EffectiveSuit is Suit.Spade or Suit.Club);
    }
}
