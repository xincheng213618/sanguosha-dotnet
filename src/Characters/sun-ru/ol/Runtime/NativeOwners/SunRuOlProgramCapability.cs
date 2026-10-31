#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class SunRuOlProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunRuOlEntries> getCharacterSunRuOlRuntime) : ISunRuOlProgramHost
{
    public SkillProgramStepOutcome RewardCurrentTurnLastZoneLoss(ProgramSkillFrame f) => getCharacterSunRuOlRuntime().BeginSunRuOlChishi(f);
    public SkillProgramStepOutcome AbolishEquipmentSlotsForDistinctBenefits(ProgramSkillFrame f) => getCharacterSunRuOlRuntime().BeginSunRuOlWeimian(f);
}
