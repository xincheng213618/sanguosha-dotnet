#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterKanZeOlRuntime
{
    internal const string XiashuGiftBind = "xiashu.gift", XiashuShownBind = "xiashu.shown", XiashuClaimBind = "xiashu.claim", XiashuGiftReason = "program.xiashu.gift", XiashuClaimReason = "program.xiashu.claim";
    internal CardConversionSource KanZeSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal SkillProgramStepOutcome BeginXiashu(ProgramSkillFrame supplied, int recipient)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (f.XiashuPaid is not null || f.InstructionIndex != 2 || f.SelectedTargetSeats is not [var selected] || selected != recipient || recipient == f.OwnerSeat || f.WindowContext is not { Window: SkillProgramTriggerWindow.PlayPhaseStarting, SourceSeat: { } actor } w || actor != f.OwnerSeat || actor != _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat)
            throw new InvalidOperationException("Xiashu requires its unpaid native own Play-start.");
        var ids = _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(f.OwnerSeat)).Select(c => c.Id).ToArray();
        if (ids.Length == 0 || !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[recipient].IsAlive)
        {
            _coreSkillProgramsOperations.FinishProgramSkill(f, true);
            return SkillProgramStepOutcome.AwaitChild;
        }

        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, XiashuGiftBind, ids, SkillProgramCardSetVisibility.Private);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        var invoice = new XiashuInvoice(KanZeSource(f), f.GameplayHash, w.ParentFrameId, f.InstructionIndex, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, recipient, before, before, ids.Length);
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XiashuPaid = new(invoice, XiashuStage.GiftChildren, before, before, ids.Length), PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(ids, CardLocation.Hand(recipient), new(XiashuGiftReason), _characterHuangQuanMouOperations.NativeGiveCompletionRecords(f.Id, f.OwnerSeat, recipient, ids, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, (_, records) =>
        {
            var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
            var paid = invoice with
            {
                GiftAfter = records.Max(m => m.Sequence)
            };
            _coreRuntimeOperations.ReplaceRuntimeTop(active with { XiashuPaid = active.XiashuPaid!with { Invoice = paid, After = paid.GiftAfter } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiashuGiftPaidEvent(f.Id, paid));
        }));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
        return SkillProgramStepOutcome.AwaitChild;
    }

    internal PromptChoice XiashuChoice(ProgramSkillFrame f, string branch, string label, IReadOnlyList<int>? cards = null) => new(new($"xiashu.{(f.Id)}.{(branch)}"), label, cards ?? [], [], new Dictionary<string, string> { { "program-action", "xiashu" }, { "branch", branch } });
    internal IReadOnlyList<PromptChoice> XiashuChoices(ProgramSkillFrame f)
    {
        if (f.XiashuPaid!.Stage == XiashuStage.OwnerClaim)
            return Array.AsReadOnly(new[] { XiashuChoice(f, "shown", "获得展示的手牌"), XiashuChoice(f, "unshown", "获得未展示的手牌") });
        var selected = f.XiashuPaid!.SelectedReveal;
        return Array.AsReadOnly(_cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(f.XiashuPaid.Invoice.RecipientSeat)).Where(c => !selected.Contains(c.Id)).Select(c => XiashuChoice(f, "card-" + c.Id, c.Kind.ToString(), [c.Id])).Append(XiashuChoice(f, "finish", "完成展示选择")).ToArray());
    }

    internal void PublishXiashuDecision(ProgramSkillFrame f)
    {
        var seat = f.XiashuPaid!.Stage == XiashuStage.OwnerClaim ? f.OwnerSeat : f.XiashuPaid.Invoice.RecipientSeat;
        var choices = XiashuChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Step = ResolutionFrameStep.AwaitingResponse, PendingMovementContinuation = null });
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, seat, "请选择下书结算。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            TargetSeat = seat,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[seat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceRulesAndPublishState();
    }

    [ProgramAiChoice(SkillProgramEffectOp.GiveAllHandAndResolveXiashu)]
    internal PromptChoice XiashuAiChoice(ProgramSkillFrame f) => f.XiashuPaid!.Stage == XiashuStage.RecipientReveal ? XiashuChoices(f).Last() : XiashuChoices(f).First();
    [ProgramAnswer("xiashu")]
    internal void ResolveXiashuChoice(PromptChoice choice)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        RequireIssuedKanZe(f);
        var r = f.XiashuPaid!;
        var seat = r.Stage == XiashuStage.OwnerClaim ? f.OwnerSeat : r.Invoice.RecipientSeat;
        if (_corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != seat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([choice], XiashuChoices(f).Where(c => c.Id == choice.Id).ToArray()))
            throw new InvalidOperationException("Xiashu decision lost its actual chooser.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (r.Stage == XiashuStage.RecipientReveal)
        {
            if (choice.Parameters["branch"] != "finish")
            {
                var ids = r.SelectedReveal.Append(choice.Cards.Single()).ToArray();
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiashuPaid = r with { SelectedReveal = ids } });
                PublishXiashuDecision(f);
                return;
            }

            var shown = r.SelectedReveal.Where(id => _cardZonesOperations._cardZones.GetLocation(id) == CardLocation.Hand(r.Invoice.RecipientSeat)).ToArray();
            _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, XiashuShownBind, shown, SkillProgramCardSetVisibility.Public);
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CardsRevealedEvent(f.Id, Array.AsReadOnly(shown.Select(id => _coreSnapshotValueProjectionOperations.ToSnapshot(_coreAdvancedLifecycleOperations.GetAdvancedCard(id))).ToArray())));
            _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XiashuPaid = r with { Stage = XiashuStage.OwnerClaim } });
            if (_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow(f.Id, "xiashu-selected-hand"))
                return;
            if (!_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive || !_coreMatchParticipantsPlayersAccess._players[r.Invoice.RecipientSeat].IsAlive)
            {
                FinishXiashu(f);
                return;
            }

            PublishXiashuDecision(f);
            return;
        }

        var isShown = choice.Parameters["branch"] == "shown";
        var visible = _coreProgramLifecycleOperations.GetProgramCardSet(f, XiashuShownBind).CardIds;
        var take = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreMatchParticipantsPlayersAccess._players[r.Invoice.RecipientSeat].IsAlive ? _cardZonesOperations._cardZones.CardsAt(CardLocation.Hand(r.Invoice.RecipientSeat)).Where(c => visible.Contains(c.Id) == isShown).Select(c => c.Id).ToArray() : [];
        _coreProgramLifecycleOperations.SetProgramCardSet(f.Id, XiashuClaimBind, take, SkillProgramCardSetVisibility.Private);
        var before = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence;
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XiashuPaid = r with { Stage = XiashuStage.ClaimChildren, Before = before, After = before, Count = take.Length }, PendingMovementContinuation = new(f.OwnerSeat, 0, null), Step = ResolutionFrameStep.ResolvingEffect });
        if (take.Length > 0)
            _coreProgramAtomicCardMovesOperations.MoveProgramCardsFromMultipleSources(take, CardLocation.Hand(f.OwnerSeat), new(XiashuClaimReason));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { XiashuPaid = f.XiashuPaid!with { After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiashuClaimPaidEvent(f.Id, before, _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence, take.Length, isShown));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal bool DrainXiashu(ProgramSkillFrame f) => _coreProgramContinuationsOperations.TryBeginProgramChildWindows(f.Id, PostEventContinuation.AwaitedProgramMovement);
    [ProgramContinuation(ProgramContinuationKind.Resume, 22800)]
    internal bool ResumeXiashu(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { XiashuPaid: { } r } f || f.Id != id || r.Stage is XiashuStage.RecipientReveal or XiashuStage.OwnerClaim)
            return false;
        RequireIssuedKanZe(f);
        if (DrainXiashu(f))
            return true;
        if (r.Stage == XiashuStage.GiftChildren && _coreMatchParticipantsPlayersAccess._players[r.Invoice.RecipientSeat].IsAlive)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiashuPaid = r with { Stage = XiashuStage.RecipientReveal }, PendingMovementContinuation = null });
            PublishXiashuDecision(f);
        }
        else
            FinishXiashu(f);
        return true;
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 22800)]
    internal bool ReturnXiashuMovement(ProgramSkillFrame f) => f.XiashuPaid is not null && ResumeXiashu(f.Id);
    internal void FinishXiashu(ProgramSkillFrame f)
    {
        var r = f.XiashuPaid!;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new XiashuCompletedEvent(f.Id, r.Invoice.GiftCount, r.Stage == XiashuStage.ClaimChildren ? r.Count : 0));
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { XiashuPaid = null, PendingMovementContinuation = null });
        _coreSkillProgramsOperations.FinishProgramSkill(f, true);
    }

    SkillProgramStepOutcome global::CardGame.Core.IKanZeProgramHost.BeginXiashu(ProgramSkillFrame f, int target) => BeginXiashu(f, target);
    SkillProgramStepOutcome global::CardGame.Core.IKanZeProgramHost.ArmKuanshi(ProgramSkillFrame f, int target) => ArmKuanshi(f, target);
}
