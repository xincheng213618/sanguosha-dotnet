#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceLihuo3160Runtime
{
    SkillProgramStepOutcome global::CardGame.Core.IShanheSourceJiangdongTrioHost.LihuoFirstDamage(ProgramSkillFrame f) => BeginShanheSourceLihuoCost(f);
    SkillProgramStepOutcome global::CardGame.Core.IShanheSourceJiangdongTrioHost.ChunlaoStorage(ProgramSkillFrame f) => _coreShanheSourceChunlao3161Operations.BeginShanheSourceChunlaoStorage(f);
    internal static bool IsShanheSourceLihuoConversion(CardConversionSource source) => source.SkillId == ShanheSourceJiangdongTrioIds.Lihuo && source.BindingId is "ordinary-slash-to-fire" or "committed-fire-target";
    internal int ShanheSourceLihuoProspectiveTargetBonus(CardUseFrame use) => use.CardKind == CardKind.Slash && _resolutionStackOperations._resolutionStack.LastOrDefault()is ProgramSkillFrame f && f.SkillId == ShanheSourceJiangdongTrioIds.Lihuo && f.WindowContext?.CardUse?.ParentCardUseFrameId == use.Id && !_coreProgramPersistentStateOperations.GetProgramBooleanState2(f.OwnerSeat, f.SkillId, f.SkillInstanceId, ShanheSourceJiangdongTrioIds.FireTargetState) ? 1 : 0;
    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 24920)]
    internal void UnlockShanheSourceLihuoTarget(IGameEvent fact)
    {
        CardConversionSource? source = fact switch
        {
            CardUseDeclaredEvent d when _coreCardUseLifecycleOperations.LifecycleCardUse(d.ResolutionId)is { CardKind: CardKind.FireSlash, Action: { } a } => a.ConversionChain.LastOrDefault(IsShanheSourceLihuoConversion),
            ProgramCurrentSlashFireChangedEvent c when c.OriginalKind == CardKind.Slash && IsShanheSourceLihuoConversion(c.Source) => c.Source,
            _ => null
        };
        if (source is not null)
            _coreProgramPersistentStateOperations.SetProgramBooleanState(source.OwnerSeat, source.SkillId, source.SkillInstanceId, ShanheSourceJiangdongTrioIds.FireTargetState, true);
    }

    internal CardUseFrame? ShanheSourceLihuoDamageUse(ProgramTriggerCandidate c, ProgramSkillWindowContext w)
    {
        if (c.SkillId != ShanheSourceJiangdongTrioIds.Lihuo || w.Window != SkillProgramTriggerWindow.AfterDamageApplied || w.Amount <= 0 || w.SourceSeat != c.OwnerSeat || w.DamageFrameId is not { } damageId || _coreDamageCursorRuntimeOperations.ActiveDamageTrigger is not { } parent || parent.Id != w.ParentFrameId || parent.ParentFrameId != damageId)
            return null;
        var attack = _coreNativeDamageHandlesOperations.GetDamageTriggerAttack(parent);
        if (!attack.DamageWasApplied || attack.IsSourceLess || attack.SourceSeat != c.OwnerSeat || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { CardKind: CardKind.FireSlash, Action: { Type: CardActionType.Use } a } use || a.ActorSeat != c.OwnerSeat || !a.ConversionChain.Any(s => IsShanheSourceLihuoConversion(s) && s.OwnerSeat == c.OwnerSeat && s.SkillInstanceId == c.SkillInstanceId))
            return null;
        return use;
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24920)]
    internal bool CanOfferShanheSourceLihuoCost(ProgramTriggerCandidate c, SkillProgramTrigger t, ProgramSkillWindowContext w) => !t.Effects.Any(e => e.Op == SkillProgramEffectOp.ResolveShanheSourceLihuoFirstDamage) || ShanheSourceLihuoDamageUse(c, w)is { } use && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceLihuoCostStartedEvent>().Any(e => e.UseId == use.Id && e.OwnerSeat == c.OwnerSeat);
    internal SkillProgramStepOutcome BeginShanheSourceLihuoCost(ProgramSkillFrame supplied)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        var c = new ProgramTriggerCandidate(f.OwnerSeat, f.SkillId, global::CardGame.Core.EngineOwners.CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(f), f.SkillInstanceId, f.GameplayHash, 0);
        if (f.ShanheSourceLihuoCost is not null || f.WindowContext is not { } w || ShanheSourceLihuoDamageUse(c, w)is not { } use || _coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceLihuoCostStartedEvent>().Any(e => e.UseId == use.Id && e.OwnerSeat == f.OwnerSeat))
            throw new InvalidOperationException("Source3160 cost requires its original first converted-Use damage.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceLihuoCost = new(use.Id, w.ParentFrameId, w.DamageFrameId!.Value) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceLihuoCostStartedEvent(f.Id, use.Id, w.DamageFrameId.Value, f.OwnerSeat, f.SkillInstanceId));
        PublishShanheSourceLihuoCost(f);
        return SkillProgramStepOutcome.AwaitChoice;
    }

    internal IReadOnlyList<PromptChoice> ShanheSourceLihuoCostChoices(ProgramSkillFrame f)
    {
        var r = f.ShanheSourceLihuoCost!;
        var cards = _coreProgramCardPaymentsOperations.BuildOwnedCardPaymentChoices(f.Id, f.OwnerSeat, f.OwnerSeat, [CardZoneKind.Hand, CardZoneKind.Equipment], OwnedCardMoveIntent.Discard);
        var result = new List<PromptChoice>();
        Dictionary<string, string> Args(string branch) => new()
        {
            ["program-action"] = "shanhe-source-lihuo-cost",
            ["frame-id"] = f.Id.ToString(CultureInfo.InvariantCulture),
            ["branch"] = branch,
            ["first"] = r.FirstCardId?.ToString(CultureInfo.InvariantCulture) ?? ""
        };
        if (cards.Count >= 2 && (r.FirstCardId is null || cards.Any(c => c.Cards.SequenceEqual([r.FirstCardId.Value]) && _cardZonesOperations._cardZones.GetLocation(r.FirstCardId.Value) == r.FirstFrom)))
            result.AddRange(cards.Where(c => r.FirstCardId != c.Cards[0]).Select(c => new PromptChoice(new($"source-lihuo.{(f.Id)}.{(r.FirstCardId)}.{(c.Cards[0])}"), r.FirstCardId is null ? "弃置两张牌：先选第一张 · " + c.Label : "选择第二张 · " + c.Label, c.Cards, [], Args("discard"))));
        result.Add(new(new($"source-lihuo.{(f.Id)}.hp"), "失去1点体力", [], [], Args("hp")));
        return Array.AsReadOnly(result.ToArray());
    }

    internal void PublishShanheSourceLihuoCost(ProgramSkillFrame f)
    {
        var choices = ShanheSourceLihuoCostChoices(f);
        var skill = _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId);
        _corePendingDecisionPolicyPipelineOperations._pendingDecision = new(DecisionKind.ProgramTrigger, f.OwnerSeat, "疠火：此牌首次造成伤害，失去1点体力或弃置两张牌。", choices.SelectMany(c => c.Cards).Distinct().ToArray(), [], f.OwnerSeat)
        {
            PromptId = _coreNativePromptBoundaryOperations.CreatePromptId(),
            IsPrivate = true,
            Choices = choices,
            SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description)
        };
        _coreMatchOutcomeStatusAccess._status = _coreMatchParticipantsPlayersAccess._players[f.OwnerSeat].IsHuman ? EngineStatus.AwaitingHumanResponse : EngineStatus.Running;
    }

    [ProgramAnswer("shanhe-source-lihuo-cost")]
    internal void AnswerShanheSourceLihuoCost(PromptChoice choice)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { ShanheSourceLihuoCost: { Paid: false } r } f || _corePendingDecisionPolicyPipelineOperations._pendingDecision?.PlayerSeat != f.OwnerSeat || !ShanheSourceLihuoCostChoices(f).Any(c => global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("Source3160 cost lost its exact private material choice.");
        _coreNativePromptBoundaryOperations.ClearPendingDecision();
        if (choice.Parameters["branch"] == "discard" && r.FirstCardId is null)
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceLihuoCost = r with { FirstCardId = choice.Cards[0], FirstFrom = _cardZonesOperations._cardZones.GetLocation(choice.Cards[0]) } });
            PublishShanheSourceLihuoCost(f);
            return;
        }

        if (choice.Parameters["branch"] == "hp")
        {
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceLihuoCost = r with { Paid = true, LostHp = true }, Step = ResolutionFrameStep.ResolvingEffect });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceLihuoPaidEvent(f.Id, r.UseId, f.OwnerSeat, null, null, true));
            _kernelCoreGameEngineAdvancedLifecycleHostOperations.CreateProgramSkillHost().LoseHp(f.Id, f.SkillId, f.OwnerSeat, 1);
        }
        else
        {
            var second = choice.Cards[0];
            var before = _coreCardMovementJournalMovementSequenceAccess._movementSequence;
            _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ShanheSourceLihuoCost = r with { Paid = true, SecondCardId = second, SecondFrom = _cardZonesOperations._cardZones.GetLocation(second), Before = before, After = before }, Step = ResolutionFrameStep.ResolvingEffect });
            _coreNativeLogicalDiscardsOperations.MoveProgramLogicalDiscard(f, f.OwnerSeat, [r.FirstCardId!.Value, second], new("shanhe.source-lihuo-3160.cost"), (_, _) =>
            {
                var current = _coreProgramLifecycleOperations.GetActiveProgramFrame(f.Id);
                _coreRuntimeOperations.ReplaceRuntimeTop(current with { ShanheSourceLihuoCost = current.ShanheSourceLihuoCost!with { After = _coreCardMovementJournalMovementSequenceAccess._movementSequence } });
            });
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ShanheSourceLihuoPaidEvent(f.Id, r.UseId, f.OwnerSeat, r.FirstCardId, second, false));
        }

        _coreRuntimeOperations.AdvanceRuntimeProgram(f.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Resume, 24920)]
    internal bool ResumeShanheSourceLihuoCost(long id)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame f || f.Id != id || f.ShanheSourceLihuoCost is not { } r)
            return false;
        if (!r.Paid)
            return true;
        if (_coreRecoveryReplacementOperations.TryBeginQueuedRecoveryReplacement(id, PostEventContinuation.Program) || _coreCharacterStateProgramsOperations.TryBeginCharacterStateProgramWindow(id, CharacterStateContinuation.Program) || _coreHpChangeProgramsOperations.TryBeginHpChangedProgramWindow(id, PostEventContinuation.Program) || _coreCardMovementProgramsOperations.TryBeginCardsMovedProgramWindow(id))
            return true;
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { ShanheSourceLihuoCost = null });
        return false;
    }

    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 24920)]
    internal void AssertShanheSourceLihuoCost(ProgramSkillFrame f)
    {
        if (f.ShanheSourceLihuoCost is not { } r)
            return;
        if (f.SkillId != ShanheSourceJiangdongTrioIds.Lihuo || f.WindowContext?.ParentFrameId != r.DamageWindowId || f.WindowContext.DamageFrameId != r.DamageId || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is null || !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceLihuoCostStartedEvent>().Any(e => e.ProgramFrameId == f.Id && e.UseId == r.UseId && e.DamageId == r.DamageId && e.OwnerSeat == f.OwnerSeat && e.SkillInstanceId == f.SkillInstanceId) || r.Paid && !_coreProgramEventHistoryOperations.ProgramEventHistory<ShanheSourceLihuoPaidEvent>().Any(e => e.ProgramFrameId == f.Id && e.UseId == r.UseId && e.LostHp == r.LostHp))
            throw new InvalidOperationException("Source3160 lost its original first-damage cost receipt.");
    }

    [ProgramAiChoice(SkillProgramEffectOp.ResolveShanheSourceLihuoFirstDamage)]
    internal PromptChoice AiShanheSourceLihuoCost(PendingDecision p) => p.Choices.FirstOrDefault(c => c.Parameters["branch"] == "discard") ?? p.Choices.Single();
}
