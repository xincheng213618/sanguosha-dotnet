namespace CardGame.Core;

internal interface IProvidedSlashPaymentTargets : IProgramSelectionQuery
{
    bool CanUse(NativeOwners.FactionCardRequestHandle request, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false);
    bool CanSupply(NativeOwners.FactionCardRequestHandle request, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false);
}

/// <summary>Checks a faction request against its actual provider materials, rather than preview privileges.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProvidedSlashPaymentTargetsRuntime(IMatchParticipantStateAccess participants,
    RuntimeEntries.CoreGameEngineAlternatingSuitAndCompletedTopEntries suits,
    RuntimeEntries.CoreGameEngineTurnRedSlashBenefitsEntries colors,
    RuntimeEntries.CoreGameEngineFinalTargetSlashEntries ranks,
    RuntimeEntries.CoreNativeEquipmentAndFactionQueriesEntries targets,
    RuntimeEntries.CoreNativeBorrowedSwordEntries borrowedSword,
    RuntimeEntries.CoreNativeQinglongEntries qinglong,
    RuntimeEntries.CoreGameEngineAssistedFactionSlashEntries assisted,
    RuntimeEntries.CoreNativeFactionSlashEntries requests) : IProvidedSlashPaymentTargets
{
    public bool CanUse(NativeOwners.FactionCardRequestHandle request, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false)
    {
        if (!EngineOwners.CoreGameEngineCardConversionsRuntime.IsFactionSlashUse(request)) return true;
        if (request.TargetSeat is not { } targetSeat || !EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind)) return false;

        var actor = participants.Players[request.OwnerSeat];
        var target = participants.Players[targetSeat];
        var ids = cards.Select(card => card.Id).ToArray();
        // Empty material is a known suitless, rank-zero use, not an unknown preview.
        var suit = cards.Count == 0 ? Suit.None : suits.PhysicalGroupSuit(actor, cards);
        var color = colors.PhysicalGroupColor(actor, cards);
        var rank = cards.Count == 0 ? 0 : ranks.ProvidedSpecificSlashRank(actor,
            participants.Players[request.CurrentCandidateSeat], cards, isTrueZhangba, kind);

        if (request.IsBorrowedSwordUse)
            return borrowedSword.IsLegalBorrowedSwordSlashTarget(actor, target, kind, suit, false, color, rank, physicalCardIds: ids);
        if (request.IsQinglongCrescentBladeUse)
            return qinglong.CanUseQinglongCrescentBladeTarget(actor, target, kind, color, false, suit, rank, physicalCardIds: ids);
        if (request.IsAssistedProgramUse)
            return assisted.IsAssistedProvidedSlashTarget(actor.Seat, targetSeat, kind, rank, allowPotentialRank: false, physicalCardIds: ids, physicalSuit: suit, effectiveColor: color);
        return targets.CanUseProvidedSlashTarget(actor, target, kind, suit, false, color, rank, physicalCardIds: ids);
    }

    public bool CanSupply(NativeOwners.FactionCardRequestHandle request, CardKind kind, IReadOnlyList<Card> cards, bool isTrueZhangba = false) =>
        CanUse(request, kind, cards, isTrueZhangba) || cards.Count > 0 &&
        requests.CanConvertFactionSlashToFire(request, participants.Players[request.CurrentCandidateSeat], cards, kind) &&
        CanUse(request, CardKind.FireSlash, cards, isTrueZhangba);
}
