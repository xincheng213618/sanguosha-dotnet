#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyWeiYanHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterWeiYanHappyGodEntries> getCharacterWeiYanHappyGodRuntime) : IHappyWeiYanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffectOp operation) => getCharacterWeiYanHappyGodRuntime().BeginHappyWeiYan(frame, operation);
}
