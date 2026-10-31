#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineCardSubsetViewsRuntime
{
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 11)]
    internal CardSnapshot[] GetPrivateSubsetCards(int viewerSeat)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Choices.Count: > 0 } prompt || prompt.PlayerSeat != viewerSeat || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || frame.OwnerSeat != viewerSeat || prompt.Choices[0].Parameters.GetValueOrDefault("program-action") != "select-subset" || prompt.Choices[0].Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(System.Globalization.CultureInfo.InvariantCulture))
            return[];
        var source = frame.CardSetBindings.SingleOrDefault(binding => binding.Name == prompt.Choices[0].Parameters.GetValueOrDefault("source-bind"));
        if (source is not { Visibility: SkillProgramCardSetVisibility.Private })
            return[];
        // Only this chooser's current private Processing cards are newly exposed.
        // Owned hand subsets already use the normal hand view and payment controls.
        var processing = _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing);
        return source.CardIds.Where(prompt.ValidCardIds.Contains).Select(id => processing.SingleOrDefault(card => card.Id == id)).OfType<Card>().Select(_coreSnapshotValueProjectionOperations.ToSnapshot).ToArray();
    }
}
