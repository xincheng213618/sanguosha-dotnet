namespace CardGame.Core;

internal static class PostCostSlashRedirectContract
{
    internal static void Validate(string path, IReadOnlyList<SkillProgramEffect> effects, SkillProgramTriggerWindow? window)
    {
        if (!effects.Any(e => e.SlashRedirectPostCostRange is not null)) return;
        if (window != SkillProgramTriggerWindow.SlashTargetRedirecting || effects is not
            [{ Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.SlashRedirectable } select,
             { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Target: SkillProgramEffectTarget.Owner,
               Amount: 1, Destination: SkillProgramCardDestination.DiscardPile, AwaitMovementTriggers: true } pay,
             { Op: SkillProgramEffectOp.RedirectCurrentAttack, Target: SkillProgramEffectTarget.SelectedTarget } redirect] ||
            select.Condition.Kind != SkillProgramConditionKind.Always || pay.Condition.Kind != SkillProgramConditionKind.Always ||
            redirect.Condition.Kind != SkillProgramConditionKind.Always || pay.ChooserRef?.Kind != ProgramParticipantRef.Owner ||
            pay.CardOwnerRef?.Kind != ProgramParticipantRef.Owner || pay.Zones.Count != 2 ||
            !pay.Zones.Contains(CardZoneKind.Hand) || !pay.Zones.Contains(CardZoneKind.Equipment) ||
            pay.SkipIfNoCards || pay.AllowDecline || pay.RevealBeforeMove || pay.ResultBind is not null ||
            pay.CoverageResultBind is not null || pay.CardKinds.Count != 0 || pay.CardCategories.Count != 0 || pay.Suits.Count != 0)
            throw new InvalidOperationException($"Invalid skill program at {path}: post-cost Slash redirect requires target selection, one awaited own HE discard and the original attack redirect.");
    }
}

public sealed partial class GameEngine
{
    private bool UsesPostCostSlashRedirectRange(ProgramSkillFrame frame) =>
        ProgramInstructionResolver.Default.Resolve(frame, _contentRegistry.GetSkill(frame.SkillId).Program!).Features
            .ForOperation(SkillProgramEffectOp.SelectAndMoveOwnedCard).Any(e => e.SlashRedirectPostCostRange == true);

    private ProgramSkillFrame? PostCostSlashRedirectFrame(CardAttackHandle attack, int owner) =>
        _resolutionStack.OfType<ProgramSkillFrame>().LastOrDefault(frame => frame.OwnerSeat == owner &&
            frame.WindowContext is { Window: SkillProgramTriggerWindow.SlashTargetRedirecting, CardUse: { } use } &&
            use.ParentCardUseFrameId == attack.ResolutionId && UsesPostCostSlashRedirectRange(frame));

    private bool IsPostCostSlashRedirectTarget(CardAttackHandle attack, int owner, int target, int? excludedEquipment)
    {
        if (owner == attack.SourceSeat || !IsProgramSlashRedirectBaseTarget(attack, owner, target)) return false;
        return HasShenTaishiCiPoweiRange(owner, target) || HasSayingRoundRange(owner, target) ||
            IsGameFactionAttackRangeTarget(owner, target) ||
            ConvertRuleValue(EvaluateDistance(_players[owner], _players[target], excludedEquipment)) <=
            ConvertRuleValue(EvaluateAttackRange(_players[owner], excludedEquipment));
    }

    private bool IsPostCostSlashRedirectPaymentPair(ProgramSkillFrame frame, CardAttackHandle attack,
        int target, CardLocation source, Card card) =>
        source.OwnerSeat == frame.OwnerSeat && source.Zone is CardZoneKind.Hand or CardZoneKind.Equipment &&
        _cardZones.GetLocation(card.Id) == source &&
        !IsSelfHandCategoryDiscardForbidden(frame.OwnerSeat, card, source, OwnedCardMoveIntent.Discard) &&
        IsPostCostSlashRedirectTarget(attack, frame.OwnerSeat, target,
            source.Zone == CardZoneKind.Equipment ? card.Id : null);

    private bool HasPostCostSlashRedirectPayment(ProgramSkillFrame frame, CardAttackHandle attack, int target)
    {
        // The shared enumerator applies real HE discard/source-equipment restrictions.
        // During target selection there is no selected target, so the payment hook
        // is inert; each surviving entity is tested against this candidate once.
        return BuildOwnedCardPaymentChoices(frame.Id, frame.OwnerSeat, frame.OwnerSeat,
                [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard)
            .Where(choice => choice.Cards.Count == 1).Any(choice =>
            {
                var card = GetAttackCard(choice.Cards[0]);
                return IsPostCostSlashRedirectPaymentPair(frame, attack, target, _cardZones.GetLocation(card.Id), card);
            });
    }

    private bool AllowsPostCostSlashRedirectPayment(ProgramSkillFrame frame, CardLocation source, Card card)
    {
        if (GetOwnedSelectionEffect(frame).SlashRedirectPostCostRange != true) return true;
        return frame.SelectedTargetSeats is [var target] && ProgramCardAttack is { } attack &&
            SameAttackOwner(ActiveCardAttack, attack) && attack.TargetSeat == frame.OwnerSeat &&
            IsPostCostSlashRedirectPaymentPair(frame, attack, target, source, card);
    }
}
