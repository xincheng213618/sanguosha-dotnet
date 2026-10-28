namespace CardGame.Core;

public sealed partial class GameEngine
{
    [ProgramCapability]
    private sealed class ShanheSourceLiuSandaoHost(GameEngine engine) : IShanheSourceLiuSandaoHost
    {
        public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) =>
            engine.ResolveShanheSourceLiuSandao(frame, operation);
    }

    private int SourceHuchenEnemyKills(int ownerSeat) => CompleteProgramEventHistory().OfType<PlayerDiedEvent>()
        .Count(fact => fact.KillerSeat == ownerSeat && IsValidPlayerSeat(fact.VictimSeat) && PublicTeamEnemy(ownerSeat, _players[fact.VictimSeat]));

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24790,
        SkillProgramEffectOp.AdjustShanheSourceHuchenDraw)]
    private bool CanOfferSourceHuchen(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context) =>
        context.Window == SkillProgramTriggerWindow.DrawPhaseStarting && context.OwnerSeat == candidate.OwnerSeat &&
        candidate.OwnerSeat == _currentSeat && SourceHuchenEnemyKills(candidate.OwnerSeat) > 0;

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 24791,
        SkillProgramEffectOp.DiscardShanheSourceZhanshenWeapons)]
    private bool CanOfferSourceZhanshen(ProgramTriggerCandidate candidate, SkillProgramTrigger trigger, ProgramSkillWindowContext context)
    {
        var owner = _players[candidate.OwnerSeat];
        return context.Window == SkillProgramTriggerWindow.TurnStartBeforeNormalFlow && context.OwnerSeat == candidate.OwnerSeat &&
            context.SourceSeat == candidate.OwnerSeat && candidate.OwnerSeat == _currentSeat && owner.IsAlive && owner.HasHpValue &&
            owner.Hp < owner.MaxHp && _players.Any(player => !player.IsAlive && OtherRevealedPublicTeamAlly(owner, player));
    }

    private SkillProgramStepOutcome ResolveShanheSourceLiuSandao(ProgramSkillFrame supplied, SkillProgramEffectOp operation)
    {
        var frame = GetActiveProgramFrame(supplied.Id);
        if (operation == SkillProgramEffectOp.AdjustShanheSourceHuchenDraw)
        {
            var count = SourceHuchenEnemyKills(frame.OwnerSeat);
            if (count > 0) AdjustProgramNormalDraw(frame, count);
            return SkillProgramStepOutcome.Continue;
        }
        if (operation != SkillProgramEffectOp.DiscardShanheSourceZhanshenWeapons || frame.InstructionIndex != 2 ||
            !ExactGameActivationAwakeningParent(frame))
            throw new InvalidOperationException("Source Zhanshen lost its actual paid Preparation awakening.");
        var weapons = GetEquipment(_players[frame.OwnerSeat]).Where(card => FieldEquipmentSlot(card) == EquipmentSlot.Weapon).ToArray();
        if (weapons.Length == 0) return SkillProgramStepOutcome.Continue;
        MoveCards(weapons, CardLocation.Equipment(frame.OwnerSeat), CardLocation.DiscardPile,
            new("skill-program." + frame.SkillId + ".awakening-weapon-discard"));
        return AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
    }

    [ProgramMovementProof(ProgramMovementProofPhase.EffectPayments, 24791)]
    private bool SourceZhanshenWeaponMovement(ProgramSkillFrame frame, SkillProgramEffect? effect, ProgramMovementContinuation movement)
    {
        if (effect?.Op != SkillProgramEffectOp.DiscardShanheSourceZhanshenWeapons || movement != frame.PendingMovementContinuation ||
            movement.SubjectSeat != frame.OwnerSeat || movement.BeforeCount != 0 || movement.CoverageResultBind is not null ||
            frame.InstructionIndex != 2 || !ExactGameActivationAwakeningParent(frame)) return false;
        var batches = _resolutionStack.OfType<CardsMovedTriggerWindowFrame>().Select(child => child.Batch)
            .Concat(_pendingCardsMovedBatches).Where(batch => batch.ParentFrameId == frame.Id &&
                batch.OriginOwnerSeat == frame.OwnerSeat && batch.OriginSkillId == frame.SkillId &&
                batch.OriginSkillInstanceId == frame.SkillInstanceId && batch.Movements.Count > 0 &&
                batch.Movements.All(move => move.Reason.Value == "skill-program." + frame.SkillId + ".awakening-weapon-discard"))
            .DistinctBy(batch => batch.Id).ToArray();
        return batches is [var paid] && paid.Movements.All(move => _cardMovements.Contains(move) &&
            move.From == CardLocation.Equipment(frame.OwnerSeat) && move.To == CardLocation.DiscardPile &&
            FieldEquipmentSlot(GetAttackCard(move.CardId)) == EquipmentSlot.Weapon);
    }
}
