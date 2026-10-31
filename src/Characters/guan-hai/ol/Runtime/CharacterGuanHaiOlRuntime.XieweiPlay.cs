#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuanHaiOlRuntime
{
    internal const string XieweiPlayPrefix = "xiewei-basic-play:";
    [ProgramActionSource(ProgramActionSourcePhase.Cardless, 27578)]
    internal IEnumerable<LegalAction> BuildXieweiPlayActions(CharacterState owner)
    {
        if (!HasGuanHaiCapability || !owner.IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || (_coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase != TurnPhase.Play || owner.Seat != _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat) && !_characterKeBiNengOlOperations.IsKeBiNengPingduanBasicQuery(owner))
            yield break;
        foreach (var s in _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(owner, CardKind.Slash, false).Where(IsXieweiSelection))
        {
            var targets = XieweiPlayTargets(owner, s);
            if (targets.Length == 0)
                continue;
            var ids = Array.AsReadOnly(s.Cards.Select(c => c.Id).Order().ToArray());
            yield return new(LegalActionKind.UseProgramSkill, null, null, "卸尾：将两张手牌置为饵，视为使用杀", CardKind.Slash, MinCardCount: 2, MaxCardCount: 2, MinTargetCount: 1, MaxTargetCount: Math.Min(targets.Length, XieweiTargetMaximum(owner)))
            {
                ProgramSkillId = s.Source.SkillId,
                ProgramActivationId = XieweiPlayPrefix + "Slash:" + string.Join('-', ids),
                ConversionSource = s.Source,
                SelectableCardIds = ids,
                SelectableTargetSeats = Array.AsReadOnly(targets),
                ProgramAiHint = new(0, 0, 0, 0, 0, 1, false, false)
            };
        }
    }

    internal int[] XieweiPlayTargets(CharacterState actor, ProgramMultiCardViewAsSelection s) => _coreMatchParticipantsPlayersAccess._players.Where(p => _coreNativeTargetLegalityOperations.CanUseVirtualSlashTarget(actor, p, CardKind.Slash, Suit.None, effectiveColor: null, physicalCardIds: s.Cards.Select(c => c.Id).ToArray())).Select(p => p.Seat).ToArray();
    internal int XieweiTargetMaximum(CharacterState actor) => Math.Max(1, global::CardGame.Core.EngineOwners.CoreGameEngineRuleQueriesRuntime.ConvertRuleValue(_coreRuleQueriesOperations.EvaluateCardTargetCount(actor, CardKind.Slash))) + (_coreNextCardTargetAdjustmentsOperations.HasLegacyNextCardTargetAdjustment(actor) || _coreNextActualUseTargetAdjustmentsOperations.HasNextActualUseAdjustment(actor) ? 1 : 0);
    internal CommandError? ValidateXieweiPlay(CharacterState actor, LegalAction a, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (_coreSkillProgramsOperations.ValidateProgramSelection(a, ids, targets)is { } error)
            return error;
        if (a.ConversionSource is not { } source || !IsXieweiSource(source) || _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, CardKind.Slash, false).SingleOrDefault(s => s.Source == source && s.Cards.Select(c => c.Id).Order().SequenceEqual(ids.Order()))is not { } selection)
            return new(CommandErrorCode.InvalidCard, "卸尾必须支付当前两张真实手牌。");
        return targets.Count < 1 || targets.Count > XieweiTargetMaximum(actor) || targets.Distinct().Count() != targets.Count || targets.Any(t => !XieweiPlayTargets(actor, selection).Contains(t)) ? new(CommandErrorCode.InvalidTarget, "卸尾的杀须遵守原生目标、距离及次数限制。") : null;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Submit, 27578)]
    internal bool SubmitXieweiPlay(UseProgramSkillCommand command, out CommandResult result)
    {
        result = null !;
        if (!command.ActivationId.StartsWith(XieweiPlayPrefix, StringComparison.Ordinal))
            return false;
        if (_coreNativePromptBoundaryOperations.ValidateHumanPrompt(command.ActorSeat, DecisionKind.PlayCard, command.PromptId, CommandErrorCode.IllegalAction)is { } error)
        {
            result = _kernelCoreMainOperations2.Reject(error.Code, error.Message);
            return true;
        }

        var actor = _coreMatchParticipantsPlayersAccess._players[command.ActorSeat];
        var a = BuildXieweiPlayActions(actor).SingleOrDefault(a => a.ProgramSkillId == command.SkillId && a.ProgramActivationId == command.ActivationId);
        if (a is null || command.SkillOwnerSeat is not null)
        {
            result = _kernelCoreMainOperations2.Reject(CommandErrorCode.IllegalAction, "卸尾真实使用菜单已失效。");
            return true;
        }

        if (ValidateXieweiPlay(actor, a, command.CardIds, command.TargetSeats)is { } invalid)
        {
            result = _kernelCoreMainOperations2.Reject(invalid.Code, invalid.Message);
            return true;
        }

        result = _kernelCoreMainOperations2.Accept(() =>
        {
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
            ExecuteXieweiPlay(actor, a, command.CardIds, command.TargetSeats);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
            if (_coreMatchDefinitionOptionsAccess._options.AdvanceAfterHumanCommands)
                _kernelCoreGameEngineRuntimeOperations.AdvanceToHumanBoundary();
        });
        return true;
    }

    [ProgramActionHandler(ProgramActionHandlerKind.Execute, 27578)]
    internal bool ExecuteXieweiPlay(CharacterState actor, LegalAction a, IReadOnlyList<int> ids, IReadOnlyList<int> targets)
    {
        if (a.ProgramActivationId?.StartsWith(XieweiPlayPrefix, StringComparison.Ordinal) != true)
            return false;
        if (ValidateXieweiPlay(actor, a, ids, targets)is { } invalid)
            throw new InvalidOperationException(invalid.Message);
        var s = _coreCardConversionsOperations.GetProgramMultiCardViewAsSelections(actor, CardKind.Slash, false).Single(s => s.Source == a.ConversionSource && s.Cards.Select(c => c.Id).Order().SequenceEqual(ids.Order()));
        if (targets.Count == 1)
            _coreNativeSlashUseOperations.ResolveSlashCore(actor, _coreMatchParticipantsPlayersAccess._players[targets.Single()], _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, s.Cards[0], s.Source), CardKind.Slash, actor.Seat, physicalCards: s.Cards, conversionSource: s.Source);
        else
            BeginXieweiMultiTargetSlash(actor, s, targets);
        return true;
    }

    internal void BeginXieweiMultiTargetSlash(CharacterState actor, ProgramMultiCardViewAsSelection s, IReadOnlyList<int> targets)
    {
        var card = _corePlayCardHistoryOperations.ApplyProgramUseAppearance(actor, s.Cards[0], s.Source);
        var armor = _coreNativeEquipmentAndFactionQueriesOperations.HasArmorBypass(actor);
        var id = _coreNativeCardUseOperations.BeginCardUse(card, actor.Seat, targets, CardKind.Slash, ignoresArmor: armor, physicalCardIds: s.Cards.Select(c => c.Id).ToArray(), conversionSource: s.Source);
        var nuzhan = _coreCardConversionsOperations.GetNuzhanModifiers(id, actor);
        _coreCardUsePaymentsOperations.PaySlashUseMaterials(s.Cards, actor.Seat, null);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        if (!IsXieweiMultiTargetUse(use))
            throw new InvalidOperationException("Xiewei's multiple targets lost their original native allowance and paid costs.");
        var counted = use.UnlimitedUse != true && !_coreJudgedRankSlashPoliciesOperations.HasIssuedJudgedRankSlashQuota(id, actor.Seat) && !_characterLuKangWumiaoOperations.HasIssuedWuLuKangDixianUse(id, actor.Seat) && !_coreRoundGainedSourceUsesOperations.HasIssuedRoundGainedBasicBonus(id, actor.Seat) && !_characterHuBanOlOperations.HasIssuedHuiyunFollowup(id, actor.Seat) && !_characterLiYiOlOperations.HasIssuedChanshuangSlash(id, actor.Seat) && !_characterSunYuanOlOperations.HasIssuedDongxinEntitySlashQuota(id, actor.Seat) && !nuzhan.IgnoresSlashLimit && !_coreCardConversionsOperations.IgnoresProgramSlashLimit(actor, s.Source) && _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._phase == TurnPhase.Play && actor.Seat == _coreActualTurnProgressCurrentSeatPhaseTurnNumberTurnProgressionAccess._currentSeat;
        if (counted)
            _coreCardUseDebitsOperations.RecordSlashUseDebit(id, actor.Seat);
        _coreNativeOwnedCardQueriesOperations.MarkSlashUsedOrPlayedDuringCurrentPlayPhase(actor.Seat, CardKind.Slash);
        var damage = 1 + actor.AlcoholDamageBonus + nuzhan.DamageBonus;
        _coreAlcoholSlashSuppressionsOperations.CaptureProgramAlcoholConsumption(id, actor);
        actor.HasAlcoholEffect = false;
        var pending = _kernelCoreMainOperations.CreateFangtianHalberdHandle(id, actor.Seat, card, CardKind.Slash, armor, damage, targets, false, counted, s.Source);
        _coreNativeCardContinuationCursorsOperations.ActiveFangtianHalberd = pending;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramViewAsConvertedEvent(id, s.Source.SkillId, s.Source.BindingId, actor.Seat, Array.AsReadOnly(s.Cards.Select(c => c.Id).ToArray()), CardKind.Slash, true, targets));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardUsedEvent(card.Id, CardKind.Slash, actor.Seat, targets[0], IgnoresArmor: armor));
        foreach (var t in targets)
            _coreNativeAiDecisionsOperations.NotifyAiOfSlash(actor, _coreMatchParticipantsPlayersAccess._players[t]);
        _coreNativeSlashUseOperations.BeginNextFangtianHalberdTarget(pending);
    }

    internal XieweiNativePayment? IssuedXieweiPayment(CardActionContext action) => _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().SelectMany(u => u.XieweiPayments ?? []).SingleOrDefault(p => p.Paid && p.ActionId == action.ActionId && XieweiActionCosts(p, action) && ValidXieweiPayment(p));
    [ProgramPipelineHook(ProgramPipelinePhase.CollectIssuedCardTrigger, 6)]
    internal void CollectIssuedXieweiCandidates(CardActionContext action, SkillProgramTriggerWindow window, List<ProgramCardTriggerCandidate> candidates)
    {
        if (window != SkillProgramTriggerWindow.CardUseCommitted || IssuedXieweiPayment(action)is not { } p || _coreProgramEventHistoryOperations.ProgramEventHistory<XieweiCommittedEvent>().Any(e => e.PaymentId == p.Id))
            return;
        var t = _contentRegistryOperations._contentRegistry.GetSkill(p.Source.SkillId).Program!.Triggers.Single(t => t.Effects is [{ Op: SkillProgramEffectOp.ResolveXieweiTwoHandBaitBasicUse }]);
        var context = _coreCardActionsOperations.CreateCardActionProgramContext(action, window, 0, p.OwnerSeat, -1, _coreProgramLifecycleOperations.CaptureProgramTriggerFacts2(_coreMatchParticipantsPlayersAccess._players[p.OwnerSeat], action));
        candidates.Add(new(p.OwnerSeat, -1, p.Source.SkillId, t.Id, p.GameplayHash, p.Source.SkillInstanceId, t.Priority, context));
    }

    [ProgramTriggerProof(ProgramTriggerProofPhase.OwnerException, 40)]
    [ProgramTriggerProof(ProgramTriggerProofPhase.InstanceException, 230)]
    internal bool IsIssuedXieweiCandidate(ProgramTriggerCandidate c, ProgramSkillWindowContext context)
    {
        if (c.SkillId != "ol:xiewei" || context.Window != SkillProgramTriggerWindow.CardUseCommitted || context.OwnerSeat != c.OwnerSeat || _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame w || w.CandidateIndex < 0 || w.CandidateIndex >= w.Candidates.Count || global::CardGame.Core.EngineOwners.CoreGameEngineCardActionsRuntime.ToSharedCandidate(w.Candidates[w.CandidateIndex]) != c || IssuedXieweiPayment(w.Action)is not { } p || p.Source.SkillInstanceId != c.SkillInstanceId || p.OwnerSeat != c.OwnerSeat || p.GameplayHash != c.GameplayHash || context.CardUse?.CardActionId != p.ActionId || _coreProgramLifecycleOperations.GetProgramTrigger2(c).Effects is not [{ Op: SkillProgramEffectOp.ResolveXieweiTwoHandBaitBasicUse }])
            return false;
        return !_coreProgramEventHistoryOperations.ProgramEventHistory<XieweiCommittedEvent>().Any(e => e.PaymentId == p.Id);
    }
}
