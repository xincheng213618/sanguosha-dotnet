#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreWushuangCombatProgramsRuntime
{
    SkillProgramStepOutcome IWushuangCombatHost.Execute(ProgramSkillFrame f, SkillProgramEffect e) => ExecuteWushuangCombat(f, e);
    internal SkillProgramStepOutcome ExecuteWushuangCombat(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (!_coreWushuangModeProjectionOperations.IsWushuangMode || f.SkillId != $"wushuang:tactic-{(effect.Amount)}" || f.WushuangCombat is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<WushuangCombatIssuedEvent>().Any(e => e.ProgramFrameId == f.Id && e.InstructionIndex == f.InstructionIndex))
            throw new InvalidOperationException("A combat tactic requires its exact unpaid owner instruction.");
        var id = effect.Amount;
        var target = f.WindowContext?.SourceSeat ?? f.OwnerSeat;
        var amount = 0;
        var nature = DamageNature.Normal;
        int[] seats = [];
        if (id is 2105 or 2106)
        {
            amount = id == 2105 ? 1 : 2;
            nature = DamageNature.Thunder;
            seats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).Select(p => p.Seat).ToArray();
        }

        if (id is 2174 or 2175)
        {
            amount = id == 2174 ? 1 : 2;
            seats = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).Select(p => p.Seat).ToArray();
        }

        if (id == 2116)
        {
            amount = 1;
            seats = [target];
        }

        if (id is 2104 or 2143)
        {
            var usage = $"draw-{(id)}";
            _skillRuntimeStateOperations._skillRuntimeState.TryConsumeUsage(f.OwnerSeat, f.SkillId, usage, SkillUsageScope.Game, int.MaxValue);
            if (_skillRuntimeStateOperations._skillRuntimeState.GetUsage(f.OwnerSeat, f.SkillId, usage, SkillUsageScope.Game) % (id == 2104 ? 9 : 6) == 0)
            {
                var enemies = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat).ToArray();
                if (enemies.Length > 0)
                {
                    target = enemies[_randomOperations._random.Next(enemies.Length)].Seat;
                    amount = 1;
                    seats = [target];
                }
            }
        }

        if (id == 2131)
        {
            var context = f.WindowContext!;
            var parent = _resolutionStackOperations._resolutionStack.OfType<DamageTriggerWindowFrame>().Single(w => w.Id == context.ParentFrameId);
            GameEngine.IDamageAttempt attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(parent);
            target = attack.SourceSeat;
            amount = attack.DamageAmount;
            nature = _coreNativeCardClassificationsOperations.GetDamageNature(attack);
            seats = [target];
        }

        var draw = id == 2130 || id == 2367 && f.WindowContext?.Window != SkillProgramTriggerWindow.SkillsChanged ? 1 : 0;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangCombat = new(id, f.InstructionIndex, target, amount, nature, DrawCount: draw) { Seats = seats } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangCombatIssuedEvent(f.Id, id, f.OwnerSeat, f.InstructionIndex));
        if (id == 2367 && f.WindowContext?.Window == SkillProgramTriggerWindow.SkillsChanged)
        {
            var lucky = 1 + _randomOperations._random.Next(13);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new WushuangLuckyNumberEvent(f.OwnerSeat, lucky));
            _coreMatchLogOperations.AddLog("WushuangLuckyNumber", $"{(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Name)} 的幸运数字为 {(lucky)}。", f.OwnerSeat);
        }

        if (id == 2019)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangCombat = f.WushuangCombat!with { Stage = 10 } });
            _ = _coreNativeJudgmentOperations.BeginJudgment(null, _coreActualTurnProgressCurrentSeatAccess._currentSeat, "闪电", f.Id, CardKind.Lightning, JudgmentContinuationKind.ProgramSkill, sourceSeat: f.OwnerSeat, programResultBind: "wushuang-lightning", programResultVisibility: SkillProgramCardSetVisibility.Public);
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (id is 2039 or 2040 or 2365)
        {
            var cards = _coreProgramLifecycleOperations.GetProgramCardSet(f, effect.SourceBind!);
            if (cards.CardIds.Count is < 1 or > 2 || cards.CardIds.Count > (id == 2040 ? 2 : 1) || cards.SourceLocations.Any(location => location.OwnerSeat != f.OwnerSeat || location.Zone is not (CardZoneKind.Hand or CardZoneKind.Equipment)) || id == 2365 && cards.CardIds.Any(card => _coreCardAttackStateOperations.GetAttackCard(card).Kind is not (CardKind.PeachGarden or CardKind.ArrowBarrage or CardKind.Lightning)) || cards.CardIds.Where((card, index) => _cardZonesOperations._cardZones.GetLocation(card) != cards.SourceLocations[index]).Any())
                throw new InvalidOperationException("Recast materials left their exact owning selection.");
            f = _coreNativeRecastInvocationsOperations.BeginFrameNativeRecastInvocation(f, f.OwnerSeat, cards.CardIds);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangCombat = f.WushuangCombat!with { Stage = 1, DrawCount = cards.CardIds.Count } });
            foreach (var card in cards.CardIds)
            {
                var location = _cardZonesOperations._cardZones.GetLocation(card);
                _corePhysicalMovementFacadesOperations.MoveCard(_cardZonesOperations._cardZones.CardsAt(location).Single(c => c.Id == card), location, CardLocation.DiscardPile, CardMoveReasons.RecastDiscard);
            }
        }

        if (id == 2020)
            _coreProgramLifecycleOperations.ChangeProgramMaximumHp(f, 7 - _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].MaxHp);
        if (id == 2185)
            foreach (var enemy in _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat))
                ChangeWushuangTargetMaximumHp(enemy, -1, f.SkillId);
        if (id is 2120 or 2121 or 2122)
        {
            var enemy = _coreMatchParticipantsPlayersAccess._players[target];
            var candidates = _corePhysicalOwnedCardQueriesOperations.GetHand(enemy).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(enemy)).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(f.OwnerSeat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard) && !_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(enemy.Seat, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
            if (candidates.Length > 0)
            {
                var card = candidates[_randomOperations._random.Next(candidates.Length)];
                _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, enemy.Seat, card, _cardZonesOperations._cardZones.GetLocation(card.Id), new("wushuang.emergency-discard"));
            }
        }

        if (id == 2198)
        {
            var enemies = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Seat != f.OwnerSeat && _corePhysicalOwnedCardQueriesOperations.GetHand(p).Count > 0).ToArray();
            if (enemies.Length > 0)
            {
                var enemy = enemies[_randomOperations._random.Next(enemies.Length)];
                var cards = _corePhysicalOwnedCardQueriesOperations.GetHand(enemy);
                var card = cards[_randomOperations._random.Next(cards.Count)];
                _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(enemy.Seat), CardLocation.Hand(f.OwnerSeat), new("wushuang.transport"));
            }
        }

        if (id is 2245 or 2246 or 2247)
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().Recover(f.Id, f.OwnerSeat, f.OwnerSeat, id == 2246 ? 3 : 2, null, null);
        if (id == 2337)
        {
            if (f.WindowContext is not { CardUse: { } context } || _coreCardUseLifecycleOperations.LifecycleCardUse(context.ParentCardUseFrameId)is not { Action: { } action, ExtraCardEffect: null } use || action.ActionId != context.CardActionId || use.SourceSeat != f.OwnerSeat || use.CardKind != CardKind.Snatch)
                throw new InvalidOperationException("Repeated Snatch requires its exact accepted original effect.");
            var window = _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().Single(w => w.Id == f.WindowContext.ParentFrameId);
            var source = new CardConversionSource(f.SkillId, f.TriggerId!, f.OwnerSeat, f.SkillInstanceId);
            _coreRuntimeOperations.ReplaceRuntimeFrame(use.Id, use with { ExtraCardEffect = new() { Source = source, ProgramFrameId = f.Id, ActionId = action.ActionId, AutomaticGameplayHash = f.GameplayHash, Trick = window.TrickContinuation } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new AutomaticExtraCardEffectIssuedEvent(f.Id, source, use.Id, action.ActionId, f.GameplayHash));
        }

        return ContinueWushuangCombat(f.Id) ? SkillProgramStepOutcome.AwaitChild : SkillProgramStepOutcome.Continue;
    }

    internal void ChangeWushuangTargetMaximumHp(CharacterState target, int delta, string skill)
    {
        var before = target.MaxHp;
        var hpBefore = target.Hp;
        target.MaxHp = _coreWushuangTacticRulesOperations.HasWushuangTactic(target.Seat, 2020) ? 7 : Math.Max(1, checked(before + delta));
        target.Hp = Math.Min(target.Hp, target.MaxHp);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(target.Seat, target.MaxHp - before, target.MaxHp, skill) { HpBeforeClamp = hpBefore });
    }

    internal bool IsWushuangNativeDrawDamageMaterial(ProgramSkillFrame producer, CardMovementBatchContext batch, CardMovementRecord material, int recipient)
    {
        if (batch.NativeDrawInvocation is { } draw)
            return draw.ParentFrameId == producer.Id && draw.OwnerSeat == recipient && draw.LastBatchId == batch.Id && draw.ActualTurnNumber == batch.TurnNumber && _coreOutsidePhaseDrawDiscardOperations.ValidNativeDrawInvocation(draw) && draw.Materials.Any(card => card.CardId == material.CardId && card.PrintedKind == material.CardKind && card.MovementSequence == material.Sequence);
        // Draw2 can emit one physical batch per card. Its last batch carries
        // the full projection; an earlier card is linked to the real recorded
        // invocation and its still-active native producer receipt instead.
        var invocations = _coreProgramEventHistoryOperations.ProgramEventHistory<NativeDrawInvocationRecordedEvent>().Where(invocation => invocation.ParentFrameId == producer.Id && invocation.OwnerSeat == recipient && invocation.TurnNumber == batch.TurnNumber && invocation.Reason == material.Reason && invocation.SequenceBefore < material.Sequence && invocation.SequenceAfter >= material.Sequence).ToArray();
        if (invocations is not [var actualDraw] || actualDraw.InvocationId <= 0 || actualDraw.Requested < 1 || actualDraw.ActualCount < 1 || actualDraw.ActualCount > actualDraw.Requested || actualDraw.SequenceBefore < 0 || actualDraw.SequenceAfter > _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence || actualDraw.LastBatchId < batch.Id || batch.ParentFrameId != producer.Id || batch.OriginOwnerSeat != producer.OwnerSeat || batch.OriginSkillId != producer.SkillId || batch.OriginSkillInstanceId != producer.SkillInstanceId || material.From != CardLocation.DrawPile || material.To != CardLocation.Hand(recipient) || batch.Movements.Count == 0 || !batch.Movements.Contains(material) || batch.Movements.Any(move => !_coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Contains(move)))
            return false;
        var cursorMatches = producer.NativeDraw is { RecipientApplied: true } cursor && cursor.InstructionIndex == producer.InstructionIndex && cursor.RecipientIndex >= 0 && cursor.RecipientIndex < cursor.Recipients.Count && cursor.Recipients[cursor.RecipientIndex] == recipient && cursor.RequestedCount == actualDraw.Requested && cursor.Reason == actualDraw.Reason;
        if (!cursorMatches && !IsWushuangPaidTargetDrawInvocation(producer, actualDraw, recipient))
            return false;
        var actual = _coreCardMovementJournalCardMovementsMovementSequenceAccess._cardMovements.Where(move => move.Sequence > actualDraw.SequenceBefore && move.Sequence <= actualDraw.SequenceAfter && move.From == CardLocation.DrawPile && move.To == CardLocation.Hand(recipient) && move.Reason == actualDraw.Reason).ToArray();
        return actual.Length == actualDraw.ActualCount && actual.Select(move => move.CardId).Distinct().Count() == actual.Length && actual.Contains(material);
    }

    internal bool IsWushuangPaidTargetDrawInvocation(ProgramSkillFrame producer, NativeDrawInvocationRecordedEvent actualDraw, int recipient)
    {
        if (producer.UniqueHpPeer is not { Stage: UniqueHpPeerStage.DrawChildren } paid || paid.ChooserSeat != recipient || !_coreRecipientContestReceiptsOperations.ValidUniqueHpPeer(producer))
            return false;
        var invoice = recipient == producer.OwnerSeat ? paid.OwnerInvoice : paid.PeerInvoice;
        return invoice is { DrawIssued: true, ActualDrawCount: > 0 and <= 2 } && invoice.ActorSeat == recipient && invoice.DrawBefore >= invoice.CostAfter && invoice.DrawAfter > invoice.DrawBefore && actualDraw.Requested == 2 && actualDraw.ActualCount == invoice.ActualDrawCount && actualDraw.TurnNumber == paid.TurnNumber && actualDraw.Reason.Value == CoreGameEngineUniqueHpPeerProgramsRuntime.UniqueHpPeerDrawReason(producer, recipient) && actualDraw.SequenceBefore == invoice.DrawBefore && actualDraw.SequenceAfter == invoice.DrawAfter && _coreProgramEventHistoryOperations.ProgramEventHistory<UniqueHpPeerDrawIssuedEvent>().Where(issued => issued.FrameId == producer.Id && issued.ActorSeat == recipient).ToArray()is [var issued] && issued.Before == invoice.DrawBefore && issued.After == invoice.DrawAfter && issued.ActualCount == invoice.ActualDrawCount;
    }

    internal bool IsWushuangNativeDrawDamageParent(ProgramSkillFrame producer)
    {
        var rootIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == producer.Id);
        // Reuse the existing typed movement/HP observer edges rather than
        // identifying each descendant by the name of its trigger window.
        while (rootIndex > 0 && WushuangNativeDrawObserverEdge(_resolutionStackOperations._resolutionStack[rootIndex - 1], _resolutionStackOperations._resolutionStack[rootIndex]))
            rootIndex--;
        if (rootIndex < 1 || _resolutionStackOperations._resolutionStack[rootIndex] is not ProgramSkillFrame root || root.WindowContext is not { } origin || root.ParentFrameId != origin.ParentFrameId)
            return false;
        if (root.UniqueHpPeer is { } paidTarget && _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId == paidTarget.CardUseFrameId && _coreActiveNativeAttackQueriesOperations.ActiveCardAttack?.ResolutionId == paidTarget.CardUseFrameId && _coreRecipientContestObserversOperations.RecipientContestPaidObserverRoot()?.Id == root.Id && _coreRecipientContestObserversOperations.HasUniqueHpPeerActualTargetObserver(paidTarget.CardUseFrameId))
            return true;
        if (origin.Window == SkillProgramTriggerWindow.BeforeDamageApplied && _resolutionStackOperations._resolutionStack[rootIndex - 1] is BeforeDamageProgramWindowFrame before)
        {
            if (before.Id != origin.ParentFrameId || before.TargetSeat != origin.TargetSeat || before.Amount != origin.Amount || (_coreDamagePreventionProgramsOperations.BeforeDamageHasNoSource(before) ? origin.SourceSeat is not null : origin.SourceSeat != before.SourceSeat) || _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId != (before.ContinuationAttackResolutionId ?? before.ParentFrameId) || before.CandidateIndex < 0 || before.CandidateIndex >= before.Candidates.Count)
                return false;
            var candidate = before.Candidates[before.CandidateIndex].Candidate;
            return candidate.OwnerSeat == root.OwnerSeat && candidate.SkillId == root.SkillId && candidate.BindingId == root.TriggerId && candidate.SkillInstanceId == root.SkillInstanceId && candidate.GameplayHash == root.GameplayHash;
        }

        return origin.Window is SkillProgramTriggerWindow.AfterDamageApplied or SkillProgramTriggerWindow.DamageAppliedBeforeDying && rootIndex >= 2 && _resolutionStackOperations._resolutionStack[rootIndex - 1] is DamageTriggerWindowFrame after && _resolutionStackOperations._resolutionStack[rootIndex - 2] is DamageFrame damage && after.Id == origin.ParentFrameId && after.ParentFrameId == damage.Id && origin.DamageFrameId == damage.Id && origin.SourceSeat == damage.SourceSeat && origin.TargetSeat == damage.TargetSeat && origin.Amount == damage.Amount && _coreProgramAttackAttemptOperations.CurrentDamageAttempt?.ResolutionId == damage.ParentFrameId && _coreDamageObserverCursorQueriesOperations.DamageProgramMatchesWindow(root, after);
    }

    internal bool WushuangNativeDrawObserverEdge(ResolutionFrame parent, ResolutionFrame child)
    {
        if (_coreDamageObserverCursorQueriesOperations.TypedDamagePostEventEdge(parent, child))
            return true;
        return parent is ProgramSkillFrame { NativeDraw: { RecipientApplied: true } paid } producer && child is CardsMovedTriggerWindowFrame moved && paid.InstructionIndex == producer.InstructionIndex && paid.RecipientIndex >= 0 && paid.RecipientIndex < paid.Recipients.Count && moved.Id == moved.Batch.Id && moved.Batch.ParentFrameId == producer.Id && moved.Batch.OriginOwnerSeat == producer.OwnerSeat && moved.Batch.OriginSkillId == producer.SkillId && moved.Batch.OriginSkillInstanceId == producer.SkillInstanceId && (moved.ResumeProgramFrameId is null || moved.ResumeProgramFrameId == producer.Id) && (moved.Batch.AwaitingProgramFrameId is null || moved.Batch.AwaitingProgramFrameId == producer.Id) && !RuntimeSkills.RuntimeRules_Core_CommonRules.HasAlternateProgramMovementReturn(moved) && (moved.Batch.NativeDrawInvocation is not { } draw || paid.RequestedCount == draw.RequestedCount && paid.Reason == draw.Reason) && moved.Batch.Movements.Any(material => material.From == CardLocation.DrawPile && material.To == CardLocation.Hand(paid.Recipients[paid.RecipientIndex]) && material.Reason == paid.Reason && IsWushuangNativeDrawDamageMaterial(producer, moved.Batch, material, paid.Recipients[paid.RecipientIndex]));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 61400)]
    internal bool ContinueWushuangCombat(long frameId)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WushuangCombat: { } r } f || f.Id != frameId)
            return false;
        if (f.InstructionIndex != r.InstructionIndex || r.Stage == 10 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)is not { Op: SkillProgramEffectOp.ResolveWushuangCombatTactic } effect || effect.Amount != r.TacticId)
            throw new InvalidOperationException("A combat tactic lost its exact owning instruction.");
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(frameId, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(frameId, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(frameId, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(frameId))
            return true;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(frameId);
        r = f.WushuangCombat!;
        if (r.DrawCount > 0 && r.Stage < 2)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangCombat = r = r with { Stage = 2 } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, r.DrawCount, r.TacticId is 2039 or 2040 or 2365 ? CardMoveReasons.RecastDraw : new("wushuang.combat.draw"));
            return true;
        }

        while (r.Index < r.Seats.Count && _coreMatchOutcomeWinnerAccess._winner == Winner.None)
        {
            var seat = r.Seats[r.Index];
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangCombat = r = r with { Stage = 3, Index = r.Index + 1 } });
            if (!_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
                continue;
            _coreSkillProgramsOperations.BeginProgramSkillDamage(f, seat, r.Amount, nature: r.Nature, sourceLess: r.TacticId == 2019);
            return true;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f with { WushuangCombat = null });
        return false;
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveWushuangCombatTactic)]
    internal void ReturnWushuangCombatDraw(ProgramSkillFrame frame, NativeDrawOutcome outcome)
    {
        if (frame.WushuangCombat is not { Stage: 2 } r || outcome.RecipientSeat != frame.OwnerSeat || outcome.RequestedCount != r.DrawCount)
            throw new InvalidOperationException("Combat draw lost its exact paid instruction.");
        var reason = r.TacticId is 2039 or 2040 or 2365 ? CardMoveReasons.RecastDraw : new CardMoveReason("wushuang.combat.draw");
        _coreProgramLifecycleOperations.CompleteProgramDrawCards(frame.Id, frame.OwnerSeat, null, SkillProgramCardSetVisibility.Private, reason, 0, outcome.ActualDrawIds);
        if (r.TacticId is 2039 or 2040 or 2365)
        {
            var effect = _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(frame)!;
            var set = _coreProgramLifecycleOperations.GetProgramCardSet(frame, effect.SourceBind!);
            foreach (var id in set.CardIds)
            {
                var card = _cardZonesOperations._cardZones.CardsAt(_cardZonesOperations._cardZones.GetLocation(id)).Single(c => c.Id == id);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardRecastEvent(frame.OwnerSeat, id, card.Kind, outcome.ActualDrawIds.Count));
            }

            _coreNativeRecastInvocationsOperations.CompleteFrameNativeRecastInvocation(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id), outcome.ActualDrawIds.Count);
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    internal bool IsWushuangLightningJudgment(ProgramSkillFrame f, int target, string reason, string bind, SkillProgramCardSetVisibility visibility) => _coreWushuangModeProjectionOperations.IsWushuangMode && f.WushuangCombat is { TacticId: 2019, Stage: 10 } && target == _coreActualTurnProgressCurrentSeatAccess._currentSeat && reason == "闪电" && bind == "wushuang-lightning" && visibility == SkillProgramCardSetVisibility.Public;
    internal bool TryReturnWushuangLightningJudgment(JudgmentFrame judgment)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { WushuangCombat: { TacticId: 2019, Stage: 10 } r } f || f.Id != judgment.ParentFrameId)
            return false;
        if (!IsWushuangLightningJudgment(f, judgment.TargetSeat, judgment.Reason, judgment.ProgramResultBind!, judgment.ProgramResultVisibility!.Value))
            throw new InvalidOperationException("Lightning tactic lost its exact judgment return.");
        var card = _coreNativeDamageHandlesOperations.GetJudgmentCard(judgment);
        var hit = card is not null && _characterZhangQiYingOlOperations.EffectiveZhenyiJudgmentSuit(judgment, _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[judgment.TargetSeat], card)) == Suit.Spade && card.Rank is >= 2 and <= 9;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { WushuangCombat = r with { Stage = 0, TargetSeat = judgment.TargetSeat, Amount = hit ? 3 : 0, Nature = DamageNature.Thunder, Seats = hit ? [judgment.TargetSeat] : [] } });
        if (card is not null && _cardZonesOperations._cardZones.GetLocation(card.Id)is { Zone: CardZoneKind.Judgment or CardZoneKind.Processing } location)
            _corePhysicalMovementFacadesOperations.MoveCard(card, location, CardLocation.DiscardPile, CardMoveReasons.JudgmentFinish);
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return true;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 61400)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 61400)]
    internal bool RetainWushuangCombat(ProgramSkillFrame f) => f.WushuangCombat is not null;
}
