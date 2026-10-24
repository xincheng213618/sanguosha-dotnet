namespace CardGame.Core;

public sealed record HulaoInitialEquipmentPlacedEvent(int BossSeat) : IGameEvent;

public sealed partial class GameEngine
{
    private string? HulaoEquipmentSkillId(CardKind kind) =>
        EquipmentRules.GetSkillBinding(kind) is { } binding && binding.SkillId.StartsWith("hulao:", StringComparison.Ordinal)
            ? binding.SkillId : null;

    private string? HulaoEquipmentSkillGrantId(Card card) =>
        HulaoEquipmentSkillId(card.Kind) is null ? null : EquipmentSkillGrantId(card);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 27500)]
    private bool AllowsHulaoEquipmentTrigger(ProgramTriggerLegalityContext gate)
    {
        var kind = gate.SkillId switch
        {
            "hulao:wushuang-halberd" => CardKind.WushuangHalberd,
            "hulao:purple-gold-crown" => CardKind.PurpleGoldCrown,
            "hulao:red-cotton-robe" => CardKind.RedCottonRobe,
            "hulao:lion-belt" => CardKind.LionBelt,
            _ => (CardKind?)null
        };
        if (kind is null) return true;
        if (!GetEquipment(gate.Owner).Any(card => card.Kind == kind &&
            HulaoEquipmentSkillGrantId(card) == gate.Candidate.SkillInstanceId)) return false;
        if (kind == CardKind.PurpleGoldCrown) return true;
        if (kind == CardKind.WushuangHalberd)
        {
            // A damage transfer or chained elemental copy is not damage to this
            // Slash's target. Virtual Slash still has a genuine native use/action.
            return CurrentDamageAttempt is { IsChainPropagation: false, IsSourceLess: false } attack &&
                attack.SourceSeat == gate.OwnerSeat && attack.EffectiveCardKind is { } slash && IsSlashCard(slash) &&
                LifecycleCardUse(attack.ResolutionId) is { Action: { Type: CardActionType.Use } action } &&
                action.ActorSeat == gate.OwnerSeat && IsSlashCard(action.EffectiveKind) &&
                action.EffectiveDesignatedTargetSeats.Contains(attack.TargetSeat);
        }
        if (IsArmorIneffectiveForTurn(gate.Owner)) return false;
        if (kind == CardKind.RedCottonRobe)
            return CurrentDamageAttempt is { } damage && damage.TargetSeat == gate.OwnerSeat &&
                !damage.IgnoresArmor && !HasDirectedCardArmorBypass(damage.ResolutionId, gate.OwnerSeat);
        if (gate.WindowContext?.CardUse is not { } cardUse ||
            LifecycleCardUse(cardUse.ParentCardUseFrameId) is not { Action: { } useAction } use ||
            useAction.ActionId != cardUse.CardActionId || useAction.ActorSeat == gate.OwnerSeat ||
            !useAction.EffectiveDesignatedTargetSeats.SequenceEqual(new[] { gate.OwnerSeat }) ||
            use.IgnoresArmor || HasDirectedCardArmorBypass(use.Id, gate.OwnerSeat)) return false;
        return !IsSlashCard(useAction.EffectiveKind) ||
            !HasCardArmorBypass(_players[useAction.ActorSeat], gate.Owner, useAction.EffectiveKind);
    }

}
