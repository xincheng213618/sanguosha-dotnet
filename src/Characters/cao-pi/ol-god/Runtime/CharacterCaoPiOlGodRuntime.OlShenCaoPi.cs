#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiOlGodRuntime
{
    OlCaoPiPlayer IOlShenCaoPiFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].MaxHp);
    long IOlShenCaoPiFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    Winner IOlShenCaoPiFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    ContentSkillDefinition IOlShenCaoPiFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IEnumerable<T> IOlShenCaoPiFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    IReadOnlyList<CardMovementRecord> IOlShenCaoPiFacts.Movements => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.AsReadOnly();

    IReadOnlyList<SkillGrant> IOlShenCaoPiFacts.Grants(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants;
    IReadOnlyList<PublicPersistentPileSource> IOlShenCaoPiFacts.ReferencedPiles(int owner, string skillId, string instance) => _corePublicPileSourcesOperations.ReferencedPublicPileSources(owner, skillId, instance);
    IReadOnlyList<Card> IOlShenCaoPiFacts.PileCards(PublicPersistentPileSource source) => _corePublicPileSourcesOperations.PublicPileCards(source);
    IReadOnlyList<Card> IOlShenCaoPiFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool IOlShenCaoPiFacts.StorageCard(int owner, string skillId, string instance, Card card, CardLocation location) => _corePublicPileCashOutOperations.PublicPileStorageCard(owner, skillId, instance, card, location);
    bool IOlShenCaoPiFacts.ExactAwakeningParent(ProgramSkillFrame frame) => _coreGameActivationAwakeningOperations.ExactGameActivationAwakeningParent(frame);
    int IOlShenCaoPiFacts.KeepValue(int owner, Card card) => CoreNativeAiDecisionsRuntime.GetKeepValue(card, _coreMatchParticipantsPlayersAccess._players[owner]);
    void IOlShenCaoPiOperations.MoveCards(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason) => _corePhysicalMovementFacadesOperations.MoveCards(cards, from, to, reason);
    void IOlShenCaoPiOperations.MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, reason, disclosure: disclosure);
    PublicPersistentPileSource IOlShenCaoPiOperations.EnsurePileSource(ProgramSkillFrame frame, int capacity) => _corePublicPileSourcesOperations.EnsurePublicPileSource(frame, capacity);
    bool IOlShenCaoPiOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.Program);
    void IOlShenCaoPiOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IOlShenCaoPiOperations.GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => _coreProgramSkillChangesOperations.GrantProgramSkills(frame, skillIds);
    void IOlShenCaoPiOperations.ObserveSkillRevisions()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
    }

    bool IOlShenCaoPiOperations.RemoveGrant(int owner, string grantId) => _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.RemoveGrant(grantId);
}
