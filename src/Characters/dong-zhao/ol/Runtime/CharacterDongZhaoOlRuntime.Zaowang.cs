#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;
using System.Globalization;
using System.Text.Json.Serialization;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterDongZhaoOlRuntime
{
    internal SkillProgramStepOutcome ArmZaowang(ProgramSkillFrame input)
    {
        var f = _coreProgramLifecycleOperations.GetActiveProgramFrame(input.Id);
        if (f.SelectedTargetSeats is not [var seat] || !_coreMatchParticipantsPlayersAccess._players[seat].IsAlive)
            throw new InvalidOperationException("Zaowang needs one living recipient.");
        var target = _coreMatchParticipantsPlayersAccess._players[seat];
        if (f.ZaowangPaidTargetSeat is not null)
            throw new InvalidOperationException("Zaowang cannot pay twice.");
        _coreRuntimeOperations.ReplaceRuntimeTop(f = f with { ZaowangPaidTargetSeat = seat });
        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.ModeKind == ContentModeKind.Identity)
            target.AddZaowangGrant(new(f.Id, f.OwnerSeat, seat, target.Role, f.SkillInstanceId, f.GameplayHash));
        target.MaxHp = checked(target.MaxHp + 1);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new MaximumHpChangedEvent(seat, 1, target.MaxHp, f.SkillId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new ZaowangGrantedEvent(f.Id, f.OwnerSeat, seat, f.SkillId, f.SkillInstanceId, f.GameplayHash, target.Hp, target.MaxHp));
        return SkillProgramStepOutcome.Continue;
    }

    internal void ApplyZaowangSuccession(long deathId, CharacterState victim)
    {
        if (_coreMatchDefinitionModeDefinitionAccess._modeDefinition.ModeKind != ContentModeKind.Identity || victim.Role != Role.Lord)
            return;
        // Multiple eligible heirs resolve in seat order, the normal trigger order.
        var successor = _coreMatchParticipantsPlayersAccess._players.Where(p => p.IsAlive && p.Role == Role.Loyalist && p.ZaowangGrants.Any(g => g.OriginalRole == Role.Loyalist)).OrderBy(p => p.Seat).FirstOrDefault();
        if (successor is null)
            return;
        victim.ExchangeIdentityWith(successor);
        successor.RoleRevealed = true;
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new IdentityExchangedEvent(deathId, victim.Seat, successor.Seat));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoleRevealedEvent(victim.Seat, victim.Role));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new RoleRevealedEvent(successor.Seat, successor.Role));
    }

    internal bool ZaowangRebelDeathWins(CharacterState victim, CharacterState? killer) => _coreMatchDefinitionModeDefinitionAccess._modeDefinition.ModeKind == ContentModeKind.Identity && victim.Role == Role.Rebel && victim.ZaowangGrants.Any(g => g.OriginalRole == Role.Rebel) && killer is { Role: Role.Lord or Role.Loyalist };
}
