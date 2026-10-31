#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnniversaryKanZeCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterKanZeAnniversaryRuntime>> getCharacterKanZeAnniversaryRuntime) : IAnniversaryKanZeHost
{
    public void Arm(ProgramSkillFrame frame, int target) => getCharacterKanZeAnniversaryRuntime().Value.ArmAnniversaryKuanshi(frame, target);
    public void Bind(ProgramSkillFrame frame) => getCharacterKanZeAnniversaryRuntime().Value.BindAnniversaryKuanshiDamage(frame);
}
