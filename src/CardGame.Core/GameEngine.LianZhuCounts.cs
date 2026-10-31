namespace CardGame.Core;

internal interface ILianZhuGiftSelectionQueries : IProgramSelectionQuery
{
    bool CanDeliver(int ownerSeat, int recipientSeat, int cardId);
}

[ProgramSelectionModule(ProgramSelectionStage.AfterTargets, 22400, SkillProgramEffectOp.RevealHandGiftAndResolveRecipientChoice)]
internal sealed class RecipientHpGiftSelectionFilter : IProgramSelectionCandidateFilter
{
    public bool Apply(ProgramSelectionContext context, ProgramSelectionCandidates candidates, IProgramSelectionQueries queries)
    {
        if (context.Features.First(SkillProgramEffectOp.RevealHandGiftAndResolveRecipientChoice)?.NumberExpression != SkillProgramNumberExpression.RecipientCurrentHp)
            return true;
        var participants = queries.Get<IProgramSelectionSourceQuery>().Participants();
        if (!participants.Any(p => p.Seat == context.OwnerSeat && p.IsAlive && p.HasHpValue)) return false;
        var delivery = queries.Get<ILianZhuGiftSelectionQueries>();
        candidates.Targets = candidates.Targets.Where(seat => participants.Any(p => p.Seat == seat && p.IsAlive && p.HasHpValue && p.Hp > 0) &&
            candidates.Cards.Any(id => delivery.CanDeliver(context.OwnerSeat, seat, id))).ToArray();
        candidates.Cards = candidates.Cards.Where(id => candidates.Targets.Any(seat => delivery.CanDeliver(context.OwnerSeat, seat, id))).ToArray();
        return candidates.Targets.Length > 0 && candidates.Cards.Length > 0;
    }
}

[ProgramSelectionValidator(ProgramSelectionValidationStage.AfterCardFilters, 22400, SkillProgramEffectOp.RevealHandGiftAndResolveRecipientChoice)]
internal sealed class RecipientHpGiftSelectionValidator : IProgramSelectionValidator
{
    public CommandError? Validate(ProgramSelectionValidationRequest request, IProgramSelectionQueries queries)
    {
        if (request.Context.Features.First(SkillProgramEffectOp.RevealHandGiftAndResolveRecipientChoice)?.NumberExpression != SkillProgramNumberExpression.RecipientCurrentHp)
            return null;
        var participants = queries.Get<IProgramSelectionSourceQuery>().Participants();
        return request.Cards is [var cardId] && request.Targets is [var recipient] &&
            participants.Any(p => p.Seat == request.Context.OwnerSeat && p.IsAlive && p.HasHpValue) &&
            participants.Any(p => p.Seat == recipient && p.IsAlive && p.HasHpValue && p.Hp > 0) &&
            queries.Get<ILianZhuGiftSelectionQueries>().CanDeliver(request.Context.OwnerSeat, recipient, cardId)
            ? null : new(CommandErrorCode.IllegalAction, "动态赠牌须有真实体力值，且材料必须能实际交入受赠者手中。");
    }
}

[ProgramSelectionModule(ProgramSelectionStage.AiHint, 22400, SkillProgramEffectOp.RevealHandGiftAndResolveRecipientChoice)]
internal sealed class RecipientHpGiftSelectionAi : IProgramSelectionAiDecorator
{
    public SkillProgramAiHint Decorate(ProgramSelectionContext context, SkillProgramAiHint hint, IProgramSelectionQueries queries)
    {
        if (context.Features.First(SkillProgramEffectOp.RevealHandGiftAndResolveRecipientChoice)?.NumberExpression != SkillProgramNumberExpression.RecipientCurrentHp)
            return hint;
        var source = queries.Get<IProgramSelectionSourceQuery>();
        var delivery = queries.Get<ILianZhuGiftSelectionQueries>();
        var targets = source.Participants().Where(p => p.Seat != context.OwnerSeat && p.IsAlive && p.HasHpValue && p.Hp > 0).ToArray();
        var ownCards = source.Hand(context.OwnerSeat).Concat(source.Equipment(context.OwnerSeat))
            .Where(card => context.Activation.SourceZones.Contains(source.Location(card.Id).Zone) &&
                targets.Any(target => delivery.CanDeliver(context.OwnerSeat, target.Seat, card.Id))).ToArray();
        // An opportunity prior over the giver's own possible materials. Only
        // public target HP enters; no recipient card faces or deck order do.
        var blackPrior = ownCards.Length == 0 ? 0d : ownCards.Count(card => source.EffectiveSuit(context.OwnerSeat, card) is Suit.Spade or Suit.Club) / (double)ownCards.Length;
        var expected = targets.Select(target => target.Hp).OfType<int>().DefaultIfEmpty(0).Max() * blackPrior;
        return hint with { OwnerDraw = checked(hint.OwnerDraw + (int)Math.Round(expected, MidpointRounding.AwayFromZero)), GivesSelected = true };
    }
}
