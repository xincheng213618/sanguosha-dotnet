#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuBuMouMobileRuntime
{
    internal const string MouLbObtainReason = "skill-program.mou-mobile:liyu.obtain", MouLbDrawReason = "skill-program.mou-mobile:liyu.draw";
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 31601, SkillProgramEffectOp.ResolveMouLuBuLiyu)]
    internal bool CanMouLbLiyu(int owner, ProgramSkillWindowContext c) => c.TargetSeat is { } victim && victim != owner && _coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _coreDamageTargetMountProgramsOperations.ActualDamageTargetMountUse(owner, c)is not null && _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[victim]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[victim])).Concat(_corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[victim])).Any(card => !_coreForeignEquipmentDiscardPoliciesOperations.IsForeignEquipmentDiscardPrevented(owner, card, _cardZonesOperations._cardZones.GetLocation(card.Id), OwnedCardMoveIntent.Obtain));
    internal SkillProgramStepOutcome BeginMouLbLiyu(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        var c = f.WindowContext!;
        if (f.MouLuBuLiyu is not null)
            throw new InvalidOperationException("Liyu cannot issue its accepted damage twice.");
        if (!CanMouLbLiyu(f.OwnerSeat, c))
            return SkillProgramStepOutcome.Continue;
        var use = _coreDamageTargetMountProgramsOperations.ActualDamageTargetMountUse(f.OwnerSeat, c)!;
        var r = new MouLuBuLiyuReceipt(MouLbSource(f), f.GameplayHash, c.ParentFrameId, c.DamageFrameId!.Value, use.Id, use.Action!.ActionId, c.TargetSeat!.Value, c.Amount, _coreActualTurnProgressTurnNumberAccess._turnNumber);
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = r });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuStartedEvent(f.Id, r.Source, r.GameplayHash, r.WindowId, r.DamageFrameId, r.OuterUseId, r.OuterActionId, r.VictimSeat, r.DamageAmount, r.ActualTurn));
        PublishMouLb(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal static int MouLbCategory(CardKind kind) => global::CardGame.Core.RuntimeSkills.RuntimeRules_lu_bu_mou_mobile_MouLiyu.MouLbCategory(kind);
    internal IReadOnlyList<PromptChoice> MouLbChoices(ProgramSkillFrame f)
    {
        var r = f.MouLuBuLiyu!;
        var choices = new List<PromptChoice>();
        Dictionary<string, string> Params(string branch) => new()
        {
            ["program-action"] = "mou-lu-bu-liyu",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch
        };
        if (r.Stage == MouLuBuLiyuStage.ChoosingCard)
        {
            if (r.Selections < r.DamageAmount && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive)
                choices.AddRange(_coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.VictimSeat, global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones, OwnedCardMoveIntent.Obtain).Select(c => c with { Parameters = new Dictionary<string, string>(c.Parameters) { ["program-action"] = "mou-lu-bu-liyu", ["branch"] = "take" } }));
            choices.Add(new(new($"mou-liyu.{(f.Id)}.finish"), $"结束获得（已选{(r.Selections)}/{(r.DamageAmount)}张）", [], [], Params("finish")));
        }
        else if (r.Stage == MouLuBuLiyuStage.VictimChoice && _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive)
        {
            choices.Add(new(new($"mou-liyu.{(f.Id)}.wushuang"), "获得完整【无双】直至下个回合结束", [], [], Params("wushuang")));
            foreach (var target in _coreMatchParticipantsPlayersAccess._players.Where(p => CanMouLbDuel(f, p.Seat)))
                choices.Add(new(new($"mou-liyu.{(f.Id)}.duel.{(target.Seat)}"), $"令吕布视为对{(target.Name)}使用【决斗】", [], [target.Seat], Params("duel")));
        }

        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishMouLb(ProgramSkillFrame f)
    {
        var r = f.MouLuBuLiyu!;
        var chooser = r.Stage == MouLuBuLiyuStage.VictimChoice ? r.VictimSeat : f.OwnerSeat;
        var choices = MouLbChoices(f);
        if (choices.Count == 0)
        {
            FinishMouLb(f);
            return;
        }

        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, chooser, r.Stage == MouLuBuLiyuStage.VictimChoice ? "利驭：选择决斗目标或获得无双。" : "利驭：获得其区域里的至多伤害数张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = r.VictimSeat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[chooser].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAnswer("mou-lu-bu-liyu")]
    internal void AnswerMouLb(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuBuLiyu: { } r } f || !ValidMouLb(f) || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != (r.Stage == MouLuBuLiyuStage.VictimChoice ? r.VictimSeat : f.OwnerSeat) || !MouLbChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Liyu answer changed its actual chooser or concealed slot.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var branch = choice.Parameters["branch"];
        if (branch == "finish")
        {
            FinishMouLbTaking(f);
            return;
        }

        if (branch == "take")
        {
            if (!r.Paid && !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId))
            {
                FinishMouLb(f);
                return;
            }

            var from = new CardLocation(Enum.Parse<CardZoneKind>(choice.Parameters["source-zone"]), r.VictimSeat);
            var card = _cardZonesOperations._cardZones.CardsAt(from)[int.Parse(choice.Parameters["slot-index"], CultureInfo.InvariantCulture)];
            var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = r = r with { Paid = true, Selections = r.Selections + 1, Stage = MouLuBuLiyuStage.ObtainChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
            _corePhysicalMovementFacadesOperations.MoveCard(card, from, card.IsGeneralWeapon && from.Zone == CardZoneKind.Equipment ? CardLocation.OutsideGame : CardLocation.Hand(f.OwnerSeat), new(MouLbObtainReason), m =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                var paid = current.MouLuBuLiyu!;
                var gain = m.To == CardLocation.Hand(f.OwnerSeat);
                var mask = paid.CategoryMask | (gain ? MouLbCategory(m.CardKind) : 0);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { MouLuBuLiyu = paid with { ActualObtained = paid.ActualObtained + (gain ? 1 : 0), CategoryMask = mask } });
                _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuObtainedEvent(f.Id, paid.Selections, m.CardId, m.CardKind, m.From, m.To, m.Sequence, gain, mask));
            }, disclosure: from.Zone == CardZoneKind.Hand ? CardMovementDisclosure.FaceDown : CardMovementDisclosure.FaceUp);
            SealMouLb(f.Id, before);
            ContinueMouLb(f.Id);
            return;
        }

        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuBranchEvent(f.Id, r.VictimSeat, branch, choice.Targets.SingleOrDefault(-1)is var t && t >= 0 ? t : null, r.CategoryMask));
        if (branch == "duel")
        {
            IssueMouLbDuel(f, choice.Targets.Single());
            return;
        }

        if (branch == "wushuang")
        {
            GrantMouLbWushuang(f);
            return;
        }

        throw new InvalidOperationException("Unknown Liyu choice.");
    }

    internal void FinishMouLbTaking(ProgramSkillFrame f)
    {
        var r = f.MouLuBuLiyu!;
        if (r.ActualObtained == 0 || !_coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouLb(f);
            return;
        }

        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = r with { Paid = true, Stage = MouLuBuLiyuStage.Drawing }, PendingMovementContinuation = null });
        _coreProgramNativeDrawOperations.BeginOperationProgramDraw(f, r.VictimSeat, r.ActualObtained, new(MouLbDrawReason));
    }

    [ProgramNativeDrawReturn(SkillProgramEffectOp.ResolveMouLuBuLiyu)]
    internal void ReturnMouLbDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        if (f.MouLuBuLiyu is not { Stage: MouLuBuLiyuStage.Drawing, Draw: null } r || o.RecipientSeat != r.VictimSeat || o.RequestedCount != r.ActualObtained)
            throw new InvalidOperationException("Liyu lost its true victim native draw receipt.");
        var mask = o.ActualDrawIds.Concat(o.ReplacementGainedIds).Aggregate(r.CategoryMask, (m, id) => m | MouLbCategory(_coreAdvancedLifecycleOperations.GetAdvancedCard(id).Kind));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = r with { Stage = MouLuBuLiyuStage.DrawChildren, Draw = o, CategoryMask = mask, Before = o.SequenceBefore, After = o.SequenceAfter }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuDrawReturnedEvent(f.Id, r.VictimSeat, o.RequestedCount, o.ActualDrawIds.Count, o.ReplacementGainedIds.Count, mask, o.SequenceBefore, o.SequenceAfter, o.AttemptFrameId));
        SealMouLb(f.Id, o.SequenceBefore);
        ContinueMouLb(f.Id);
    }

    internal void ContinueMouLb(long id)
    {
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not null || _resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuBuLiyu: { } r } f || f.Id != id)
            return;
        if (!ValidMouLb(f))
            throw new InvalidOperationException("Liyu lost its accepted original damage and actual invoices.");
        if (r.Stage == MouLuBuLiyuStage.DuelIssued)
            return;
        if (r.Stage is MouLuBuLiyuStage.ChoosingCard or MouLuBuLiyuStage.VictimChoice)
        {
            PublishMouLb(f);
            return;
        }

        if (DrainMouLb(f))
            return;
        f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
        r = f.MouLuBuLiyu!;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { PendingMovementContinuation = null });
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
        {
            FinishMouLb(f);
            return;
        }

        if (r.Stage == MouLuBuLiyuStage.ObtainChildren)
        {
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive || r.Selections >= r.DamageAmount || _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, r.VictimSeat, global::CardGame.Core.EngineOwners.CoreGameEngineDamageTargetMountProgramsDefinitions.DamageTargetMountZones, OwnedCardMoveIntent.Obtain).Count == 0)
            {
                FinishMouLbTaking(f);
                return;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = r with { Stage = MouLuBuLiyuStage.ChoosingCard } });
            PublishMouLb(f);
            return;
        }

        if (r.Stage == MouLuBuLiyuStage.DrawChildren && r.CategoryMask == 7 && _coreMatchParticipantsPlayersAccess._players[r.VictimSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = r with { Stage = MouLuBuLiyuStage.VictimChoice } });
            PublishMouLb(f);
            return;
        }

        FinishMouLb(f);
    }

    internal void FinishMouLb(ProgramSkillFrame f)
    {
        var r = f.MouLuBuLiyu!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuCompletedEvent(f.Id, r.Selections, r.ActualObtained, r.CategoryMask));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveMouLuBuLiyu)]
    internal PromptChoice ChooseMouLbAi(PendingDecision p, ProgramSkillFrame f)
    {
        if (f.MouLuBuLiyu!.Stage == MouLuBuLiyuStage.ChoosingCard)
            return p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "take") ?? p.Choices.Single();
        // The actual victim chooses using public teams and actual legal targets.
        return p.Choices.Where(c => c.Parameters["branch"] == "duel").FirstOrDefault(c => !global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[p.PlayerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()]) && global::CardGame.Core.EngineOwners.CoreGameEngineProgramOwnedCardDistributionsRuntime.AreProgramDistributionAllies(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], _coreMatchParticipantsPlayersAccess._players[c.Targets.Single()])) ?? p.Choices.Single(c => c.Parameters["branch"] == "wushuang");
    }

    internal void GrantMouLbWushuang(ProgramSkillFrame f)
    {
        var r = f.MouLuBuLiyu!;
        var recipient = _coreMatchParticipantsPlayersAccess._players[r.VictimSeat];
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuBuLiyu = r with { Stage = MouLuBuLiyuStage.GrantChildren, Before = before, After = before }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        var grantId = $"mou-liyu:{(f.Id)}:{(recipient.Seat)}";
        var instance = _coreNationalSkillsOperations.GetSkillBindingShard(recipient).ActiveGrants.FirstOrDefault(g => g.SkillId == MouLuBuMobileComposition.Wushuang)?.SkillInstanceId ?? $"mou-liyu-shared:{(recipient.Seat)}:wushuang";
        recipient.SkillGrants.Grant(new SkillGrant(grantId, MouLuBuMobileComposition.Wushuang, instance, grantId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuGrantEvent(f.Id, r.Source, recipient.Seat, grantId, instance, _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Count()));
        ContinueMouLb(f.Id);
    }

    [ProgramRuleObserver(ProgramRulePhase.AfterEvent, 31600)]
    internal void ExpireMouLbGrant(ActualTurnCursorReturnedEvent returned)
    {
        foreach (var issued in _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuGrantEvent>().Where(e => e.RecipientSeat == returned.OwnerSeat).ToArray())
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuBuLiyuGrantExpiredEvent>().Any(e => e.GrantId == issued.GrantId))
                continue;
            var due = _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<TurnStartedEvent>().Skip(issued.AfterTurnStartOrdinal).FirstOrDefault(e => e.ActorSeat == issued.RecipientSeat);
            if (due?.TurnNumber != returned.ActualTurn)
                continue;
            _coreMatchParticipantsPlayersAccess._players[issued.RecipientSeat].SkillGrants.RemoveGrant(issued.GrantId);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuBuLiyuGrantExpiredEvent(issued.GrantId, issued.RecipientSeat, returned.ActualTurn));
        }
    }
}
