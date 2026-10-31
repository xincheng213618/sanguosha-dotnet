namespace CardGame.Core;

// The rule protocols retain their own validation. Projection only asks whether
// an existing receipt authorizes its faces; it never reconstructs that proof.
internal interface IProgramPublicCardProtocolQueries : IProgramSelectionQuery
{
    string CompletedUndamagedRevealBind { get; }
    bool ValidMouLuSuReceipt(ProgramSkillFrame frame);
    bool ValidRecipientCategoryMarkReceipt(ProgramSkillFrame frame);
}

[ProgramRuntimeModule]
internal sealed class ProgramPublicCardProjector(INativeFrameRoutingFacts frames,
    ICardZoneQueries zones, IProgramEventHistoryQueries history,
    IProgramCardSnapshotQueries snapshots, IProgramContentQueries content,
    IProgramPublicCardProtocolQueries protocols)
{
    [ProgramViewContributor(ProgramViewContribution.ProgramPublicCards, 10)]
    internal IReadOnlyList<CardSnapshot> GetProgramPublicCards()
    {
        HashSet<int>? publicIds = null;
        var handControl = false;
        var handDraft = false;
        var comparison = false;
        var completedReveal = false;
        var categoryGift = false;
        for (var index = 0; index < frames.FrameCount; index++)
        {
            if (frames.FrameAt(index) is not ProgramSkillFrame frame) continue;
            for (var bindingIndex = 0; bindingIndex < frame.CardSetBindings.Count; bindingIndex++)
            {
                var binding = frame.CardSetBindings[bindingIndex];
                if (binding.Visibility != SkillProgramCardSetVisibility.Public) continue;
                for (var cardIndex = 0; cardIndex < binding.CardIds.Count; cardIndex++)
                    AddPublicId(binding.CardIds[cardIndex]);
            }
            handControl |= frame.HandControlDraft is
                { Operation: SkillProgramEffectOp.RevealHandColorDiscardAndTake, Stage: "color", RevealedCardIds: not null };
            handDraft |= frame.PublicHandDraft is { Stage: >= 2 };
            comparison |= frame.HandComparisonDraft is not null;
            completedReveal |= frame.CompletedUndamagedTargetReveal is { SelectionIssued: true, RevealedMaterials.Count: > 0 };
            categoryGift |= frame.RecipientCategoryMark is
                { Operation: SkillProgramEffectOp.GiveHandAndGrantCategoryMark,
                  Stage: RecipientCategoryMarkStage.GiftChildren or RecipientCategoryMarkStage.Complete, Token: not null };
            if (frame.PhaseNamePrediction is { Stage: PhaseNamePredictionStage.Guessing, RevealedCardId: { } predictionId })
                AddPublicId(predictionId);
        }
        if (handControl) foreach (var id in HandControlPublicCardIds()) AddPublicId(id);
        if (handDraft) foreach (var id in PublicHandDraftCardIds()) AddPublicId(id);
        if (content.HasTriggerOperation(SkillProgramEffectOp.ResolveMouLuSuMengmou))
            foreach (var id in MouLuSuPublicShownIds()) AddPublicId(id);
        if (comparison) foreach (var id in HandComparisonPublicIds()) AddPublicId(id);

        // Receipt-backed faces precede current physical appearances. Query these
        // protocols only when a live frame can expose a card, preserving their
        // validation and duplicate checks even when the same ID is bound below.
        List<CardSnapshot>? cards = null;
        HashSet<int>? shownIds = null;
        if (completedReveal) foreach (var card in CompletedUndamagedTargetRevealPublicCards()) AddCard(card);
        if (categoryGift) foreach (var card in RecipientCategoryMarkPublicCards()) AddCard(card);
        if (publicIds is not null)
        {
            var orderedIds = publicIds.ToArray();
            Array.Sort(orderedIds);
            foreach (var cardId in orderedIds)
            {
                var location = zones.GetLocation(cardId);
                AddCard(snapshots.Snapshot(zones.CardsAt(location).Single(card => card.Id == cardId)));
            }
        }
        return cards is null ? Array.Empty<CardSnapshot>() : cards.ToArray();

        void AddPublicId(int id) => (publicIds ??= []).Add(id);
        void AddCard(CardSnapshot card)
        {
            if ((shownIds ??= []).Add(card.Id)) (cards ??= []).Add(card);
        }
    }

    private IEnumerable<ProgramSkillFrame> ProgramFrames()
    {
        // Read the scheduler in place. Empty snapshots allocate no frame array.
        for (var index = 0; index < frames.FrameCount; index++)
            if (frames.FrameAt(index) is ProgramSkillFrame frame) yield return frame;
    }

    // Public only while the real revealed hand awaits its color payment. Resolve
    // the selected IDs through the physical zones in the final projection pass.
    private IEnumerable<int> HandControlPublicCardIds() => ProgramFrames()
        .Where(frame => frame.HandControlDraft is
            { Operation: SkillProgramEffectOp.RevealHandColorDiscardAndTake, Stage: "color", RevealedCardIds: not null })
        .SelectMany(frame => frame.HandControlDraft!.RevealedCardIds!);

    private IEnumerable<int> PublicHandDraftCardIds() => ProgramFrames()
        .Where(frame => frame.PublicHandDraft is { Stage: >= 2 })
        .SelectMany(frame => frame.PublicHandDraft!.CardIds
            .Where(id => zones.GetLocation(id) == CardLocation.Hand(frame.OwnerSeat)));

    private IEnumerable<int> MouLuSuPublicShownIds() => ProgramFrames()
        .Where(frame => frame.MouLuSuAnniversary is
            { Operation: SkillProgramEffectOp.ResolveMouLuSuMingshi,
              Stage: MouLuSuAnniversaryStage.Recipient or MouLuSuAnniversaryStage.Gift or MouLuSuAnniversaryStage.GiftChildren } &&
            protocols.ValidMouLuSuReceipt(frame))
        .SelectMany(frame => frame.MouLuSuAnniversary!.Cards
            .Where(id => zones.GetLocation(id) == CardLocation.Hand(frame.OwnerSeat)));

    private IEnumerable<int> HandComparisonPublicIds() => ProgramFrames()
        .Where(frame => frame.HandComparisonDraft is not null)
        .SelectMany(frame => frame.HandComparisonDraft!.RevealedIds.Prepend(frame.HandComparisonDraft.SourceCardId))
        .Distinct();

    private IReadOnlyList<CardSnapshot> CompletedUndamagedTargetRevealPublicCards()
    {
        var frameIds = ProgramFrames()
            .Where(frame => frame.CompletedUndamagedTargetReveal is { SelectionIssued: true, RevealedMaterials.Count: > 0 })
            .Select(frame => frame.Id).ToHashSet();
        return frameIds.Count == 0 ? [] : Array.AsReadOnly(history.Read<ProgramCardsRevealedEvent>()
            .Where(shown => frameIds.Contains(shown.FrameId) && shown.Bind == protocols.CompletedUndamagedRevealBind)
            .SelectMany(shown => shown.Cards).DistinctBy(card => card.Id).ToArray());
    }

    private IReadOnlyList<CardSnapshot> RecipientCategoryMarkPublicCards() => Array.AsReadOnly(ProgramFrames()
        .Where(frame => frame.RecipientCategoryMark is
            { Operation: SkillProgramEffectOp.GiveHandAndGrantCategoryMark,
              Stage: RecipientCategoryMarkStage.GiftChildren or RecipientCategoryMarkStage.Complete, Token: not null } &&
            protocols.ValidRecipientCategoryMarkReceipt(frame))
        .SelectMany(frame => history.Read<ProgramCardsRevealedEvent>().Where(shown => shown.FrameId == frame.Id && shown.SkillId == frame.SkillId &&
            shown.BindingId == frame.TriggerId && shown.OwnerSeat == frame.RecipientCategoryMark!.HolderSeat && shown.Bind == "category-mark-gift")
            .SelectMany(shown => shown.Cards))
        .GroupBy(card => card.Id).Select(group => group.Single()).ToArray());
}
