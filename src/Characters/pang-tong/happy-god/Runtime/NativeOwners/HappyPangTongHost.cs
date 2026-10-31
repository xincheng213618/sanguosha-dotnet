#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HappyPangTongHost(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterPangTongHappyGodEntries> getCharacterPangTongHappyGodRuntime) : IHappyPangTongHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f, SkillProgramEffectOp op) => getCharacterPangTongHappyGodRuntime().BeginHappyPangTong(f, op);
}
