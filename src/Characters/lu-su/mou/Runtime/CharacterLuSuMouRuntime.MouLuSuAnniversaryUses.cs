#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLuSuMouRuntime
{
    internal ProgramSkillFrame? MouLuSuUseQuery(int actor)
    {
        var top = _resolutionStackOperations._resolutionStack.LastOrDefault();
        var parent = top switch
        {
            DrawFundedDistinctBasicFrame p when IsMouLuSuPaymentFrame(p) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.Payment.ParentFrameId),
            ChainedStateBasicFrame p when IsMouLuSuPaymentFrame(p) => _resolutionStackOperations._resolutionStack.SingleOrDefault(f => f.Id == p.Payment.ParentFrameId),
            ProgramSkillFrame { EquipmentSlotNamedUse: { } slot }when MouLuSuSlotParent(slot)is { } p => p,
            _ => top
        };
        return parent is ProgramSkillFrame { MouLuSuAnniversary: { Yin: false, Operation: SkillProgramEffectOp.ResolveMouLuSuMengmou, Stage: MouLuSuAnniversaryStage.Request or MouLuSuAnniversaryStage.DraftCards or MouLuSuAnniversaryStage.DraftTargets or MouLuSuAnniversaryStage.Issuing } r } f && r.Other == actor && r.Index < r.X && _coreMatchParticipantsPlayersAccess._players[actor].IsAlive && MouLuSuAnniversary.Valid(f) ? f : null;
    }

    internal bool IsMouLuSuUseQuery(CharacterState actor) => MouLuSuUseQuery(actor.Seat)is not null;
    internal static LegalAction FreezeMouLuSuAction(LegalAction a) => a with
    {
        TargetSeats = Array.AsReadOnly(a.TargetSeats.ToArray()),
        AdditionalConversionSources = Array.AsReadOnly((a.AdditionalConversionSources ?? []).ToArray()),
        SelectableCardIds = Array.AsReadOnly(a.SelectableCardIds.ToArray()),
        SelectableTargetSeats = Array.AsReadOnly(a.SelectableTargetSeats.ToArray()),
        SelectableCardTargets = a.SelectableCardTargets is null ? null : Array.AsReadOnly(a.SelectableCardTargets.ToArray()),
        SelectableTargetSequences = a.SelectableTargetSequences is null ? null : Array.AsReadOnly(a.SelectableTargetSequences.Select(x => (IReadOnlyList<int>)Array.AsReadOnly(x.ToArray())).ToArray())
    };
    internal IReadOnlyList<LegalAction> MouLuSuUseActions(ProgramSkillFrame f) => f.MouLuSuAnniversary is { } r && MouLuSuUseQuery(r.Other)?.Id == f.Id && _coreMatchOutcomeWinnerAccess._winner == Winner.None ? Array.AsReadOnly(_coreNativeLegalActionsOperations.BuildLegalActions(_coreMatchParticipantsPlayersAccess._players[r.Other], includeProgramActions: false).Where(a => a.Kind == LegalActionKind.Slash || a.Kind == LegalActionKind.UseEquipmentEffect && a.EquipmentKind == CardKind.ZhangbaSerpentSpear || a.PlayedCardKind is { } kind && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(kind) && a.Kind != LegalActionKind.UseProgramSkill).Select(FreezeMouLuSuAction).ToArray()) : [];
    [ProgramTargetRule(ProgramTargetRulePoint.SlashQuotaBeforeLimit, 36100)]
    internal bool MouLuSuRequestedQuota(ProgramSlashQuotaQuery q) => IsMouLuSuUseQuery(q.Actor);
    internal void IssueMouLuSuUse(ProgramSkillFrame f)
    {
        if (!MouLuSuAnniversary.Valid(f) || f.MouLuSuAnniversary is not { Stage: MouLuSuAnniversaryStage.Issuing, SelectedAction: { } action } r || MouLuSuUseQuery(r.Other)?.Id != f.Id)
            throw new InvalidOperationException("Mengmou lost its actual Yang Slash request.");
        if (action.DrawFundedDistinctBasicUse is not null)
        {
            _coreDrawFundedDistinctBasicPaymentOperations.BeginDrawFundedDistinctBasic(_coreMatchParticipantsPlayersAccess._players[r.Other], action.ConversionSource!, action.PlayedCardKind!.Value, action.TargetSeats, r.OriginalDecision!, new(DrawFundedDistinctBasicIntent.Play, f.Id, f.Id, null, f.InstructionIndex, action.TargetSeat));
            return;
        }

        if (action.ChainedStateBasicUse is not null)
        {
            _coreChainedStateBasicConversionsOperations.BeginChainedStateBasic(_coreMatchParticipantsPlayersAccess._players[r.Other], action.ConversionSource!, action.PlayedCardKind!.Value, action.TargetSeats, r.OriginalDecision!, new(ChainedStateBasicIntent.Play, f.Id, f.Id, null, f.InstructionIndex, action.TargetSeat));
            return;
        }

        if (action.TieredRoundZeroUse is not null && action.ConversionSource is { } source && _characterWoLongFengChuOlOperations.IsEquipmentSlotNamedSkill(source.SkillId))
        {
            _corePendingDecisionPolicyPipelineOperations._pendingDecision = r.OriginalDecision;
            if (_characterWoLongFengChuOlOperations.TryBeginEquipmentSlotNamedPlay(_coreMatchParticipantsPlayersAccess._players[r.Other], action))
                return;
            _coreNativePromptBoundaryOperations.ClearPendingDecision();
        }

        _coreNativeActionDispatchOperations.ExecuteAction(_coreMatchParticipantsPlayersAccess._players[r.Other], action, r.Cards, r.Targets);
    }

    [ProgramRuleObserver(ProgramRulePhase.BeforeEvent, 36100)]
    internal void BindMouLuSuUse(CardUseDeclaredEvent fact)
    {
        if (!HasMouLuSuAnniversary || _coreCardUseLifecycleOperations.LifecycleCardUse(fact.ResolutionId)is not { Action: { Type: CardActionType.Use } a } use)
            return;
        var i = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == use.Id);
        if (i < 1 || _resolutionStackOperations._resolutionStack[i - 1] is not ProgramSkillFrame { MouLuSuAnniversary: { Stage: MouLuSuAnniversaryStage.Issuing, UseId: 0, ActionId: 0, SelectedAction: { } selected } r } f)
            return;
        var expected = selected.Kind == LegalActionKind.UseEquipmentEffect ? r.Cards : selected.CardId is> 0 ? [selected.CardId.Value] : Array.Empty<int>();
        if (!MouLuSuAnniversary.Valid(f) || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(a.EffectiveKind) || a.ActorSeat != r.Other || a.ProviderSeat != r.Other || a.RequesterSeat is not null || use.SourceSeat != r.Other || !a.PhysicalCards.Select(c => c.CardId).Order().SequenceEqual(expected.Order()) || selected.PlayedCardKind is { } kind && a.EffectiveKind != kind || selected.ConversionSource is { } source && !a.ConversionChain.Contains(source))
            throw new InvalidOperationException("Mengmou changed its selected actual Slash source or physical materials.");
        var origin = new MouLuSuAnniversaryUseOrigin(f.Id, r.Source, r.GameplayHash, r.Index, r.Other, use.Id, a.ActionId);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { MouLuSuAnniversary = r with { Stage = MouLuSuAnniversaryStage.NativeUse, UseId = use.Id, ActionId = a.ActionId } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(use.Id, u => u with { MouLuSuAnniversary = origin, UnlimitedUse = true });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuSuAnniversaryUseIssuedEvent(origin));
    }

    internal bool MouLuSuNativeMatches(CardUseFrame use) => use.MouLuSuAnniversary is { } o && _resolutionStackOperations._resolutionStack.SingleOrDefault(x => x.Id == o.FrameId)is ProgramSkillFrame { MouLuSuAnniversary: { Stage: MouLuSuAnniversaryStage.NativeUse } r } f && MouLuSuAnniversary.Valid(f) && o.Source == r.Source && o.GameplayHash == r.GameplayHash && o.Index == r.Index && o.Actor == r.Other && o.UseId == use.Id && o.UseId == r.UseId && o.ActionId == r.ActionId && use.Action?.ActionId == o.ActionId && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && _coreShownEntityTurnPoliciesOperations.ShownEntityUseActorMatches(use, o.Actor, o.Actor) && _coreProgramEventHistoryOperations.ProgramEventHistory<MouLuSuAnniversaryUseIssuedEvent>().Count(e => e.Origin == o) == 1;
    [ProgramCardUsePayment(ProgramCardUsePaymentStage.BeforeSlashDebit, 36100)]
    internal bool MouLuSuNoSlashDebit(long id, int actor) => _coreCardUseLifecycleOperations.LifecycleCardUse(id)is { MouLuSuAnniversary: { } o } use && actor == o.Actor && MouLuSuNativeMatches(use);
    [NativeUseValidation(36100)]
    internal void ValidateMouLuSuUse(CardUseFrame use)
    {
        if (use.MouLuSuAnniversary is not null && (use.Step != ResolutionFrameStep.Completed || !MouLuSuNativeMatches(use)))
            throw new InvalidOperationException("Mengmou must return its actual completed native Slash.");
    }

    [NativeUseReturn(36100)]
    internal void ReturnMouLuSuUse(CardUseFrame use)
    {
        if (use.MouLuSuAnniversary is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.FrameId);
        var r = f.MouLuSuAnniversary!;
        if (!MouLuSuNativeMatches(use) || r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Mengmou cannot replace or repeat its native Slash return.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { MouLuSuAnniversary = r with { Returned = true } });
        if (use.CardAttack is not { Active: true })
            SettleMouLuSuUse(f);
    }

    internal void SettleMouLuSuUse(ProgramSkillFrame f)
    {
        var r = f.MouLuSuAnniversary!;
        if (!r.Returned || _coreCardUseLifecycleOperations.LifecycleCardUse(r.UseId)is not null)
            throw new InvalidOperationException("Mengmou must await its real whole-use tail.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { MouLuSuAnniversary = r with { Stage = MouLuSuAnniversaryStage.UseChildren, Index = r.Index + 1, After = _coreCardMovementJournalCardMovementsMovementSequenceAccess._movementSequence }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MouLuSuAnniversaryUseReturnedEvent(f.Id, r.Index, r.UseId, r.ActionId));
        MouLuSuAnniversary.Continue(f.Id);
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterCleanup, 36100)]
    internal bool ReturnMouLuSuAttack()
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { MouLuSuAnniversary: { Stage: MouLuSuAnniversaryStage.NativeUse, Returned: true } } f)
            return false;
        SettleMouLuSuUse(f);
        return true;
    }

    internal void AppendMouLuSuIssuedDamageCandidates(global::CardGame.Core.GameEngine.IDamageAttempt attack, SkillProgramTriggerWindow window, List<DamageTriggerCandidate> result)
    {
        if (window != SkillProgramTriggerWindow.AfterDamageApplied || _coreCardUseLifecycleOperations.LifecycleCardUse(attack.ResolutionId)is not { MouLuSuAnniversary: { } origin } use || !MouLuSuNativeMatches(use) || attack.SourceSeat != origin.Actor || attack.IsSourceLess)
            return;
        var trigger = _contentRegistryOperations._contentRegistry.GetSkill(origin.Source.SkillId).Program!.Triggers.Single(t => t.Id == "requested-slash-damage" && t.Effects is [{ Op: SkillProgramEffectOp.RecoverMouLuSuMengmouDamage }]);
        result.RemoveAll(c => c.OwnerSeat == origin.Source.OwnerSeat && c.ProgramId == origin.Source.SkillId && c.ProgramTriggerId == trigger.Id);
        result.Add(new(origin.Source.OwnerSeat, $"program:{(origin.Source.SkillId)}:{(trigger.Id)}:{(origin.Source.SkillInstanceId)}:occurrence-0", trigger.Priority, false, origin.Source.SkillId, trigger.Id, origin.Source.SkillInstanceId, origin.GameplayHash, 0));
    }

    [ProgramIssuedTrigger(430)]
    internal bool IsIssuedMouLuSuDamageCandidate(ProgramTriggerCandidate candidate, ProgramSkillWindowContext context) => candidate.BindingId == "requested-slash-damage" && _coreProgramLifecycleOperations.GetProgramTrigger2(candidate).Effects is [{ Op: SkillProgramEffectOp.RecoverMouLuSuMengmouDamage }] && _characterLuSuMouOperations.CreateMouLuSuAnniversaryFacts().Damage(candidate, context)is { } damage && context.DamageFrameId is { } id && _coreCardUseLifecycleOperations.LifecycleCardUse(damage.UseId)is { Action: { } action } && !_coreProgramEventHistoryOperations.ProgramEventHistory<MouLuSuAnniversaryDamageRewardIssuedEvent>().Any(e => e.DamageFrameId == id && e.UseId == damage.UseId && e.ActionId == action.ActionId);
}
