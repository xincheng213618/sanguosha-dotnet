#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLeJiuCapability(Func<RuntimeEntries.CharacterLeJiuAnniversaryEntries> getCharacterLeJiuAnniversaryRuntime) : IAnniversaryLeJiuHost
{
    public void Arm(ProgramSkillFrame f) => getCharacterLeJiuAnniversaryRuntime().ArmLeJiuCuijin(f);
    public void Bind(ProgramSkillFrame f) => getCharacterLeJiuAnniversaryRuntime().BindLeJiuCuijin(f);
}
