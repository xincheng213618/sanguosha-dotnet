#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CharacterZhaoZhongOlRuntime
{
    internal void GrantPengbiSkill(ProgramSkillFrame f, int recipient, string skill)
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            _observedSkillGrantRevisionsOperations._observedSkillGrantRevisions.TryAdd(player.Seat, player.SkillGrants.GrantRevision);
        var sourceId = $"zhao-zhong.pengbi:{(f.Id)}:{(recipient)}";
        var grantId = sourceId + ":" + skill;
        _coreMatchParticipantsPlayersAccess._players[recipient].SkillGrants.Grant(new(grantId, skill, grantId, sourceId));
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new PengbiSkillGrantedEvent(f.Id, recipient, skill, grantId, grantId, sourceId));
    }
}
