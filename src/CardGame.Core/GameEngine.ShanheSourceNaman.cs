namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheSourceNamanHost(GameEngine engine) : IShanheSourceNamanHost
    {
        public SkillProgramStepOutcome Claim(ProgramSkillFrame frame) => engine.ClaimShanheSourceNamanResponseCards(frame);
    }

    private ProgramCardTriggerWindowFrame? ShanheSourceNamanResponse(ProgramSkillWindowContext? context, int ownerSeat)
    {
        if (context is not { Window: SkillProgramTriggerWindow.CardResponseCompleted, CardUse: { } identity } ||
            _resolutionStack.SingleOrDefault(frame => frame.Id == context.ParentFrameId) is not ProgramCardTriggerWindowFrame window ||
            window.ResponseCompletion is not { CostsDrained: true } completion || !ValidResponseCompletionWindow(window) ||
            window.Action is not { Type: CardActionType.Response } action || !IsSlashCard(action.EffectiveKind) ||
            completion.CompletionActorSeat == ownerSeat || identity.CardActionId != action.ActionId ||
            identity.ParentCardUseFrameId != window.ParentFrameId) return null;
        return window;
    }

    private int[] ShanheSourceNamanMaterials(ProgramSkillWindowContext? context, int ownerSeat)
    {
        if (ShanheSourceNamanResponse(context, ownerSeat) is not { ResponseCompletion: { } completion, Action: { } action })
            return [];

        // The original completed-response receipt proves the exact final native
        // movements. Printed card kinds cannot identify a converted Slash.
        var finished = completion.NativeCosts.Where(move => move.From == CardLocation.Processing &&
                move.To == CardLocation.DiscardPile && move.Reason == CardMoveReasons.ResponseFinished)
            .Select(move => move.CardId).ToHashSet();
        return action.PhysicalCards.Where(cost => finished.Contains(cost.CardId) &&
                _cardZones.GetLocation(cost.CardId) == CardLocation.DiscardPile)
            .Select(cost => cost.CardId).Distinct().ToArray();
    }

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 23130,
        SkillProgramEffectOp.ClaimShanheSourceNamanResponseCards)]
    private bool CanOfferShanheSourceNaman(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger,
        ProgramSkillWindowContext context) => ShanheSourceNamanMaterials(context, candidate.OwnerSeat).Length > 0;

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 23130)]
    private bool IsShanheSourceNamanMovement(ProgramSkillFrame frame, SkillProgramEffect? paidEffect,
        ProgramMovementContinuation movement)
    {
        if (paidEffect?.Op != SkillProgramEffectOp.ClaimShanheSourceNamanResponseCards ||
            movement != frame.PendingMovementContinuation || movement.SubjectSeat != frame.OwnerSeat ||
            movement.BeforeCount != 0 || movement.CoverageResultBind is not null ||
            ShanheSourceNamanResponse(frame.WindowContext, frame.OwnerSeat) is not
                { ResponseCompletion: { } completion, Action: { } action }) return false;
        var batches = _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(child => child.Batch)
            .Concat(_pendingCardsMovedBatches).Where(batch => batch.ParentFrameId == frame.Id &&
                batch.OriginOwnerSeat == frame.OwnerSeat && batch.OriginSkillId == frame.SkillId &&
                batch.OriginSkillInstanceId == frame.SkillInstanceId && batch.Movements.Count > 0 &&
                batch.Movements.All(move => move.Reason.Value == "skill-program." + frame.SkillId + ".naman-response-claim"))
            .DistinctBy(batch => batch.Id).ToArray();
        return batches is [var paid] && paid.Movements.Select(move => move.CardId).Distinct().Count() == paid.Movements.Count &&
            paid.Movements.All(move => _cardMovements.Contains(move) && move.From == CardLocation.DiscardPile &&
                move.To == CardLocation.Hand(frame.OwnerSeat) && action.PhysicalCards.Any(cost => cost.CardId == move.CardId) &&
                completion.NativeCosts.Any(cost => cost.CardId == move.CardId && cost.Sequence < move.Sequence &&
                    cost.From == CardLocation.Processing && cost.To == CardLocation.DiscardPile && cost.Reason == CardMoveReasons.ResponseFinished));
    }

    private SkillProgramStepOutcome ClaimShanheSourceNamanResponseCards(ProgramSkillFrame supplied)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (_winner != Winner.None || !_players[frame.OwnerSeat].IsAlive ||
            !HasRuntimeSkillInstance(_players[frame.OwnerSeat], frame.SkillId, frame.SkillInstanceId))
            return SkillProgramStepOutcome.Continue;
        var materials = ShanheSourceNamanMaterials(frame.WindowContext, frame.OwnerSeat);
        if (materials.Length == 0) return SkillProgramStepOutcome.Continue;
        MoveCards(materials.Select(GetAttackCard).ToArray(), CardLocation.DiscardPile, CardLocation.Hand(frame.OwnerSeat),
            new("skill-program." + frame.SkillId + ".naman-response-claim"));
        return AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }
}
