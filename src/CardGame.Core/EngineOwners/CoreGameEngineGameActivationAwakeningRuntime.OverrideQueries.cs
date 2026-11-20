#nullable enable

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreGameEngineGameActivationAwakeningRuntime
{
    public bool Granted(int owner, string skill, string? binding = null, string? instanceId = null) =>
        _awakeningContent.FindSkill(skill) is { Program.GameplayHash: var hash } definition &&
        ProgramAwakeningBindings.For(definition).Any(trigger => binding is null || trigger.Id == binding) &&
        _awakeningHistory.Read<ShenGuoJiaAwakeningOverrideEvent>().Any(fact =>
            fact.Recipient == owner && fact.SkillId == skill && hash == fact.GameplayHash &&
            (instanceId is null || fact.InstanceId == instanceId) &&
            _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[owner], skill, fact.InstanceId));

    public bool Issued(ProgramSkillFrame frame) =>
        _awakeningContent.FindSkill(frame.SkillId) is { Program.GameplayHash: var hash } definition &&
        hash == frame.GameplayHash &&
        ProgramAwakeningBindings.For(definition)
            .Any(trigger => trigger.Id == CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame)) &&
        _awakeningHistory.Read<ShenGuoJiaAwakeningOverrideEvent>().Any(fact => fact.FrameId < frame.Id &&
            fact.Recipient == frame.OwnerSeat && fact.SkillId == frame.SkillId && fact.InstanceId == frame.SkillInstanceId &&
            fact.GameplayHash == frame.GameplayHash);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AwakeningConditionOverride, 38000)]
    internal bool HasAwakeningOverride(ProgramTriggerLegalityContext gate) =>
        Granted(gate.OwnerSeat, gate.SkillId, gate.Candidate.BindingId, gate.Candidate.SkillInstanceId);
}
