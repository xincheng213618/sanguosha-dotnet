#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class HappyWeiYanHost(Func<RuntimeEntries.CharacterWeiYanHappyGodEntries> getCharacterWeiYanHappyGodRuntime) : IHappyWeiYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWeiYanHappyGodRuntime().BeginHappyWeiYan(frame, operation);
}
