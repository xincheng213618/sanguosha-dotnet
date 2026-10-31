#nullable enable

namespace CardGame.Core.EngineOwners;
internal sealed partial class CoreGameEngineConversionStatesRuntime
{
    internal void InitializeStructuredConversionSkills()
    {
        foreach (var player in _coreMatchParticipantsPlayersAccess._players)
            foreach (var skillId in _coreNationalSkillsOperations.EnabledContentSkillIds(player))
                RegisterTaggedConversionSkill(player, skillId);
    }

    internal void RegisterTaggedConversionSkill(CharacterState player, string skillId)
    {
        var definition = _contentRegistryOperations._contentRegistry.GetSkill(skillId);
        var program = definition.Program;
        var optIn = program is not null && (program.Activations.Any(binding => ProgramInstructionResolver.Default.Features(binding).UsesConversionPolarity) || program.Triggers.Any(binding => ProgramInstructionResolver.Default.Features(binding).UsesConversionPolarity));
        if (optIn || program is null && (definition.Tags & SkillTag.Conversion) != 0)
            _skillRuntimeStateOperations._skillRuntimeState.RegisterConversionSkill(player.Seat, skillId, CharacterYanYanBoundaryMobileRuntime.InitialBoundaryYanYanPolarity(player, skillId));
    }
}
