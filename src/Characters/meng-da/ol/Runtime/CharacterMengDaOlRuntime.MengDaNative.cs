#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterMengDaOlRuntime
{
    internal void IssueGoudeSlashUse(long useId, ProgramSkillFrame parent, CardActionContext action)
    {
        if (parent.Goude is not { Stage: GoudeStage.NativeSlash, Option: GoudeOption.Slash, NativeOrigin: null, TargetSeat: { } target } r)
            return;
        var useIndex = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == useId);
        if (useIndex < 1 || useIndex != _resolutionStackOperations._resolutionStack.Count - 1 || _resolutionStackOperations._resolutionStack[useIndex - 1] is not ProgramSkillFrame f || f != parent)
            throw new InvalidOperationException("Goude must issue its native Slash from the exact immediate original program parent.");
        if (f.Goude != r || f.InstructionIndex != 1 || _corePrivateOfferReturnsOperations.GetPausedPrivateOfferEffect(f)?.Op != SkillProgramEffectOp.ResolveGoudeActualTurnEnd || _coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { CardId: 0, CardKind: CardKind.Slash, PhysicalCardIds.Count: 0 } use || use.Action?.ActionId != action.ActionId || action.Type != CardActionType.Use || action.ActorSeat != f.OwnerSeat || action.ProviderSeat != f.OwnerSeat || action.PhysicalCards.Count != 0 || !action.TargetSeats.SequenceEqual([target]) || !action.ConversionChain.SequenceEqual([r.Source]) || _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeSlashIssuedEvent>().Any(e => e.Origin.ProgramFrameId == f.Id))
            throw new InvalidOperationException("Goude cannot bind an unrelated or duplicate native zero-entity Slash.");
        var origin = new GoudeSlashOrigin(f.Id, r.Source, r.GameplayHash, r.ActualTurn, r.TurnOwner, useId, action.ActionId, f.OwnerSeat, action.ProviderSeat, target);
        _coreRuntimeOperations.ReplaceRuntimeFrame(f.Id, f with { Goude = r with { NativeOrigin = origin } });
        _coreCardUseLifecycleOperations.UpdateLifecycleCardUse(useId, u => u with { GoudeSlash = origin });
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeSlashIssuedEvent(origin));
    }

    internal bool IsGoudeSlashUse(long useId)
    {
        if (_coreCardUseLifecycleOperations.LifecycleCardUse(useId)is not { GoudeSlash: { } o, CardId: 0, PhysicalCardIds.Count: 0, Action: { Type: CardActionType.Use } action } use || !global::CardGame.Core.EngineOwners.CoreNativeCardClassificationsRuntime.IsSlashCard(use.CardKind) || action.EffectiveKind != use.CardKind || action.ActionId != o.ActionId || action.ProviderSeat != o.ProviderSeat || action.PhysicalCards.Count != 0 || !action.ConversionChain.Contains(o.Source) || o.UseId != useId || o.Source.OwnerSeat != o.OriginalActor || o.OriginalActor != o.ProviderSeat || o.ActualTurn != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnNumber || o.TurnOwner != _coreActualTurnProgressTurnNumberTurnProgressionAccess._turnProgression.OwnerSeat || _contentRegistryOperations._contentRegistry.GetSkill(o.Source.SkillId).Program?.GameplayHash != o.GameplayHash || _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeSlashIssuedEvent>().Count(e => e.Origin == o) != 1)
            return false;
        var useIndex = _resolutionStackOperations._resolutionStack.FindIndex(x => x.Id == useId);
        if (useIndex < 1 || _resolutionStackOperations._resolutionStack[useIndex - 1] is not ProgramSkillFrame { Goude: { Stage: GoudeStage.NativeSlash, NativeCompleted: false } r } f || f.Id != o.ProgramFrameId || r.NativeOrigin != o || r.Source != o.Source || r.GameplayHash != o.GameplayHash || r.Option != GoudeOption.Slash || r.TargetSeat != o.OriginalTarget || f.InstructionIndex != 1)
            return false;
        var actor = o.OriginalActor;
        foreach (var changed in _coreProgramEventHistoryOperations.ProgramEventHistory<ProgramCardUseActorReplacedEvent>().Where(e => e.CardUseFrameId == useId))
        {
            if (changed.PreviousActorSeat != actor || changed.OwnerSeat != actor || changed.ProviderSeat != o.ProviderSeat || changed.FrameId <= useId || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(changed.ActorSeat) || changed.ActorSeat == actor || !_coreProgramEventHistoryOperations.ProgramEventHistory<ProgramBindingStartedEvent>().Any(e => e.FrameId == changed.FrameId && e.SkillId == changed.SkillId && e.OwnerSeat == changed.OwnerSeat && e.Window == SkillProgramTriggerWindow.CardUseTargetsFinalized))
                return false;
            actor = changed.ActorSeat;
        }

        return use.SourceSeat == actor && action.ActorSeat == actor;
    }

    [NativeUseValidation(27240)]
    internal void ValidateGoudeNativeUse(CardUseFrame use)
    {
        if (use.GoudeSlash is not null && !IsGoudeSlashUse(use.Id))
            throw new InvalidOperationException("Goude's native Slash lost its exact issuing parent, zero materials or real actor chain.");
    }

    [NativeUseReturn(27240)]
    internal void ReturnGoudeNativeUse(CardUseFrame use)
    {
        if (use.GoudeSlash is not { } o)
            return;
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(o.ProgramFrameId);
        if (f.Goude is not { Stage: GoudeStage.NativeSlash, NativeCompleted: false } r || r.NativeOrigin != o || use.Step != ResolutionFrameStep.Completed || _coreCardUseLifecycleOperations.LifecycleCardUse(use.Id)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == use.Id) != 1)
            throw new InvalidOperationException("Goude cannot return a native Slash before its whole genuine Use finishes.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Goude = r with { NativeCompleted = true } });
    }

    [NativeAttackReturn(NativeAttackReturnPhase.AfterBorrowedSword, 27240)]
    internal bool ReturnGoudeAttack(global::CardGame.Core.GameEngine.AttackCompletionReceipt completion)
    {
        if (_resolutionStackOperations._resolutionStack.LastOrDefault()is not ProgramSkillFrame { Goude: { Stage: GoudeStage.NativeSlash, NativeCompleted: true, NativeOrigin: { } o } r } f || completion.ResolutionId != o.UseId)
            return false;
        if (completion.ProgramFrameId != f.Id || _coreCardUseLifecycleOperations.LifecycleCardUse(o.UseId)is not null || _coreProgramEventHistoryOperations.ProgramEventHistory<CardUseFinishedEvent>().Count(e => e.ResolutionId == o.UseId) != 1 || _coreProgramEventHistoryOperations.ProgramEventHistory<GoudeSlashReturnedEvent>().Any(e => e.FrameId == f.Id))
            throw new InvalidOperationException("Goude lost its exact once-only native whole-Slash return.");
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new GoudeSlashReturnedEvent(f.Id, o.UseId));
        _coreRuntimeOperations.ReplaceRuntimeTop(f with { Goude = r with { Stage = GoudeStage.NativeChildren }, PendingMovementContinuation = new(f.OwnerSeat, 0, null) });
        ContinueMengDa(f.Id);
        return true;
    }

    internal bool HasGoudeCompletedSlashChild(ProgramCardTriggerWindowFrame window)
    {
        if (window.Continuation != ProgramCardContinuation.CompletedSlash || _coreCardUseLifecycleOperations.LifecycleCardUse(window.ParentFrameId)is not { GoudeSlash: { } o } use || !IsGoudeSlashUse(use.Id) || window.Action.ActionId != o.ActionId)
            return false;
        var at = _resolutionStackOperations._resolutionStack.FindIndex(f => f.Id == window.Id);
        return at > 1 && _resolutionStackOperations._resolutionStack[at - 1].Id == use.Id && _resolutionStackOperations._resolutionStack[at - 2] is ProgramSkillFrame { Goude.NativeOrigin: { } origin } f && f.Id == o.ProgramFrameId && origin == o;
    }
}
