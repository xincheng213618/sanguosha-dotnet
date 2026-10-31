#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEnginePindianRevealWindowsRuntime
{
    internal bool HasPindianRevealCapability => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.AddRaoshePindianRankAndRestoreGushe) || _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw);

    internal bool TryBeginPairPindianReveal(PindianFrame frame, PindianResult result)
    {
        if (!HasPindianRevealCapability)
            return false;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || frame.Result is not null || frame.OpponentSeat != result.OpponentSeat || frame.SourceSeat != result.SourceSeat || (frame.SourceCardId != result.SourceCardId && !_characterPangJiOlOperations.IsDouyuPairAssignment(frame, result) && !_characterLuKangWumiaoOperations.IsWuLuKangAssignedPair(frame, result)) || result.SourceCardId == result.OpponentCardId || _cardZonesOperations._cardZones.GetLocation(result.SourceCardId) != CardLocation.Processing || _cardZonesOperations._cardZones.GetLocation(result.OpponentCardId) != CardLocation.Processing)
            throw new InvalidOperationException("A reveal window lost its original pair and paid entities.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame with { Result = result });
        var cards = Array.AsReadOnly(new[] { new PindianRevealedCard(result.SourceSeat, result.SourceCardId, result.SourceRank, result.SourceRank), new PindianRevealedCard(result.OpponentSeat, result.OpponentCardId, result.OpponentRank, result.OpponentRank) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianCardsRevealedEvent(frame.Id, cards, false));
        BeginPindianRevealWindow(frame.Id, false, cards);
        return true;
    }

    internal void BeginPindianRevealWindow(long parent, bool simultaneous, IReadOnlyList<PindianRevealedCard> cards)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != parent || cards.Count < 2 || cards.Select(c => c.Seat).Distinct().Count() != cards.Count || cards.Select(c => c.CardId).Distinct().Count() != cards.Count || cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != CardLocation.Processing))
            throw new InvalidOperationException("A Pindian reveal requires its actual complete paid group.");
        var candidates = cards.SelectMany(c => _coreProgramLifecycleOperations.CollectProgramTriggerCandidates(_coreMatchParticipantsPlayersAccess._players[c.Seat], SkillProgramTriggerWindow.PindianCardsRevealed)).Concat(_characterCuiLieAnniversaryOperations.AnniversaryCuiLieIssuedPindianCandidates(cards)).DistinctBy(c => (c.OwnerSeat, c.SkillId, c.BindingId, c.SkillInstanceId)).Where(c => _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects is [{ Op: SkillProgramEffectOp.AddRaoshePindianRankAndRestoreGushe or SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw }]).OrderByDescending(c => c.Priority).ThenBy(c => _coreTableOrderOperations.RelativeTableOrder(0, c.OwnerSeat)).ThenBy(c => c.SkillId, StringComparer.Ordinal).ThenBy(c => c.SkillInstanceId, StringComparer.Ordinal).ToArray();
        _coreRuntimeOperations.PushRuntimeFrame(new PindianRevealWindowFrame(++_coreResolutionIdentityBoundaryOperations._resolutionSequence, parent, simultaneous, cards, candidates));
        ContinuePindianRevealWindow();
    }

    internal ProgramSkillWindowContext PindianRevealContext(PindianRevealWindowFrame w, ProgramTriggerCandidate c) => new(SkillProgramTriggerWindow.PindianCardsRevealed, w.Id, c.OwnerSeat, SourceSeat: c.OwnerSeat, TargetSeat: c.OwnerSeat, Amount: w.Cards.Single(card => card.Seat == c.OwnerSeat).Rank, Facts: _coreProgramLifecycleOperations.CaptureProgramTriggerFacts(_coreMatchParticipantsPlayersAccess._players[c.OwnerSeat]));
    internal bool TryGetPindianRevealTrigger(out ProgramTriggerCandidate candidate, out ProgramSkillWindowContext context)
    {
        candidate = null !;
        context = null !;
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not PindianRevealWindowFrame w || w.CandidateIndex >= w.Candidates.Count)
            return false;
        candidate = w.Candidates[w.CandidateIndex];
        context = PindianRevealContext(w, candidate);
        return true;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 240)]
    internal bool CanRunPindianReveal(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (context.Window != SkillProgramTriggerWindow.PindianCardsRevealed)
            return true;
        var w = _resolutionStackOperations._resolutionStack.OfType<PindianRevealWindowFrame>().SingleOrDefault(w => w.Id == context.ParentFrameId);
        return w is not null && w.CandidateIndex >= 0 && w.CandidateIndex < w.Candidates.Count && w.Candidates[w.CandidateIndex] == c && context.SourceSeat == c.OwnerSeat && context.TargetSeat == c.OwnerSeat && w.Cards.Any(card => card.Seat == c.OwnerSeat && card.Rank == context.Amount && (_coreProgramLifecycleOperations.GetProgramTrigger(c).Effects is [{ Op: SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw }] || card.Rank <= _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.RaoShe))) && _coreMatchParticipantsPlayersAccess._players[c.OwnerSeat].IsAlive && _coreProgramLifecycleOperations.GetProgramTrigger(c).Effects is [{ Op: SkillProgramEffectOp.AddRaoshePindianRankAndRestoreGushe or SkillProgramEffectOp.ResolveAnniversaryCuiLieZijuePindianDraw }];
    }

    [NativeFrameAutomaticStep(NativeFrameAutomaticPhase.AfterOpening, 50, NativeFrameAutomaticPending.RequireNoneBeforeFrame, NativeFrameAutomaticPublish.AfterAdvance)]
    [NativeFrameRoute(typeof(PindianRevealWindowFrame), 260)]
    internal void ContinuePindianRevealWindow()
    {
        while (_resolutionStackOperations._resolutionStack.LastOrDefault()is PindianRevealWindowFrame w)
        {
            AssertPindianRevealWindows();
            if (w.CandidateIndex == w.Candidates.Count)
            {
                CompletePindianRevealWindow(w);
                return;
            }

            var c = w.Candidates[w.CandidateIndex];
            var context = PindianRevealContext(w, c);
            if (!_coreProgramLifecycleOperations.CanRunProgramTrigger(c, context))
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1 });
                continue;
            }

            if (!_coreProgramLifecycleOperations.GetProgramTrigger(c).Optional)
            {
                _coreProgramLifecycleOperations.BeginProgramBinding(c, context);
                return;
            }

            _coreProgramLifecycleOperations.ExposeProgramTriggerDecision(c, context);
            return;
        }
    }

    internal void CompletePindianRevealBinding(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not PindianRevealWindowFrame w || w.Id != context.ParentFrameId || w.CandidateIndex >= w.Candidates.Count || w.Candidates[w.CandidateIndex] != c)
            throw new InvalidOperationException("A reveal binding lost its exact typed parent candidate.");
        _coreRuntimeOperations.ReplaceRuntimeTop(w with { CandidateIndex = w.CandidateIndex + 1 });
        ContinuePindianRevealWindow();
    }

    internal void CompletePindianRevealBinding(ProgramSkillFrame f)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not PindianRevealWindowFrame w || f.WindowContext is not { } c || w.Id != c.ParentFrameId || w.CandidateIndex >= w.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]))
            throw new InvalidOperationException("The completed reveal program lost its exact native candidate.");
        CompletePindianRevealBinding(w.Candidates[w.CandidateIndex], c);
    }

    internal void AddRaoshePindianRankAndRestoreGushe(ProgramSkillFrame f, string skillId)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == f.Id);
        if (f.InstructionIndex != 1 || f.TriggerId is null || f.WindowContext is not { Window: SkillProgramTriggerWindow.PindianCardsRevealed } c || index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not PindianRevealWindowFrame w || w.Id != c.ParentFrameId || w.CandidateIndex >= w.Candidates.Count || !global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountObserverRuntime.MountObserverCandidateMatches(f, w.Candidates[w.CandidateIndex]))
            throw new InvalidOperationException("JiCi lost its own exact actual reveal.");
        var card = w.Cards.Single(card => card.Seat == f.OwnerSeat);
        var bonus = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.RaoShe);
        if (card.Rank > bonus || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive)
            throw new InvalidOperationException("JiCi's revealed rank no longer qualifies.");
        var program = _contentRegistryOperations._contentRegistry.GetSkill(skillId).Program ?? throw new InvalidOperationException("JiCi refund requires an actual paired Gushe definition.");
        var activations = program.Activations.Where(a => a.Effects is [{ Op: SkillProgramEffectOp.StartSimultaneousPindian }]).ToArray();
        if (activations is not [var activation] || activation.UsesPerPhase != 1)
            throw new InvalidOperationException("JiCi may restore only its exact simultaneous contest phase activation.");
        var key = (f.OwnerSeat, skillId, activation.UsageGroup);
        var before = _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses.GetValueOrDefault(key);
        if (before > 0)
            _coreGameEngineSkillProgramsProgramPhaseUsesAccess._programPhaseUses[key] = 0;
        var after = Math.Min(13, checked(card.Rank + bonus));
        _coreRuntimeOperations.ReplaceRuntimeFrame(w.Id, w with { Cards = w.Cards.Select(c => c.Seat == f.OwnerSeat ? c with { Rank = after } : c).ToArray() });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianRevealRankAdjustedEvent(w.Id, _coreSimultaneousPindianOperations.SimultaneousSource(f), card.CardId, card.Rank, bonus, after, skillId, activation.UsageGroup, before));
    }

    internal void CompletePindianRevealWindow(PindianRevealWindowFrame w)
    {
        _coreRuntimeOperations.PopResolutionFrame(w.Id, ResolutionFrameKind.PindianRevealWindow);
        if (w.Simultaneous)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { SimultaneousPindian: { Stage: SimultaneousPindianStage.Revealing } r } f || f.Id != w.ParentFrameId)
                throw new InvalidOperationException("The simultaneous common reveal lost its program return.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { SimultaneousPindian = r with { Cards = w.Cards, Stage = SimultaneousPindianStage.Comparing } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not PindianFrame { Result: { } old } pair || pair.Id != w.ParentFrameId || w.Cards.Count != 2)
            throw new InvalidOperationException("The single pair reveal lost its unchanged Pindian return.");
        var result = old with
        {
            SourceRank = w.Cards.Single(c => c.Seat == pair.SourceSeat).Rank,
            OpponentRank = w.Cards.Single(c => c.Seat == pair.OpponentSeat).Rank
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(pair = pair with { Result = result });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PindianResultDeterminedEvent(pair.Id, pair.SkillId, result));
        _coreMatchLogOperations.AddLog("Pindian", $"{(_coreMatchParticipantsPlayersAccess._players[result.SourceSeat].Name)} 以 {(result.SourceRank)} 点与 {(_coreMatchParticipantsPlayersAccess._players[result.OpponentSeat].Name)} 的 {(result.OpponentRank)} 点拼点，" + (result.SourceWon ? "发起者获胜。" : "发起者未赢。"), result.SourceSeat, result.OpponentSeat);
        if (_characterZhangLiaoMobileGodOperations.TryBeginMobileZhitiPindianPrograms(pair))
            return;
        if (!_corePindianClaimsOperations.BeginPindianClaims(pair))
            _corePindianOperations.CompletePindian((PindianFrame)_resolutionStackOperations._resolutionStack[^1]);
    }

    internal bool IsPairPindianRevealSubtree(PindianFrame pair) => HasPindianRevealCapability && _resolutionStackOperations._resolutionStack.OfType<PindianRevealWindowFrame>().Any(w => !w.Simultaneous && w.ParentFrameId == pair.Id && ExactPairRevealParent(pair, w));
    internal bool ExactPairRevealParent(PindianFrame pair, PindianRevealWindowFrame w)
    {
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == pair.Id);
        if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame parent || parent.Id != pair.ParentFrameId || pair.PindianStep != PindianStep.ChooseOpponentCard || pair.Result is not { } result || pair.ProgramResultBind is null || parent.PindianResultBindings.Any(b => b.Name == pair.ProgramResultBind) || pair.SourceSeat != result.SourceSeat || pair.OpponentSeat != result.OpponentSeat || (pair.SourceCardId != result.SourceCardId && !_characterPangJiOlOperations.IsDouyuPairAssignment(pair, result) && !_characterLuKangWumiaoOperations.IsWuLuKangAssignedPair(pair, result)) || !w.Cards.Select(c => (c.Seat, c.CardId, c.OriginalRank)).SequenceEqual(new[] { (result.SourceSeat, result.SourceCardId, result.SourceRank), (result.OpponentSeat, result.OpponentCardId, result.OpponentRank) }))
            return false;
        var held = pair.ParentProcessingCardIds ?? [];
        return !held.Contains(result.SourceCardId) && !held.Contains(result.OpponentCardId) && _cardZonesOperations._cardZones.CardsAt(CardLocation.Processing).Select(c => c.Id).Order().SequenceEqual(held.Concat(w.Cards.Select(c => c.CardId)).Order()) && _coreMatchParticipantsPlayersAccess._players[result.SourceSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[result.OpponentSeat].IsAlive;
    }

    internal void AssertPindianRevealWindows()
    {
        foreach (var w in _resolutionStackOperations._resolutionStack.OfType<PindianRevealWindowFrame>())
        {
            var index = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == w.Id);
            if (index < 1 || _resolutionStackOperations._resolutionStack[index - 1].Id != w.ParentFrameId || w.CandidateIndex < 0 || w.CandidateIndex > w.Candidates.Count || w.Cards.Count is < 2 or > 4 || w.Cards.Select(c => c.Seat).Distinct().Count() != w.Cards.Count || w.Cards.Select(c => c.CardId).Distinct().Count() != w.Cards.Count || w.Cards.Any(c => _cardZonesOperations._cardZones.GetLocation(c.CardId) != CardLocation.Processing) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PindianCardsRevealedEvent>().Count(e => e.ParentFrameId == w.ParentFrameId && e.Simultaneous == w.Simultaneous && e.Cards.Select(c => (c.Seat, c.CardId, c.OriginalRank)).SequenceEqual(w.Cards.Select(c => (c.Seat, c.CardId, c.OriginalRank)))) != 1 || (w.Simultaneous ? _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame { SimultaneousPindian.Stage: SimultaneousPindianStage.Revealing } : _resolutionStackOperations._resolutionStack[index - 1] is not PindianFrame pair || !ExactPairRevealParent(pair, w)))
                throw new InvalidOperationException("The Pindian reveal window lost its exact paid parent, common entities or public original ranks.");
            foreach (var card in w.Cards)
            {
                var expected = card.OriginalRank;
                foreach (var adjustment in _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<PindianRevealRankAdjustedEvent>().Where(e => e.WindowFrameId == w.Id && e.CardId == card.CardId))
                {
                    if (adjustment.Source.OwnerSeat != card.Seat || adjustment.Before != expected || adjustment.Bonus < expected || adjustment.After != Math.Min(13, expected + adjustment.Bonus) || !w.Candidates.Any(c => c.OwnerSeat == card.Seat && c.SkillId == adjustment.Source.SkillId && c.BindingId == adjustment.Source.BindingId && c.SkillInstanceId == adjustment.Source.SkillInstanceId) || adjustment.PreviousPhaseUses is < 0 or > 1)
                        throw new InvalidOperationException("A Pindian revealed rank lacks its exact actual JiCi adjustment chain.");
                    expected = adjustment.After;
                }

                if (expected != card.Rank || card.OriginalRank is < 1 or > 13)
                    throw new InvalidOperationException("A Pindian reveal acquired an unissued rank.");
            }
        }
    }
}
