#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappySunQuanCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterSunQuanHappyGodEntries> getCharacterSunQuanHappyGodRuntime) : IHappySunQuanHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterSunQuanHappyGodRuntime().BeginHappySunQuan(f, op);
}
