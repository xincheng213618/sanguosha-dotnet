#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangGaiMouRuntime
{
    int IMouHuangGaiFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnNumber;

    int IMouHuangGaiFacts.TurnOwner => _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    long IMouHuangGaiFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool IMouHuangGaiFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    int IMouHuangGaiFacts.LivingCount => _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive);

    bool IMouHuangGaiFacts.Alive(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouHuangGaiFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    bool IMouHuangGaiFacts.OwnPlay(int seat) => seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play;
    bool IMouHuangGaiFacts.LiejiParent(ProgramSkillFrame f) => MouHgLiejiParent(f);
    bool IMouHuangGaiFacts.HasNativeUse(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is not null;
    bool IMouHuangGaiFacts.SameChoice(PromptChoice left, PromptChoice right) => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([left], [right]);
    string IMouHuangGaiFacts.Name(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].Name;
    SkillProgram? IMouHuangGaiFacts.Program(string id) => _contentRegistryOperations._contentRegistry.GetSkill(id).Program;
    SkillPromptPresentation IMouHuangGaiFacts.Presentation(string id)
    {
        var s = _contentRegistryOperations._contentRegistry.GetSkill(id);
        return new(s.Id, s.Name, s.Name, s.Description);
    }

    double IMouHuangGaiFacts.TargetScore(int owner, int target) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), target, new(0, 0, 1, 0, 0, 0, false, false));
    IReadOnlyList<int> IMouHuangGaiFacts.Seats() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Select(p => p.Seat).ToArray());
    IReadOnlyList<int> IMouHuangGaiFacts.DamageHand(int owner) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]).Where(c => CoreGameEngineMouLuMengRuntime.IsDamageCardKind(_coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[owner], c))).Select(c => c.Id).ToArray());
    IReadOnlyList<int> IMouHuangGaiFacts.Targets(ProgramSkillFrame f) => MouHgTargets(f);
    IReadOnlyList<int> IMouHuangGaiFacts.ProcessingReveals(ProgramSkillFrame f) => Array.AsReadOnly(f.MouHuangGai!.Reveals.Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Processing).Select(c => c.CardId).ToArray());
    IReadOnlyList<MouHgLiejiGrant> IMouHuangGaiFacts.Grants(int owner) => Array.AsReadOnly((_coreMatchParticipantsPlayersAccess._players[owner].MouHgLiejiGrants ?? []).ToArray());
    MouHgUseFacts? IMouHuangGaiFacts.Use(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { Action: { Type: CardActionType.Use } a } u ? new(u.Id, a.ActionId, Array.AsReadOnly(a.PhysicalCards.Select(c => c.CardId).ToArray()), u.MouHgBonuses is { } b ? Array.AsReadOnly(b.ToArray()) : null) : null;
    IEnumerable<T> IMouHuangGaiFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    void IMouHuangGaiNative.SetGrants(int owner, IReadOnlyList<MouHgLiejiGrant>? grants) => _coreMatchParticipantsPlayersAccess._players[owner].MouHgLiejiGrants = grants is null ? null : Array.AsReadOnly(grants.ToArray());
    void IMouHuangGaiNative.SetUseBonuses(long useId, IReadOnlyList<MouHgUseBonus> bonuses) => _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { MouHgBonuses = bonuses });
    MouHgReveal? IMouHuangGaiNative.Reveal(ProgramSkillFrame f, string binding)
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

    void IMouHuangGaiNative.Obtain(ProgramSkillFrame f, IReadOnlyList<int> ids) => _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(MouHuangGaiRuntime.ObtainReason), (_, moves) => MouHuangGaiRules.MovesPaid(f.Id, moves), disclosure: CardMovementDisclosure.FaceUp);
    void IMouHuangGaiNative.DiscardSlash(ProgramSkillFrame f)
    {
        var shown = f.MouHuangGai!.Reveals[^1];
        if (_cardZonesOperations._cardZones.GetLocation(shown.CardId) == CardLocation.Processing)
            _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(shown.CardId), CardLocation.Processing, CardLocation.DiscardPile, new(MouHuangGaiRuntime.DiscardReason), m => MouHuangGaiRules.MovesPaid(f.Id, [m]));
    }

    void IMouHuangGaiNative.Slash(ProgramSkillFrame f, int target) => IssueMouHgSlash(f, target);
    bool IMouHuangGaiNative.Drain(long id) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id);
    void IMouHuangGaiNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IMouHuangGaiNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
}
