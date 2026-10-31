#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhangChangPuOlRuntime
{
    internal void ShenShenDrawAndArmBonus(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var owner = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !owner.IsAlive || !_coreNationalSkillsOperations.HasRuntimeSkillInstance(owner, active.SkillId, active.SkillInstanceId))
            return;
        var alive = _coreMatchParticipantsPlayersAccess._players.Where(player => player.IsAlive).ToArray();
        var minimumHand = alive.Min(player => _corePhysicalOwnedCardQueriesOperations.GetHand(player).Count);
        _corePhysicalDrawFacadesOperations.DrawCards(owner, _corePhysicalOwnedCardQueriesOperations.GetHand(owner).Count == minimumHand ? 2 : 1, true, new($"skill-program.{(active.SkillId)}.shengshen-draw"));
        var minimumHp = alive.Min(player => Math.Max(0, player.Hp));
        var marker = effect.Marker!.Value;
        var current = owner.Markers.GetValueOrDefault(marker);
        var armed = Math.Min(4, current + (Math.Max(0, owner.Hp) == minimumHp ? 2 : 1));
        if (armed != current)
            _coreAttributedMarkerEventsOperations.MutateParticipantMarker(_coreProgramLifecycleOperations.GetActiveProgramFrame(active.Id), active.OwnerSeat, marker, armed - current);
    }
}
