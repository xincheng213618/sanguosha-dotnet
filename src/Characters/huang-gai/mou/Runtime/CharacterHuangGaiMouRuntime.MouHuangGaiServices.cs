#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangGaiMouRuntime
{
    int global::CardGame.Core.IMouHuangGaiFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.IMouHuangGaiFacts.TurnOwner => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    long global::CardGame.Core.IMouHuangGaiFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouHuangGaiFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    int global::CardGame.Core.IMouHuangGaiFacts.LivingCount => _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive);

    bool global::CardGame.Core.IMouHuangGaiFacts.Alive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouHuangGaiFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    bool global::CardGame.Core.IMouHuangGaiFacts.OwnPlay(int seat) => seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play;
    bool global::CardGame.Core.IMouHuangGaiFacts.LiejiParent(ProgramSkillFrame f) => MouHgLiejiParent(f);
    bool global::CardGame.Core.IMouHuangGaiFacts.HasNativeUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not null;
    bool global::CardGame.Core.IMouHuangGaiFacts.SameChoice(PromptChoice left, PromptChoice right) => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([left], [right]);
    string global::CardGame.Core.IMouHuangGaiFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    SkillProgram? global::CardGame.Core.IMouHuangGaiFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program;
    SkillPromptPresentation global::CardGame.Core.IMouHuangGaiFacts.Presentation(string id)
    {
        var s = _contentRegistryOperations._contentRegistry.GetSkill(id);
        return new(s.Id, s.Name, s.Name, s.Description);
    }

    double global::CardGame.Core.IMouHuangGaiFacts.TargetScore(int owner, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, new(0, 0, 1, 0, 0, 0, false, false));
    IReadOnlyList<int> global::CardGame.Core.IMouHuangGaiFacts.Seats() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(p => p.Seat).ToArray());
    IReadOnlyList<int> global::CardGame.Core.IMouHuangGaiFacts.DamageHand(int owner) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(c => global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], c))).Select(c => c.Id).ToArray());
    IReadOnlyList<int> global::CardGame.Core.IMouHuangGaiFacts.Targets(ProgramSkillFrame f) => MouHgTargets(f);
    IReadOnlyList<int> global::CardGame.Core.IMouHuangGaiFacts.ProcessingReveals(ProgramSkillFrame f) => Array.AsReadOnly(f.MouHuangGai!.Reveals.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing).Select(c => c.CardId).ToArray());
    IReadOnlyList<MouHgLiejiGrant> global::CardGame.Core.IMouHuangGaiFacts.Grants(int owner) => Array.AsReadOnly((_coreMatchParticipantsPlayersAccess._players[owner].MouHgLiejiGrants ?? []).ToArray());
    MouHgUseFacts? global::CardGame.Core.IMouHuangGaiFacts.Use(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { Type: CardActionType.Use } a } u ? new(u.Id, a.ActionId, Array.AsReadOnly(a.PhysicalCards.Select(c => c.CardId).ToArray()), u.MouHgBonuses is { } b ? Array.AsReadOnly(b.ToArray()) : null) : null;
    IEnumerable<T> global::CardGame.Core.IMouHuangGaiFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    void global::CardGame.Core.IMouHuangGaiNative.SetGrants(int owner, IReadOnlyList<MouHgLiejiGrant>? grants) => _coreMatchParticipantsPlayersAccess._players[owner].MouHgLiejiGrants = grants is null ? null : Array.AsReadOnly(grants.ToArray());
    void global::CardGame.Core.IMouHuangGaiNative.SetUseBonuses(long useId, IReadOnlyList<MouHgUseBonus> bonuses) => _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { MouHgBonuses = bonuses });
    MouHgReveal? global::CardGame.Core.IMouHuangGaiNative.Reveal(ProgramSkillFrame f, string binding)
    {
        var before = f.MouHuangGai!.Before;
        _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, f.OwnerSeat, 1, null, binding, SkillProgramCardSetVisibility.Public);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var card = _coreProgramLifecycleOperations.GetProgramCardSet(f, binding).CardIds.SingleOrDefault();
        if (card == 0)
            return null;
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Last(m => m.Sequence > before && m.CardId == card && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing);
        return new(card, actual.CardKind, actual.Sequence);
    }

    void global::CardGame.Core.IMouHuangGaiNative.Obtain(ProgramSkillFrame f, IReadOnlyList<int> ids) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(MouHuangGaiRuntime.ObtainReason), (_, moves) => MouHuangGaiRules.MovesPaid(f.Id, moves), disclosure: CardMovementDisclosure.FaceUp);
    void global::CardGame.Core.IMouHuangGaiNative.DiscardSlash(ProgramSkillFrame f)
    {
        var shown = f.MouHuangGai!.Reveals[^1];
        if (_cardZonesOperations._cardZones.GetLocation(shown.CardId) == CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(shown.CardId), CardLocation.Processing, CardLocation.DiscardPile, new(MouHuangGaiRuntime.DiscardReason), m => MouHuangGaiRules.MovesPaid(f.Id, [m]));
    }

    void global::CardGame.Core.IMouHuangGaiNative.Slash(ProgramSkillFrame f, int target) => IssueMouHgSlash(f, target);
    bool global::CardGame.Core.IMouHuangGaiNative.Drain(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    void global::CardGame.Core.IMouHuangGaiNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IMouHuangGaiNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
}
