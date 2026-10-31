#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineProgramBooleanStateExpirationRuntime
{
    internal void ScheduleProgramBooleanStateExpiration(ProgramSkillFrame frame, IReadOnlyList<string> stateIds, SkillProgramDamageModifierExpiration expiration = SkillProgramDamageModifierExpiration.CurrentTurnEnd)
    {
        _coreCardUseModulesOperations.ValidateProgramTurnEffectGrant(frame);
        if (!Enum.IsDefined(expiration))
            throw new InvalidOperationException("Unknown boolean state expiration.");
        var source = global::CardGame.Core.EngineOwners.CoreGameEngineCardUseModulesRuntime.CreateProgramTurnEffectSource(frame);
        foreach (var stateId in stateIds.Distinct(StringComparer.Ordinal))
        {
            _ = _coreProgramPersistentStateOperations.GetProgramBooleanStateDefinition(frame, stateId);
            _coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess._programBooleanStateLeases.RemoveAll(item => item.Source.OwnerSeat == source.OwnerSeat && item.Source.SkillId == source.SkillId && item.Source.SkillInstanceId == source.SkillInstanceId && item.StateId == stateId);
            _coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess._programBooleanStateLeases.Add(new(source, stateId, _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber, _coreActualTurnProgressCurrentSeatTurnNumberAccess._currentSeat, expiration));
        }
    }

    internal void ExpireProgramBooleanStateLeases(int turnNumber, int turnSeat) => ExpireProgramBooleanStateLeases(item => item.Expiration == SkillProgramDamageModifierExpiration.CurrentTurnEnd && item.TurnNumber == turnNumber && item.TurnSeat == turnSeat);
    internal void ExpireProgramBooleanStateLeasesAtOwnerTurnStart(int ownerSeat, bool ownerDied = false) => ExpireProgramBooleanStateLeases(item => item.Source.OwnerSeat == ownerSeat && (ownerDied || item.Expiration == SkillProgramDamageModifierExpiration.NextOwnerTurnStart && item.TurnNumber < _coreActualTurnProgressCurrentSeatTurnNumberAccess._turnNumber));
    internal void ExpireProgramBooleanStateLeases(Func<global::CardGame.Core.GameEngine.ProgramBooleanStateLease, bool> expires)
    {
        var expired = _coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess._programBooleanStateLeases.Where(expires).ToArray();
        _coreGameEngineProgramBooleanStateExpirationProgramBooleanStateLeasesAccess._programBooleanStateLeases.RemoveAll(item => expires(item));
        foreach (var item in expired)
            _coreProgramPersistentStateOperations.SetProgramBooleanState(item.Source.OwnerSeat, item.Source.SkillId, item.Source.SkillInstanceId, item.StateId, false);
    }
}
