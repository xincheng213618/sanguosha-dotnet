#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineHulaoEquipmentRuntime
{
    internal string? HulaoEquipmentSkillId(CardKind kind) => _coreEquipmentContentOperations.EquipmentRules.GetSkillBinding(kind)is { } binding && binding.SkillId.StartsWith("hulao:", StringComparison.Ordinal) ? binding.SkillId : null;
    internal string? HulaoEquipmentSkillGrantId(Card card) => HulaoEquipmentSkillId(card.Kind)is null ? null : _coreEquipmentSkillsOperations.EquipmentSkillGrantId(card);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27500)]
    internal bool AllowsHulaoEquipmentTrigger(ProgramTriggerLegalityContext gate)
    {
        var kind = gate.SkillId switch
        {
            "hulao:wushuang-halberd" => CardKind.WushuangHalberd,
            "hulao:purple-gold-crown" => CardKind.PurpleGoldCrown,
            "hulao:red-cotton-robe" => CardKind.RedCottonRobe,
            "hulao:lion-belt" => CardKind.LionBelt,
            _ => (CardKind? )null
        };
        if (kind is null)
            return true;
        if (!_corePhysicalOwnedCardQueriesOperations.GetEquipment(gate.Owner).Any(card => card.Kind == kind && HulaoEquipmentSkillGrantId(card) == gate.Candidate.SkillInstanceId))
            return false;
        if (kind == CardKind.PurpleGoldCrown)
            return true;
        if (kind == CardKind.WushuangHalberd)
        {
            // A damage transfer or chained elemental copy is not damage to this
            // Slash's target. Virtual Slash still has a genuine native use/action.
            return _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { IsChainPropagation: false, IsSourceLess: false } attack && attack.SourceSeat == gate.OwnerSeat && attack.EffectiveCardKind is { } slash && CoreNativeCardClassificationsRuntime.IsSlashCard(slash) && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { Action: { Type: CardActionType.Use } action } && action.ActorSeat == gate.OwnerSeat && CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) && action.EffectiveDesignatedTargetSeats.Contains(attack.TargetSeat);
        }

        if (_coreExtendedViewAsOperations.IsArmorIneffectiveForTurn(gate.Owner))
            return false;
        if (kind == CardKind.RedCottonRobe)
            return _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } damage && damage.TargetSeat == gate.OwnerSeat && !damage.IgnoresArmor && !_coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(damage.ResolutionId, gate.OwnerSeat);
        if (gate.WindowContext?.CardUse is not { } cardUse || _coreCardUseLifecycleOperations.LifecycleCardUse(cardUse.ParentCardUseFrameId)is not { Action: { } useAction } use || useAction.ActionId != cardUse.CardActionId || useAction.ActorSeat == gate.OwnerSeat || !useAction.EffectiveDesignatedTargetSeats.SequenceEqual(new[] { gate.OwnerSeat }) || use.IgnoresArmor || _coreDirectedCardPolicyQueriesOperations.HasDirectedCardArmorBypass(use.Id, gate.OwnerSeat))
            return false;
        return !CoreNativeCardClassificationsRuntime.IsSlashCard(useAction.EffectiveKind) || !_coreDirectedCardPolicyQueriesOperations.HasCardArmorBypass(_coreMatchParticipantsPlayersAccess._players[useAction.ActorSeat], gate.Owner, useAction.EffectiveKind);
    }
}
