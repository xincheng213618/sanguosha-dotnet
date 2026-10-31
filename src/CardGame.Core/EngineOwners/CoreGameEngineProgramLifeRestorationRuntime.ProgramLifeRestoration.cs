#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramLifeRestorationRuntime
{
    internal bool RestoreProgramCharacterLife(ProgramSkillFrame frame, CharacterState target, int hp)
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
            _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IronChainStateChangedEvent(frame.Id, frame.OwnerSeat, target.Seat, false));
        }

        return wasFaceDown;
    }

    internal void PublishProgramRevivalFaceState(CharacterState target, bool wasFaceDown)
    {
        if (wasFaceDown)
            _coreCharacterStateProgramsOperations.RecordCharacterStateChange(target.Seat, SkillProgramTriggerWindow.CharacterTurnedFaceUp);
        _coreCharacterTurnedOverProgramsOperations.RecordCharacterTurnedOver(target.Seat, wasFaceDown);
    }
}
