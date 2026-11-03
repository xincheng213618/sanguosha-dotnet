namespace CardGame.Core;

internal sealed partial class MouXiaoQiaoOlRuntime
{
    internal bool JueyanAvailable(int owner, int cardId)
    {
        if (!facts.Alive(owner) || facts.Location(cardId) != CardLocation.Hand(owner)) return false;
        var kind = facts.EffectiveKind(owner, cardId);
        var ordinary = IsOrdinary(kind); var redBasic = CardCatalog.Get(kind).CategoryName == "基本牌" && facts.EffectiveRed(owner, cardId) == true;
        return (ordinary || redBasic) && !facts.History<MouXiaoQiaoJueyanUsedEvent>().Any(e =>
            e.Source.OwnerSeat == owner && e.Round == facts.Round && e.RedBasic == redBasic);
    }
    internal static bool IsOrdinary(CardKind kind) => CardCatalog.Get(kind).CategoryName == "锦囊牌" &&
        kind is not (CardKind.Indulgence or CardKind.SupplyShortage or CardKind.Lightning);
    [ProgramSelectionModule(ProgramSelectionStage.CardAfterDeclaration, 34000, SkillProgramEffectOp.ResolveMouXiaoQiaoJueyan)]
    internal sealed class JueyanCards : IProgramSelectionCardFilter
    {
        public bool Allows(ProgramSelectionContext c, Card card, IProgramSelectionQueries queries)
        {
            var runtime = queries.Get<IMouXiaoQiaoOlFacts>();
            var kind = runtime.EffectiveKind(c.OwnerSeat, card.Id);
            var redBasic = CardCatalog.Get(kind).CategoryName == "基本牌" && runtime.EffectiveRed(c.OwnerSeat, card.Id) == true;
            return runtime.Location(card.Id) == CardLocation.Hand(c.OwnerSeat) && (IsOrdinary(kind) || redBasic) &&
                !runtime.History<MouXiaoQiaoJueyanUsedEvent>().Any(e => e.Source.OwnerSeat == c.OwnerSeat && e.Round == runtime.Round && e.RedBasic == redBasic) &&
                runtime.Sources(c.OwnerSeat, c.SkillId).Any(source => runtime.NativePlans(c.OwnerSeat, kind, new(c.SkillId, c.Activation.Id, c.OwnerSeat, source.SkillInstanceId)).Count > 0);
        }
    }
    private void BeginJueyan(ProgramSkillFrame f)
    {
        if (f.SkillId != Jueyan || f.ActivationId != "retained-hand-use" || f.SelectedCardIds is not [var id] ||
            !JueyanAvailable(f.OwnerSeat, id)) throw new InvalidOperationException("Jueyan requires one currently legal retained Hand entity and an unspent Round branch.");
        var kind = facts.EffectiveKind(f.OwnerSeat, id); var redBasic = !IsOrdinary(kind);
        if (f.MouXiaoQiaoJueyanResponse is not null ? !facts.IsResponseRequest(f) : facts.NativePlans(f.OwnerSeat, kind, Source(f)).Count == 0)
            throw new InvalidOperationException("Jueyan cannot retain an otherwise unusable card.");
        var r = f.MouXiaoQiaoOl! with { Paid = true, Stage = MouXiaoQiaoOlStage.RevealChildren, SelectedCard = id,
            OutputKind = kind, ShownSuit = facts.EffectiveSuit(f.OwnerSeat, id), RedBasic = redBasic };
        frames.ReplaceTop(f = f with { MouXiaoQiaoOl = r });
        events.AdvanceFact(new MouXiaoQiaoJueyanUsedEvent(f.Id, r.Source, f.GameplayHash, r.Round, id, kind, r.ShownSuit, redBasic));
        operations.Reveal(f, [id]); Continue(f.Id);
    }
    private void AfterJueyanReveal(ProgramSkillFrame f)
    {
        if (f.MouXiaoQiaoOl is not { Operation: SkillProgramEffectOp.ResolveMouXiaoQiaoJueyan, Paid: true } r)
            throw new InvalidOperationException("Jueyan cannot issue a native Use without its exact retained-Hand public reveal.");
        if (f.MouXiaoQiaoJueyanResponse is not null)
        { events.AdvanceFact(new MouXiaoQiaoOlCompletedEvent(f.Id, r.Operation)); operations.ReturnResponse(f); return; }
        frames.ReplaceTop(f = f with { MouXiaoQiaoOl = r with { Stage = MouXiaoQiaoOlStage.UseChoice } }); Publish(f);
    }
    [ProgramAnswerBefore(34000)]
    internal bool AnswerResponse(PromptChoice choice) => choice.Parameters.GetValueOrDefault("response") == "mou-xiao-qiao-jueyan" && operations.BeginResponse(choice);
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 34000)]
    internal void CapturePublicHandReveal(IGameEvent payload)
    {
        if (!facts.Enabled || !facts.SetupComplete || facts.Turn <= 0 || facts.Round <= 0) return;
        var (parentId, ids) = facts.PublicReveal(payload);
        if (ids.Count == 0 || frames.Find(parentId) is not { } parent) return;
        foreach (var owner in Enumerable.Range(0, facts.PlayerCount).Where(facts.Alive))
        {
            var source = facts.Sources(owner, Jueyan).FirstOrDefault(); if (source is null) continue;
            foreach (var card in facts.Hand(owner).Where(c => ids.Contains(c.Id)))
            {
                var suit = facts.EffectiveSuit(owner, card.Id);
                if (suit == Suit.None || facts.History<MouXiaoQiaoJueyanSuitClaimedEvent>().Any(e => e.Source.OwnerSeat == owner && e.Round == facts.Round && e.Suit == suit)) continue;
                var actualSource = new CardConversionSource(source.SkillId, source.BindingId, owner, source.SkillInstanceId);
                var claim = new PublicHandRevealClaim(parentId, actualSource, source.GameplayHash, facts.Round, suit, card.Id,
                    RetainOwnerDeath: true);
                reveals.Issue(parentId, claim);
                events.AdvanceFact(new MouXiaoQiaoJueyanSuitClaimedEvent(parentId, actualSource, source.GameplayHash, facts.Round, suit, card.Id));
            }
        }
    }
}
