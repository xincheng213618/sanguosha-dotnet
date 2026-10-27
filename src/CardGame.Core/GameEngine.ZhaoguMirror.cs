using System.Globalization;
using CardGame.Core.Campaigns;

namespace CardGame.Core;

public sealed partial class GameEngine
{
    private const string ZhaoguMirrorSkill = "shanhe:zhaogu-mirror-6018";
    private const string ZhaoguTemplateAction = "shanhe-zhaogu-template", ZhaoguUseAction = "shanhe-zhaogu-use";
    private const string ZhaoguCancelAction = "shanhe-zhaogu-cancel", ZhaoguRevealKey = "shanhe-zhaogu-shown";

    [ProgramCapability]
    private sealed class ZhaoguMirrorCapability(GameEngine engine) : IZhaoguMirrorHost
    { public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame) => engine.BeginZhaoguMirror(frame); }

    private bool IsZhaoguMode => _modeDefinition.Id == ShanheBattleSetup.BattleModeId && _options.ShanheBattle is not null;
    private static bool IsZhaoguTemplateKind(CardKind kind) => kind == CardKind.Nullification ||
        CardUseCategoryCatalog.Get(kind) is CardUseCategories.Basic or CardUseCategories.InstantTrick;
    private IEnumerable<Card> ZhaoguTemplates(int seat) => GetHand(_players[seat]).Where(card => IsZhaoguTemplateKind(card.Kind));
    private CardConversionSource ZhaoguSource(ProgramSkillFrame frame) =>
        new(frame.SkillId, GetProgramBindingId(frame), frame.OwnerSeat, frame.SkillInstanceId);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 62057, SkillProgramEffectOp.ResolveShanheZhaoguMirror)]
    private bool CanOfferZhaoguMirror(ProgramTriggerLegalityContext context) => IsZhaoguMode &&
        context.OwnerSeat == _currentSeat && context.WindowContext?.Window == SkillProgramTriggerWindow.PlayEnding &&
        ZhaoguTemplates(context.OwnerSeat).Any();

    private bool ValidZhaoguHeader(ProgramSkillFrame frame) => frame.ZhaoguMirror is { } receipt && IsZhaoguMode &&
        frame.SkillId == ZhaoguMirrorSkill && frame.TriggerId == "show-hand-card-and-use" &&
        receipt.Source == ZhaoguSource(frame) && receipt.GameplayHash == frame.GameplayHash &&
        _contentRegistry.GetSkill(frame.SkillId).Program?.GameplayHash == receipt.GameplayHash &&
        receipt.InstructionIndex == frame.InstructionIndex && frame.InstructionIndex == 1 &&
        receipt.TurnNumber == _turnNumber && receipt.LifecycleFrameId == frame.WindowContext?.ParentFrameId &&
        IsExactOwnPlayEndingProgram(frame);

    private SkillProgramStepOutcome BeginZhaoguMirror(ProgramSkillFrame input)
    {
        var frame = GetActiveProgramFrame(input.Id);
        if (!IsZhaoguMode || frame.SkillId != ZhaoguMirrorSkill || frame.ZhaoguMirror is not null ||
            !IsExactOwnPlayEndingProgram(frame) || frame.InstructionIndex != 1)
            throw new InvalidOperationException("Zhaogu Mirror lost its original unpaid own PlayEnding instruction.");
        if (!ZhaoguTemplates(frame.OwnerSeat).Any()) return SkillProgramStepOutcome.Continue;
        ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = new(ZhaoguSource(frame), frame.GameplayHash,
            frame.InstructionIndex, frame.WindowContext!.ParentFrameId, _turnNumber) });
        PublishZhaoguChoice(frame); return SkillProgramStepOutcome.AwaitChoice;
    }

    private PromptChoice ZhaoguChoice(ProgramSkillFrame frame, string action, string token, string label,
        IReadOnlyList<int> cards, IReadOnlyList<int> targets) => new(new($"{action}.{frame.Id}.{token}"), label, cards, targets,
            new Dictionary<string, string> { ["program-action"] = action,
                ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["mirror-option"] = token });

    private IReadOnlyList<ProgramOrdinaryTrickUseOption> ZhaoguTrickOptions(ProgramSkillFrame frame)
    {
        if (frame.ZhaoguMirror?.Template is not { } template ||
            CardUseCategoryCatalog.Get(template.Kind) != CardUseCategories.InstantTrick || template.Kind == CardKind.Nullification ||
            !_players[frame.OwnerSeat].IsAlive || _winner != Winner.None) return [];
        return BuildProgramOrdinaryTrickUseOptions(_players[frame.OwnerSeat], template.Kind, Suit.None,
            enforceUsePermission: true, beneficiaryShieldSuit: Suit.None, actualEffectiveColor: null,
            hasActualColor: true, physicalCardIds: [], includeNextActualUseAdjustment: true);
    }

    private IReadOnlyList<PromptChoice> ZhaoguChoices(ProgramSkillFrame frame)
    {
        var receipt = frame.ZhaoguMirror!; var choices = new List<PromptChoice>();
        if (receipt.Stage == ZhaoguMirrorStage.Template)
            choices.AddRange(ZhaoguTemplates(frame.OwnerSeat).Select(card => ZhaoguChoice(frame, ZhaoguTemplateAction,
                card.Id.ToString(CultureInfo.InvariantCulture), "展示【" + card.DisplayName + "】", [card.Id], [])));
        else if (receipt.Stage == ZhaoguMirrorStage.UseChooser)
        {
            choices.AddRange(ZhaoguVirtualBasicOptions(frame).Select(option => ZhaoguChoice(frame, ZhaoguUseAction,
                $"{option.Kind}:{option.Target}", "视为对" + _players[option.Target].Name + "使用【" + CardCatalog.Get(option.Kind).DisplayName + "】", [], [option.Target])));
            choices.AddRange(ZhaoguTrickOptions(frame).Select(option => ZhaoguChoice(frame, ZhaoguUseAction,
                option.Id.Value, option.Description, [], option.TargetSeats)));
        }
        choices.Add(ZhaoguChoice(frame, ZhaoguCancelAction, "cancel", "取消", [], []));
        return Array.AsReadOnly(choices.ToArray());
    }

    private void PublishZhaoguChoice(ProgramSkillFrame frame)
    {
        if (!ValidZhaoguHeader(frame)) throw new InvalidOperationException("Zhaogu Mirror lost its original chooser receipt.");
        var choices = ZhaoguChoices(frame); var skill = _contentRegistry.GetSkill(frame.SkillId);
        _pendingDecision = new(DecisionKind.ProgramTrigger, frame.OwnerSeat,
            frame.ZhaoguMirror!.Stage == ZhaoguMirrorStage.Template ? "展示一张基本牌或普通锦囊牌。" : "选择视为使用的合法用法，或取消。",
            choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), frame.OwnerSeat)
        {
            PromptId = CreatePromptId(), IsPrivate = frame.ZhaoguMirror.Stage == ZhaoguMirrorStage.Template,
            SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description), Choices = choices
        };
        _status = _players[frame.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    private ProgramSkillFrame ZhaoguChoiceFrame(PromptChoice selected, ZhaoguMirrorStage stage)
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame frame || !ValidZhaoguHeader(frame) ||
            frame.ZhaoguMirror!.Stage != stage || _pendingDecision?.PlayerSeat != frame.OwnerSeat ||
            selected.Parameters.GetValueOrDefault("frame-id") != frame.Id.ToString(CultureInfo.InvariantCulture) ||
            !ZhaoguChoices(frame).Any(c => c.Id == selected.Id && c.Cards.SequenceEqual(selected.Cards) && c.Targets.SequenceEqual(selected.Targets)))
            throw new InvalidOperationException("Zhaogu Mirror choice lost its actual owner, stage or legal option.");
        return frame;
    }

    [ProgramAnswer(ZhaoguTemplateAction)]
    private void ResolveZhaoguTemplate(PromptChoice selected)
    {
        var frame = ZhaoguChoiceFrame(selected, ZhaoguMirrorStage.Template);
        var card = ZhaoguTemplates(frame.OwnerSeat).Single(card => selected.Cards.SequenceEqual([card.Id]));
        ClearPendingDecision();
        ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = frame.ZhaoguMirror! with
            { Template = ToSnapshot(card), Stage = ZhaoguMirrorStage.RevealChildren } });
        AdvanceEventRulesAndQueueFact(new ProgramCardsRevealedEvent(frame.Id, frame.SkillId, frame.TriggerId!, frame.OwnerSeat,
            ZhaoguRevealKey, Array.AsReadOnly(new[] { ToSnapshot(card) })));
        if (!TryBeginPublicHandRevealWindow(frame.Id, ZhaoguRevealKey)) ContinueZhaoguAfterReveal(GetActiveProgramFrame(frame.Id));
    }

    [PublicHandRevealReturn(ZhaoguRevealKey)]
    private void ContinueZhaoguAfterReveal(ProgramSkillFrame frame)
    {
        if (!ValidZhaoguHeader(frame) || frame.ZhaoguMirror is not { Stage: ZhaoguMirrorStage.RevealChildren, Template: not null })
            throw new InvalidOperationException("Zhaogu Mirror reveal lost its original template return.");
        ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = frame.ZhaoguMirror with { Stage = ZhaoguMirrorStage.UseChooser } });
        if (!ZhaoguVirtualBasicOptions(frame).Any() && ZhaoguTrickOptions(frame).Count == 0) { CompleteZhaogu(frame); return; }
        PublishZhaoguChoice(frame);
    }

    [ProgramAnswer(ZhaoguCancelAction)]
    private void CancelZhaogu(PromptChoice selected)
    {
        var stage = (_resolutionStack.LastOrDefault() as ProgramSkillFrame)?.ZhaoguMirror?.Stage ?? throw new InvalidOperationException("Missing mirror cancellation.");
        var frame = ZhaoguChoiceFrame(selected, stage);
        if (stage is not (ZhaoguMirrorStage.Template or ZhaoguMirrorStage.UseChooser)) throw new InvalidOperationException("An issued mirror child cannot be canceled as an unpaid choice.");
        ClearPendingDecision(); CompleteZhaogu(frame);
    }

    [ProgramAnswer(ZhaoguUseAction)]
    private void ResolveZhaoguUse(PromptChoice selected)
    {
        var frame = ZhaoguChoiceFrame(selected, ZhaoguMirrorStage.UseChooser);
        var token = selected.Parameters["mirror-option"];
        var basic = ZhaoguVirtualBasicOptions(frame).Where(o => token == $"{o.Kind}:{o.Target}" && selected.Targets.SequenceEqual([o.Target])).ToArray();
        if (basic.Length == 1)
        {
            ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = frame.ZhaoguMirror! with { Stage = ZhaoguMirrorStage.BasicChild },
                VirtualBasicDraft = new(frame.InstructionIndex) });
            ResolveVirtualBasicChoice(new(selected.Id, selected.Description, [], selected.Targets,
                new Dictionary<string, string> { ["program-action"] = "virtual-basic",
                    ["frame-id"] = frame.Id.ToString(CultureInfo.InvariantCulture), ["basic-option"] = token }));
            return;
        }
        var option = ZhaoguTrickOptions(frame).Single(o => o.Id.Value == token && o.TargetSeats.SequenceEqual(selected.Targets));
        ClearPendingDecision();
        var kind = frame.ZhaoguMirror!.Template!.Kind;
        ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = frame.ZhaoguMirror with { Stage = ZhaoguMirrorStage.TrickIssuing } });
        var origin = new ProgramVirtualOrdinaryTrickOrigin(frame.Id, frame.InstructionIndex, frame.WindowContext!.ParentFrameId,
            frame.OwnerSeat, frame.OwnerSeat, option.TargetSeats.FirstOrDefault(frame.OwnerSeat), frame.SkillId, frame.TriggerId!,
            frame.SkillInstanceId, frame.GameplayHash, _turnNumber, kind, SkillProgramConditionKind.Always, null, null);
        var card = new Card(0, kind, Suit.None, 0);
        if (option.NextActualUseAdjusted) _selectedNextCardTargetSeats = option.TargetSeats;
        var id = BeginCardUse(card, frame.OwnerSeat, kind == CardKind.DrawTwo && option.TargetSeats.Count == 0 ? [frame.OwnerSeat] : option.TargetSeats,
            kind, physicalCardIds: [], virtualOrdinaryTrickOrigin: origin);
        var use = LifecycleCardUse(id)!;
        ReplaceRuntimeFrame(frame.Id, frame = _resolutionStack.OfType<ProgramSkillFrame>().Single(parent => parent.Id == frame.Id) with
            { ZhaoguMirror = frame.ZhaoguMirror with { Stage = ZhaoguMirrorStage.TrickChild, TrickChildId = id, TrickActionId = use.Action!.ActionId } });
        BeginJizhiOrNullificationWindow(id, card, frame.OwnerSeat, use.TargetSeats, option.ActionKind, option.TargetCardId, option.RequiredCardKind, kind);
    }

    private IEnumerable<(CardKind Kind, int Target)> ZhaoguVirtualBasicOptions(ProgramSkillFrame frame) =>
        frame.ZhaoguMirror?.Template is { } template && CardUseCategoryCatalog.Get(template.Kind) == CardUseCategories.Basic
            ? VirtualBasicOptions(frame.OwnerSeat, true).Where(option => option.Kind == template.Kind) : [];

    private bool IsZhaoguVirtualBasicDraft(ProgramSkillFrame frame, ProgramVirtualBasicDraft draft) =>
        ValidZhaoguHeader(frame) && frame.ZhaoguMirror is { Stage: ZhaoguMirrorStage.BasicChild, Template: not null } &&
        draft.InstructionIndex == frame.InstructionIndex;

    private ProgramSkillFrame CompleteZhaoguVirtualBasicReceipt(ProgramSkillFrame frame, long childId)
    {
        if (frame.ZhaoguMirror is null) return frame;
        if (!IsZhaoguVirtualBasicDraft(frame, frame.VirtualBasicDraft!) || frame.VirtualBasicDraft?.ChildFrameId != childId ||
            ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == childId && e.CardId == 0 && e.CardKind == frame.ZhaoguMirror.Template!.Kind) != 1)
            throw new InvalidOperationException("Zhaogu Mirror basic child lost its actual once-completed generic return.");
        return frame with { ZhaoguMirror = null };
    }

    private bool IsZhaoguVirtualOrdinaryTrickUse(CardUseFrame use)
    {
        if (use.VirtualOrdinaryTrickOrigin is not { SkillId: ZhaoguMirrorSkill } origin ||
            _resolutionStack.OfType<ProgramSkillFrame>().SingleOrDefault(f => f.Id == origin.ParentProgramFrameId) is not { ZhaoguMirror: { Template: { } template } receipt } frame ||
            !ValidZhaoguHeader(frame) || receipt.Stage is not (ZhaoguMirrorStage.TrickIssuing or ZhaoguMirrorStage.TrickChild) ||
            use.CardId != 0 || use.PhysicalCardIds is not { Count: 0 } || use.VirtualBasicReturn is not null ||
            use.Action is not { Type: CardActionType.Use, PhysicalCards.Count: 0, ConversionChain.Count: 0,
                EffectiveSuit: Suit.None, EffectiveRank: 0, EffectiveIsRed: null, RequesterSeat: null, ResponderSeat: null, OpponentSeat: null } action ||
            action.ProviderSeat != frame.OwnerSeat || action.ActorSeat != use.SourceSeat || action.ActionId != origin.CardActionId ||
            action.EffectiveKind != template.Kind || use.CardKind != template.Kind || !action.TargetSeats.SequenceEqual(use.TargetSeats) ||
            use.TargetSeats.Count == 0 && !(HasDinghanCancellation(use) || HasHuaManSouyingCancellation(use) || HasOverflowTargetCancellation(use)) ||
            use.TargetSeats.Any(seat => !IsValidPlayerSeat(seat)) || use.TargetSeats.Distinct().Count() != use.TargetSeats.Count ||
            origin.OwnerSeat != frame.OwnerSeat || origin.InitialActorSeat != frame.OwnerSeat || !IsValidPlayerSeat(origin.InitialTargetSeat) ||
            origin.BindingId != frame.TriggerId || origin.SkillInstanceId != frame.SkillInstanceId || origin.GameplayHash != frame.GameplayHash ||
            origin.InstructionIndex != frame.InstructionIndex || origin.LifecycleFrameId != receipt.LifecycleFrameId || origin.TurnNumber != receipt.TurnNumber ||
            origin.OutputKind != template.Kind || origin.ConditionKind != SkillProgramConditionKind.Always || origin.PindianBind is not null || origin.FrozenSourceWon is not null ||
            receipt.Stage == ZhaoguMirrorStage.TrickChild && (receipt.TrickChildId != use.Id || receipt.TrickActionId != action.ActionId)) return false;
        return ProgramEventHistory<CardUseDeclaredEvent>().Any(e => e.ResolutionId == use.Id)
            ? MatchesDeclaredActualDamageUse(use, action.ActorSeat, origin.InitialActorSeat)
            : receipt.Stage == ZhaoguMirrorStage.TrickIssuing && action.ActorSeat == frame.OwnerSeat;
    }

    private void ReturnZhaoguVirtualOrdinaryTrickUse(CardUseFrame use)
    {
        var origin = use.VirtualOrdinaryTrickOrigin!; var frame = GetActiveProgramFrame(origin.ParentProgramFrameId);
        var receipt = frame.ZhaoguMirror;
        if (!IsZhaoguVirtualOrdinaryTrickUse(use) || receipt is not { Stage: ZhaoguMirrorStage.TrickChild } ||
            _resolutionStack.LastOrDefault()?.Id != frame.Id || LifecycleCardUse(use.Id) is not null || use.Step != ResolutionFrameStep.Completed ||
            ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id && e.CardId == 0 && e.CardKind == origin.OutputKind) != 1)
            throw new InvalidOperationException("Zhaogu Mirror trick lost its exact once-completed native return.");
        if (use.CardAttack is { Active: true })
        {
            if (ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Any(e => e.UseId == use.Id))
                throw new InvalidOperationException("Zhaogu Mirror attack returned twice.");
            ReplaceRuntimeTop(frame with { ZhaoguMirror = receipt with { Stage = ZhaoguMirrorStage.TrickCleanup } });
            AdvanceEventRulesAndQueueFact(new ProgramVirtualOrdinaryAttackReturnedEvent(use.Id, origin)); return;
        }
        CompleteZhaogu(frame);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 62057)]
    private bool CompleteZhaoguAttackCleanup()
    {
        if (_resolutionStack.LastOrDefault() is not ProgramSkillFrame { ZhaoguMirror: { Stage: ZhaoguMirrorStage.TrickCleanup } receipt } frame) return false;
        if (!ValidZhaoguHeader(frame) || LifecycleCardUse(receipt.TrickChildId) is not null ||
            !ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Any(e => e.UseId == receipt.TrickChildId &&
                e.Origin.ParentProgramFrameId == frame.Id && e.Origin.CardActionId == receipt.TrickActionId))
            throw new InvalidOperationException("Zhaogu Mirror must wait for its original native attack cleanup.");
        CompleteZhaogu(frame); return true;
    }

    private void CompleteZhaogu(ProgramSkillFrame frame)
    { ReplaceRuntimeTop(frame = frame with { ZhaoguMirror = null }); AdvanceRuntimeProgram(frame.Id); }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 62057)]
    private bool ContinueIssuedZhaogu(ProgramSkillFrame frame) => frame.ZhaoguMirror is not null && ValidZhaoguHeader(frame);

    private void AssertZhaoguMirror(ProgramSkillFrame frame, SkillProgramEffect? paused)
    {
        if (frame.ZhaoguMirror is not { } receipt) return;
        if (paused?.Op != SkillProgramEffectOp.ResolveShanheZhaoguMirror || !ValidZhaoguHeader(frame) ||
            (receipt.Stage == ZhaoguMirrorStage.Template ? receipt.Template is not null : receipt.Template is null) ||
            receipt.Template is { } template && ProgramEventHistory<ProgramCardsRevealedEvent>().Count(e => e.FrameId == frame.Id &&
                e.SkillId == frame.SkillId && e.BindingId == frame.TriggerId && e.Bind == ZhaoguRevealKey &&
                e.Cards is [var card] && card.Id == template.Id && card.Kind == template.Kind) != 1)
            throw new InvalidOperationException("Zhaogu Mirror lost its frozen template, original phase header or single public reveal.");
        if (receipt.Stage is ZhaoguMirrorStage.Template or ZhaoguMirrorStage.UseChooser)
        {
            if (_resolutionStack.LastOrDefault()?.Id != frame.Id || _pendingDecision is not { Kind: DecisionKind.ProgramTrigger } prompt ||
                prompt.PlayerSeat != frame.OwnerSeat || !prompt.Choices.Select(c => c.Id).SequenceEqual(ZhaoguChoices(frame).Select(c => c.Id)))
                throw new InvalidOperationException("Zhaogu Mirror lost its unpaid exact choice.");
        }
        else if (receipt.Stage == ZhaoguMirrorStage.BasicChild)
        {
            if (frame.VirtualBasicDraft is not { } draft || !IsZhaoguVirtualBasicDraft(frame, draft) ||
                draft.ChildFrameId is { } child && (LifecycleCardUse(child) is not { } basic || basic.CardKind != receipt.Template!.Kind))
                throw new InvalidOperationException("Zhaogu Mirror basic child lost its single generic draft.");
        }
        else if (receipt.Stage == ZhaoguMirrorStage.TrickChild &&
            (LifecycleCardUse(receipt.TrickChildId) is not { } use || !IsZhaoguVirtualOrdinaryTrickUse(use)))
            throw new InvalidOperationException("Zhaogu Mirror trick child lost its actual original Use.");
        else if (receipt.Stage == ZhaoguMirrorStage.RevealChildren && !IsPublicHandRevealProducerSuspended(frame.Id, ZhaoguRevealKey))
            throw new InvalidOperationException("Zhaogu Mirror reveal lost its native reveal child.");
        else if (receipt.Stage == ZhaoguMirrorStage.TrickCleanup &&
            (receipt.TrickChildId <= 0 || receipt.TrickActionId <= 0 || LifecycleCardUse(receipt.TrickChildId) is not null ||
             ProgramEventHistory<ProgramVirtualOrdinaryAttackReturnedEvent>().Count(e => e.UseId == receipt.TrickChildId &&
                 e.Origin.ParentProgramFrameId == frame.Id && e.Origin.CardActionId == receipt.TrickActionId &&
                 e.Origin.SkillInstanceId == frame.SkillInstanceId) != 1))
            throw new InvalidOperationException("Zhaogu Mirror cleanup lost its original retired attack and single typed return.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheZhaoguMirror)]
    private PromptChoice ChooseZhaoguMirror(PendingDecision prompt, ProgramSkillFrame frame) =>
        prompt.Choices.FirstOrDefault(choice => choice.Parameters.GetValueOrDefault("program-action") != ZhaoguCancelAction) ?? prompt.Choices.First();
}
