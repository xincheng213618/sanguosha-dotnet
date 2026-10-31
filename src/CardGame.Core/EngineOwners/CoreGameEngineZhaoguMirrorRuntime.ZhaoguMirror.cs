#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineZhaoguMirrorRuntime
{
    internal const string ZhaoguMirrorSkill = "shanhe:zhaogu-mirror-6018";
    internal const string ZhaoguTemplateAction = "shanhe-zhaogu-template", ZhaoguUseAction = "shanhe-zhaogu-use";
    internal const string ZhaoguCancelAction = "shanhe-zhaogu-cancel", ZhaoguRevealKey = "shanhe-zhaogu-shown";
    SkillProgramStepOutcome global::CardGame.Core.IZhaoguMirrorHost.Resolve(ProgramSkillFrame frame) => BeginZhaoguMirror(frame);
    internal bool IsZhaoguMode => _coreMatchDefinitionModeDefinitionOptionsAccess._modeDefinition.Id == ShanheBattleSetup.BattleModeId && _coreMatchDefinitionModeDefinitionOptionsAccess._options.ShanheBattle is not null;

    internal static bool IsZhaoguTemplateKind(CardKind kind) => kind == CardKind.Nullification || CardUseCategoryCatalog.Get(kind)is CardUseCategories.Basic or CardUseCategories.InstantTrick;
    internal IEnumerable<Card> ZhaoguTemplates(int seat) => _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[seat]).Where(card => IsZhaoguTemplateKind(card.Kind));
    internal CardConversionSource ZhaoguSource(ProgramSkillFrame frame) => new(frame.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62057, SkillProgramEffectOp.ResolveShanheZhaoguMirror)]
    internal bool CanOfferZhaoguMirror(ProgramTriggerLegalityContext context) => IsZhaoguMode && context.OwnerSeat == _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat && context.WindowContext?.Window == SkillProgramTriggerWindow.PlayEnding && ZhaoguTemplates(context.OwnerSeat).Any();
    internal bool ValidZhaoguHeader(ProgramSkillFrame frame) => frame.ZhaoguMirror is { } receipt && IsZhaoguMode && frame.SkillId == ZhaoguMirrorSkill && frame.TriggerId == "show-hand-card-and-use" && receipt.Source == ZhaoguSource(frame) && receipt.GameplayHash == frame.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash == receipt.GameplayHash && receipt.InstructionIndex == frame.InstructionIndex && frame.InstructionIndex == 1 && receipt.TurnNumber == _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber && receipt.LifecycleFrameId == frame.WindowContext?.ParentFrameId && _coreVirtualOrdinaryTrickUsesOperations.IsExactOwnPlayEndingProgram(frame);
    internal SkillProgramStepOutcome BeginZhaoguMirror(ProgramSkillFrame input)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (!IsZhaoguMode || frame.SkillId != ZhaoguMirrorSkill || frame.ZhaoguMirror is not null || !_coreVirtualOrdinaryTrickUsesOperations.IsExactOwnPlayEndingProgram(frame) || frame.InstructionIndex != 1)
            throw new InvalidOperationException("Zhaogu Mirror lost its original unpaid own PlayEnding instruction.");
        if (!ZhaoguTemplates(frame.OwnerSeat).Any())
            return SkillProgramStepOutcome.Continue;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = new(ZhaoguSource(frame), frame.GameplayHash, frame.InstructionIndex, frame.WindowContext!.ParentFrameId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber) });
        PublishZhaoguChoice(frame);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal PromptChoice ZhaoguChoice(ProgramSkillFrame frame, string action, string token, string label, IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new($"{(action)}.{(frame.Id)}.{(token)}"), label, cards, targets, new Dictionary<string, string> { ["program-action"] = action, ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["mirror-option"] = token });
    internal IReadOnlyList<global::CardGame.Core.GameEngine.ProgramOrdinaryTrickUseOption> ZhaoguTrickOptions(ProgramSkillFrame frame)
    {
        if (frame.ZhaoguMirror?.Template is not { } template || CardUseCategoryCatalog.Get(template.Kind) != CardUseCategories.InstantTrick || template.Kind == CardKind.Nullification || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None)
            return[];
        return _coreProgramOrdinaryTrickUsesOperations.BuildProgramOrdinaryTrickUseOptions(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], template.Kind, Suit.None, enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null, hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: true);
    }

    internal IReadOnlyList<PromptChoice> ZhaoguChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.ZhaoguMirror!;
        var choices = new List<PromptChoice>();
        if (receipt.Stage == ZhaoguMirrorStage.Template)
            choices.AddRange(ZhaoguTemplates(frame.OwnerSeat).Select(card => ZhaoguChoice(frame, ZhaoguTemplateAction, card.Id.ToString(CultureInfo.InvariantCulture), "展示【" + card.DisplayName + "】", [card.Id], [])));
        else if (receipt.Stage == ZhaoguMirrorStage.UseChooser)
        {
            choices.AddRange(ZhaoguVirtualBasicOptions(frame).Select(option => ZhaoguChoice(frame, ZhaoguUseAction, $"{(option.Kind)}:{(option.Target)}", "视为对" + _coreMatchParticipantsPlayersAccess._players[option.Target].Name + "使用【" + CardCatalog.Get(option.Kind).DisplayName + "】", [], [option.Target])));
            choices.AddRange(ZhaoguTrickOptions(frame).Select(option => ZhaoguChoice(frame, ZhaoguUseAction, option.Id.Value, option.Description, [], option.TargetSeats)));
        }

        choices.Add(ZhaoguChoice(frame, ZhaoguCancelAction, "cancel", "取消", [], []));
        return Array.AsReadOnly(choices.ToArray());
    }

    internal void PublishZhaoguChoice(ProgramSkillFrame frame)
    {
        if (!ValidZhaoguHeader(frame))
            throw new InvalidOperationException("Zhaogu Mirror lost its original chooser receipt.");
        var choices = ZhaoguChoices(frame);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(frame.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat, frame.ZhaoguMirror!.Stage == ZhaoguMirrorStage.Template ? "展示一张基本牌或普通锦囊牌。" : "选择视为使用的合法用法，或取消。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = frame.ZhaoguMirror.Stage == ZhaoguMirrorStage.Template,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = choices
        };
        _coreMatchOutcomeStatusWinnerAccess._status = _coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    internal ProgramSkillFrame ZhaoguChoiceFrame(PromptChoice selected, ZhaoguMirrorStage stage)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame frame || !ValidZhaoguHeader(frame) || frame.ZhaoguMirror!.Stage != stage || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != frame.OwnerSeat || selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture) || !ZhaoguChoices(frame).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("Zhaogu Mirror choice lost its actual owner, stage or legal option.");
        return frame;
    }

    [ProgramAnswer(ZhaoguTemplateAction)]
    internal void ResolveZhaoguTemplate(PromptChoice selected)
    {
        var frame = ZhaoguChoiceFrame(selected, ZhaoguMirrorStage.Template);
        var card = ZhaoguTemplates(frame.OwnerSeat).Single(card => selected.Cards.SequenceEqual([card.Id]));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = frame.ZhaoguMirror!with { Template = _coreSnapshotValueProjectionOperations.ToSnapshot(card), Stage = ZhaoguMirrorStage.RevealChildren } });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(frame.Id, frame.SkillId, frame.TriggerId!, frame.OwnerSeat, ZhaoguRevealKey, Array.AsReadOnly(new[] { _coreSnapshotValueProjectionOperations.ToSnapshot(card) })));
        if (!_characterXiaoQiaoMouOlOperations.TryBeginPublicHandRevealWindow2(frame.Id, ZhaoguRevealKey))
            ContinueZhaoguAfterReveal(_coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id));
    }

    [PublicHandRevealReturn(ZhaoguRevealKey)]
    internal void ContinueZhaoguAfterReveal(ProgramSkillFrame frame)
    {
        if (!ValidZhaoguHeader(frame) || frame.ZhaoguMirror is not { Stage: ZhaoguMirrorStage.RevealChildren, Template: not null })
            throw new InvalidOperationException("Zhaogu Mirror reveal lost its original template return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = frame.ZhaoguMirror with { Stage = ZhaoguMirrorStage.UseChooser } });
        if (!ZhaoguVirtualBasicOptions(frame).Any() && ZhaoguTrickOptions(frame).Count == 0)
        {
            CompleteZhaogu(frame);
            return;
        }

        PublishZhaoguChoice(frame);
    }

    [ProgramAnswer(ZhaoguCancelAction)]
    internal void CancelZhaogu(PromptChoice selected)
    {
        var stage = (_resolutionStackOperations._resolutionStack.LastOrDefault() as ProgramSkillFrame)?.ZhaoguMirror?.Stage ?? throw new InvalidOperationException("Missing mirror cancellation.");
        var frame = ZhaoguChoiceFrame(selected, stage);
        if (stage is not (ZhaoguMirrorStage.Template or ZhaoguMirrorStage.UseChooser))
            throw new InvalidOperationException("An issued mirror child cannot be canceled as an unpaid choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        CompleteZhaogu(frame);
    }

    [ProgramAnswer(ZhaoguUseAction)]
    internal void ResolveZhaoguUse(PromptChoice selected)
    {
        var frame = ZhaoguChoiceFrame(selected, ZhaoguMirrorStage.UseChooser);
        var token = selected.Parameters["mirror-option"];
        var basic = ZhaoguVirtualBasicOptions(frame).Where(o => token == $"{(o.Kind)}:{(o.Target)}" && selected.Targets.SequenceEqual([o.Target])).ToArray();
        if (basic.Length == 1)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = frame.ZhaoguMirror!with { Stage = ZhaoguMirrorStage.BasicChild }, VirtualBasicDraft = new(frame.InstructionIndex) });
            _coreVirtualBasicOffersOperations.ResolveVirtualBasicChoice(new(selected.Id, selected.Description, [], selected.Targets, new Dictionary<string, string> { ["program-action"] = "virtual-basic", ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["basic-option"] = token }));
            return;
        }

        var option = ZhaoguTrickOptions(frame).Single(o => o.Id.Value == token && o.TargetSeats.SequenceEqual(selected.Targets));
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        var kind = frame.ZhaoguMirror!.Template!.Kind;
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = frame.ZhaoguMirror with { Stage = ZhaoguMirrorStage.TrickIssuing } });
        var origin = new ProgramVirtualOrdinaryTrickOrigin(frame.Id, frame.InstructionIndex, frame.WindowContext!.ParentFrameId, frame.OwnerSeat, frame.OwnerSeat, option.TargetSeats.FirstOrDefault(frame.OwnerSeat), frame.SkillId, frame.TriggerId!, frame.SkillInstanceId, frame.GameplayHash, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, kind, Suit.None, 0);
        if (option.NextActualUseAdjusted)
            _coreGameEngineNextCardTargetAdjustmentsSelectedNextCardTargetSeatsAccess._selectedNextCardTargetSeats = option.TargetSeats;
        var id = _coreNativeCardUseOperations.BeginCardUse(card, frame.OwnerSeat, kind == CardKind.DrawTwo && option.TargetSeats.Count == 0 ? [frame.OwnerSeat] : option.TargetSeats, kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = _coreCardUseLifecycleOperations.LifecycleCardUse(id)!;
        _coreRuntimeOperations.ReplaceRuntimeFrame(frame.Id, frame = _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().Single(parent => parent.Id == frame.Id)with { ZhaoguMirror = frame.ZhaoguMirror with { Stage = ZhaoguMirrorStage.TrickChild, TrickChildId = id, TrickActionId = use.Action!.ActionId } });
        _coreNativeNullificationOperations.BeginJizhiOrNullificationWindow(id, card, frame.OwnerSeat, use.TargetSeats, option.ActionKind, option.TargetCardId, option.RequiredCardKind, kind);
    }

    internal IEnumerable<(CardKind Kind, int Target)> ZhaoguVirtualBasicOptions(ProgramSkillFrame frame) => frame.ZhaoguMirror?.Template is { } template && CardUseCategoryCatalog.Get(template.Kind) == CardUseCategories.Basic ? _coreVirtualBasicOffersOperations.VirtualBasicOptions2(frame.OwnerSeat, true).Where(option => option.Kind == template.Kind) : [];
    internal bool IsZhaoguVirtualBasicDraft(ProgramSkillFrame frame, ProgramVirtualBasicDraft draft) => ValidZhaoguHeader(frame) && frame.ZhaoguMirror is { Stage: ZhaoguMirrorStage.BasicChild, Template: not null } && draft.InstructionIndex == frame.InstructionIndex;
    internal ProgramSkillFrame CompleteZhaoguVirtualBasicReceipt(ProgramSkillFrame frame, long childId)
    {
        if (frame.ZhaoguMirror is null)
            return frame;
        if (!IsZhaoguVirtualBasicDraft(frame, frame.VirtualBasicDraft!) || frame.VirtualBasicDraft?.ChildFrameId != childId || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == childId && e.CardId == 0 && e.CardKind == frame.ZhaoguMirror.Template!.Kind) != 1)
            throw new InvalidOperationException("Zhaogu Mirror basic child lost its actual once-completed generic return.");
        return frame with
        {
            ZhaoguMirror = null
        };
    }

    internal bool IsZhaoguVirtualOrdinaryTrickUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: ZhaoguMirrorSkill } origin || _resolutionStackOperations._resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == origin.ParentProgramFrameId)is not { ZhaoguMirror: { Template: { } template } receipt } frame || !ValidZhaoguHeader(frame) || receipt.Stage is not (ZhaoguMirrorStage.TrickIssuing or ZhaoguMirrorStage.TrickChild) || use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.VirtualBasicReturn is not null || use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0, EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: null, RequesterSeat: null, ResponderSeat: null, OpponentSeat: null } action || action.ProviderSeat != frame.OwnerSeat || action.ActorSeat != use.SourceSeat || action.ActionId != origin.CardActionId || action.EffectiveKind != template.Kind || use.CardKind != template.Kind || !action.TargetSeats.SequenceEqual(use.TargetSeats) || use.TargetSeats.Count == 0 && !(_characterXunYuMobileGodOperations.HasDinghanCancellation(use) || _characterHuaManOlOperations.HasHuaManSouyingCancellation(use) || _coreOverflowTargetCancellationOperations.HasOverflowTargetCancellation(use)) || use.TargetSeats.Any(seat => !_coreTableSeatQueriesOperations.IsValidPlayerSeat(seat)) || use.TargetSeats.Distinct().Count() != use.TargetSeats.Count || origin.OwnerSeat != frame.OwnerSeat || origin.InitialActorSeat != frame.OwnerSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(origin.InitialTargetSeat) || origin.BindingId != frame.TriggerId || origin.SkillInstanceId != frame.SkillInstanceId || origin.GameplayHash != frame.GameplayHash || origin.InstructionIndex != frame.InstructionIndex || origin.LifecycleFrameId != receipt.LifecycleFrameId || origin.TurnNumber != receipt.TurnNumber || origin.OutputKind != template.Kind || origin.ConditionKind != SkillProgramConditionKind.Always || origin.PindianBind is not null || origin.FrozenSourceWon is not null || receipt.Stage == ZhaoguMirrorStage.TrickChild && (receipt.TrickChildId != use.Id || receipt.TrickActionId != action.ActionId))
            return false;
        return _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id) ? _coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, origin.InitialActorSeat) : receipt.Stage == ZhaoguMirrorStage.TrickIssuing && action.ActorSeat == frame.OwnerSeat;
    }

    internal void ReturnZhaoguVirtualOrdinaryTrickUse(CardUseFrame use)
    {
        var origin = use.VirtualOrdinaryTrickOrigin!;
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(origin.ParentProgramFrameId);
        var receipt = frame.ZhaoguMirror;
        if (!IsZhaoguVirtualOrdinaryTrickUse(use) || receipt is not { Stage: ZhaoguMirrorStage.TrickChild } || _resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || use.Step != ResolutionFrameStep.Completed || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == 0 && e.CardKind == origin.OutputKind) != 1)
            throw new InvalidOperationException("Zhaogu Mirror trick lost its exact once-completed native return.");
        if (use.CardAttack is { Active: true })
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Any(e => e.UseId == use.Id))
                throw new InvalidOperationException("Zhaogu Mirror attack returned twice.");
            _coreRuntimeOperations.ReplaceRuntimeTop(frame with { ZhaoguMirror = receipt with { Stage = ZhaoguMirrorStage.TrickCleanup } });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ProgramVirtualOrdinaryAttackReturnedEvent(use.Id, origin));
            return;
        }

        CompleteZhaogu(frame);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 62057)]
    internal bool CompleteZhaoguAttackCleanup()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ZhaoguMirror: { Stage: ZhaoguMirrorStage.TrickCleanup } receipt } frame)
            return false;
        if (!ValidZhaoguHeader(frame) || _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.TrickChildId)is not null || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Any(e => e.UseId == receipt.TrickChildId && e.Origin.ParentProgramFrameId == frame.Id && e.Origin.CardActionId == receipt.TrickActionId))
            throw new InvalidOperationException("Zhaogu Mirror must wait for its original native attack cleanup.");
        CompleteZhaogu(frame);
        return true;
    }

    internal void CompleteZhaogu(ProgramSkillFrame frame)
    {
        _coreRuntimeOperations.ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = null });
        _coreRuntimeOperations.AdvanceRuntimeProgram(frame.Id);
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62057)]
    internal bool ContinueIssuedZhaogu(ProgramSkillFrame frame) => frame.ZhaoguMirror is not null && ValidZhaoguHeader(frame);
    internal void AssertZhaoguMirror(ProgramSkillFrame frame, SkillProgramEffect? paused)
    {
        if (frame.ZhaoguMirror is not { } receipt)
            return;
        if (paused?.Op != SkillProgramEffectOp.ResolveShanheZhaoguMirror || !ValidZhaoguHeader(frame) || (receipt.Stage == ZhaoguMirrorStage.Template ? receipt.Template is not null : receipt.Template is null) || receipt.Template is { } template && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardsRevealedEvent>().Count(e => e.FrameId == frame.Id && e.SkillId == frame.SkillId && e.BindingId == frame.TriggerId && e.Bind == ZhaoguRevealKey && e.Cards is [var card] && card.Id == template.Id && card.Kind == template.Kind) != 1)
            throw new InvalidOperationException("Zhaogu Mirror lost its frozen template, original phase header or single public reveal.");
        if (receipt.Stage is ZhaoguMirrorStage.Template or ZhaoguMirrorStage.UseChooser)
        {
            if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id != frame.Id || _corePendingDecisionPolicyPipelineOperations._pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt || prompt.PlayerSeat != frame.OwnerSeat || !prompt.Choices.Select(c => c.Id).SequenceEqual(ZhaoguChoices(frame).Select(c => c.Id)))
                throw new InvalidOperationException("Zhaogu Mirror lost its unpaid exact choice.");
        }
        else if (receipt.Stage == ZhaoguMirrorStage.BasicChild)
        {
            if (frame.VirtualBasicDraft is not { } draft || !IsZhaoguVirtualBasicDraft(frame, draft) || draft.ChildFrameId is { } child && (_coreCardUseLifecycleOperations.LifecycleCardUse(child)is not { } basic || basic.CardKind != receipt.Template!.Kind))
                throw new InvalidOperationException("Zhaogu Mirror basic child lost its single generic draft.");
        }
        else if (receipt.Stage == ZhaoguMirrorStage.TrickChild && (_coreCardUseLifecycleOperations.LifecycleCardUse(receipt.TrickChildId)is not { } use || !IsZhaoguVirtualOrdinaryTrickUse(use)))
            throw new InvalidOperationException("Zhaogu Mirror trick child lost its actual original Use.");
        else if (receipt.Stage == ZhaoguMirrorStage.RevealChildren && !_characterXiaoQiaoMouOlOperations.IsPublicHandRevealProducerSuspended(frame.Id, ZhaoguRevealKey))
            throw new InvalidOperationException("Zhaogu Mirror reveal lost its native reveal child.");
        else if (receipt.Stage == ZhaoguMirrorStage.TrickCleanup && (receipt.TrickChildId <= 0 || receipt.TrickActionId <= 0 || _coreCardUseLifecycleOperations.LifecycleCardUse(receipt.TrickChildId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Count(e => e.UseId == receipt.TrickChildId && e.Origin.ParentProgramFrameId == frame.Id && e.Origin.CardActionId == receipt.TrickActionId && e.Origin.SkillInstanceId == frame.SkillInstanceId) != 1))
            throw new InvalidOperationException("Zhaogu Mirror cleanup lost its original retired attack and single typed return.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheZhaoguMirror)]
    internal PromptChoice ChooseZhaoguMirror(PendingDecision prompt, ProgramSkillFrame frame) => prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") != ZhaoguCancelAction) ?? prompt.Choices.First();
}
