#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterCaoPiOlGodRuntime
{
    OlCaoPiPlayer global::CardGame.Core.IOlShenCaoPiFacts.Player(int seat) => new(seat, _coreMatchParticipantsPlayersAccess._players[seat].IsAlive, _coreMatchParticipantsPlayersAccess._players[seat].IsHuman, _coreMatchParticipantsPlayersAccess._players[seat].MaxHp);
    bool global::CardGame.Core.IOlShenCaoPiFacts.ValidSeat(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat);
    long global::CardGame.Core.IOlShenCaoPiFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    Winner global::CardGame.Core.IOlShenCaoPiFacts.Winner => _coreMatchOutcomeWinnerAccess._winner;

    ContentSkillDefinition global::CardGame.Core.IOlShenCaoPiFacts.Skill(string skillId) => _contentRegistryOperations._contentRegistry.GetSkill(skillId);
    IEnumerable<T> global::CardGame.Core.IOlShenCaoPiFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<CardMovementRecord> global::CardGame.Core.IOlShenCaoPiFacts.Movements => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.AsReadOnly();

    IReadOnlyList<SkillGrant> global::CardGame.Core.IOlShenCaoPiFacts.Grants(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].SkillGrants.Grants;
    IReadOnlyList<PublicPersistentPileSource> global::CardGame.Core.IOlShenCaoPiFacts.ReferencedPiles(int owner, string skillId, string instance) => _corePublicPileSourcesOperations.ReferencedPublicPileSources(owner, skillId, instance);
    IReadOnlyList<Card> global::CardGame.Core.IOlShenCaoPiFacts.PileCards(PublicPersistentPileSource source) => _corePublicPileSourcesOperations.PublicPileCards(source);
    IReadOnlyList<Card> global::CardGame.Core.IOlShenCaoPiFacts.Hand(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]);
    bool global::CardGame.Core.IOlShenCaoPiFacts.StorageCard(int owner, string skillId, string instance, Card card, CardLocation location) => _corePublicPileCashOutOperations.PublicPileStorageCard(owner, skillId, instance, card, location);
    bool global::CardGame.Core.IOlShenCaoPiFacts.ExactAwakeningParent(ProgramSkillFrame frame) => _coreGameActivationAwakeningOperations.ExactGameActivationAwakeningParent(frame);
    int global::CardGame.Core.IOlShenCaoPiFacts.KeepValue(int owner, Card card) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(card, _coreMatchParticipantsPlayersAccess._players[owner]);
    void global::CardGame.Core.IOlShenCaoPiOperations.MoveCards(IReadOnlyList<Card> cards, CardLocation from, CardLocation to, CardMoveReason reason) => _corePhysicalMovementFacadesOperations.MoveCards(cards, from, to, reason);
    void global::CardGame.Core.IOlShenCaoPiOperations.MoveCard(Card card, CardLocation from, CardLocation to, CardMoveReason reason, CardMovementDisclosure disclosure) => _corePhysicalMovementFacadesOperations.MoveCard(card, from, to, reason, disclosure: disclosure);
    PublicPersistentPileSource global::CardGame.Core.IOlShenCaoPiOperations.EnsurePileSource(ProgramSkillFrame frame, int capacity) => _corePublicPileSourcesOperations.EnsurePublicPileSource(frame, capacity);
    bool global::CardGame.Core.IOlShenCaoPiOperations.DrainChildWindows(long frameId) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(frameId, PostEventContinuation.Program);
    void global::CardGame.Core.IOlShenCaoPiOperations.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IOlShenCaoPiOperations.GrantSkills(ProgramSkillFrame frame, IReadOnlyList<string> skillIds) => _coreProgramSkillChangesOperations.GrantProgramSkills(frame, skillIds);
    void global::CardGame.Core.IOlShenCaoPiOperations.ObserveSkillRevisions()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.Revision);
    }

    bool global::CardGame.Core.IOlShenCaoPiOperations.RemoveGrant(int owner, string grantId) => _coreMatchParticipantsPlayersAccess._players[owner].SkillGrants.RemoveGrant(grantId);
}
