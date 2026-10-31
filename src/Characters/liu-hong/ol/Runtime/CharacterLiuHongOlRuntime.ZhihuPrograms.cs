#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterLiuHongOlRuntime
{
    internal static string ZhihuGrantSourcePrefix => "acquired:zhihu:";

    internal SkillProgramStepOutcome ZhihuProgramExpire(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var active = _coreProgramLifecycleOperations.GetActiveProgramFrame(frame.Id);
        var holder = _coreMatchParticipantsPlayersAccess._players[active.OwnerSeat];
        if (_coreMatchOutcomeStatusWinnerAccess._winner != Winner.None || !holder.IsAlive)
            return SkillProgramStepOutcome.Continue;
        foreach (var grant in holder.SkillGrants.Grants.Where(grant => grant.IsEnabled && grant.SourceId.StartsWith(ZhihuGrantSourcePrefix, StringComparison.Ordinal)).ToArray())
        {
            var segment = grant.SourceId[ZhihuGrantSourcePrefix.Length..];
            var separator = segment.IndexOf(':');
            if (separator > 0 && int.TryParse(segment[..separator], System.Globalization.CultureInfo.InvariantCulture, out var grantor) && grantor == _coreActualTurnProgressCurrentSeatPhaseAccess._currentSeat)
            {
                holder.SkillGrants.RemoveGrant(grant.GrantId);
                _coreMatchLogOperations.AddLog("SkillEffect", $"{(holder.Name)} 的“执笏”随 {(_coreMatchParticipantsPlayersAccess._players[grantor].Name)} 的回合开始而失效。", holder.Seat);
            }
        }

        return SkillProgramStepOutcome.Continue;
    }
}
