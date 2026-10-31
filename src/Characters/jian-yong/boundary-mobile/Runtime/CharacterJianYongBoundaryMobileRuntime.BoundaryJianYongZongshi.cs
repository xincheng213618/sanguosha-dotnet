#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterJianYongBoundaryMobileRuntime
{
    internal const string BoundaryZongshiReason = "skill-program.boundary:zongshih-mobile.obtain";
    internal bool TryBeginBoundaryJianYongZongshi(PindianFrame p) => p.Result is { } r && TryBeginBoundaryZongshiContest(p, r, -1);
    internal bool TryBeginBoundaryJianYongSimultaneousZongshi(ProgramSkillFrame f) => f.SimultaneousPindian is { ComparisonIssued: true } r && TryBeginBoundaryZongshiContest(f, _coreSimultaneousPindianOperations.SimultaneousResult(f), r.ComparisonIndex);
    internal bool BoundaryZongshiNativeResult(ResolutionFrame p, out PindianResult result, out int comparison)
    {
        comparison = -1;
        result = null !;
        if (p is PindianFrame { Result: { } pair })
        {
            result = pair;
            return true;
        }

        if (p is ProgramSkillFrame { SimultaneousPindian: { ComparisonIssued: true } r } f && r.ComparisonIndex < r.Opponents.Count)
        {
            comparison = r.ComparisonIndex;
            result = _coreSimultaneousPindianOperations.SimultaneousResult(f);
            return true;
        }

        return false;
    }

    internal bool TryBeginBoundaryZongshiContest(ResolutionFrame supplied, PindianResult result, int comparison)
    {
        var p = _resolutionStackOperations._resolutionStack.Single(f => f.Id == supplied.Id);
        var r = p.BoundaryJianYongZongshiContest;
        if (r is null || r.Result != result || r.ComparisonIndex != comparison)
        {
            if (r is not null && r.Cursor < r.Candidates.Count)
                throw new InvalidOperationException("Zongshi cannot replace an unfinished native comparison.");
            var candidates = new[]
            {
                result.SourceSeat,
                result.OpponentSeat
            }.SelectMany(seat => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[seat], SkillProgramTriggerWindow.PindianResultDetermined)).Where(c => c.SkillId == BoundaryJianYongComposition.Zongshi).ToArray();
            if (candidates.Length == 0)
                return false;
            r = new(p.Id, comparison, result)
            {
                Candidates = candidates
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(p = p with { BoundaryJianYongZongshiContest = r });
        }

        while (r.Cursor < r.Candidates.Count)
        {
            var c = r.Candidates[r.Cursor];
            var context = BoundaryZongshiContext(p, c);
            if (_coreProgramLifecycleOperations.CanRunProgramTrigger(c, context))
            {
                _coreProgramLifecycleOperations.BeginProgramBinding(c, context);
                return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(p = p with { BoundaryJianYongZongshiContest = r = r with { Cursor = r.Cursor + 1 } });
        }

        return false;
    }

    internal ProgramSkillWindowContext BoundaryZongshiContext(ResolutionFrame p, ProgramTriggerCandidate c)
    {
        var r = p.BoundaryJianYongZongshiContest!.Result;
        return new(SkillProgramTriggerWindow.PindianResultDetermined, p.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: r.SourceSeat == c.OwnerSeat ? r.OpponentSeat : r.SourceSeat, Facts: null);
    }

    internal bool BoundaryZongshiParent(ProgramSkillFrame f, ResolutionFrame p) => p.BoundaryJianYongZongshiContest is { } r && r.ParentFrameId == p.Id && BoundaryZongshiNativeResult(p, out var result, out var comparison) && r.Result == result && r.ComparisonIndex == comparison && r.Cursor < r.Candidates.Count && global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, r.Candidates[r.Cursor]) && f.WindowContext == BoundaryZongshiContext(p, r.Candidates[r.Cursor]);
    [ProgramTriggerProof(ProgramTriggerProofPhase.NativeWindow, 160, SkillProgramTriggerWindow.PindianResultDetermined)]
    internal bool IsBoundaryJianYongZongshiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context) => context.Window == SkillProgramTriggerWindow.PindianResultDetermined && c.SkillId == BoundaryJianYongComposition.Zongshi && _resolutionStackOperations._resolutionStack.SingleOrDefault(p => p.Id == context.ParentFrameId)is { BoundaryJianYongZongshiContest: { } r } parent && r.ParentFrameId == parent.Id && BoundaryZongshiNativeResult(parent, out var result, out var comparison) && r.Result == result && r.ComparisonIndex == comparison && r.Cursor < r.Candidates.Count && r.Candidates[r.Cursor] == c && context == BoundaryZongshiContext(parent, c);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 56500, SkillProgramEffectOp.ResolveBoundaryJianYongZongshi)]
    internal bool BoundaryZongshiQualifies(ProgramTriggerLegalityContext g) => g.WindowContext is { } context && IsBoundaryJianYongZongshiCandidate(g.Candidate, context);
    [ProgramPipelineHook(ProgramPipelinePhase.ReturnCompletedProgramBinding, 56500)]
    internal bool ReturnBoundaryZongshiBinding(ProgramSkillFrame f, bool completed)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not { } parent || !BoundaryZongshiParent(f, parent))
            return false;
        var r = parent.BoundaryJianYongZongshiContest!;
        _coreRuntimeOperations.ReplaceRuntimeTop(parent = parent with { BoundaryJianYongZongshiContest = r with { Cursor = r.Cursor + 1 } });
        if (parent is PindianFrame pair)
        {
            if (!_corePindianClaimsOperations.BeginPindianClaims(pair))
                _corePindianOperations.CompletePindian((PindianFrame)_resolutionStackOperations._resolutionStack[^1]);
        }
        else if (parent is ProgramSkillFrame group)
            _coreRuntimeOperations.AdvanceRuntimeProgram(group.Id);
        else
            throw new InvalidOperationException("Zongshi lost its exact original native comparison return.");
        return true;
    }

    internal SkillProgramStepOutcome BeginBoundaryJianYongZongshiChoice(ProgramSkillFrame f)
    {
        var n = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        if (n < 1 || !BoundaryZongshiParent(f, _resolutionStackOperations._resolutionStack[n - 1]) || f.BoundaryJianYongZongshi is not null || f.InstructionIndex != 1)
            throw new InvalidOperationException("Mobile Zongshi requires the original completed own Pindian and unused choice.");
        var top = _corePhysicalDrawFacadesOperations.EnsureDrawPile() ? _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).Last().Id : (int? )null;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryJianYongZongshi = new(f.WindowContext!.ParentFrameId, top, ComparisonIndex: _resolutionStackOperations._resolutionStack[n - 1].BoundaryJianYongZongshiContest!.ComparisonIndex) });
        PublishBoundaryZongshiChoice(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal int[] BoundaryZongshiLower(ResolutionFrame p) => p.BoundaryJianYongZongshiContest is { Result: { } r } ? (r.SourceRank == r.OpponentRank ? new[]
    {
        r.SourceCardId,
        r.OpponentCardId
    }

    : new[]
    {
        r.SourceRank < r.OpponentRank ? r.SourceCardId : r.OpponentCardId
    }

    ).Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Processing).ToArray() : [];
    internal IReadOnlyList<PromptChoice> BoundaryZongshiChoices(ProgramSkillFrame f)
    {
        var r = f.BoundaryJianYongZongshi!;
        var p = _resolutionStackOperations._resolutionStack.Single(p => p.Id == r.PindianId);
        var result = new List<PromptChoice>();
        void Add(string branch, int? id, string label) => result.Add(new(new($"mobile-zongshi.{(f.Id)}.{(branch)}.{(id)}"), label, id is { } card ? [card] : [], [], new Dictionary<string, string> { { "program-action", "boundary-jian-yong-zongshi" }, { "branch", branch } }));
        if (r.TopCardId is { } top)
        {
            var card = _coreAdvancedLifecycleOperations.GetAdvancedCard(top);
            var suit = card.Suit switch
            {
                Suit.Spade => "♠",
                Suit.Heart => "♥",
                Suit.Club => "♣",
                Suit.Diamond => "♦",
                _ => ""
            };
            Add("top", top, $"获得所观看的牌堆顶牌【{(card.DisplayName)}】（{(suit)}{(card.RankText)}）");
        }

        foreach (var id in BoundaryZongshiLower(p))
            Add("contest", id, $"获得拼点牌【{(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).DisplayName)}】（{((p.BoundaryJianYongZongshiContest!.Result.SourceCardId == id ? p.BoundaryJianYongZongshiContest.Result.SourceRank : p.BoundaryJianYongZongshiContest.Result.OpponentRank))}）");
        Add("skip", null, "不获得牌");
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishBoundaryZongshiChoice(ProgramSkillFrame f)
    {
        var choices = BoundaryZongshiChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "纵适：已观看牌堆顶牌，选择获得顶牌或点数较小的拼点牌。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("boundary-jian-yong-zongshi")]
    internal void AnswerBoundaryZongshi(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack[^1];
        RequireBoundaryZongshi(f);
        var r = f.BoundaryJianYongZongshi!;
        if (r.Choice != 0 || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], BoundaryZongshiChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Zongshi lost its exact original private peek choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Cards.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryJianYongZongshi = null });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var id = choice.Cards.Single();
        var from = choice.Parameters["branch"] == "top" ? CardLocation.DrawPile : CardLocation.Processing;
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        if (_cardZonesOperations._cardZones.GetLocation(id) != from || from == CardLocation.DrawPile && _cardZonesOperations._cardZones.CardsAt(from).Last().Id != id)
            throw new InvalidOperationException("Zongshi cannot substitute its viewed top or original contest entity.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryJianYongZongshi = r with { Choice = 1, PaidCardId = id, From = from, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _corePhysicalMovementFacadesOperations.MoveCard(_coreAdvancedLifecycleOperations.GetAdvancedCard(id), from, CardLocation.Hand(f.OwnerSeat), new(BoundaryZongshiReason));
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { BoundaryJianYongZongshi = f.BoundaryJianYongZongshi!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new BoundaryJianYongZongshiPaidEvent(f.Id, r.PindianId, f.OwnerSeat, from.Zone, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, r.ComparisonIndex));
        if (_coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(f.Id, f.OwnerSeat) == SkillProgramStepOutcome.Continue)
            FinishBoundaryZongshi(f.Id);
    }

    internal void FinishBoundaryZongshi(long id)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { BoundaryJianYongZongshi = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 56500)]
    internal bool ResumeBoundaryZongshi(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { BoundaryJianYongZongshi: { } r } f || f.Id != id)
            return false;
        RequireBoundaryZongshi(f);
        if (r.Choice == 0)
        {
            PublishBoundaryZongshiChoice(f);
            return true;
        }

        if (_coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id) || _coreSkillOwnershipWindowsOperations.TryBeginAdvancedSkillsChanged(id))
            return true;
        FinishBoundaryZongshi(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 56500)]
    internal bool ReturnBoundaryZongshiMovement(ProgramSkillFrame f) => f.BoundaryJianYongZongshi is not null && ResumeBoundaryZongshi(f.Id);
    internal void RequireBoundaryZongshi(ProgramSkillFrame f)
    {
        var n = _resolutionStackOperations._resolutionStack.FindIndex(p => p.Id == f.Id);
        var r = f.BoundaryJianYongZongshi;
        if (r is null || f.SkillId != BoundaryJianYongComposition.Zongshi || f.InstructionIndex != 1 || n < 1 || !BoundaryZongshiParent(f, _resolutionStackOperations._resolutionStack[n - 1]) || r.PindianId != f.WindowContext!.ParentFrameId || r.Choice is < 0 or > 1)
            throw new InvalidOperationException("Zongshi lost its original Pindian, exact participant source or stage.");
        if (r.Choice == 0 && r.TopCardId is { } viewed && (_cardZonesOperations._cardZones.GetLocation(viewed) != CardLocation.DrawPile || _cardZonesOperations._cardZones.CardsAt(CardLocation.DrawPile).LastOrDefault()?.Id != viewed))
            throw new InvalidOperationException("Zongshi lost its original privately viewed deck top before choosing.");
        var parent = _resolutionStackOperations._resolutionStack[n - 1];
        var pair = parent.BoundaryJianYongZongshiContest!.Result;
        var comparisons = _coreProgramEventHistoryOperations.ProgramEventHistory<PindianResultDeterminedEvent>().Where(e => e.FrameId == parent.Id).ToArray();
        var comparison = r.ComparisonIndex < 0 ? 0 : r.ComparisonIndex;
        var lower = pair.SourceRank == pair.OpponentRank ? new[]
        {
            pair.SourceCardId,
            pair.OpponentCardId
        }

        : new[]
        {
            pair.SourceRank < pair.OpponentRank ? pair.SourceCardId : pair.OpponentCardId
        };
        if (r.Choice == 1 && (r.From == CardLocation.DrawPile ? r.PaidCardId != r.TopCardId : r.From != CardLocation.Processing || r.PaidCardId is not { } claimed || !lower.Contains(claimed)))
            throw new InvalidOperationException("Zongshi payment must be its viewed top or exact lower/equal final-rank contest entity.");
        if (r.Choice == 1 && (r.PaidCardId is not { } id || r.From is not { } from || r.After <= r.Before || r.After > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Count(m => m.Sequence > r.Before && m.Sequence <= r.After && m.CardId == id && m.From == from && m.To == CardLocation.Hand(f.OwnerSeat) && m.Reason.Value == BoundaryZongshiReason) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<BoundaryJianYongZongshiPaidEvent>().Count(e => e == new BoundaryJianYongZongshiPaidEvent(f.Id, r.PindianId, f.OwnerSeat, from.Zone, r.Before, r.After, r.ComparisonIndex)) != 1))
            throw new InvalidOperationException("Zongshi lost its exact once-paid native Obtain.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 56500)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 56500)]
    internal bool KeepBoundaryZongshiPaid(ProgramSkillFrame f)
    {
        if (f.BoundaryJianYongZongshi is not { Choice: 1 })
            return false;
        RequireBoundaryZongshi(f);
        return true;
    }

    internal bool IsBoundaryJianYongZongshiSubtree(PindianFrame p)
    {
        var n = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == p.Id);
        if (n < 0 || n + 1 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[n + 1] is not ProgramSkillFrame f || !BoundaryZongshiParent(f, p))
            return false;
        if (f.BoundaryJianYongZongshi is not null)
            RequireBoundaryZongshi(f);
        return n + 2 == _resolutionStackOperations._resolutionStack.Count || BoundaryZongshiFirstChild(f, _resolutionStackOperations._resolutionStack[n + 2]) && BoundaryZongshiSuffix(n + 1);
    }

    internal bool BoundaryZongshiFirstChild(ProgramSkillFrame f, ResolutionFrame child)
    {
        if (f.BoundaryJianYongZongshi is not { Choice: 1 } r || child is not CardsMovedTriggerWindowFrame moved || !global::CardGame.Core.EngineOwners.CoreDamageObserverCursorQueriesRuntime.ProgramMovementReturnMatches(f, moved) || moved.Batch.ParentFrameId != f.Id)
            return false;
        RequireBoundaryZongshi(f);
        return moved.Batch.OriginOwnerSeat == f.OwnerSeat && moved.Batch.OriginSkillId == f.SkillId && moved.Batch.OriginSkillInstanceId == f.SkillInstanceId && moved.Batch.Movements.Count > 0 && moved.Batch.Movements.All(m => _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(m) && m.Sequence > r.Before && m.Sequence <= r.After);
    }

    internal bool BoundaryZongshiSuffix(int n) => _coreObserverSubtreesOperations.HasObserverSuffix(n, (i) => _coreObserverSubtreesOperations.IsMovementObserverEdge(i) || _coreSameNameHandDiscardOrDamageObserversOperations.IsSameNameHandObserverEdge(i) || _characterYanPuOlOperations.IsYanPuDyingExitEdge(i) || _coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(_resolutionStackOperations2._resolutionStack()[i - 1], _resolutionStackOperations2._resolutionStack()[i]) || _coreRecoveryReplacementDamageRidesOperations.RecoveryReplacementFrameRidesOn(_resolutionStackOperations2._resolutionStack()[i], _resolutionStackOperations2._resolutionStack()[i - 1]), (i, d) => _coreObserverSubtreesOperations.IsSuspendedOrPaidRescueObserverSuffix(i, d));
    internal ProgramSkillFrame? BoundaryZongshiObserverRoot() => _coreObserverSubtreesOperations.FindProgramObserverRoot((f, c) => _characterJianYongBoundaryMobileOperations.BoundaryZongshiFirstChild(f, c), (n) => _characterJianYongBoundaryMobileOperations.BoundaryZongshiSuffix(n));
    [ProgramDyingProof(ProgramDyingProofPhase.Runtime, 56500)]
    internal bool BoundaryZongshiDyingChild() => _coreDyingCursorRuntimeOperations.ActiveDying is { } d && BoundaryZongshiObserverRoot()is { } f && _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == d.Id) > _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterResponseHealth, 56500)]
    internal bool AdvanceBoundaryZongshiChild() => _corePendingDecisionPolicyPipelineOperations._pendingDecision is null && BoundaryZongshiObserverRoot()is not null && _coreObserverSubtreesOperations.TryAdvanceObserverSubtree(true);
    [ProgramAiChoice(SkillProgramEffectOp.ResolveBoundaryJianYongZongshi)]
    internal PromptChoice SelectAiBoundaryZongshi(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters.GetValueOrDefault("branch") == "top") ?? p.Choices.First();
}
