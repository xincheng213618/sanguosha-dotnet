#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhouYuMouRuntime
{
    internal MouZhouYuRuntime MouZyAnniversary => _mouZhouYuOperations.Get();

    int global::CardGame.Core.IMouZhouYuFacts.Turn => _coreActualTurnProgressTurnNumberAccess._turnNumber;

    long global::CardGame.Core.IMouZhouYuFacts.Sequence => _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;

    bool global::CardGame.Core.IMouZhouYuFacts.Finished => _coreMatchOutcomeWinnerAccess._winner != Winner.None;

    bool global::CardGame.Core.IMouZhouYuFacts.Alive(int s) => _coreTableSeatQueriesOperations.IsValidPlayerSeat(s) && _coreMatchParticipantsPlayersAccess._players[s].IsAlive;
    bool global::CardGame.Core.IMouZhouYuFacts.Human(int s) => _coreMatchParticipantsPlayersAccess._players[s].IsHuman;
    string global::CardGame.Core.IMouZhouYuFacts.Name(int s) => _coreMatchParticipantsPlayersAccess._players[s].Name;
    int global::CardGame.Core.IMouZhouYuFacts.HandCount(int s) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Count;
    int global::CardGame.Core.IMouZhouYuFacts.FactionCount => _coreRuleQueriesOperations.GetLivingFactionCount();

    IReadOnlyList<int> global::CardGame.Core.IMouZhouYuFacts.Living() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray());
    SkillProgram? global::CardGame.Core.IMouZhouYuFacts.Program(string id) => _contentRegistryOperations._contentRegistry.Skills.GetValueOrDefault(id)?.Program;
    IEnumerable<T> global::CardGame.Core.IMouZhouYuFacts.History<T>() => _coreProgramEventHistoryOperations.ProgramEventHistory<T>();
    string global::CardGame.Core.IMouZhouYuFacts.Instance(int s, string skill) => _coreNationalSkillsOperations.GetRuntimeSkillInstanceId(_coreMatchParticipantsPlayersAccess._players[s], skill);
    bool global::CardGame.Core.IMouZhouYuFacts.Yin(int s, string instance) => _coreProgramPersistentStateOperations.GetProgramBooleanState(s, MouZhouYuRuntime.Yingmou, instance, "yin");
    IReadOnlyList<CardConversionSource> global::CardGame.Core.IMouZhouYuFacts.FireSources() => Array.AsReadOnly(_coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && _coreSkillProgramsOperations.EnabledSkillPrograms(p).Any(x => x.Id == MouZhouYuRuntime.Ronghuo)).Select(p => new CardConversionSource(MouZhouYuRuntime.Ronghuo, "next-fire-use", p.Seat, ((global::CardGame.Core.IMouZhouYuFacts)this).Instance(p.Seat, MouZhouYuRuntime.Ronghuo))).ToArray());
    long? global::CardGame.Core.IMouZhouYuFacts.FireDamage(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != MouZhouYuRuntime.Ronghuo || c.BindingId != "fire-after-damage" || w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.OwnerSeat != c.OwnerSeat || w.Amount <= 0 || w.OccurrenceIndex != c.OccurrenceIndex || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not DamageTriggerWindowFrame window || window.ParentFrameId != w.DamageFrameId || window.TriggerWindow != w.Window || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex].ToProgramCandidate() != c || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == window.ParentFrameId)is not DamageFrame { Nature: DamageNature.Fire, Amount: > 0 } d || d.TargetSeat != w.TargetSeat || d.Amount != w.Amount)
            return null;
        return d.Id;
    }

    MouZhouYuCompletedUse? global::CardGame.Core.IMouZhouYuFacts.Use(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (w.Window != SkillProgramTriggerWindow.CardUseCompleted || w.OwnerSeat != c.OwnerSeat || w.CardUse is not { } card || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == w.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.Action.ActionId != card.CardActionId || window.Action is not { Type: CardActionType.Use } a || a.ActorSeat != c.OwnerSeat || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || window.Candidates[window.CandidateIndex] is not { } now || now.OwnerSeat != c.OwnerSeat || now.SkillId != c.SkillId || now.TriggerId != c.BindingId || now.SkillInstanceId != c.SkillInstanceId || now.GameplayHash != c.GameplayHash)
            return null;
        var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SingleOrDefault(x => x.Action?.ActionId == a.ActionId);
        return new(window.Id, use?.Id ?? 0, a);
    }

    bool global::CardGame.Core.IMouZhouYuFacts.Parent(ProgramSkillFrame f)
    {
        if (f.TriggerId != f.ActivationId || f.WindowContext is not { } w || w.OwnerSeat != f.OwnerSeat)
            return false;
        var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (n < 1 || _resolutionStackOperations._resolutionStack[n - 1].Id != w.ParentFrameId)
            return false;
        if (f.TriggerId == "opening-state")
            return w.Window == SkillProgramTriggerWindow.GameStarting && _resolutionStackOperations._resolutionStack[n - 1] is ProgramLifecycleTriggerWindowFrame { Window: SkillProgramTriggerWindow.GameStarting };
        return f.TriggerId == "used-other-targets" && ((global::CardGame.Core.IMouZhouYuFacts)this).Use(new(f.OwnerSeat, f.SkillId, f.TriggerId, f.SkillInstanceId, f.GameplayHash, 0), w)is not null;
    }

    IReadOnlyList<MouZhouYuCard> global::CardGame.Core.IMouZhouYuFacts.Hand(int s) => Array.AsReadOnly(_corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[s]).Select(c =>
    {
        var kind = _coreAdvancedLifecycleOperations.AdvancedEffectiveHandKind(_coreMatchParticipantsPlayersAccess._players[s], c);
        return new MouZhouYuCard(c.Id, CardLocation.Hand(s), c.DisplayName, kind, global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) || global::CardGame.Core.EngineOwners.CoreGameEngineProgramCardPaymentsRuntime.GetProgramCardCategory(kind) == SkillProgramCardCategory.Trick && (global::CardGame.Core.EngineOwners.CoreGameEngineMouLuMengRuntime.IsDamageCardKind(kind) || global::CardGame.Core.EngineOwners.CoreGameEngineCompletedUndamagedTargetRevealRuntime.IsCompletedUndamagedDamageUse(kind) || kind == CardKind.Lightning));
    }).ToArray());
    IReadOnlyList<MouZhouYuCard> global::CardGame.Core.IMouZhouYuFacts.DiscardableHand(int s) => ((global::CardGame.Core.IMouZhouYuFacts)this).Hand(s).Where(c => !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(s, _coreAdvancedLifecycleOperations.GetAdvancedCard(c.Id), c.From, OwnedCardMoveIntent.Discard)).ToArray();
    IReadOnlyList<LegalAction> global::CardGame.Core.IMouZhouYuFacts.UseOptions(ProgramSkillFrame f) => MouZyUseOptions(f);
    IReadOnlyList<IReadOnlyList<int>> global::CardGame.Core.IMouZhouYuFacts.FireOptions(ProgramSkillFrame f) => MouZyFireOptions(f);
    void global::CardGame.Core.IMouZhouYuNative.Finish(ProgramSkillFrame f) => _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    void global::CardGame.Core.IMouZhouYuNative.Publish() => _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    bool global::CardGame.Core.IMouZhouYuNative.Drain(ProgramSkillFrame f) => _coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(f.Id, CharacterStateContinuation.Program) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(f.Id) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(f.Id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(f.Id);
    void global::CardGame.Core.IMouZhouYuNative.State(ProgramSkillFrame f, bool yin)
    {
        if (!MouZyAnniversary.Valid(f))
            throw new InvalidOperationException("Yingmou state lost its original accepted branch.");
        _coreProgramPersistentStateOperations.SetProgramBooleanState(f.OwnerSeat, MouZhouYuRuntime.Yingmou, f.SkillInstanceId, "yin", yin);
    }

    void global::CardGame.Core.IMouZhouYuNative.Use(ProgramSkillFrame f) => IssueMouZyUse(f);
    void global::CardGame.Core.IMouZhouYuNative.Discard(ProgramSkillFrame f)
    {
        if (!MouZyAnniversary.Valid(f) || f.MouZhouYuAnniversary is not { Stage: MouZhouYuStage.DiscardChildren, Frozen.Count: 0 } r || r.Selected.Count == 0 || r.Selected.Any(id => !_characterZhouYuMouOperations.CreateMouZhouYuFacts().DiscardableHand(r.Actor).Any(c => c.Id == id)))
            throw new InvalidOperationException("Yingmou discard requires no related Hand cards and the actor's real discardable Hand.");
        _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Actor, r.Selected, new(MouZhouYuRuntime.DiscardReason), (_, moves) =>
        {
            var parent = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = parent.MouZhouYuAnniversary!;
            if (!MouZyAnniversary.Valid(parent) || moves.Count != r.Selected.Count || moves.Any(m => !r.Selected.Contains(m.CardId) || m.From != CardLocation.Hand(r.Actor) || m.Sequence <= r.Before || m.To != CardLocation.DiscardPile && m.To != CardLocation.OutsideGame))
                throw new InvalidOperationException("Yingmou lost its primary native Hand discard receipt.");
            _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, parent with { MouZhouYuAnniversary = paid with { After = moves.Max(m => m.Sequence) } });
            foreach (var m in moves)
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouZhouYuDiscardPaidEvent(f.Id, r.Actor, m.CardId, m.Sequence));
        });
    }

    void global::CardGame.Core.IMouZhouYuNative.Damage(ProgramSkillFrame f)
    {
        if (!MouZyAnniversary.Valid(f) || f.MouZhouYuAnniversary is not { Stage: MouZhouYuStage.DamageChildren, Damaged: false } r)
            throw new InvalidOperationException("Yingmou final fire damage requires its genuine undamaged child chain.");
        _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Target, 1, nature: DamageNature.Fire, explicitSourceSeat: f.OwnerSeat);
    }

    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 36000)]
    internal int MouZyFireBonus(global::CardGame.Core.GameEngine.IDamageAttempt a)
    {
        var u = a is global::CardGame.Core.NativeOwners.CardAttackHandle && !a.IsChainPropagation ? _coreCardUseLifecycleOperations.LifecycleCardUse(a.ResolutionId) : null;
        return MouZyAnniversary.DamageBonus(u?.SourceSeat ?? -1, u?.CardKind ?? CardKind.Dodge, u?.Action is { Type: CardActionType.Use });
    }

    [ProgramDamageHook(ProgramDamagePhase.AppliedBeforeDying, 36000)]
    internal bool MouZyDamageApplied(global::CardGame.Core.GameEngine.IDamageAttempt a, long id)
    {
        if (!a.DamageWasApplied || _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == id)is not DamageFrame { Amount: > 0 } d)
            return false;
        foreach (var f in _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Where(f => f.MouZhouYuAnniversary is { Target: var t } && t == d.TargetSeat).ToArray())
        {
            var n = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
            if (_resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == id) > n && MouZyAnniversary.Valid(f))
                MouZyAnniversary.Damaged(f, id, d.TargetSeat, d.Amount);
        }

        return false;
    }
}
