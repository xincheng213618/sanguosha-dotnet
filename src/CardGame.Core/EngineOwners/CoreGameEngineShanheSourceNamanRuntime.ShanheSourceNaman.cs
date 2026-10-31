#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineShanheSourceNamanRuntime
{
    SkillProgramStepOutcome IShanheSourceNamanHost.Claim(ProgramSkillFrame frame) => ClaimShanheSourceNamanResponseCards(frame);
    internal ProgramCardTriggerWindowFrame? ShanheSourceNamanResponse(ProgramSkillWindowContext? context, int ownerSeat)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardResponseCompleted, CardUse: { } identity } || _resolutionStackOperations._resolutionStack.SingleOrDefault(frame => frame.Id == context.ParentFrameId)is not ProgramCardTriggerWindowFrame window || window.ResponseCompletion is not { CostsDrained: true } completion || !_coreResponseCompletionOperations.ValidResponseCompletionWindow(window) || window.Action is not { Type: CardActionType.Response } action || !CoreNativeCardClassificationsRuntime.IsSlashCard(action.EffectiveKind) || completion.CompletionActorSeat == ownerSeat || identity.CardActionId != action.ActionId || identity.ParentCardUseFrameId != window.ParentFrameId)
            return null;
        return window;
    }

    internal int[] ShanheSourceNamanMaterials(ProgramSkillWindowContext? context, int ownerSeat)
    {
        if (ShanheSourceNamanResponse(context, ownerSeat)is not { ResponseCompletion: { } completion, Action: { } action })
            return[];
        // The original completed-response receipt proves the exact final native
        // movements. Printed card kinds cannot identify a converted Slash.
        var finished = completion.NativeCosts.Where(move => move.From == CardLocation.Processing && move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.ResponseFinished).Select(move => move.CardId).ToHashSet();
        return action.PhysicalCards.Where(cost => finished.Contains(cost.CardId) && _cardZonesOperations._cardZones.GetLocation(cost.CardId) == CardLocation.DiscardPile).Select(cost => cost.CardId).Distinct().ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 23130, SkillProgramEffectOp.ClaimShanheSourceNamanResponseCards)]
    internal bool CanOfferShanheSourceNaman(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) => ShanheSourceNamanMaterials(context, candidate.OwnerSeat).Length > 0;

    internal SkillProgramStepOutcome ClaimShanheSourceNamanResponseCards(ProgramSkillFrame supplied)
    {
        var frame = _coreProgramLifecycleOperations.GetActiveProgramFrame(supplied.Id);
        if (_coreMatchOutcomeWinnerAccess._winner != Winner.None || !_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat].IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var materials = ShanheSourceNamanMaterials(frame.WindowContext, frame.OwnerSeat);
        if (materials.Length == 0)
            return SkillProgramStepOutcome.Continue;
        _corePhysicalMovementFacadesOperations.MoveCards(materials.Select(_coreCardAttackStateOperations.GetAttackCard).ToArray(), CardLocation.DiscardPile, CardLocation.Hand(frame.OwnerSeat), new("skill-program." + frame.SkillId + ".naman-response-claim"));
        return _coreBoundCardMovementContinuationOperations.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }
}
