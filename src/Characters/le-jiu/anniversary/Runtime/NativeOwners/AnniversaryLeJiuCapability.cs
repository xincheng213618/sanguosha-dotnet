#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLeJiuCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CharacterLeJiuAnniversaryEntries> getCharacterLeJiuAnniversaryRuntime) : IAnniversaryLeJiuHost
{
    public void Arm(ProgramSkillFrame f) => getCharacterLeJiuAnniversaryRuntime().ArmLeJiuCuijin(f);
    public void Bind(ProgramSkillFrame f) => getCharacterLeJiuAnniversaryRuntime().BindLeJiuCuijin(f);
}
