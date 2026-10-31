#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreProgramLoseDeathSourceSkillsOperationRuntime
{
    internal void LoseProgramDeathSourceSkills(ProgramSkillFrame frame)
    {
        var context = frame.WindowContext;
        if (context?.Window != SkillProgramTriggerWindow.OwnerDied)
            throw new InvalidOperationException("Death source skill loss requires an owner-death window.");
        if (context.SourceSeat is not { } sourceSeat || !_coreTableSeatQueriesOperations.IsValidPlayerSeat(sourceSeat))
            return;
        var source = _coreMatchParticipantsPlayersAccess._players[sourceSeat];
        var lost = source.SkillGrants.Grants.Where(grant => grant.IsEnabled && !grant.IsModeRule && !grant.SourceId.StartsWith("equipment:", StringComparison.Ordinal)).ToArray();
        foreach (var grant in lost)
            source.SkillGrants.SetEnabled(grant.GrantId, false);
        _kernelCoreGameEngineRuleProgressionOperations.AdvanceEventRulesAndQueueFact(new CharacterSkillsLostEvent(sourceSeat, frame.OwnerSeat, Array.AsReadOnly(lost.Select(grant => grant.SkillId).Distinct(StringComparer.Ordinal).ToArray())));
        _coreMatchLogOperations.AddLog("SkillTriggered", $"{(source.Name)} 受到【断肠】影响，失去所有武将技能。", frame.OwnerSeat, sourceSeat);
    }
}
