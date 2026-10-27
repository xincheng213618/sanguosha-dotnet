namespace CardGame.Core;

[ProgramPassiveFactRequirements("shanhe:black-light-armor-6011", ProgramTrackedFact.NativeActionAppearance)]
internal static class BlackLightArmorContract
{
    internal const string Skill = "shanhe:black-light-armor-6011";

    [ProgramCatalogValidation(62051)]
    internal static void Validate(string path, SkillProgram program)
    {
        if (program.Id != Skill) return;
        if (program.Activations.Count != 0 || program.ViewAs.Count != 0 || program.Modifiers.Count != 0 ||
            program.CardPolicies.Count != 0 || program.DamageModifiers.Count != 0 || program.Contributions.Count != 0 ||
            program.Triggers is not [{ Id: "multi-target-immunity", Window: SkillProgramTriggerWindow.CardUseTargetsFinalized,
                OwnerRelation: SkillProgramCardActionOwnerRelation.Target, Optional: false,
                Condition.Kind: SkillProgramTriggerConditionKind.Always,
                Effects: [{ Op: SkillProgramEffectOp.NullifyCurrentCardEffect, Target: SkillProgramEffectTarget.Owner,
                    Condition.Kind: SkillProgramConditionKind.Always }] } trigger] ||
            trigger.CardKinds.Count != 0 || trigger.Suits.Count != 0 || trigger.UsageScope is not null ||
            trigger.UsageLimit is not null || trigger.DynamicUsageLimit is not null || trigger.ChoiceGroup is not null ||
            trigger.SourceSkillId is not null || trigger.SourceViewAsId is not null ||
            trigger.CardCategories.Count != 2 || !trigger.CardCategories.Contains(SkillProgramCardCategory.Basic) ||
            !trigger.CardCategories.Contains(SkillProgramCardCategory.Trick))
            throw new InvalidOperationException(path + ": Black Light Armor requires its single locked finalized-target native effect nullification.");
    }
}

public sealed partial class GameEngine
{
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62051)]
    private bool CanOfferBlackLightArmor(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger,
        ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != BlackLightArmorContract.Skill) return true;
        if (context is not { Window: SkillProgramTriggerWindow.CardUseTargetsFinalized, CardUse: { } identity,
                TargetSeat: { } target } || target != candidate.OwnerSeat || !IsValidPlayerSeat(target) ||
            !_players[target].IsAlive || IsArmorIneffectiveForTurn(_players[target]) ||
            GetEquipment(_players[target]).SingleOrDefault(card => card.Kind == CardKind.BlackLightArmor &&
                IsActiveProgramSourceEquipmentCard(target, candidate.SkillId, candidate.SkillInstanceId, card)) is null ||
            _resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(frame => frame.Id == context.ParentFrameId) is not { } window ||
            window.ParentFrameId != identity.ParentCardUseFrameId || window.Action.ActionId != identity.CardActionId ||
            _resolutionStack.OfType<CardUseFrame>().SingleOrDefault(frame => frame.Id == window.ParentFrameId) is not { Action: { Type: CardActionType.Use } action } use ||
            use.SourceSeat != action.ActorSeat || action.ActorSeat != identity.ActorSeat || action.EffectiveKind != identity.EffectiveKind ||
            action.ActionId != identity.CardActionId || window.Action.ActionId != action.ActionId ||
            !window.Action.TargetSeats.SequenceEqual(use.TargetSeats) || use.IneffectiveTargetSeats?.Contains(target) == true)
            return false;

        var primary = use.CardKind == CardKind.BorrowedSword
            ? use.TargetSeats.Where((_, index) => index % 2 == 0).Distinct().ToArray()
            : use.TargetSeats.Distinct().ToArray();
        if (primary.Length <= 1 || !primary.Contains(target) ||
            IsSlashCard(use.CardKind) && HasCardArmorBypass(_players[action.ActorSeat], _players[target], use.CardKind) ||
            HasDirectedCardArmorBypass(use.Id, target) || HasCurrentCardEnhancement(use.Id, CurrentCardEnhancement.IgnoreArmor) ||
            use.IgnoresArmor || use.CardAttack is { IgnoresArmor: true } attack && attack.TargetSeat == target)
            return false;

        var trick = CardCatalog.Get(use.CardKind).CategoryName == "锦囊牌";
        return IsSlashCard(use.CardKind) || trick &&
            (global::CardGame.Core.RuntimeSkills.RuntimeRules_Core_MouLuMeng.IsDamageCardKind(use.CardKind) ||
             use.CardKind == CardKind.UnexpectedAssault ||
             action.EffectiveIsRed == false || action.EffectiveIsRed is null && action.EffectiveSuit is Suit.Spade or Suit.Club);
    }
}
