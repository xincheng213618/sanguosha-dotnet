#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanPingMouRuntime
{
    internal MouGuanPingRuntime MouGuanPing => _mouGuanPingOperations.Get();

    int IMouGuanPingFacts.Turn => _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber;

    int IMouGuanPingFacts.TurnOwner => _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat;

    int IMouGuanPingFacts.PhaseId => _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId;

    bool IMouGuanPingFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouGuanPingFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool IMouGuanPingFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    SkillProgram IMouGuanPingFacts.Program() => _contentRegistryOperations._contentRegistry.GetSkill(MouGuanPingComposition.Skill).Program!;
    int IMouGuanPingFacts.PhaseUsage(int owner) => _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault((owner, MouGuanPingComposition.Skill, MouGuanPingComposition.Activation));
    IEnumerable<T> IMouGuanPingFacts.History<T>() => _coreProgramEventHistoryOperations.Value.ProgramEventHistory<T>();
    IReadOnlyList<ProgramMultiCardViewAsSelection> IMouGuanPingFacts.Groups(int owner)
    {
        if (!((IMouGuanPingFacts)this).Alive(owner) || !_coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[owner], MouGuanPingComposition.Skill))
            return[];
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        var source = new CardConversionSource(MouGuanPingComposition.Skill, MouGuanPingComposition.Binding, owner, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(actor, MouGuanPingComposition.Skill));
        return Array.AsReadOnly(new[] { true, false }.Select(red => _corePhysicalOwnedCardQueriesOperations.GetHand(actor).Where(c => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(actor, c)) == red).Select(c => c.Id).Order().ToArray()).Where(ids => ids.Length > 0).Select(ids => _coreCardConversionsOperations.FindProgramMultiCardViewAsSelection(actor, ids, CardKind.Slash, false, source)).Where(s => s is not null).Select(s => ProgramSelectionProjection.Freeze(s)!).ToArray());
    }

    IReadOnlyList<int> IMouGuanPingFacts.Targets(int owner, IReadOnlyList<Card> cards)
    {
        var actor = _coreMatchParticipantsPlayersAccess._players[owner];
        var selection = new ProgramMultiCardViewAsSelection(cards, new(MouGuanPingComposition.Skill, MouGuanPingComposition.Binding, owner, _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(actor, MouGuanPingComposition.Skill)), CardKind.Slash);
        return Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(t => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, t, physicalSuit: _coreGuanHaiBridgesOperations.ProgramMultiCardSuit(actor, selection), effectiveColor: _coreGuanHaiBridgesOperations.ProgramMultiCardColor(actor, selection), physicalCardIds: cards.Select(c => c.Id).ToArray(), ignoreDistance: true, conversionSource: selection.Source, countsTowardLimit: false)).Select(t => t.Seat).ToArray());
    }

    bool IMouGuanPingFacts.WholeColor(int owner, IReadOnlyList<int> ids)
    {
        if (ids.Count == 0 || ids.Distinct().Count() != ids.Count || !((IMouGuanPingFacts)this).Alive(owner))
            return false;
        var hand = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[owner]);
        var cards = hand.Where(c => ids.Contains(c.Id)).ToArray();
        if (cards.Length != ids.Count || CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], cards[0]))is not { } red)
            return false;
        return hand.Where(c => CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[owner], c)) == red).Select(c => c.Id).Order().SequenceEqual(ids.Order());
    }

    CardUseFrame? IMouGuanPingFacts.Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouGuanPingComposition.Skill || c.BindingId != MouGuanPingComposition.Trigger || w.Window != SkillProgramTriggerWindow.CardUseCommitted || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } info || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != info.CardActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } current || current.OwnerSeat != c.OwnerSeat || current.SkillId != c.SkillId || current.TriggerId != c.BindingId || current.SkillInstanceId != c.SkillInstanceId || current.GameplayHash != c.GameplayHash)
            return null;
        return _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(u => u.Action?.ActionId == info.CardActionId && u.MouGuanPing is { } r && r.Source.OwnerSeat == c.OwnerSeat && r.Source.SkillInstanceId == c.SkillInstanceId && r.GameplayHash == c.GameplayHash);
    }

    CardUseFrame? IMouGuanPingFacts.Use(long id) => _coreCardUseLifecycleOperations.LifecycleCardUse(id);
    void IMouGuanPingNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void IMouGuanPingNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool IMouGuanPingNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouGuanPingNative.SetMask(ProgramSkillFrame f, int mask)
    {
        if (f.MouGuanPing is not { } r || r.Mask != mask || _coreCardUseLifecycleOperations.LifecycleCardUse(r.Use.UseId)is not { MouGuanPing: { } receipt } use || receipt.ActionId != r.Use.ActionId || receipt != r.Use with
        {
            OptionMask = receipt.OptionMask
        }

        )
            throw new InvalidOperationException("Wuwei may only modify its own original issued Slash.");
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouGuanPing = receipt with { OptionMask = mask } });
    }

    void IMouGuanPingNative.Suppress(ProgramSkillFrame f, int target)
    {
        if (!MouGuanPing.Valid(f) || f.MouGuanPing is not { Stage: MouGuanPingStage.Children } r || !r.Targets.Contains(target))
            throw new InvalidOperationException("Wuwei suppression requires an actual original Slash target.");
        _coreProgramSkillChangesOperations.ApplyProgramSkillState(f, target,
            new(SkillStateAction.Suppress, SkillStateScope.NonLocked, SkillStateDuration.CurrentTurn) { CharacterOnly = false }, []);
    }

    [ProgramCardLegality(ProgramCardLegalityStage.ConfiguredConversionBan, 35000)]
    internal bool MouGuanPingSingleCardAlias(ProgramConfiguredViewAsQuery q) => q.Instance.SkillId == MouGuanPingComposition.Skill;
    [ProgramCardLegality(ProgramCardLegalityStage.ViewAsBan, 35000)]
    internal bool MouGuanPingWholeColorBan(ProgramViewAsLegalityQuery q) => q.Source.SkillId == MouGuanPingComposition.Skill && (q.Source.BindingId != MouGuanPingComposition.Binding || q.Kind != CardKind.Slash || q.ForResponse || q.DyingUse || !_characterGuanPingMouOperations.CreateMouGuanPingFacts().WholeColor(q.Owner.Seat, q.CardIds));
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 35000)]
    internal bool MouGuanPingActualActivationQuota(ProgramSlashQuotaQuery q) => q.Kind == CardKind.Slash && q.PhysicalCardIds is { Count: > 0 } ids && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { SkillId: MouGuanPingComposition.Skill, ActivationId: MouGuanPingComposition.Activation, TriggerId: null, InstructionIndex: 1 } f && f.OwnerSeat == q.Actor.Seat && f.SelectedTargetSeats.Contains(q.Target.Seat) && f.SelectedCardIds.Order().SequenceEqual(ids.Order()) && f.GameplayHash == _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program!.GameplayHash && _coreNationalSkillsOperations.HasRuntimeSkillInstance(q.Actor, f.SkillId, f.SkillInstanceId) && _characterGuanPingMouOperations.CreateMouGuanPingFacts().WholeColor(f.OwnerSeat, ids);
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 35000)]
    internal void IssueMouGuanPingUse(CardUseDeclaredEvent declared)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(declared.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use || a.ConversionChain.SingleOrDefault(s => s.SkillId == MouGuanPingComposition.Skill)is not { BindingId: MouGuanPingComposition.Binding } source)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { SkillId: MouGuanPingComposition.Skill, ActivationId: MouGuanPingComposition.Activation, TriggerId: null, InstructionIndex: 1 } f || f.OwnerSeat != source.OwnerSeat || source.SkillInstanceId != f.SkillInstanceId || a.ActorSeat != f.OwnerSeat || a.ProviderSeat != f.OwnerSeat || a.EffectiveKind != CardKind.Slash || !a.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(f.SelectedCardIds.Order()) || a.PhysicalCards.Any(c => c.From != CardLocation.Hand(f.OwnerSeat)) || !_characterGuanPingMouOperations.CreateMouGuanPingFacts().WholeColor(f.OwnerSeat, f.SelectedCardIds) || use.MouGuanPing is not null)
            throw new InvalidOperationException("Wuwei requires its native activation and all currently payable same-color Hand entities.");
        var types = a.PhysicalCards.Select(c => CardUseCategoryCatalog.Get(c.CardKind)).Select(c => c is CardUseCategories.InstantTrick or CardUseCategories.DelayedTrick ? CardUseCategories.InstantTrick : c).Distinct().Count();
        var extra = _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouGuanPingUseIssuedEvent>().Any(e => e.Receipt.Source.OwnerSeat == f.OwnerSeat && e.Receipt.ActualTurn == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && e.Receipt.TurnOwner == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && e.Receipt.PhaseId == _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId);
        var receipt = new MouGuanPingUseReceipt(use.Id, a.ActionId, f.Id, source, f.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, _coreGameEngineCardUseDebitsCardUseDebitPhaseInstanceIdAccess._cardUseDebitPhaseInstanceId, types, extra);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouGuanPing = receipt, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouGuanPingUseIssuedEvent(receipt));
    }

    [ProgramDistanceRule(ProgramDistanceStage.PhysicalSlash, 35000)]
    [ProgramDistanceRule(ProgramDistanceStage.VirtualSlash, 35000)]
    internal bool MouGuanPingIssuedDistance(ProgramSlashDistanceQuery q) => q.Conversion is { SkillId: MouGuanPingComposition.Skill, BindingId: MouGuanPingComposition.Binding } s && s.OwnerSeat == q.Actor.Seat && (q.ExistingUseFrameId is { } id ? _coreCardUseLifecycleOperations.LifecycleCardUse(id)?.MouGuanPing?.Source == s : q.PhysicalCardIds is { } ids && _characterGuanPingMouOperations.CreateMouGuanPingFacts().WholeColor(q.Actor.Seat, ids));
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 35000)]
    internal int MouGuanPingDamage(GameEngine.IDamageAttempt attack) => !attack.IsChainPropagation && attack is NativeOwners.CardAttackHandle && _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is { Action: { } a, MouGuanPing: { OptionMask: 7 } r } u && r.UseId == u.Id && r.ActionId == a.ActionId && r.Source.OwnerSeat == a.ActorSeat && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<MouGuanPingCompletedEvent>().Count(e => e.UseId == u.Id && e.Mask == 7 && e.Count == 3) == 1 ? 1 : 0;
}
