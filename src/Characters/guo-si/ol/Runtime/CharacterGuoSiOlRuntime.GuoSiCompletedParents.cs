#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterGuoSiOlRuntime
{
    internal bool IsSuspendedGuoSiCompletedParent(CardUseFrame original)
    {
        if (original.Step != ResolutionFrameStep.Completed || !HasGuoSiCapability || original.Action is not { Type: CardActionType.Use } action)
            return false;
        var index = _resolutionStackOperations._resolutionStack.FindIndex(frame => frame.Id == original.Id);
        if (index < 0 || index + 2 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 1] is not ProgramCardTriggerWindowFrame window || _resolutionStackOperations._resolutionStack[index + 2] is not ProgramSkillFrame program || window.ParentFrameId != original.Id || window.Continuation is not (ProgramCardContinuation.CompletedCard or ProgramCardContinuation.CompletedSlash) || window.CompletedResponseReturn is not null || !window.Activated || !CoreGameEngineOverflowTargetCancellationObserversRuntime.SameOverflowAction(window.Action, action) || window.CandidateIndex < 0 || window.CandidateIndex >= window.Candidates.Count || program.WindowContext is not { Window: SkillProgramTriggerWindow.CardUseCompleted, CardUse: { } identity } context || context.ParentFrameId != window.Id || identity.ParentCardUseFrameId != original.Id || identity.CardActionId != action.ActionId || identity.ActorSeat != program.OwnerSeat || action.ActorSeat != program.OwnerSeat || program.GuoSi is not { Native: { } receipt } pending || receipt.ParentActionId != action.ActionId)
            return false;
        var candidate = window.Candidates[window.CandidateIndex];
        if (candidate.OwnerSeat != program.OwnerSeat || candidate.SkillId != program.SkillId || candidate.SkillInstanceId != program.SkillInstanceId || candidate.TriggerId != program.TriggerId || candidate.GameplayHash != program.GameplayHash || _contentRegistryOperations._contentRegistry.GetSkill(program.SkillId).Program is not { } definition || definition.GameplayHash != program.GameplayHash || program.InstructionIndex < 1 || ProgramInstructionResolver.Default.Resolve(program, definition).GetPausedInstruction(program.InstructionIndex).Effect.Op != SkillProgramEffectOp.OfferSidaoCompletedPairSnatch || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == original.Id && fact.CardId == original.CardId && fact.CardKind == original.CardKind) != 1 || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuoSiNativeIssuedEvent>().Count(fact => fact.Receipt == receipt) != 1)
            return false;
        RequireIssuedGuoSi(program);
        if (pending.Stage == GuoSiStage.Completed)
        {
            // Native completion returns before its producer's call stack exits.
            // The owning binding drains on the next resume, with no live child.
            return index + 3 == _resolutionStackOperations._resolutionStack.Count && !_resolutionStackOperations._resolutionStack.OfType<CardUseFrame>().Any(frame => frame.Id == receipt.NativeUseId) && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<GuoSiNativeReturnedEvent>().Count(fact => fact.Receipt == receipt) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseFinishedEvent>().Count(fact => fact.ResolutionId == receipt.NativeUseId) == 1 && _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardUseDeclaredEvent>().Count(fact => fact.ResolutionId == receipt.NativeUseId && fact.SourceSeat == program.OwnerSeat && fact.CardId == receipt.Material && fact.CardKind == CardKind.Snatch) == 1 && ((!_coreMatchParticipantsPlayersAccess._players[program.OwnerSeat].IsAlive || _coreMatchOutcomeStatusWinnerAccess._winner != Winner.None) || _coreAlternatingBenefitsOperations.CompleteProgramEventHistory().OfType<CardActionAcceptedEvent>().Any(fact => fact.Action.ActionId == receipt.ActionId && fact.Action.ParentActionId == action.ActionId && fact.Action.Type == CardActionType.Use && fact.Action.EffectiveKind == CardKind.Snatch && fact.Action.ActorSeat == program.OwnerSeat && fact.Action.ProviderSeat == program.OwnerSeat && fact.Action.PhysicalCards is [var cost] && cost.CardId == receipt.Material && cost.From == CardLocation.Hand(program.OwnerSeat) && fact.Action.ConversionChain.SequenceEqual([receipt.Source])));
        }

        if (index + 3 >= _resolutionStackOperations._resolutionStack.Count || _resolutionStackOperations._resolutionStack[index + 3] is not CardUseFrame child || !GuoSiNativeRidesOn(program, child) || child.Action is not { Type: CardActionType.Use } native || native.ParentActionId != action.ActionId || native.ActionId == action.ActionId || native.EffectiveKind != CardKind.Snatch || child.GuoSiNative != receipt)
            return false;
        AssertGuoSiNativeUse(child);
        return true;
    }
}
