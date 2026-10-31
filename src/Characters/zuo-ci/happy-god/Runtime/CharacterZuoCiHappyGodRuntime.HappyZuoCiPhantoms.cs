#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZuoCiHappyGodRuntime
{
    internal static HappyZuoCiFace HappyZcFace(Card card) => new(card.Kind, card.Suit, card.Rank, card.PrintedName);
    internal (int Owner, HappyZuoCiState State, HappyZuoCiEntity Entity)? HappyZcEntity(int id)
    {
        if (id <= 0 || !_coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Contains(id))
            return null;
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            if (p.HappyZuoCi is { } state && state.Entities.SingleOrDefault(e => e.CardId == id)is { } entity)
                return (p.Seat, state, entity);
        return null;
    }

    // Registration only; the query still belongs to legacy GameEngine.HappyZuoCi.Phantoms.
    [ProgramPipelineHook(ProgramPipelinePhase.CardSkillMarkProjection, 260)]
    internal void ProjectHappyZuoCiPhantomCardSkillMark(int cardId, ref CardSkillMarkProjection projection)
    {
        projection.Append(HappyZuoCiPhantomCardLabel(cardId));
    }

    internal string? HappyZuoCiPhantomCardLabel(int cardId) => HappyZcEntity(cardId)is null ? null : "幻术";
    internal bool IsHappyZuoCiHandLimitExempt(CharacterState owner, int cardId) => HappyZcEntity(cardId)is { } found && found.Owner == owner.Seat;
    internal bool HappyZcSourceProof(int owner, HappyZuoCiState state) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(owner) && state.Source is { SkillId: HappyZuoCiComposition.Huanshu, BindingId: "initial-phantoms" } && state.Source.OwnerSeat == owner && state.InitialFrameId > 0 && state.Deck == new CardLocation(CardZoneKind.PrivatePersistentPile, owner, $"happy-huanshu:{(state.Source.SkillInstanceId)}") && _contentRegistryOperations._contentRegistry.GetSkill(state.Source.SkillId).Program?.GameplayHash == state.GameplayHash && _coreProgramEventHistoryOperations.Value.ProgramEventHistory<HappyZuoCiPoolInitializedEvent>().Count(e => e.FrameId == state.InitialFrameId && e.Source == state.Source && e.GameplayHash == state.GameplayHash) == 1;
    internal CardLocation NormalizeHappyZuoCiPhantomDestination(Card card, CardLocation from, CardLocation to)
    {
        if (from == to || HappyZcEntity(card.Id)is not { } found)
            return to;
        if (!HappyZcSourceProof(found.Owner, found.State))
            throw new InvalidOperationException("A phantom lost its unique original issued deck.");
        if (from == CardLocation.OutsideGame)
            return to == found.State.Deck ? to : CardLocation.OutsideGame;
        if (to == CardLocation.OutsideGame || to.Zone == CardZoneKind.DiscardPile)
            return CardLocation.OutsideGame;
        // A foreign Hand must really receive the entity before the after-event
        // destruction. This preserves actual gain and the original give invoice.
        if (to.OwnerSeat == found.Owner || to.Zone is CardZoneKind.Hand or CardZoneKind.DrawPile or CardZoneKind.Processing or CardZoneKind.Judgment or CardZoneKind.BuquWound or CardZoneKind.Authority or CardZoneKind.Chunlao or CardZoneKind.PojunHold or CardZoneKind.PrivateReserve or CardZoneKind.PublicDeferredPile or CardZoneKind.PublicPersistentPile or CardZoneKind.PrivateTurnHold or CardZoneKind.RoundHold or CardZoneKind.PrivatePersistentPile)
            return to;
        return CardLocation.OutsideGame;
    }

    internal HappyZuoCiPhantomDestinationPlan? PlanHappyZuoCiPhantomDestination(Card card, CardLocation from, CardLocation requestedTo, CardMoveReason reason)
    {
        if (HappyZcEntity(card.Id)is not { } found)
            return null;
        var actual = NormalizeHappyZuoCiPhantomDestination(card, from, requestedTo);
        if (actual == requestedTo)
            return null;
        if (found.Entity.Faces.Count == 0 || found.Entity.Faces[^1] != HappyZcFace(card) || _cardZonesOperations._cardZones.GetLocation(card.Id) != from)
            throw new InvalidOperationException("A phantom destination plan requires its exact live physical face and source.");
        var gain = reason.Value == "skill-program.happy:huanshu.foreign-destroy" ? _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == card.Id && m.To == from) : null;
        if (reason.Value == "skill-program.happy:huanshu.foreign-destroy" && (gain is null || from.Zone != CardZoneKind.Hand || from.OwnerSeat == found.Owner || gain.Sequence != _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence))
            throw new InvalidOperationException("Foreign phantom destruction must immediately follow the real receiving movement.");
        return new(card.Id, found.Owner, found.State.InitialFrameId, found.State.Source, found.State.GameplayHash, found.Entity.Faces.Count - 1, HappyZcFace(card), from, requestedTo, actual, reason, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Count(), gain?.Sequence, gain?.To.OwnerSeat);
    }

    internal bool HappyZcPlanProof(HappyZuoCiPhantomDestinationPlan plan) => HappyZcEntity(plan.CardId)is { } found && found.Owner == plan.Issuer && HappyZcSourceProof(found.Owner, found.State) && plan.InitialFrameId == found.State.InitialFrameId && plan.Source == found.State.Source && plan.GameplayHash == found.State.GameplayHash && plan.ActualTo == CardLocation.OutsideGame && plan.RequestedTo != plan.ActualTo && plan.FaceVersion >= 0 && plan.FaceVersion < found.Entity.Faces.Count && found.Entity.Faces[plan.FaceVersion] == plan.Face && plan.SequenceBefore >= 0 && plan.EventCountBefore >= 0 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().Take(plan.EventCountBefore).OfType<HappyZuoCiPoolInitializedEvent>().Count(e => e.FrameId == plan.InitialFrameId && e.Source == plan.Source && e.GameplayHash == plan.GameplayHash) == 1 && (plan.OriginalForeignGainSequence is null ? plan.OriginalRecipient is null : plan.OriginalRecipient is { } recipient && recipient != plan.Issuer && plan.From == CardLocation.Hand(recipient) && plan.Reason.Value == "skill-program.happy:huanshu.foreign-destroy" && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == plan.OriginalForeignGainSequence && m.CardId == plan.CardId && m.To == plan.From) == 1);
    internal void ValidateHappyZuoCiPhantomMovement(CardMovementRecord movement)
    {
        if (movement.HappyZuoCiPhantomDestination is { } plan && (!HappyZcPlanProof(plan) || movement.CardId != plan.CardId || movement.CardKind != plan.Face.Kind || movement.From != plan.From || movement.To != plan.ActualTo || movement.Reason != plan.Reason || movement.Sequence <= plan.SequenceBefore || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence == movement.Sequence && m == movement) != 1))
            throw new InvalidOperationException("A phantom movement lost its frozen original recipient, physical face or historical issuer.");
    }

    internal bool IsHappyZuoCiDestroyedMaterial(CardMovementRecord m) => m.To == CardLocation.OutsideGame && m.HappyZuoCiPhantomDestination is { } plan && HappyZcPlanProof(plan) && m.From == plan.From && m.CardId == plan.CardId && m.CardKind == plan.Face.Kind && m.Reason == plan.Reason && m.Sequence > plan.SequenceBefore && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(actual => actual.Sequence == m.Sequence && actual == m) == 1;
    internal bool IsHappyZuoCiDestroyedResponseFinish(CardActionContext action, CardActionCost cost, CardMovementRecord finish) => action.Type == CardActionType.Response && action.PhysicalCards.Contains(cost) && cost.From.OwnerSeat == action.ProviderSeat && finish.CardId == cost.CardId && finish.CardKind == cost.CardKind && finish.From == CardLocation.Processing && finish.HappyZuoCiPhantomDestination is { RequestedTo: var requested, OriginalForeignGainSequence: null } plan && requested == CardLocation.DiscardPile && plan.Issuer == action.ProviderSeat && IsHappyZuoCiDestroyedMaterial(finish) && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == cost.CardId && m.Sequence <= plan.SequenceBefore)is { } entry && entry.CardKind == cost.CardKind && entry.From == cost.From && entry.To == CardLocation.Processing && entry.Sequence < finish.Sequence && (entry.Reason == CardMoveReasons.Respond && finish.Reason == CardMoveReasons.ResponseFinished || entry.Reason == CardMoveReasons.Nullification && finish.Reason == CardMoveReasons.NullificationFinished || entry.Reason.Value == "conversion.declaration.pay" && _resolutionStackOperations._resolutionStack.Any(owner => owner.AcceptedDeclarationPayment is { Claimed: true } paid && paid.ProviderSeat == action.ProviderSeat && paid.ActorSeat == action.ActorSeat && paid.Cost == cost && action.ConversionChain.Contains(paid.Source) && finish.Reason == (paid.ReturnPurpose == CardDeclarationPurpose.Counterspell ? CardMoveReasons.NullificationFinished : CardMoveReasons.ResponseFinished) && _coreResponseCompletionOperations.IsResponseCompletionEntry(action, paid.ReturnPurpose switch
    {
        CardDeclarationPurpose.Counterspell => ProgramCardContinuation.NullificationResponse,
        CardDeclarationPurpose.Dodge => ProgramCardContinuation.Dodge,
        CardDeclarationPurpose.Duel => ProgramCardContinuation.DuelSlash,
        CardDeclarationPurpose.Group => ProgramCardContinuation.GroupResponse,
        CardDeclarationPurpose.FactionDefense => ProgramCardContinuation.FactionDefenseDodge,
        CardDeclarationPurpose.FactionSlash => ProgramCardContinuation.FactionSlashDuelSlash,
        CardDeclarationPurpose.QizhengResponse => ProgramCardContinuation.QizhengResponse,
        _ => ProgramCardContinuation.CompletedCard
    }, owner.Id, cost, entry)));
    internal bool IsHappyZuoCiDestroyedMaterial(int cardId, CardLocation from, CardLocation to) => to == CardLocation.OutsideGame && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cardId && m.To == to && IsHappyZuoCiDestroyedMaterial(m) && (m.From == from || m.HappyZuoCiPhantomDestination?.OriginalForeignGainSequence is { } sequence && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(g => g.Sequence == sequence && g.CardId == cardId && g.From == from)));
    internal bool IsHappyZuoCiDestroyedMaterial(long useId, int cardId) => _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is { Action: { } action } && action.PhysicalCards.SingleOrDefault(c => c.CardId == cardId)is { } material && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Any(m => m.CardId == cardId && m.CardKind == material.CardKind && m.From == CardLocation.Processing && IsHappyZuoCiDestroyedMaterial(m) && m.HappyZuoCiPhantomDestination!.SequenceBefore >= _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(p => p.CardId == cardId && p.From == material.From && p.To == CardLocation.Processing).Select(p => (long)p.Sequence).DefaultIfEmpty(long.MaxValue).Max());
    internal IEnumerable<ProgramTriggerCandidate> CollectHappyZuoCiPhantomRewardCandidates(CardMovementBatchContext batch)
    {
        for (var i = 0; i < batch.Movements.Count; i++)
            if (HappyZcForeignGain(batch.Movements[i])is { } owner)
                foreach (var candidate in _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[owner], SkillProgramTriggerWindow.CardsMoved).Where(c => c.SkillId == HappyZuoCiComposition.Huanshu && c.BindingId == "foreign-phantom"))
                    yield return candidate with
                    {
                        OccurrenceIndex = i
                    };
    }

    internal int? HappyZcForeignGain(CardMovementRecord m) => IsHappyZuoCiDestroyedMaterial(m) && m.HappyZuoCiPhantomDestination is { OriginalForeignGainSequence: not null, OriginalRecipient: { } recipient, Issuer: var owner } && recipient != owner ? owner : null;
    internal bool IsHappyZuoCiForeignRewardCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (candidate.SkillId != HappyZuoCiComposition.Huanshu || candidate.BindingId != "foreign-phantom" || context.Window != SkillProgramTriggerWindow.CardsMoved || context.OwnerSeat != candidate.OwnerSeat || context.TargetSeat != candidate.OwnerSeat || context.OccurrenceIndex != candidate.OccurrenceIndex || context.MovementBatch is not { } batch || context.MovementIndex != candidate.OccurrenceIndex || candidate.OccurrenceIndex < 0 || candidate.OccurrenceIndex >= batch.Movements.Count || _resolutionStackOperations._resolutionStack.OfType<CardsMovedTriggerWindowFrame>().LastOrDefault()is not { } window || window.Id != context.ParentFrameId || window.Batch != batch || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] != candidate || window.Contexts is not { } contexts || window.CandidateIndex >= contexts.Count || contexts[window.CandidateIndex] != context)
            return false;
        var movement = batch.Movements[candidate.OccurrenceIndex];
        return HappyZcForeignGain(movement) == candidate.OwnerSeat && context.SourceSeat == movement.HappyZuoCiPhantomDestination!.OriginalRecipient && movement.HappyZuoCiPhantomDestination.OriginalForeignGainSequence is { } sequence && sequence < movement.Sequence && _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(gain => gain.Sequence == sequence && gain.CardId == movement.CardId && gain.To == CardLocation.Hand(context.SourceSeat!.Value)) == 1;
    }

    internal ProgramSkillWindowContext CreateHappyZuoCiPhantomRewardContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (candidate.OccurrenceIndex < 0 || candidate.OccurrenceIndex >= frame.Batch.Movements.Count || HappyZcForeignGain(frame.Batch.Movements[candidate.OccurrenceIndex]) != candidate.OwnerSeat)
            throw new InvalidOperationException("A phantom reward requires its actual destroyed foreign-hand request.");
        if (frame.Contexts is { } contexts)
        {
            if (frame.CandidateIndex < 0 || frame.CandidateIndex >= frame.Candidates.Count || frame.CandidateIndex >= contexts.Count || frame.Candidates[frame.CandidateIndex] != candidate || contexts[frame.CandidateIndex] is not { } frozen || frozen.Window != SkillProgramTriggerWindow.CardsMoved || frozen.ParentFrameId != frame.Id || frozen.MovementBatch != frame.Batch || frozen.OwnerSeat != candidate.OwnerSeat || frozen.TargetSeat != candidate.OwnerSeat || frozen.MovementIndex != candidate.OccurrenceIndex || frozen.OccurrenceIndex != candidate.OccurrenceIndex || frozen.SourceSeat != frame.Batch.Movements[candidate.OccurrenceIndex].HappyZuoCiPhantomDestination!.OriginalRecipient)
                throw new InvalidOperationException("A phantom reward lost its original frozen current-candidate context.");
            return frozen;
        }

        var movement = frame.Batch.Movements[candidate.OccurrenceIndex];
        return new(SkillProgramTriggerWindow.CardsMoved, frame.Id, candidate.OwnerSeat, SourceSeat: movement.HappyZuoCiPhantomDestination!.OriginalRecipient, TargetSeat: candidate.OwnerSeat, OccurrenceIndex: candidate.OccurrenceIndex, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[candidate.OwnerSeat]), MovementBatch: frame.Batch, MovementIndex: candidate.OccurrenceIndex);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 42600)]
    internal void CaptureHappyZuoCiNewPrototype(CardMovedEvent fact)
    {
        if (fact.From != CardLocation.OutsideGame || fact.To.OwnerSeat is null || !_coreMatchParticipantsPlayersAccess._players.Any(p => p.IsAlive && p.HappyZuoCi is not null) || HappyZcEntity(fact.CardId)is not null)
            return;
        var face = HappyZcFace(_coreAdvancedLifecycleOperations.GetAdvancedCard(fact.CardId));
        foreach (var p in _coreMatchParticipantsPlayersAccess._players)
            if (p.HappyZuoCi is { } state && p.IsAlive && !state.Entities.Any(e => e.PrototypeId == fact.CardId))
                CreateHappyZcPoolCards(p.Seat, _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id ?? p.HappyZuoCi.InitialFrameId, [new(fact.CardId, face)]);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 42601)]
    internal void DestroyForeignHappyZuoCiPhantom(CardMovedEvent fact)
    {
        if (fact.To is not { Zone: CardZoneKind.Hand, OwnerSeat: { } recipient } || HappyZcEntity(fact.CardId)is not { } found || recipient == found.Owner)
            return;
        if (_cardZonesOperations._cardZones.GetLocation(fact.CardId) != fact.To || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault()is not { } movement || movement.CardId != fact.CardId || movement.From != fact.From || movement.To != fact.To || movement.Reason != fact.Reason)
            throw new InvalidOperationException("A foreign phantom must be destroyed immediately after its actual hand gain.");
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(fact.CardId), fact.To, CardLocation.DiscardPile, new("skill-program.happy:huanshu.foreign-destroy"));
    }

    internal void CreateHappyZcPoolCards(ProgramSkillFrame f, IReadOnlyList<HappyZuoCiPrototype> prototypes) => CreateHappyZcPoolCards(f.OwnerSeat, f.Id, prototypes);
    internal void CreateHappyZcPoolCards(int ownerSeat, long creationFrameId, IReadOnlyList<HappyZuoCiPrototype> prototypes)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[ownerSeat];
        var state = owner.HappyZuoCi!;
        var cards = new List<Card>();
        var entities = state.Entities.ToList();
        var next = checked(_coreTrustedCardZoneDiagnosticsOperations.CreateCardZoneDiagnostics().Select(c => c.CardId).DefaultIfEmpty(0).Max() + 1);
        foreach (var p in prototypes)
        {
            var card = new Card(next++, p.Face.Kind, p.Face.Suit, p.Face.Rank)
            {
                PrintedName = p.Face.PrintedName,
                GrantedSkillIds = Array.AsReadOnly(Array.Empty<string>())
            };
            _cardZonesOperations._cardZones.AddGeneratedCard(card);
            _coreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess._generatedPhysicalCardIds.Add(card.Id);
            cards.Add(card);
            entities.Add(new HappyZuoCiEntity(card.Id, p.PhysicalId, creationFrameId) { Faces = [p.Face] });
        }

        owner.HappyZuoCi = state with
        {
            Entities = entities
        };
        if (cards.Count > 0)
        {
            _corePhysicalMovementFacadesOperations.MoveCards(cards, CardLocation.OutsideGame, state.Deck, new("skill-program.happy:huanshu.pool-create"));
            _cardZonesOperations._cardZones.Shuffle(state.Deck, _randomOperations._random);
        }
    }

    internal void ChangeHappyZcFace(CharacterState owner, Card original, HappyZuoCiFace face, bool manual)
    {
        var state = owner.HappyZuoCi ?? throw new InvalidOperationException("A phantom face needs its original deck.");
        var entity = state.Entities.Single(e => e.CardId == original.Id);
        if (_cardZonesOperations._cardZones.GetLocation(original.Id) != CardLocation.Hand(owner.Seat) || entity.Faces.Count == 0 || entity.Faces[^1] != HappyZcFace(original) || manual && entity.ManuallyTransformed)
            throw new InvalidOperationException("Only the exact remaining own hand phantom can change face once manually.");
        _cardZonesOperations._cardZones.ReplaceGeneratedCardFace(original, original with { Kind = face.Kind, Suit = face.Suit, Rank = face.Rank, PrintedName = face.PrintedName });
        owner.HappyZuoCi = state with
        {
            Entities = state.Entities.Select(e => e.CardId == original.Id ? e with { Faces = e.Faces.Append(face).ToArray(), ManuallyTransformed = e.ManuallyTransformed || manual } : e).ToArray()
        };
    }

    [ProgramPipelineHook(ProgramPipelinePhase.CollectMovementCandidates, 4500)]
    internal void CollectWindow_CollectHappyZuoCiPhantomRewardCandidates(CardMovementBatchContext batch, List<ProgramTriggerCandidate> result) => result.AddRange(CollectHappyZuoCiPhantomRewardCandidates(batch));
    [ProgramPipelineHook(ProgramPipelinePhase.ExcludeOrdinaryMovementCandidate, 600)]
    internal bool ExcludeOrdinaryMovement_HappyZuoCiPhantom(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger) => candidate.SkillId == HappyZuoCiComposition.Huanshu && candidate.BindingId == "foreign-phantom";
    [ProgramPipelineHook(ProgramPipelinePhase.CreateMovementContext, 1000)]
    internal ProgramSkillWindowContext? TryCreateHappyZuoCiPhantomRewardContext(CardsMovedTriggerWindowFrame frame, ProgramTriggerCandidate candidate)
    {
        if (candidate.SkillId == "happy:huanshu" && candidate.BindingId == "foreign-phantom")
            return CreateHappyZuoCiPhantomRewardContext(frame, candidate);
        return null;
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.MovementOwner, 40, ProgramTriggerProofMatch.Decision)]
    internal bool? ProveHappyZuoCiMovement(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.SkillId == HappyZuoCiComposition.Huanshu && candidate.BindingId == "foreign-phantom" ? IsHappyZuoCiForeignRewardCandidate(candidate, context) : null;
}
