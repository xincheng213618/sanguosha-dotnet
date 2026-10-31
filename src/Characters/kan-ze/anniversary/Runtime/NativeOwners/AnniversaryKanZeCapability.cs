#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryKanZeCapability(Func<RuntimeEntries.CharacterKanZeAnniversaryEntries> getCharacterKanZeAnniversaryRuntime) : IAnniversaryKanZeHost
{
    public void Arm(ProgramSkillFrame frame, int target) => getCharacterKanZeAnniversaryRuntime().ArmAnniversaryKuanshi(frame, target);
    public void Bind(ProgramSkillFrame frame) => getCharacterKanZeAnniversaryRuntime().BindAnniversaryKuanshiDamage(frame);
}
