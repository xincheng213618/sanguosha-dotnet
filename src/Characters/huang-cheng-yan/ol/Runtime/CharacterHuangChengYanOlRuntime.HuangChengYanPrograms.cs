#nullable enable
using global::System;
using global::System.Collections.Generic;
using global::System.Linq;
using global::CardGame.Core;

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterHuangChengYanOlRuntime
{
    internal string GetSkillDisplayName(string skillId)
    {
        try
        {
            return _contentRegistryOperations._contentRegistry!.GetSkill(skillId).Name;
        }
        catch (InvalidOperationException)
        {
            return skillId;
        }
    }

    internal void PurgeExpiredRoundGrants()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            foreach (var grant in player.SkillGrants.Grants.Where(grant => grant.SourceId.StartsWith("acquired:round:", StringComparison.Ordinal)).ToArray())
            {
                var segment = grant.SourceId["acquired:round:".Length..];
                var separator = segment.IndexOf(':');
                if (separator > 0 && int.TryParse(segment[..separator], System.Globalization.CultureInfo.InvariantCulture, out var round) && round < _coreActualRoundProgressRoundNumberAccess._roundNumber)
                    player.SkillGrants.RemoveGrant(grant.GrantId);
            }
    }

    SkillProgramStepOutcome global::CardGame.Core.IHuangChengYanProgramHost.JiezhenReplaceSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => JiezhenProgramReplaceSkills(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IHuangChengYanProgramHost.JiezhenRestoreSkills(ProgramSkillFrame frame, SkillProgramEffect effect) => JiezhenProgramRestoreSkills(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IHuangChengYanProgramHost.ZecaiRoundSettlement(ProgramSkillFrame frame, SkillProgramEffect effect) => ZecaiProgramRoundSettlement(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IHuangChengYanProgramHost.YinshiPreventSourcelessDamage(ProgramSkillFrame frame, SkillProgramEffect effect) => YinshiProgramPreventSourcelessDamage(frame, effect);
    SkillProgramStepOutcome global::CardGame.Core.IHuangChengYanProgramHost.YinshiClaimBaguaJudgmentCard(ProgramSkillFrame frame, SkillProgramEffect effect) => YinshiProgramClaimBaguaJudgmentCard(frame, effect);
}
