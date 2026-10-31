#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterSunQuanOlMagicRuntime
{
    internal bool IsMoQuanyuAddedTargetFact(CardUseFrame use, MoQuanyuTargetAddedEvent fact) => use.Action is { Type: CardActionType.Use } a && global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) && fact.UseId == use.Id && fact.ActionId == a.ActionId && fact.Source.SkillId == "ol:mo-quanyu" && fact.Source.BindingId == "singleton-slash-effects" && fact.Source.OwnerSeat == use.SourceSeat && fact.TargetIndex >= 1 && _coreTableSeatQueriesOperations.IsValidPlayerSeat(fact.TargetSeat) && fact.TargetSeat != use.SourceSeat && _contentRegistryOperations._contentRegistry.GetSkill(fact.Source.SkillId).Program?.GameplayHash == fact.GameplayHash && _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashStartedEvent>().Count(e => e.FrameId == fact.FrameId && e.Source == fact.Source && e.GameplayHash == fact.GameplayHash && e.Original.UseId == use.Id && e.Original.ActionId == a.ActionId && (e.Mask & MoQuanyuBit(MoQuanyuEffect.Qingming)) != 0) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Count(e => e.FrameId == fact.FrameId && e.SkillId == fact.Source.SkillId && e.BindingId == fact.Source.BindingId && e.OwnerSeat == fact.Source.OwnerSeat && e.SkillInstanceId == fact.Source.SkillInstanceId && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized) == 1 && _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuTargetAddedEvent>().Count(e => e.FrameId == fact.FrameId && e.UseId == use.Id) == 1;
    internal bool HasMoQuanyuAddedTargetTail(CardUseFrame use) => _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuTargetAddedEvent>().Any(e => IsMoQuanyuAddedTargetFact(use, e));
    internal bool ValidMoQuanyuSlash(ProgramSkillFrame f) => f.MoQuanyuSlash is { } r && f.SkillId == "ol:mo-quanyu" && f.TriggerId == "singleton-slash-effects" && f.InstructionIndex == 1 && r.Source == new CardConversionSource(f.SkillId, f.TriggerId, f.OwnerSeat, f.SkillInstanceId) && r.GameplayHash == f.GameplayHash && _contentRegistryOperations._contentRegistry.GetSkill(f.SkillId).Program?.GameplayHash == f.GameplayHash && r.Mask is> 0 and <= 63 && r.Stage == MoQuanyuSlashStage.AdditionalTarget && f.WindowContext is { } context && TryGetMoQuanyuSingletonUse(f.OwnerSeat, context, out var use, out _, out _) && use.MoQuanyuSoleUse == r.Original && use.MoQuanyuSlashPolicy is null && _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashStartedEvent>().Count(e => e.FrameId == f.Id && e.Source == r.Source && e.GameplayHash == r.GameplayHash && e.Original == r.Original && e.Mask == r.Mask && e.Entered == r.Entered) == 1 && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashAppliedEvent>().Any(e => e.FrameId == f.Id);
    [ProgramFrameInvariant(ProgramFrameInvariantPhase.AfterSelectionState, 28912)]
    internal void AssertMoQuanyuSlash(ProgramSkillFrame f)
    {
        if (f.MoQuanyuSlash is null)
        {
            if (_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashStartedEvent>().Any(e => e.FrameId == f.Id) && !_coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashAppliedEvent>().Any(e => e.FrameId == f.Id))
                throw new InvalidOperationException("Quanyu cannot discard an issued sword policy.");
            return;
        }

        if (!ValidMoQuanyuSlash(f))
            throw new InvalidOperationException("Quanyu lost its original finalized singleton, signed source or frozen effect mask.");
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()?.Id == f.Id && f.Step == ResolutionFrameStep.AwaitingResponse && (_corePendingDecisionPolicyPipelineOperations._pendingDecision is not { IsPrivate: false } prompt || prompt.PlayerSeat != f.OwnerSeat || !global::CardGame.Core.EngineOwners.CoreGameEngineAssistedPhysicalCardsRuntime.AssistedChoicesEqual(prompt.Choices, f.MoQuanyuSlash.Choices)))
            throw new InvalidOperationException("Quanyu cannot replace its issued additional-target options.");
    }

    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 28912)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 28912)]
    internal bool ContinueIssuedMoQuanyuSlash(ProgramSkillFrame f) => f.MoQuanyuSlash is not null && ValidMoQuanyuSlash(f);
    internal void AssertMoQuanyuSlashUses()
    {
        foreach (var use in _resolutionStackOperations._resolutionStack.OfType<CardUseFrame>())
        {
            if (use.MoQuanyuSoleUse is { } sole && (use.Action is not { Type: CardActionType.Use } a || sole.UseId != use.Id || sole.ActionId != a.ActionId || sole.ActorSeat != use.SourceSeat || sole.Kind != use.CardKind || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(sole.TargetSeat) || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSoleUseCapturedEvent>().Count(e => e.Original == sole) != 1))
                throw new InvalidOperationException("A Mo singleton receipt must retain its exact original native Use.");
            if (use.MoQuanyuQuota is { } q && (use.Action is not { Type: CardActionType.Use } action || q.UseId != use.Id || q.ActionId != action.ActionId || q.ActorSeat != use.SourceSeat || q.Kind != use.CardKind || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || (q.ChoiceMask & MoQuanyuBit(MoQuanyuEffect.Liuxing)) == 0 || q.Source.SkillId != "ol:mo-quanyu" || q.Source.OwnerSeat != use.SourceSeat || _contentRegistryOperations._contentRegistry.GetSkill(q.Source.SkillId).Program?.GameplayHash != q.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuQuotaAcceptedEvent>().Count(e => e.Receipt == q) != 1))
                throw new InvalidOperationException("Meteor's quota receipt lost the accepted sole-target Slash.");
            if (use.MoQuanyuSlashPolicy is { } r && (use.MoQuanyuSoleUse != r.Original || r.Stage != MoQuanyuSlashStage.Applied || r.Mask is < 1 or > 63 || _contentRegistryOperations._contentRegistry.GetSkill(r.Source.SkillId).Program?.GameplayHash != r.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashStartedEvent>().Where(e => e.Original == r.Original).ToArray()is not [var start] || start.Source != r.Source || start.GameplayHash != r.GameplayHash || start.Mask != r.Mask || start.Entered != r.Entered || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuSlashAppliedEvent>().Count(e => e.FrameId == start.FrameId && e.UseId == use.Id && e.ActionId == r.Original.ActionId && e.Mask == r.Mask && e.AddedTargetSeat == r.AddedTargetSeat) != 1))
                throw new InvalidOperationException("Quanyu cannot change or duplicate an accepted Slash policy.");
            if (use.MoQuanyuExtraRound is { } round && (use.MoQuanyuSlashPolicy is not { } policy || (policy.Mask & MoQuanyuBit(MoQuanyuEffect.Baili)) == 0 || round.Source != policy.Source || round.GameplayHash != policy.GameplayHash || round.ActionId != policy.Original.ActionId || round.Native.ActionId != round.ActionId || round.Native.Source != round.Source || round.Native.WholeGroupRepeated != round.Started || round.Active && !round.Started || round.Native.OriginalAttacks.GroupBy(s => s.TargetIndex).Any(g => g.Count() != 1) || round.Native.OriginalAttacks.Any(s => s.TargetIndex < 0 || s.TargetIndex >= use.TargetSeats.Count) || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuExtraRoundIssuedEvent>().Count(e => e.FrameId == round.ProgramFrameId && e.UseId == use.Id && e.ActionId == round.ActionId && e.Source == round.Source && e.GameplayHash == round.GameplayHash) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<MoQuanyuExtraRoundStartedEvent>().Count(e => e.UseId == use.Id) != (round.Started ? 1 : 0)))
                throw new InvalidOperationException("Baili must retain its own additional native effect without replacing other issuers.");
        }
    }
}
