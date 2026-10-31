#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMaChaoMobileGodRuntime
{
    internal const string MobileMaChaoYuli = "mobile-god:yuli", MobileMaChaoTingwei = "mobile-god:tingwei", MobileMaChaoJimie = "mobile-god:jimie";
    internal bool HasMobileShenMaChao => _contentRegistryOperations._contentRegistry.ProgramDependencies.HasTriggerOperation(SkillProgramEffectOp.ResolveMobileShenMaChaoTingwei);

    internal static CardConversionSource MobileMaChaoSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal DamageNature MobileMaChaoYuliNature(global::CardGame.Core.GameEngine.IDamageAttempt attack, DamageNature priorNature) => !attack.IsSourceLess && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], MobileMaChaoYuli) ? DamageNature.Thunder : priorNature;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 40625)]
    internal int MobileMaChaoSourceDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => !attack.IsSourceLess && !attack.IsChainPropagation && _coreTableSeatQueriesOperations.IsValidPlayerSeat(attack.SourceSeat) && _coreNationalSkillsOperations.HasRuntimeSkill(_coreMatchParticipantsPlayersAccess._players[attack.SourceSeat], MobileMaChaoYuli) && _coreNativeDamagePoliciesOperations.GetDamageNatureBeforeMobileMaChao(attack) == DamageNature.Thunder ? 1 : 0;
    [ProgramDamageHook(ProgramDamagePhase.AdditiveAmount, 40626)]
    internal int MobileTingweiCardDamageBonus(global::CardGame.Core.GameEngine.IDamageAttempt attack) => attack.IsChainPropagation ? 0 : (_coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)?.MobileTingweiBoosts ?? []).Count(b => b.Target == attack.TargetSeat && b.UseId == attack.ResolutionId && _coreCardUseLifecycleOperations.LifecycleCardUse(b.UseId)?.Action?.ActionId == b.ActionId);
    internal bool IsMobileMaChaoTingweiGrantQualified(CharacterState owner, SkillGrant grant) => !HasMobileShenMaChao || _contentRegistryOperations._contentRegistry.GetSkill(grant.SkillId).Tags.HasFlag(SkillTag.Locked) || !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileTingweiSuppressedEvent>().Any(e => e.Target == owner.Seat && !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileTingweiExpiredEvent>().Any(x => x.FrameId == e.FrameId));
    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 40625)]
    internal void ExpireMobileTingwei(IGameEvent fact)
    {
        if (!HasMobileShenMaChao || fact is not (TurnEndedEvent or PlayerDiedEvent))
            return;
        foreach (var e in _coreProgramEventHistoryOperations.ProgramEventHistory<MobileTingweiSuppressedEvent>().Where(e => !_coreProgramEventHistoryOperations.ProgramEventHistory<MobileTingweiExpiredEvent>().Any(x => x.FrameId == e.FrameId) && (fact is TurnEndedEvent t && t.ActorSeat == e.Target && t.TurnNumber > e.ActualTurn || fact is PlayerDiedEvent d && d.VictimSeat == e.Target)).ToArray())
        {
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileTingweiExpiredEvent(e.FrameId, e.Target, _coreActualTurnProgressTurnNumberAccess._turnNumber));
            _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
        }
    }

    internal long LastMobileJimieUse(int owner) => _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaChaoJimieUsedEvent>().LastOrDefault(e => e.Source.OwnerSeat == owner)?.FrameId ?? 0;
    internal bool CanMobileJimie(int owner) => LastMobileJimieUse(owner)is 0 || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaChaoJimieRefreshedEvent>().Any(e => e.Owner == owner && e.UseFrameId == LastMobileJimieUse(owner));
    internal void RecordMobileYuli(ProgramSkillFrame f, int part)
    {
        var last = LastMobileJimieUse(f.OwnerSeat);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileMaChaoYuliExecutedEvent(f.Id, f.OwnerSeat, part, last));
        if (last == 0 || CanMobileJimie(f.OwnerSeat))
            return;
        var used = _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaChaoYuliExecutedEvent>().Where(e => e.Owner == f.OwnerSeat && e.JimieUseFrameId == last).Select(e => e.Part).ToHashSet();
        if (used.SetEquals([1, 2]))
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileMaChaoJimieRefreshedEvent(last, f.OwnerSeat));
    }

    internal CardActionContext? MobileMaChaoAction(ProgramSkillWindowContext? c) => c is null ? null : _resolutionStackOperations._resolutionStack.OfType<ProgramCardTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId)?.Action;
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40625)]
    internal bool MobileMaChaoTriggerEligible(ProgramTriggerLegalityContext g)
    {
        var op = _coreProgramLifecycleOperations.GetProgramTrigger2(g.Candidate).Effects[0].Op;
        if (op == SkillProgramEffectOp.ResolveMobileShenMaChaoJimie)
            return CanMobileJimie(g.OwnerSeat) && g.Owner.Markers.GetValueOrDefault(PlayerMarkerKind.MobileShenMaChaoTing) >= 8;
        if (op == SkillProgramEffectOp.ResolveMobileShenMaChaoTingwei)
            return MobileMaChaoAction(g.WindowContext)is { Type: CardActionType.Use } a && a.ActorSeat == g.OwnerSeat && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) && a.TargetSeats.Any(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive);
        if (op != SkillProgramEffectOp.ResolveMobileShenMaChaoYuli)
            return true;
        return g.WindowContext is { Amount: > 0 } && _coreProgramAttackAttemptOperations.CurrentDamageAttempt is { } damage && (g.Trigger.Id == "source-thunder" ? !damage.IsSourceLess && damage.SourceSeat == g.OwnerSeat : damage.TargetSeat == g.OwnerSeat && _coreNativeCardClassificationsOperations.GetDamageNature(damage) == DamageNature.Thunder);
    }

    SkillProgramStepOutcome global::CardGame.Core.IMobileShenMaChaoHost.Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => BeginMobileMaChao(f, op);
    internal SkillProgramStepOutcome BeginMobileMaChao(ProgramSkillFrame supplied, SkillProgramEffectOp op)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.MobileShenMaChao is not null || f.InstructionIndex != 1 || f.TriggerId is null)
            throw new InvalidOperationException("Mobile Ma Chao requires his original accepted trigger instruction.");
        var r = new MobileMaChaoReceipt(MobileMaChaoSource(f), f.GameplayHash, 1, op, MobileMaChaoStage.Children);
        if (op == SkillProgramEffectOp.ResolveMobileShenMaChaoYuli)
        {
            RecordMobileYuli(f, f.TriggerId == "source-thunder" ? 1 : 2);
            if (f.TriggerId == "source-thunder")
                return SkillProgramStepOutcome.Continue;
            var amount = f.WindowContext!.Amount;
            _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { MobileShenMaChao = r with { Stage = MobileMaChaoStage.Drawing, DrawCount = amount, Target = f.OwnerSeat } });
            _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, f.OwnerSeat, amount, new("skill.mobile-shen-ma-chao.yuli.draw"));
            return SkillProgramStepOutcome.AwaitChild;
        }

        if (op == SkillProgramEffectOp.ResolveMobileShenMaChaoTingwei)
        {
            var a = MobileMaChaoAction(f.WindowContext) ?? throw new InvalidOperationException("Tingwei lost its actual whole Slash action.");
            var use = _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Single(u => u.Action?.ActionId == a.ActionId);
            r = r with
            {
                Stage = MobileMaChaoStage.Target,
                UseId = use.Id,
                ActionId = a.ActionId,
                Targets = a.TargetSeats.ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = r });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileTingweiStartedEvent(f.Id, r.Source, use.Id, a.ActionId));
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.MobileShenMaChaoTing, 4);
        }
        else
        {
            if (!CanMobileJimie(f.OwnerSeat) || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MobileShenMaChaoTing) < 8)
                throw new InvalidOperationException("Jimie lost its available limited use or eight real Ting.");
            r = r with
            {
                Stage = MobileMaChaoStage.Target,
                Targets = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive).OrderBy(p => _coreTableOrderOperations.RelativeTableOrder(f.OwnerSeat, p.Seat)).Select(p => p.Seat).ToArray()
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = r });
        }

        ContinueMobileMaChao(f.Id);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice MobileMaChaoChoice(ProgramSkillFrame f, string option, string text, int? target = null) => new(new($"mobile-ma-chao.{(f.Id)}.{(f.MobileShenMaChao!.Stage)}.{(option)}"), text, [], target is { } seat ? [seat] : [], new Dictionary<string, string> { ["program-action"] = "mobile-ma-chao", ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture), ["option"] = option });
    internal IReadOnlyList<PromptChoice> MobileMaChaoChoices(ProgramSkillFrame f)
    {
        var r = f.MobileShenMaChao!;
        if (r.Stage == MobileMaChaoStage.Target)
            return Array.AsReadOnly(r.Targets.Where(t => _coreMatchParticipantsPlayersAccess._players[t].IsAlive).Select(t => MobileMaChaoChoice(f, "target:" + t, "选择" + _coreMatchParticipantsPlayersAccess._players[t].Name, t)).ToArray());
        if (r.Stage == MobileMaChaoStage.Gift)
            return Array.AsReadOnly(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, r.Target, r.Target, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Transfer, cardCategories: [SkillProgramCardCategory.Equipment], canSelect: (_, c) => !c.IsGeneralWeapon).Select(c => PromptChoiceProjection.Freeze(c with { Id = new($"mobile-ma-chao.{(f.Id)}.gift.{(c.Id.Value)}"), Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mobile-ma-chao", ["option"] = "gift" } })).ToArray());
        if (r.Stage != MobileMaChaoStage.Options)
            return[];
        var gift = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[r.Target]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[r.Target])).Any(c => _coreEquipmentContentOperations.EquipmentRules.IsEquipment(c.Kind) && !c.IsGeneralWeapon);
        var discard = MobileMaChaoDiscardable(f.OwnerSeat, r.Target).Length > 0;
        var names = new[]
        {
            "非锁定技失效至下回合结束",
            "交给神马超一张装备牌",
            "此牌对自己伤害+1",
            "随机弃置一张牌"
        };
        return Array.AsReadOnly(Enumerable.Range(0, 16).Where(mask => (gift || (mask & 2) == 0) && (discard || (mask & 8) == 0)).Select(mask => MobileMaChaoChoice(f, "mask:" + mask, mask == 0 ? "均不选择，进入连环状态" : string.Join("；", Enumerable.Range(0, 4).Where(i => (mask & (1 << i)) != 0).Select(i => names[i])))).ToArray());
    }

    internal Card[] MobileMaChaoDiscardable(int source, int target) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[target]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[target])).Where(c => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(target, c, _cardZonesOperations._cardZones.GetLocation(c.Id), OwnedCardMoveIntent.Discard)).ToArray();
    internal void PublishMobileMaChao(ProgramSkillFrame f)
    {
        var r = f.MobileShenMaChao!;
        var choices = MobileMaChaoChoices(f);
        if (choices.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShenMaChao = r with { Stage = MobileMaChaoStage.Execute } });
            ContinueMobileMaChao(f.Id);
            return;
        }

        var chooser = r.Stage == MobileMaChaoStage.Target ? f.OwnerSeat : r.Target;
        var s = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MobileMaChaoStage.Options ? "霆威：一次选择要执行的任意项。" : "请选择。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, s.Name, s.Name, s.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mobile-ma-chao")]
    internal void AnswerMobileMaChao(PromptChoice c)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShenMaChao: { } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MobileMaChaoStage.Target ? f.OwnerSeat : r.Target) || !MobileMaChaoChoices(f).Any(x => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([x], [c])))
            throw new InvalidOperationException("Mobile Ma Chao requires the exact frozen target, subset or genuine equipment payment.");
        AssertMobileMaChao(f);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == MobileMaChaoStage.Target)
        {
            var target = c.Targets.Single();
            if (r.Op == SkillProgramEffectOp.ResolveMobileShenMaChaoJimie)
            {
                if (!CanMobileJimie(f.OwnerSeat) || _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MobileShenMaChaoTing) < 8)
                    throw new InvalidOperationException("Jimie cannot spend unavailable Ting or reuse an unrefreshed limit.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = r with { Target = target, Stage = MobileMaChaoStage.Children } });
                _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.MobileShenMaChaoTing, -8);
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileMaChaoJimieUsedEvent(f.Id, r.Source, target, 8, _coreActualTurnProgressTurnNumberAccess._turnNumber));
            }
            else
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = r with { Target = target, Stage = MobileMaChaoStage.Options } });
        }
        else if (r.Stage == MobileMaChaoStage.Options)
        {
            var mask = int.Parse(c.Parameters["option"][5..], CultureInfo.InvariantCulture);
            var paid = System.Numerics.BitOperations.PopCount((uint)mask);
            if (_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].Markers.GetValueOrDefault(PlayerMarkerKind.MobileShenMaChaoTing) < paid)
                throw new InvalidOperationException("Tingwei cannot spend absent real markers.");
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = r with { Mask = mask, Cursor = 1, Stage = MobileMaChaoStage.Execute } });
            if (paid > 0)
                _coreAttributedMarkerEventsOperations.MutateParticipantMarker(f, f.OwnerSeat, PlayerMarkerKind.MobileShenMaChaoTing, -paid);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileTingweiSelectedEvent(f.Id, r.Target, mask, paid));
            if (mask == 0)
                _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().SetChainedState(f, true, r.Target);
        }
        else
        {
            var zone = Enum.Parse<CardZoneKind>(c.Parameters["source-zone"]);
            var slot = int.Parse(c.Parameters["slot-index"], CultureInfo.InvariantCulture);
            var from = new CardLocation(zone, r.Target);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[slot];
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = r with { Stage = MobileMaChaoStage.Execute, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, CardLocation.Hand(f.OwnerSeat), new("skill.mobile-shen-ma-chao.tingwei.gift"));
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = f.MobileShenMaChao!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileMaChaoMovedEvent(f.Id, "gift", r.Target, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
        }

        ContinueMobileMaChao(f.Id);
    }

    internal bool DrainMobileMaChao(long id) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(id, PostEventContinuation.AwaitedProgramMovement);
    internal void ContinueMobileMaChao(long id)
    {
        while (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame { MobileShenMaChao: { } r } f && f.Id == id)
        {
            if (r.Stage == MobileMaChaoStage.Drawing || f.AttackAttempt is not null)
                return;
            if (DrainMobileMaChao(id))
                return;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            r = f.MobileShenMaChao!;
            if (f.PendingMovementContinuation is not null)
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
            if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || r.Target >= 0 && !_coreMatchParticipantsPlayersAccess._players[r.Target].IsAlive)
            {
                FinishMobileMaChao(f);
                return;
            }

            if (r.Stage is MobileMaChaoStage.Target or MobileMaChaoStage.Options or MobileMaChaoStage.Gift)
            {
                PublishMobileMaChao(f);
                return;
            }

            if (r.Op == SkillProgramEffectOp.ResolveMobileShenMaChaoJimie && r.Stage == MobileMaChaoStage.Children)
            {
                var amount = _coreMatchParticipantsPlayersAccess._players[r.Target].MaxHp;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = r with { Damage = amount, Stage = MobileMaChaoStage.Damage } });
                _coreSkillProgramsOperations.BeginProgramSkillDamage(f, r.Target, amount);
                return;
            }

            if (r.Stage == MobileMaChaoStage.Execute && r.Cursor <= 4)
            {
                var option = r.Cursor;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MobileShenMaChao = r with { Cursor = option + 1 } });
                if ((r.Mask & (1 << (option - 1))) == 0)
                    continue;
                if (option == 1)
                {
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileTingweiSuppressedEvent(f.Id, r.Source, r.Target, _coreActualTurnProgressTurnNumberAccess._turnNumber));
                    _coreGameEngineCurrentTurnSkillSuppressionsCurrentTurnSkillSuppressionRevisionAccess._currentTurnSkillSuppressionRevision++;
                }
                else if (option == 2)
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShenMaChao = f.MobileShenMaChao!with { Stage = MobileMaChaoStage.Gift } });
                else if (option == 3)
                {
                    var b = new MobileTingweiBoost(f.Id, r.Source, r.UseId, r.ActionId, r.Target);
                    _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(r.UseId, u => u with { MobileTingweiBoosts = (u.MobileTingweiBoosts ?? []).Append(b).ToArray() });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileTingweiBoostedEvent(b));
                }
                else
                {
                    var cards = MobileMaChaoDiscardable(f.OwnerSeat, r.Target);
                    if (cards.Length == 0)
                        continue;
                    var randomBefore = _randomOperations._random.State;
                    var card = cards[_randomOperations._random.Next(cards.Length)];
                    var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
                    _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { MobileShenMaChao = _coreProgramLifecycleOperations.GetActiveProgramFrame(id).MobileShenMaChao!with { Before = before, After = before, RandomBefore = randomBefore, RandomAfter = _randomOperations._random.State }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
                    _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, r.Target, [card.Id], new("skill.mobile-shen-ma-chao.tingwei.random-discard"));
                    f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
                    _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShenMaChao = f.MobileShenMaChao!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
                    _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileMaChaoMovedEvent(id, "discard", r.Target, before, _coreCardMovementJournalMovementSequenceAccess._movementSequence));
                }

                continue;
            }

            FinishMobileMaChao(f);
            return;
        }
    }

    internal void FinishMobileMaChao(ProgramSkillFrame f)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShenMaChao = null, PendingMovementContinuation = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 40625)]
    internal bool ResumeMobileMaChao(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MobileShenMaChao: not null } f || f.Id != id)
            return false;
        ContinueMobileMaChao(id);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 40625)]
    internal bool ReturnMobileMaChaoMovement(ProgramSkillFrame f) => f.MobileShenMaChao is not null && ResumeMobileMaChao(f.Id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMobileShenMaChaoYuli)]
    internal void ReturnMobileMaChaoDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MobileShenMaChao is not { Stage: MobileMaChaoStage.Drawing } r || o.RecipientSeat != f.OwnerSeat || o.RequestedCount != r.DrawCount || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaChaoDrawReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Yuli lost its exact prevented native damage producer return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MobileShenMaChao = r with { Stage = MobileMaChaoStage.Children } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MobileMaChaoDrawReturnedEvent(f.Id, f.OwnerSeat, o.RequestedCount, o.ActualDrawIds.Count, o.SequenceBefore, o.SequenceAfter));
        ResumeMobileMaChao(f.Id);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.NativeCost, 40625)]
    internal bool MobileMaChaoMovementProof(ProgramSkillFrame f, SkillProgramEffect? e, ProgramMovementContinuation p) => f.MobileShenMaChao is { Stage: MobileMaChaoStage.Execute } && p.SubjectSeat == f.OwnerSeat && p.BeforeCount == 0 && p.CoverageResultBind is null;
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40625)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40625)]
    internal bool RetainMobileMaChao(ProgramSkillFrame f) => f.MobileShenMaChao is not null;
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShenMaChaoTingwei)]
    internal PromptChoice ChooseMobileTingwei(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShenMaChaoJimie)]
    internal PromptChoice ChooseMobileJimie(PendingDecision p, ProgramSkillFrame f) => p.Choices.Last();
    [ProgramAiChoice(SkillProgramEffectOp.ResolveMobileShenMaChaoYuli)]
    internal PromptChoice ChooseMobileYuli(PendingDecision p, ProgramSkillFrame f) => p.Choices.First();
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 40625)]
    internal void AssertMobileMaChao(ProgramSkillFrame f)
    {
        if (f.MobileShenMaChao is not { } r)
            return;
        if (r.Source != MobileMaChaoSource(f) || r.GameplayHash != f.GameplayHash || r.InstructionIndex != 1 || f.InstructionIndex != 1 || !Enum.IsDefined(r.Stage) || r.Targets.Distinct().Count() != r.Targets.Count || r.Targets.Any(t => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(t)) || r.Mask is < -1 or > 15 || r.Cursor is < 0 or > 5 || r.After < r.Before)
            throw new InvalidOperationException("Mobile Ma Chao lost his original trigger, frozen targets/subset or movement cursor.");
        if (r.Op == SkillProgramEffectOp.ResolveMobileShenMaChaoTingwei && (_coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not { Action: { } a } || a.ActionId != r.ActionId || a.ActorSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || !a.TargetSeats.SequenceEqual(r.Targets) || _coreProgramEventHistoryOperations.ProgramEventHistory<MobileTingweiStartedEvent>().Count(e => e == new MobileTingweiStartedEvent(f.Id, r.Source, r.UseId, r.ActionId)) != 1 || r.Mask >= 0 && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileTingweiSelectedEvent>().Count(e => e == new MobileTingweiSelectedEvent(f.Id, r.Target, r.Mask, System.Numerics.BitOperations.PopCount((uint)r.Mask))) != 1))
            throw new InvalidOperationException("Tingwei lost its actual whole Slash, chosen target or once-paid subset.");
        if (r.Op == SkillProgramEffectOp.ResolveMobileShenMaChaoJimie && r.Stage != MobileMaChaoStage.Target && _coreProgramEventHistoryOperations.ProgramEventHistory<MobileMaChaoJimieUsedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.Target == r.Target && e.Paid == 8) != 1)
            throw new InvalidOperationException("Jimie lost its once-paid eight Ting and actual rechargeable limited use.");
    }
}
