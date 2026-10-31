#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterChengYuMouRuntime
{
    internal MouChengYuAnniversaryRuntime MouChengYuAnniversary => _mouChengYuAnniversaryOperations.Get();
    internal bool HasMouChengYuAnniversary => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ConsumeMouChengYuAnniversaryShizha);

    [ProgramTriggerProof(ProgramTriggerProofPhase.HpChangeOwner, 100)]
    internal bool IsMouCyForeignHealthCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != MouChengYuAnniversaryRuntime.Shizha || candidate.BindingId != "hp-change" || candidate.OccurrenceIndex != 0 || context.Window != SkillProgramTriggerWindow.AfterHealthChanged || context.HpChange is not { Amount: > 0 } change || context.OwnerSeat != candidate.OwnerSeat || context.TargetSeat != change.TargetSeat || context.SourceSeat != change.SourceSeat || context.Amount != change.Amount || change.HpBefore == change.HpAfter || _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is not [{ Op: SkillProgramEffectOp.ResolveMouChengYuAnniversaryShizha }] || _resolutionStackOperations._resolutionStack.OfType<HpChangedTriggerWindowFrame>().LastOrDefault()is not { } hp || hp.Id != context.ParentFrameId || hp.Change.Id != change.Id || hp.Change != change || hp.CandidateIndex < 0 || hp.CandidateIndex >= hp.Candidates.Count || hp.Candidates[hp.CandidateIndex] != candidate)
            return false;
        return true;
    }

    int IMouChengYuAnniversaryFacts.Turn => _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._turnNumber;

    int IMouChengYuAnniversaryFacts.Round => _coreActualRoundProgressRoundNumberAccess._roundNumber;

    long IMouChengYuAnniversaryFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    int IMouChengYuAnniversaryFacts.LivingCount => _coreMatchParticipantsPlayersAccess._players.Count(p => p.IsAlive);

    bool IMouChengYuAnniversaryFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool IMouChengYuAnniversaryFacts.Enabled => HasMouChengYuAnniversary;

    bool IMouChengYuAnniversaryFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool IMouChengYuAnniversaryFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    bool IMouChengYuAnniversaryFacts.OwnPlay(int s) => s == _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._currentSeat && _coreActualTurnProgressCurrentSeatPhaseTurnNumberAccess._phase == TurnPhase.Play;
    string IMouChengYuAnniversaryFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    CardKind IMouChengYuAnniversaryFacts.CardName(CardKind k) => CoreGameEngineNamedAmbushRuntime.AmbushCardName(k);
    SkillProgram? IMouChengYuAnniversaryFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    SkillPromptPresentation IMouChengYuAnniversaryFacts.Presentation(string id)
    {
        var s = _contentRegistryOperations._contentRegistry.GetSkill(id);
        return new(s.Id, s.Name, s.Name, s.Description);
    }

    IEnumerable<T> IMouChengYuAnniversaryFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    IReadOnlyList<int> IMouChengYuAnniversaryFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    IReadOnlyList<ProgramTriggerCandidate> IMouChengYuAnniversaryFacts.Candidates(int owner, SkillProgramTriggerWindow window) => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[owner], window);
    bool IMouChengYuAnniversaryFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId is null)
            return f.SkillId == MouChengYuAnniversaryRuntime.Gaojian && f.ActivationId == "reveal-and-advise" && f.WindowContext is null && ((IMouChengYuAnniversaryFacts)this).History<ProgramSkillStartedEvent>().Count(s => s.FrameId == f.Id && s.OwnerSeat == f.OwnerSeat && s.SkillId == f.SkillId && s.ActivationId == f.ActivationId) == 1;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (at < 1 || f.SkillId != MouChengYuAnniversaryRuntime.Shizha || f.ActivationId != f.TriggerId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        if (f.TriggerId == "hp-change" && _resolutionStackOperations._resolutionStack[at - 1] is HpChangedTriggerWindowFrame hp && w.Window == SkillProgramTriggerWindow.AfterHealthChanged && w.HpChange is { } change && hp.Id == w.ParentFrameId && (hp.Change.Id, hp.Change.ParentFrameId, hp.Change.SourceSeat, hp.Change.TargetSeat, hp.Change.Kind, hp.Change.Amount, hp.Change.HpBefore, hp.Change.HpAfter) == (change.Id, change.ParentFrameId, change.SourceSeat, change.TargetSeat, change.Kind, change.Amount, change.HpBefore, change.HpAfter) && hp.CandidateIndex >= 0 && hp.CandidateIndex < hp.Candidates.Count)
        {
            var c = hp.Candidates[hp.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.BindingId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        if (f.TriggerId == "consume-name" && _resolutionStackOperations._resolutionStack[at - 1] is ProgramCardTriggerWindowFrame p && p.Id == w.ParentFrameId && p.CandidateIndex >= 0 && p.CandidateIndex < p.Candidates.Count && MouCyUse(w)is not null)
        {
            var c = p.Candidates[p.CandidateIndex];
            return c.OwnerSeat == f.OwnerSeat && c.SkillId == f.SkillId && c.TriggerId == f.TriggerId && c.SkillInstanceId == f.SkillInstanceId && c.GameplayHash == f.GameplayHash;
        }

        return false;
    }

    MouCyUse? IMouChengYuAnniversaryFacts.Use(ProgramSkillWindowContext c) => MouCyUse(c);
    bool IMouChengYuAnniversaryFacts.At(int id, CardLocation location) => _cardZonesOperations._cardZones.GetLocation(id) == location;
    bool IMouChengYuAnniversaryFacts.SameChoice(PromptChoice a, PromptChoice b) => CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([a], [b]);
    bool IMouChengYuAnniversaryFacts.DamageCard(CardKind k) => CoreGameEngineMouLuMengRuntime.IsDamageCardKind(k) || CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(k) || k == CardKind.Lightning;
    IReadOnlyList<MouCyCard> IMouChengYuAnniversaryNative.Peek(ProgramSkillFrame f)
    {
        if (!MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary!.Stage != MouCyStage.Peek)
            throw new InvalidOperationException("Shizha requires its paid HP window before privately viewing the real deck.");
        _ = _corePhysicalDrawFacadesOperations.EnsureDrawPile();
        return Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).TakeLast(3).Reverse().Select(c => new MouCyCard(c.Id, c.Kind, c.DisplayName)).ToArray());
    }

    MouCyCard? IMouChengYuAnniversaryNative.Reveal(ProgramSkillFrame f)
    {
        if (!MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary is not { Stage: MouCyStage.RevealChildren, Target: >= 0 } r || !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            throw new InvalidOperationException("Gaojian requires its current living target and own reveal slot.");
        var bind = "gaojian-" + r.RevealCount.ToString(System.Globalization.CultureInfo.InvariantCulture);
        _coreProgramLifecycleOperations.RevealProgramTopCards(f.Id, r.Target, 1, null, bind, SkillProgramCardSetVisibility.Public);
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        var set = _coreProgramLifecycleOperations.GetProgramCardSet(f, bind);
        if (set.CardIds.Count == 0)
            return null;
        var id = set.CardIds.Single();
        var move = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Single(m => m.CardId == id && m.Sequence > r.Before && m.From == CardLocation.DrawPile && m.To == CardLocation.Processing);
        return new(id, move.CardKind, _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName);
    }

    void IMouChengYuAnniversaryNative.Move(ProgramSkillFrame f, int id, CardLocation from, CardLocation to)
    {
        if (!MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary is not { } r || r.CardId != id || _cardZonesOperations._cardZones.GetLocation(id) != from || (r.Stage switch
        {
            MouCyStage.ObtainChildren => from != CardLocation.DrawPile || to != CardLocation.Hand(f.OwnerSeat) || !r.View.Any(c => c.Id == id),
            MouCyStage.GainChildren => from != CardLocation.Processing || to != CardLocation.Hand(r.Target),
            MouCyStage.DiscardChildren => from != CardLocation.Processing || to != CardLocation.DiscardPile,
            _ => true
        }))
            throw new InvalidOperationException("Mou Cheng Yu can move only his exact current viewed or revealed physical card.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources([id], to, new("skill-program.mou-cheng-yu." + r.Stage), (_, moves) =>
        {
            if (moves.Any(m => m.From != from || m.To != to && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Mou Cheng Yu lost the native movement replacement receipt.");
            MouChengYuAnniversary.Moved(f.Id, moves);
        }, disclosure: r.Stage == MouCyStage.ObtainChildren ? CardMovementDisclosure.Unspecified : CardMovementDisclosure.FaceUp);
    }

    void IMouChengYuAnniversaryNative.Invalidate(ProgramSkillFrame f, MouCyUseInvalidatedEvent proof) => ApplyMouCyInvalidation(f, proof);
    void IMouChengYuAnniversaryNative.Damage(ProgramSkillFrame f, int target, int amount)
    {
        if (!MouChengYuAnniversary.Valid(f) || f.MouChengYuAnniversary is not { Stage: MouCyStage.Damage, DamageCard: true } r || r.Target != target || r.RevealCount != amount)
            throw new InvalidOperationException("Gaojian damage requires the actual revealed damage card and total count.");
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, target, amount, nature: DamageNature.Fire);
    }

    bool IMouChengYuAnniversaryNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void IMouChengYuAnniversaryNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void IMouChengYuAnniversaryNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 36800)]
    internal CardSnapshot[] MouCyPrivateTop(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.OwnerSeat == viewer && f.MouChengYuAnniversary is { Stage: MouCyStage.Peek } && MouChengYuAnniversary.Valid(f)).SelectMany(f => f.MouChengYuAnniversary!.View).Where(c => _cardZonesOperations._cardZones.GetLocation(c.Id) == CardLocation.DrawPile).DistinctBy(c => c.Id).Select(c => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Id))).ToArray();
    [ProgramViewContributor(ProgramViewContribution.NamedAmbushPiles, 36800)]
    internal IReadOnlyList<NamedAmbushPileSnapshot>? MouCyNamesView(IReadOnlyList<NamedAmbushPileSnapshot>? existing, int holder, int viewer, bool revealAll)
    {
        if (!HasMouChengYuAnniversary)
            return existing;
        var records = MouChengYuAnniversary.Names(holder);
        if (records.Count == 0)
            return existing;
        IReadOnlyList<NamedAmbushPrivateEntrySnapshot>? entries = viewer == holder || revealAll ? Array.AsReadOnly(records.Select(r => new NamedAmbushPrivateEntrySnapshot(_coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(r.CardId)), r.Name)).ToArray()) : null;
        return Array.AsReadOnly((existing ?? []).Append(new NamedAmbushPileSnapshot(MouChengYuAnniversaryRuntime.Shizha, "识诈", records.Count, entries)).ToArray());
    }
    internal bool MouCyMovement(ProgramSkillFrame f, SkillProgramEffect? effect, ProgramMovementContinuation movement) => f.MouChengYuAnniversary is { Stage: MouCyStage.ObtainChildren or MouCyStage.RevealChildren or MouCyStage.GainChildren or MouCyStage.DiscardChildren } && MouChengYuAnniversary.Valid(f) && effect?.Op == f.MouChengYuAnniversary.Operation && movement == f.PendingMovementContinuation && movement.BeforeCount == 0 && movement.CoverageResultBind is null;
}
