#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class DingYuanProgramCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CharacterDingYuanOlRuntime>> getCharacterDingYuanOlRuntime) : IDingYuanProgramHost
{
    public void PrepareAdoptedChild(ProgramSkillFrame frame, bool transfer) => getCharacterDingYuanOlRuntime().Value.PrepareAdoptedChild(frame, transfer);
    public void AssignAdoptedChild(ProgramSkillFrame frame) => getCharacterDingYuanOlRuntime().Value.AssignAdoptedChild(frame);
    public void QualifyFirstRoundDamage(ProgramSkillFrame frame) => getCharacterDingYuanOlRuntime().Value.QualifyFirstRoundDamage(frame);
}
