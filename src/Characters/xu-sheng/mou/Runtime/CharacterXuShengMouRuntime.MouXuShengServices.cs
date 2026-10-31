#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterXuShengMouRuntime
{
    internal MouXuShengRuntime MouXuSheng => _mouXuShengOperations.Get();

    int global::CardGame.Core.IMouXuShengFacts.Turn => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber;

    int global::CardGame.Core.IMouXuShengFacts.TurnOwner => _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat;

    long global::CardGame.Core.IMouXuShengFacts.MovementSequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouXuShengFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouXuShengFacts.Alive(int seat) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(seat) && _coreMatchParticipantsPlayersAccess._players[seat].IsAlive;
    bool global::CardGame.Core.IMouXuShengFacts.Human(int seat) => _coreMatchParticipantsPlayersAccess._players[seat].IsHuman;
    string global::CardGame.Core.IMouXuShengFacts.CardLabel(int id) => _coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName;
    bool global::CardGame.Core.IMouXuShengFacts.Friendly(int owner, int other) => _coreAiBrainsAiBrainsAccess._aiBrains[owner].ScoreProgramTarget(_corePlayerViewProjectionOperations.CreateSnapshot(owner), other, new(0, 0, 0, 1, 0, 0, false, false)) > 0;
    int global::CardGame.Core.IMouXuShengFacts.KeepValue(int owner, int id) => global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), _coreMatchParticipantsPlayersAccess._players[owner]);
    SkillProgram? global::CardGame.Core.IMouXuShengFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> global::CardGame.Core.IMouXuShengFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    MouXuShengUse? global::CardGame.Core.IMouXuShengFacts.Use(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.OwnerSeat != candidate.OwnerSeat || context.CardUse is not { } info || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != info.CardActionId || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } current || current.OwnerSeat != candidate.OwnerSeat || current.SkillId != candidate.SkillId || current.TriggerId != candidate.BindingId || current.SkillInstanceId != candidate.SkillInstanceId || current.GameplayHash != candidate.GameplayHash)
            return null;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(f => f.Action?.ActionId == info.CardActionId);
        return new(window.Id, window.Action, use?.Id ?? 0);
    }

    bool global::CardGame.Core.IMouXuShengFacts.DamageParent(ProgramSkillFrame f)
    {
        if (f.SkillId != MouXuShengComposition.Yizhen || f.TriggerId != "mutual-private-hand" || f.WindowContext is not { } context || !((global::CardGame.Core.IMouXuShengFacts)this).SourcedDamage(context, f.OwnerSeat))
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        return index > 0 && _resolutionStackOperations._resolutionStack[index - 1] is DamageTriggerWindowFrame window && window.Id == context.ParentFrameId && window.ParentFrameId == context.DamageFrameId && window.SourceSeat == context.SourceSeat && window.TargetSeat == f.OwnerSeat && window.CandidateIndex >= 0 && window.CandidateIndex < window.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, window.Candidates[window.CandidateIndex].ToProgramCandidate());
    }

    bool global::CardGame.Core.IMouXuShengFacts.SourcedDamage(ProgramSkillWindowContext context, int owner) => context is { Window: SkillProgramTriggerWindow.AfterDamageApplied, SourceSeat: { } source, TargetSeat: var target, DamageFrameId: not null, Amount: > 0 } && target == owner && _coreTableSeatQueriesOperations.IsValidPlayerSeat(source);
    IReadOnlyList<MouXuShengCard> global::CardGame.Core.IMouXuShengFacts.Hand(int seat) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Select(c => new MouXuShengCard(c.Id, seat, global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[seat], c)))).ToArray());
    bool global::CardGame.Core.IMouXuShengFacts.Discardable(int chooser, MouXuShengCard card) => _cardZonesOperations._cardZones.GetLocation(card.CardId) == CardLocation.Hand(card.Holder) && !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(chooser, _coreAdvancedLifecycleOperations.GetAdvancedCard(card.CardId), CardLocation.Hand(card.Holder), OwnedCardMoveIntent.Discard);
    CardLocation global::CardGame.Core.IMouXuShengFacts.Location(int id) => _cardZonesOperations._cardZones.GetLocation(id);
    void global::CardGame.Core.IMouXuShengNative.PublishState() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    void global::CardGame.Core.IMouXuShengNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    bool global::CardGame.Core.IMouXuShengNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouXuShengNative.Boost(ProgramSkillFrame f)
    {
        if (!MouXuSheng.Valid(f) || f.MouXuSheng is not { Accepted: true, Operation: SkillProgramEffectOp.ResolveMouXuShengQinqiang } r)
            throw new InvalidOperationException("Qinqiang requires its accepted original true Hand Use.");
        // A true response Use owns its response window, never the incoming
        // card's damage. Such a non-damaging card may still spend this item.
        if (r.UseId == 0)
            return;
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action: { } action } use || action.ActionId != r.ActionId || action.ActorSeat != f.OwnerSeat)
            throw new InvalidOperationException("Qinqiang must attach only to its own accepted card.");
        var boost = new MouXuShengBoost(f.Id, r.Source, f.GameplayHash, r.UseId, r.ActionId, r.X);
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouXuShengBoosts = (u.MouXuShengBoosts ?? []).Append(boost).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuShengBoostIssuedEvent(boost));
    }

    void global::CardGame.Core.IMouXuShengNative.Discard(ProgramSkillFrame f)
    {
        if (!MouXuSheng.Valid(f) || f.MouXuSheng is not { Stage: MouXuShengStage.DiscardChildren, Paying: { } selected } r || !_characterXuShengMouOperations.CreateMouXuShengFacts().Discardable(selected.Chooser, selected.Card))
            throw new InvalidOperationException("Yizhen requires the selected opposing real Hand entity.");
        var card = selected.Card with
        {
            IsRed = global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[selected.Card.Holder], _coreAdvancedLifecycleOperations.GetAdvancedCard(selected.Card.CardId)))
        };
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, selected.Chooser, [card.CardId], new(MouXuShengRuntime.DiscardReason), (_, records) =>
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(x => x.Id == f.Id);
            if (parent?.MouXuSheng is not { Stage: MouXuShengStage.DiscardChildren } paid || !MouXuSheng.Valid(parent) || paid.Paying != selected || records is not [var moved] || !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(moved) || moved.CardId != card.CardId || moved.From != CardLocation.Hand(card.Holder) || moved.Sequence <= paid.Before || moved.Reason.Value != MouXuShengRuntime.DiscardReason || moved.To != CardLocation.DiscardPile && moved.To != CardLocation.OutsideGame)
                throw new InvalidOperationException("Yizhen's primary discard must retain its chooser, original holder and exact material.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouXuSheng = paid with { After = moved.Sequence, Paid = paid.Paid.Append(new(selected.Chooser, card, moved.Sequence)).ToArray() } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuShengDiscardPaidEvent(f.Id, selected.Chooser, card.Holder, card.CardId, card.IsRed, moved.Sequence));
        });
    }

    void global::CardGame.Core.IMouXuShengNative.Claim(ProgramSkillFrame f, IReadOnlyList<int> ids)
    {
        if (!MouXuSheng.Valid(f) || f.MouXuSheng is not { Stage: MouXuShengStage.ClaimChildren, Paid: [var first, var second] } r || first.Card.IsRed is not { } color || second.Card.IsRed != color || ids.Any(id => !r.Paid.Any(p => p.Card.CardId == id) || _cardZonesOperations._cardZones.GetLocation(id) != CardLocation.DiscardPile))
            throw new InvalidOperationException("Yizhen may only recover its matching-color actual discarded entities still in the discard pile.");
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(f.OwnerSeat), new(MouXuShengRuntime.ClaimReason), (_, records) =>
        {
            var parent = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(x => x.Id == f.Id);
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouXuSheng = parent.MouXuSheng!with { After = records.Max(m => m.Sequence) } });
            foreach (var moved in records.Where(m => m.To == CardLocation.Hand(f.OwnerSeat)))
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouXuShengClaimedEvent(f.Id, moved.CardId, moved.Sequence));
        }, disclosure: CardMovementDisclosure.FaceUp);
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 34900)]
    internal int MouXuShengDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack)
    {
        if (attack.IsChainPropagation || attack is not global::CardGame.Core.NativeOwners.CardAttackHandle || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { Action: { } action, MouXuShengBoosts: { } boosts } use)
            return 0;
        return boosts.Where(b => b.UseId == use.Id && b.ActionId == action.ActionId && b.Source.OwnerSeat == action.ActorSeat && _contentRegistryOperations._contentRegistry.GetSkill(b.Source.SkillId).Program?.GameplayHash == b.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuShengBoostIssuedEvent>().Count(e => e.Boost == b) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MouXuShengBenefitUsedEvent>().Count(e => e.FrameId == b.FrameId && e.ActionId == b.ActionId && e.Damage && e.X == b.X) == 1).Sum(b => b.X);
    }

    [ProgramViewContributor(ProgramViewContribution.PrivateCards, 34900)]
    internal CardSnapshot[] MouXuShengPrivateHandViews(int viewer) => _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.MouXuSheng is { Operation: SkillProgramEffectOp.ResolveMouXuShengYizhen } && MouXuSheng.Valid(f)).SelectMany(f => f.MouXuSheng!.Views.Where(v => v.Viewer == viewer).SelectMany(v => v.Cards)).Where(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) == CardLocation.Hand(c.Holder)).DistinctBy(c => c.CardId).Select(c => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.CardId))).ToArray();
}
