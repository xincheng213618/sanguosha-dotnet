namespace CardGame.Core.EngineOwners;

internal sealed partial class CharacterXiaoQiaoMouOlRuntime
{
    private IReadOnlyList<CardConversionSource> MouXqRetainedSources(CharacterState actor) =>
        Array.AsReadOnly(_coreNationalSkillsOperations.GetSkillBindingShard(actor).ProgramInstances
            .Where(instance => instance.SkillId == MouXiaoQiaoOlRuntime.Jueyan)
            .SelectMany(instance => instance.Program.Activations
                .Where(activation => activation.Id == "retained-hand-use")
                .Select(activation => new CardConversionSource(instance.SkillId, activation.Id,
                    actor.Seat, instance.SkillInstanceId)))
            .OrderBy(source => source.SkillInstanceId, StringComparer.Ordinal).ToArray());
}
