namespace CardGame.Core;

public sealed partial class GameEngine
{
    // The caller owns qualification, payment, card cleanup, HP amount and any
    // post-revival draw. Restoring life itself is not a Recovery event.
    private bool RestoreProgramCharacterLife(ProgramSkillFrame frame, CharacterState target, int hp)
    {
        if (target.IsAlive || !target.HasHpValue || target.MaxHp <= 0 || hp <= 0 || hp > target.MaxHp)
            throw new InvalidOperationException("Life restoration requires one dead character and positive bounded HP.");
        var wasFaceDown = target.IsFaceDown;
        target.IsAlive = true;
        target.Hp = hp;
        target.IsFaceDown = false;
        target.AlcoholDamageBonus = 0;
        target.UsedPlayPhaseAlcoholThisTurn = false;
        if (target.IsChained)
        {
            target.IsChained = false;
            AdvanceEventRulesAndQueueFact(new IronChainStateChangedEvent(frame.Id, frame.OwnerSeat, target.Seat, false));
        }
        return wasFaceDown;
    }

    private void PublishProgramRevivalFaceState(CharacterState target, bool wasFaceDown)
    {
        if (wasFaceDown) RecordCharacterStateChange(target.Seat, SkillProgramTriggerWindow.CharacterTurnedFaceUp);
        RecordCharacterTurnedOver(target.Seat, wasFaceDown);
    }
}
