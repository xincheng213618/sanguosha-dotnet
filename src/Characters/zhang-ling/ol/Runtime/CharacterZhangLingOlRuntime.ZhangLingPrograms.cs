#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangLingOlRuntime
{
    internal const string CategoryTalismanSkill = "ol:shoufu-talisman";
    internal bool TracksCategoryTalismans => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasActivationOperation(SkillProgramEffectOp.PlaceBoundHandAsCategoryTalisman);

    internal CategoryTalismanPlacedEvent? LiveCategoryTalisman(int seat) => !TracksCategoryTalismans ? null : _coreProgramEventHistoryOperations.ProgramEventHistory<CategoryTalismanPlacedEvent>().LastOrDefault(e => e.HolderSeat == seat && _cardZonesOperations._cardZones.GetLocation(e.CardId) == e.Pile.Location && !_coreProgramEventHistoryOperations.ProgramEventHistory<CategoryTalismanReleasedEvent>().Any(release => release.PlacementFrameId == e.FrameId));
    internal bool IsCategoryTalismanUseForbidden(int actorSeat, CardKind effectiveKind, CardActionType type) => type is CardActionType.Use or CardActionType.Response && LiveCategoryTalisman(actorSeat)is { } talisman && global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(effectiveKind) == talisman.Category;
    SkillProgramStepOutcome global::CardGame.Core.IZhangLingProgramHost.PlaceCategoryTalisman(ProgramSkillFrame frame, string bind) => PlaceCategoryTalisman(frame, bind);
    SkillProgramStepOutcome global::CardGame.Core.IZhangLingProgramHost.ReleaseCategoryTalisman(ProgramSkillFrame frame) => ReleaseCategoryTalisman(frame);
    SkillProgramStepOutcome global::CardGame.Core.IZhangLingProgramHost.UseHuqiSlash(ProgramSkillFrame frame, string bind) => UseHuqiSlash(frame, bind);
    IReadOnlyList<int> global::CardGame.Core.ICategoryTalismanSelectionQuery.EligibleRecipients(int ownerSeat) => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != ownerSeat && LiveCategoryTalisman(p.Seat)is null).Select(p => p.Seat).ToArray());
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 26050)]
    internal void ObserveZhangLingNativeFacts(IGameEvent payload)
    {
        if (payload is DamageAppliedEvent { Amount: > 0 } damageApplied && _resolutionStackOperations._resolutionStack.LastOrDefault()is DamageFrame damage && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { DamageWasApplied: true } attack && damage.ParentFrameId == attack.ResolutionId && damage.TargetSeat == damageApplied.TargetSeat && damage.SourceSeat == damageApplied.SourceSeat && (TracksCategoryTalismans || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.UseRedJudgmentSlashAtDamageSource)))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangLingDamageAppliedEvent(damage.Id, damageApplied.SourceLess ? null : damageApplied.SourceSeat, damageApplied.TargetSeat, damageApplied.Amount));
        if (payload is ProgramSkillStartedEvent or ProgramBindingStartedEvent)
        {
            var id = payload is ProgramSkillStartedEvent activation ? activation.FrameId : ((ProgramBindingStartedEvent)payload).FrameId;
            if (_resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == id)is { SkillId: "ol:huqi" or "ol:shoufu" } frame && !_coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhangLingBindingIssuedEvent>().Any(e => e.FrameId == id))
            {
                var damageId = frame.WindowContext?.DamageFrameId;
                var source = damageId is { } nativeDamage ? _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhangLingDamageAppliedEvent>().SingleOrDefault(e => e.DamageFrameId == nativeDamage)?.SourceSeat : null;
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZhangLingBindingIssuedEvent(id, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, damageId, source));
            }
        }

        if (!TracksCategoryTalismans || payload is not CardMovedEvent moved || _coreActualDiscardRecoveryPhasesOperations.CurrentActualDiscardRecoveryPhase()is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase)
            return;
        var record = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.LastOrDefault(m => m.CardId == moved.CardId && m.From == moved.From && m.To == moved.To && m.Reason == moved.Reason);
        if (record is null || _coreProgramDiscardOriginsOperations.GetProgramDiscardSource(record)?.OwnerSeat is not { } owner || owner != phase.ActorSeat || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CategoryTalismanDiscardRecordedEvent>().Any(e => e.MovementSequence == record.Sequence))
            return;
        var card = _cardZonesOperations._cardZones.CardsAt(record.To).Single(c => c.Id == record.CardId);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CategoryTalismanDiscardRecordedEvent(phase, owner, card.Id, record.Sequence, global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind)));
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeStatePublish, 65)]
    internal void SynchronizeCategoryTalismanGrants()
    {
        if (!TracksCategoryTalismans || _characterZhangLingOlSynchronizingCategoryTalismansAccess._synchronizingCategoryTalismans)
            return;
        _characterZhangLingOlSynchronizingCategoryTalismansAccess._synchronizingCategoryTalismans = true;
        try
        {
            foreach (var player in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive))
                if (LiveCategoryTalisman(player.Seat)is { } talisman && !player.SkillGrants.Grants.Any(g => g.GrantId == talisman.GrantId))
                    player.SkillGrants.Grant(new(talisman.GrantId, CategoryTalismanSkill, talisman.Pile.SkillInstanceId, "state:category-talisman"));
        }
        finally
        {
            _characterZhangLingOlSynchronizingCategoryTalismansAccess._synchronizingCategoryTalismans = false;
        }
    }

    internal bool CanReleaseCategoryTalisman(int holder, ProgramSkillWindowContext context)
    {
        if (LiveCategoryTalisman(holder)is not { } talisman)
            return false;
        if (context.Window == SkillProgramTriggerWindow.AfterDamageApplied)
            return context.TargetSeat == holder && context.DamageFrameId is { } damage && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhangLingDamageAppliedEvent>().Any(e => e.DamageFrameId == damage && e.TargetSeat == holder && e.Amount > 0);
        if (context.Window != SkillProgramTriggerWindow.CardsMoved || context.MovementBatch?.DiscardRecoveryPhase is not { Kind: ActualDiscardRecoveryPhaseKind.Discard } phase || phase.ActorSeat != holder)
            return false;
        return _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CategoryTalismanDiscardRecordedEvent>().Count(e => e.Phase == phase && e.OwnerSeat == holder && e.Category == talisman.Category) >= 2;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 26050)]
    internal bool CanOfferCategoryTalismanRelease(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => !trigger.Effects.Any(e => e.Op == SkillProgramEffectOp.ReleaseCategoryTalisman) || LiveCategoryTalisman(candidate.OwnerSeat)is { } t && t.Pile.SkillInstanceId == candidate.SkillInstanceId && CanReleaseCategoryTalisman(candidate.OwnerSeat, context);
    internal SkillProgramStepOutcome PlaceCategoryTalisman(ProgramSkillFrame supplied, string bind)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var cards = frame.CardSetBindings.Single(b => b.Name == bind).CardIds;
        if (cards is not [var cardId] || frame.SelectedTargetSeats is not [var target] || target == frame.OwnerSeat || !_coreMatchParticipantsPlayersAccess._players[target].IsAlive || LiveCategoryTalisman(target)is not null || _cardZonesOperations._cardZones.GetLocation(cardId) != CardLocation.Hand(frame.OwnerSeat))
            throw new InvalidOperationException("A talisman requires one currently owned hand entity and one other unmarked recipient.");
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat]).Single(c => c.Id == cardId);
        var grantId = $"state:category-talisman:{(frame.Id)}";
        var pile = new PublicPersistentPileSource(target, CategoryTalismanSkill, grantId, 1, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileSourcesRuntime.PublicPileIdentity(CategoryTalismanSkill, grantId));
        _cardZonesOperations._cardZones.EnsurePublicPersistentPile(pile.Location);
        _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Add((target, CategoryTalismanSkill, grantId), pile);
        _coreMatchParticipantsPlayersAccess._players[target].SkillGrants.Grant(new(grantId, CategoryTalismanSkill, grantId, "state:category-talisman"));
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var movement = new CategoryTalismanMovementReceipt(SkillProgramEffectOp.PlaceBoundHandAsCategoryTalisman, frame.InstructionIndex, cardId, CardLocation.Hand(frame.OwnerSeat), pile.Location, before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CategoryTalismanMovement = movement, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(card, movement.From, movement.To, new("program.category-talisman.place"));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { CategoryTalismanMovement = movement with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CategoryTalismanPlacedEvent(frame.Id, new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId), frame.GameplayHash, target, cardId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(card.Kind), grantId, pile, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(frame))
            ReturnCategoryTalismanMovement(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal SkillProgramStepOutcome ReleaseCategoryTalisman(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (frame.WindowContext is not { } context || !CanReleaseCategoryTalisman(frame.OwnerSeat, context) || LiveCategoryTalisman(frame.OwnerSeat)is not { } talisman)
            return SkillProgramStepOutcome.Continue;
        var card = _corePublicPileSourcesOperations.PublicPileCards(talisman.Pile).Single(c => c.Id == talisman.CardId);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var movement = new CategoryTalismanMovementReceipt(SkillProgramEffectOp.ReleaseCategoryTalisman, frame.InstructionIndex, card.Id, talisman.Pile.Location, CardLocation.DiscardPile, before, before);
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CategoryTalismanMovement = movement, PendingMovementContinuation = new(frame.OwnerSeat, 0, null) });
        _coreGameEnginePublicPileSourcesPublicPersistentPilesAccess._publicPersistentPiles.Remove((talisman.HolderSeat, CategoryTalismanSkill, talisman.Pile.SkillInstanceId));
        _coreMatchParticipantsPlayersAccess._players[talisman.HolderSeat].SkillGrants.RemoveGrant(talisman.GrantId);
        _corePhysicalMovementFacadesOperations.MoveCard(card, movement.From, movement.To, new("program.category-talisman.release"));
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id)with { CategoryTalismanMovement = movement with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CategoryTalismanReleasedEvent(frame.Id, talisman.FrameId, talisman.HolderSeat, card.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence));
        if (!_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(frame))
            ReturnCategoryTalismanMovement(frame);
        return SkillProgramStepOutcome.AwaitChild;
    }

    [ProgramMovementProof(ProgramMovementProofPhase.BoundEquipment, 70)]
    internal bool IsCategoryTalismanMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation pending) => frame.CategoryTalismanMovement is { } receipt && receipt.InstructionIndex == frame.InstructionIndex && effect?.Op == receipt.Operation && pending.SubjectSeat == frame.OwnerSeat && pending.BeforeCount == 0 && pending.CoverageResultBind is null;
    [ProgramContinuation(ProgramContinuationKind.Resume, 26050)]
    internal bool ResumeCategoryTalismanMovement(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { CategoryTalismanMovement: not null } frame || frame.Id != id)
            return false;
        return ReturnCategoryTalismanMovement(frame);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 26050)]
    internal bool ReturnCategoryTalismanMovement(ProgramSkillFrame frame)
    {
        if (frame.CategoryTalismanMovement is not { } movement)
            return false;
        if (frame.PendingMovementContinuation is null || movement.After <= movement.Before || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.CardId == movement.CardId && m.From == movement.From && m.To == movement.To && m.Sequence > movement.Before && m.Sequence <= movement.After) != 1)
            throw new InvalidOperationException("A talisman movement lost its exact once-paid entity receipt.");
        if (_coreFireTargetBenefitsOperations.TryDrainFireTargetMovement(frame))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { CategoryTalismanMovement = null, PendingMovementContinuation = null });
        if (movement.Operation == SkillProgramEffectOp.UseRedJudgmentSlashAtDamageSource && IssueHuqiSlash(frame, movement.JudgmentBind!) != SkillProgramStepOutcome.Continue)
            return true;
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 26050)]
    internal bool ContinueIssuedZhangLingBinding(ProgramSkillFrame frame) => frame.CategoryTalismanMovement is not null || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<ZhangLingBindingIssuedEvent>().Any(e => e.FrameId == frame.Id && e.GameplayHash == frame.GameplayHash && e.Source == new CardConversionSource(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId));
}
