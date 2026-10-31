#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreHulaoActivityProgramsRuntime
{
    SkillProgramStepOutcome global::CardGame.Core.IHulaoActivityProgramHost.ExecuteHulaoActivity(ProgramSkillFrame f, SkillProgramEffect e) => ExecuteHulaoActivity(f, e);
    internal static CardConversionSource HulaoProgramSource(ProgramSkillFrame f) => new(f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    internal bool HulaoProgramSourceValid(ProgramSkillFrame f) => _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], f.SkillId, f.SkillInstanceId) && _coreSkillProgramsOperations.EnabledSkillPrograms(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(p => p.Id == f.SkillId && p.GameplayHash == f.GameplayHash);
    internal SkillProgramStepOutcome ExecuteHulaoActivity(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        switch (effect.Op)
        {
            case SkillProgramEffectOp.ResolveHulaoXiuluo:
                if (f.HulaoXiuluo is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow } xiuluo)
                    throw new InvalidOperationException("Xiuluo requires its original preparation window.");
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HulaoXiuluo = new(f.InstructionIndex, HulaoProgramSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, xiuluo.ParentFrameId) });
                if (PublishHulaoXiuluo(f))
                    return SkillProgramStepOutcome.AwaitChoice;
                _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id)with { HulaoXiuluo = null });
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.OfferHulaoDamagePeach:
                if (f.HulaoPeach is not null || f.WindowContext is not { Window: SkillProgramTriggerWindow.AfterDamageApplied } damage || damage.TargetSeat != f.OwnerSeat)
                    throw new InvalidOperationException("Shenqu Peach requires the owner's original applied-damage window.");
                if (HulaoPeaches(f).Count == 0)
                    return SkillProgramStepOutcome.Continue;
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HulaoPeach = new(f.InstructionIndex, HulaoProgramSource(f), f.GameplayHash, _coreActualTurnProgressTurnNumberAccess._turnNumber, damage.ParentFrameId) });
                PublishHulaoPeach(f);
                return SkillProgramStepOutcome.AwaitChoice;
            case SkillProgramEffectOp.PreventHulaoArmoredLargeDamage:
                if (f.WindowContext?.Window != SkillProgramTriggerWindow.BeforeDamageApplied || _resolutionStackOperations._resolutionStack.ElementAtOrDefault(_resolutionStackOperations._resolutionStack.Count - 2)is not BeforeDamageProgramWindowFrame window || window.Id != f.WindowContext.ParentFrameId || window.TargetSeat != f.OwnerSeat)
                    throw new InvalidOperationException("Aozhan requires the live owning before-damage frame.");
                if (!window.Prevented && window.Amount > 1 && _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(c => EquipmentCatalog.Get(c.Kind).Slot == EquipmentSlot.Armor))
                    _coreDamagePreventionProgramsOperations.PreventProgramCurrentDamage(f);
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.BindPublicAllyDeath:
                if (f.WindowContext is not { Window: SkillProgramTriggerWindow.CharacterDied, TargetSeat: { } victim })
                    throw new InvalidOperationException("United supply requires an actual character-death window.");
                var ally = victim != f.OwnerSeat && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive && !_coreMatchParticipantsPlayersAccess._players[victim].IsAlive && _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].TeamId is { } team && _coreMatchParticipantsPlayersAccess._players[victim].TeamId == team;
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { ChoiceBindings = Array.AsReadOnly(f.ChoiceBindings.Append(new ProgramChoiceResultBinding(effect.ResultBind!, ally ? "ally" : "other", f.OwnerSeat)).ToArray()) });
                return SkillProgramStepOutcome.Continue;
            case SkillProgramEffectOp.SkipHulaoTreasureJudgment:
                return _corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Any(c => EquipmentCatalog.Get(c.Kind).Slot == EquipmentSlot.Treasure) ? _coreProgramTurnPhaseOperations.SkipProgramTurnPhases(f, [SkillProgramTurnPhase.Judgment]) : SkillProgramStepOutcome.Continue;
            default:
                throw new InvalidOperationException("Unsupported Hulao activity operation.");
        }
    }

    internal IReadOnlyList<PromptChoice> HulaoXiuluoChoices(ProgramSkillFrame f)
    {
        var choices = new List<PromptChoice>();
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (HulaoProgramSourceValid(f))
            foreach (var card in _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(owner)).OrderBy(c => c.Id))
            {
                var from = _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(owner, card);
                if (_coreTurnHandCategoryRestrictionsOperations.IsSelfHandCategoryDiscardForbidden(f.OwnerSeat, card, from, OwnedCardMoveIntent.Discard))
                    continue;
                var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(owner, card);
                if (_corePhysicalOwnedCardQueriesOperations.GetJudgment(owner).Any(j => j.Suit == suit))
                    choices.Add(new(new($"hulao-xiuluo.{(f.Id)}.{(card.Id)}"), $"弃置【{(card.DisplayName)}】，然后选择弃置一张同花色判定牌。", [card.Id], [], new Dictionary<string, string> { ["program-action"] = "hulao-xiuluo" }));
            }

        return choices;
    }

    internal bool PublishHulaoXiuluo(ProgramSkillFrame f)
    {
        var choices = HulaoXiuluoChoices(f);
        if (choices.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoXiuluo = f.HulaoXiuluo!with { Stage = HulaoXiuluoStage.Complete } });
            return false;
        }

        PublishHulaoPrompt(f, "弃置一张牌并移除一张同花色判定牌，或结束修罗。", choices.Append(new(new($"hulao-xiuluo.{(f.Id)}.stop"), "结束修罗", [], [], new Dictionary<string, string> { ["program-action"] = "hulao-xiuluo" })).ToArray());
        return true;
    }

    [ProgramAnswer("hulao-xiuluo")]
    internal void ResolveHulaoXiuluoChoice(PromptChoice answer)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.HulaoXiuluo!;
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (answer.Id.Value == $"hulao-xiuluo.{(f.Id)}.stop")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoXiuluo = r with { Stage = HulaoXiuluoStage.Complete } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var choice = HulaoXiuluoChoices(f).Single(c => c.Id == answer.Id);
        var card = _corePhysicalOwnedCardQueriesOperations.GetHand(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Concat(_corePhysicalOwnedCardQueriesOperations.GetEquipment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).Single(c => c.Id == choice.Cards.Single());
        var from = _coreNativeOwnedCardQueriesOperations.FindOwnedCardLocation(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
        var suit = _coreNativeCardAppearanceOperations.EffectiveSuit(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat], card);
        r = r with
        {
            Stage = HulaoXiuluoStage.CostChildren,
            Iteration = r.Iteration + 1,
            CostCardId = card.Id,
            CostFrom = from,
            PaidSuit = suit,
            JudgmentCardId = null
        };
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoXiuluo = r, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, from, new("program.hulao.xiuluo.cost"), parentFrameId: f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HulaoXiuluoPaidEvent(f.Id, f.OwnerSeat, r.Iteration, card.Id, suit));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal IReadOnlyList<PromptChoice> HulaoXiuluoJudgmentChoices(ProgramSkillFrame f) => !_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsAlive ? [] : _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Where(c => c.Suit == f.HulaoXiuluo!.PaidSuit).OrderBy(c => c.Id).Select(c => new PromptChoice(new($"hulao-xiuluo-judgment.{(f.Id)}.{(c.Id)}"), $"弃置判定区内的【{(c.DisplayName)}】。", [c.Id], [], new Dictionary<string, string> { ["program-action"] = "hulao-xiuluo-judgment" })).ToArray();
    internal bool PublishHulaoXiuluoJudgment(ProgramSkillFrame f)
    {
        var choices = HulaoXiuluoJudgmentChoices(f);
        if (choices.Count == 0)
            return false;
        PublishHulaoPrompt(f, "修罗的弃牌效果已结算：选择一张现存的同花色判定牌弃置。", choices);
        return true;
    }

    [ProgramAnswer("hulao-xiuluo-judgment")]
    internal void ResolveHulaoXiuluoJudgmentChoice(PromptChoice answer)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.HulaoXiuluo!;
        if (r.Stage != HulaoXiuluoStage.JudgmentOffering || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat)
            throw new InvalidOperationException("Xiuluo lost the post-payment judgment choice.");
        var choice = HulaoXiuluoJudgmentChoices(f).Single(c => c.Id == answer.Id);
        var card = _corePhysicalOwnedCardQueriesOperations.GetJudgment(_coreMatchParticipantsPlayersAccess._players[f.OwnerSeat]).Single(c => c.Id == choice.Cards.Single());
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoXiuluo = r with { Stage = HulaoXiuluoStage.JudgmentChildren, JudgmentCardId = card.Id }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _coreNativeLogicalDiscardsOperations.MoveLogicalDiscard(f.OwnerSeat, f.OwnerSeat, card, CardLocation.Judgment(f.OwnerSeat), new("program.hulao.xiuluo.judgment"), parentFrameId: f.Id);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HulaoXiuluoJudgmentDiscardedEvent(f.Id, f.OwnerSeat, r.Iteration, card.Id));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    internal IReadOnlyList<Card> HulaoPeaches(ProgramSkillFrame f)
    {
        var owner = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat];
        if (!HulaoProgramSourceValid(f) || !owner.HasHpValue || owner.Hp >= owner.MaxHp || _coreCardUseModulesOperations.IsCardUseForbidden(f.OwnerSeat, CardKind.Peach, CardActionType.Use) || _coreHandComparisonOperations.IsSelfTargetForbiddenAction(owner, CardKind.Peach, [f.OwnerSeat]) || _coreDirectedTurnCardPoliciesOperations.IsDirectedCardTargetProhibited(f.OwnerSeat, f.OwnerSeat, CardKind.Peach))
            return[];
        return _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Where(c => c.Kind == CardKind.Peach && !_coreCardUseModulesOperations.IsTurnHandCardRestricted(owner, c) && !_coreProgramParticipantPhaseOperationsOperations.IsTurnPhysicalUseForbidden(f.OwnerSeat, [c.Id]) && !_coreNativeDamagePoliciesOperations.IsCardTargetProhibited(owner, CardKind.Peach, _coreNativeCardAppearanceOperations.EffectiveSuit(owner, c), global::CardGame.Core.EngineOwners.CoreGameEnginePublicPileColorsRuntime.SuitColor(_coreNativeCardAppearanceOperations.EffectiveSuit(owner, c)))).OrderBy(c => c.Id).ToArray();
    }

    internal IReadOnlyList<PromptChoice> HulaoPeachChoices(ProgramSkillFrame f) => HulaoPeaches(f).Select(card => new PromptChoice(new($"hulao-peach.{(f.Id)}.{(card.Id)}"), "使用一张【桃】。", [card.Id], [f.OwnerSeat], new Dictionary<string, string> { ["program-action"] = "hulao-peach" })).Append(new(new($"hulao-peach.{(f.Id)}.stop"), "不使用【桃】", [], [], new Dictionary<string, string> { ["program-action"] = "hulao-peach" })).ToArray();
    internal void PublishHulaoPeach(ProgramSkillFrame f) => PublishHulaoPrompt(f, "神躯：使用一张真实的【桃】，或放弃。", HulaoPeachChoices(f));
    internal void PublishHulaoPrompt(ProgramSkillFrame f, string text, IReadOnlyList<PromptChoice> choices)
    {
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, text, choices.SelectMany(c => c.Cards).Distinct().ToArray(), choices.SelectMany(c => c.Targets).Distinct().ToArray(), f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = Array.AsReadOnly(choices.ToArray()),
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("hulao-peach")]
    internal void ResolveHulaoPeachChoice(PromptChoice answer)
    {
        var f = (ProgramSkillFrame)_resolutionStackOperations._resolutionStack.Last();
        var r = f.HulaoPeach!;
        var choice = HulaoPeachChoices(f).Single(c => c.Id == answer.Id);
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Cards.Count == 0)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoPeach = r with { Stage = HulaoPeachStage.Complete } });
            _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
            return;
        }

        var card = HulaoPeaches(f).Single(c => c.Id == choice.Cards.Single());
        var useId = _coreResolutionIdentityBoundaryOperations._resolutionSequence + 1;
        r = r with
        {
            Stage = HulaoPeachStage.Using,
            CardId = card.Id,
            CardUseFrameId = useId
        };
        var origin = new HulaoPeachOrigin(f.Id, r);
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoPeach = r });
        var actual = _coreNativeCardUseOperations.BeginCardUse(card, f.OwnerSeat, [f.OwnerSeat], playedCardKind: CardKind.Peach);
        if (actual != useId)
            throw new InvalidOperationException("Shenqu Peach issuance changed its reserved real-use identity.");
        var use = (CardUseFrame)_resolutionStackOperations._resolutionStack.Single(frame => frame.Id == useId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(useId, use with { HulaoPeachOrigin = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HulaoPeachIssuedEvent(origin));
        _corePhysicalMovementFacadesOperations.MoveCard(card, CardLocation.Hand(f.OwnerSeat), CardLocation.Processing, CardMoveReasons.Use);
        _coreCardUseLifecycleOperations.BeginSimpleCardUse(useId, new(card.Id, SimpleCardUseEffect.Recovery, 1));
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 27600)]
    internal bool ResumeHulaoActivity(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.HulaoXiuluo is null && f.HulaoPeach is null)
            return false;
        if (f.HulaoXiuluo is { } x)
        {
            if (x.Stage == HulaoXiuluoStage.Complete)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoXiuluo = null });
                return false;
            }

            if (x.Stage == HulaoXiuluoStage.Offering)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null && !PublishHulaoXiuluo(f))
                    _coreRuntimeOperations.AdvanceRuntimeProgram(id);
                return true;
            }

            if (x.Stage == HulaoXiuluoStage.JudgmentOffering)
            {
                if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                    PublishHulaoXiuluoJudgment(f);
                return true;
            }

            if (HulaoAwaitMovementChildren(id))
                return true;
            f = _coreProgramLifecycleOperations.GetActiveProgramFrame(id);
            x = f.HulaoXiuluo!;
            if (x.Stage == HulaoXiuluoStage.CostChildren)
            {
                _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HulaoXiuluo = x = x with { Stage = HulaoXiuluoStage.JudgmentOffering } });
                if (PublishHulaoXiuluoJudgment(f))
                    return true;
            }

            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { HulaoXiuluo = x with { Stage = HulaoXiuluoStage.Offering } });
            if (!PublishHulaoXiuluo(f))
                _coreRuntimeOperations.AdvanceRuntimeProgram(id);
            return true;
        }

        var p = f.HulaoPeach!;
        if (p.Stage == HulaoPeachStage.Complete)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoPeach = null });
            return false;
        }

        if (p.Stage == HulaoPeachStage.Offering)
        {
            if (_corePendingDecisionPolicyPipelineOperations._pendingDecision is null)
                PublishHulaoPeach(f);
            return true;
        }

        if (p.Stage != HulaoPeachStage.Children)
            throw new InvalidOperationException("A real Shenqu Peach returned without its typed completion.");
        if (HulaoAwaitMovementChildren(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { HulaoPeach = p with { Stage = HulaoPeachStage.Complete } });
        _coreRuntimeOperations.AdvanceRuntimeProgram(id);
        return true;
    }

    internal bool HulaoAwaitMovementChildren(long id)
    {
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.AwaitedProgramMovement) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.AwaitedProgramMovement) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(_coreProgramLifecycleOperations.GetActiveProgramFrame(id)with { PendingMovementContinuation = null });
        return false;
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 27600)]
    internal bool ContinuePaidHulaoAfterSkillLoss(ProgramSkillFrame f) => f.HulaoXiuluo?.Stage is HulaoXiuluoStage.CostChildren or HulaoXiuluoStage.JudgmentOffering or HulaoXiuluoStage.JudgmentChildren || f.HulaoPeach?.Stage is HulaoPeachStage.Using or HulaoPeachStage.Children;
    [NativeUseValidation(27601)]
    internal void ValidateHulaoPeachUse(CardUseFrame use)
    {
        if (use.HulaoPeachOrigin is not { } origin)
            return;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == use.Id);
        var r = origin.Issuance;
        if (index <= 0 || _resolutionStackOperations._resolutionStack[index - 1] is not ProgramSkillFrame f || f.Id != origin.ProgramFrameId || f.HulaoPeach != r || r.Stage != HulaoPeachStage.Using || use.Id != r.CardUseFrameId || use.CardId != r.CardId || use.CardKind != CardKind.Peach || use.Action is not { Type: CardActionType.Use, EffectiveKind: CardKind.Peach } action || action.ProviderSeat != f.OwnerSeat || action.PhysicalCards is not [var physical] || physical.CardId != r.CardId || !_coreActualTurnDamageClaimsOperations.MatchesDeclaredActualDamageUse(use, action.ActorSeat, f.OwnerSeat))
            throw new InvalidOperationException("A real Shenqu Peach lost its original physical payment and program owner.");
    }

    [NativeUseReturn(27601)]
    internal void ReturnHulaoPeachUse(CardUseFrame use)
    {
        if (use.HulaoPeachOrigin is not { } origin)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(origin.ProgramFrameId);
        var r = origin.Issuance;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { HulaoPeach = r with { Stage = HulaoPeachStage.Children }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new HulaoPeachReturnedEvent(origin));
        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }
    internal bool HulaoActivityFrameRidesOn(ResolutionFrame child, ResolutionFrame parent)
    {
        if (child is CardUseFrame { HulaoPeachOrigin: { } origin } use && parent is ProgramSkillFrame owner)
        {
            if (origin.ProgramFrameId != owner.Id || owner.HulaoPeach != origin.Issuance)
                return false;
            ValidateHulaoPeachUse(use);
            return true;
        }

        if (child is ProgramCardTriggerWindowFrame window && parent is CardUseFrame { HulaoPeachOrigin: not null } peach)
        {
            ValidateHulaoPeachUse(peach);
            return window.ParentFrameId == peach.Id && window.Action.ActionId == peach.Action?.ActionId;
        }

        if (child is ProgramSkillFrame { WindowContext: { } context } observer && parent is ProgramCardTriggerWindowFrame cardWindow && _coreCardUseLifecycleOperations.LifecycleCardUse(cardWindow.ParentFrameId)is { HulaoPeachOrigin: not null } actual)
        {
            ValidateHulaoPeachUse(actual);
            return context.ParentFrameId == cardWindow.Id && context.CardUse?.ParentCardUseFrameId == actual.Id && cardWindow.Candidates.Any(candidate => candidate.OwnerSeat == observer.OwnerSeat && candidate.SkillId == observer.SkillId && candidate.SkillInstanceId == observer.SkillInstanceId && candidate.TriggerId == observer.TriggerId);
        }

        return false;
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveHulaoXiuluo)]
    internal PromptChoice SelectAiHulaoXiuluo(PendingDecision decision, ProgramSkillFrame f) => SelectHulaoActivityChoice(decision, f);
    [ProgramAiChoice(SkillProgramEffectOp.OfferHulaoDamagePeach)]
    internal PromptChoice SelectAiHulaoPeach(PendingDecision decision, ProgramSkillFrame f) => SelectHulaoActivityChoice(decision, f);
    internal PromptChoice SelectHulaoActivityChoice(PendingDecision decision, ProgramSkillFrame f) => decision.Choices.OrderBy(c => c.Cards.Count == 0 ? 1 : 0).ThenBy(c => c.Cards.Count == 0 ? 0 : global::CardGame.Core.EngineOwners.CoreNativeAiDecisionsRuntime.GetKeepValue(_coreAdvancedLifecycleOperations.GetAdvancedCard(c.Cards[0]), _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat])).First();
}
