#nullable enable

namespace CardGame.Core.EngineOwners;

internal sealed partial class CoreGameEngineGameActivationAwakeningRuntime
{
    public bool Granted(int owner, string skill, string? binding = null) =>
        _awakeningContent.FindSkill(skill) is { } definition && definition.Tags.HasFlag(SkillTag.Awakening) &&
        (binding is null || IsAwakeningBinding(definition.Program!, binding)) &&
        _awakeningHistory.Read<ShenGuoJiaAwakeningOverrideEvent>().Any(fact =>
            fact.Recipient == owner && fact.SkillId == skill && definition.Program?.GameplayHash == fact.GameplayHash &&
            _coreNationalSkillsOperations.HasRuntimeSkillInstance(_coreMatchParticipantsPlayersAccess._players[owner], skill, fact.InstanceId));

    public bool Issued(ProgramSkillFrame frame) =>
        _awakeningContent.FindSkill(frame.SkillId) is { } definition && definition.Tags.HasFlag(SkillTag.Awakening) &&
        definition.Program?.GameplayHash == frame.GameplayHash &&
        IsAwakeningBinding(definition.Program!, CoreGameEngineProgramLifecycleRuntime.GetProgramBindingId(frame)) &&
        _awakeningHistory.Read<ShenGuoJiaAwakeningOverrideEvent>().Any(fact => fact.FrameId < frame.Id &&
            fact.Recipient == frame.OwnerSeat && fact.SkillId == frame.SkillId && fact.InstanceId == frame.SkillInstanceId &&
            fact.GameplayHash == frame.GameplayHash);

    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.AwakeningConditionOverride, 38000)]
    internal bool HasAwakeningOverride(ProgramTriggerLegalityContext gate) =>
        Granted(gate.OwnerSeat, gate.SkillId, gate.Candidate.BindingId);

    private static bool IsAwakeningBinding(SkillProgram program, string binding) => program.Triggers.Any(trigger =>
        string.Equals(trigger.Id, binding, StringComparison.Ordinal) && trigger.Effects.Any(effect => effect.Op is
            SkillProgramEffectOp.ChangeMaximumHp or SkillProgramEffectOp.GrantSkills or
            SkillProgramEffectOp.ReplaceSkillsOnAwakening or SkillProgramEffectOp.ResolveMouJiangWeiZhiji or
            SkillProgramEffectOp.ResolveMouCaoShuangFanshi || effect.Op.ToString().Contains("Awaken", StringComparison.Ordinal)));
}
