#nullable enable
using System.Globalization;

namespace CardGame.Core.NativeOwners;
internal sealed class HuoJunProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterHuoJunOlEntries> getCharacterHuoJunOlRuntime) : IHuoJunProgramHost
{
    public SkillProgramStepOutcome RestoreSlotAndUseMatchingEquipment(ProgramSkillFrame f) => getCharacterHuoJunOlRuntime().BeginHuoJunFenrui(f);
}
