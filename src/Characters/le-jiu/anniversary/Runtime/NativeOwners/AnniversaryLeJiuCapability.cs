#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryLeJiuCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterLeJiuAnniversaryRuntime>> getCharacterLeJiuAnniversaryRuntime) : IAnniversaryLeJiuHost
{
    public void Arm(ProgramSkillFrame f) => getCharacterLeJiuAnniversaryRuntime().Value.ArmLeJiuCuijin(f);
    public void Bind(ProgramSkillFrame f) => getCharacterLeJiuAnniversaryRuntime().Value.BindLeJiuCuijin(f);
}
