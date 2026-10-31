#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceJiangdongCapability(Func<RuntimeEntries.CoreGameEngineShanheSourceLihuo3160Entries> getCoreGameEngineShanheSourceLihuo3160Runtime, Func<RuntimeEntries.CoreGameEngineShanheSourceChunlao3161Entries> getCoreGameEngineShanheSourceChunlao3161Runtime) : IShanheSourceJiangdongTrioHost
{
    public SkillProgramStepOutcome LihuoFirstDamage(ProgramSkillFrame f) => getCoreGameEngineShanheSourceLihuo3160Runtime().BeginShanheSourceLihuoCost(f);
    public SkillProgramStepOutcome ChunlaoStorage(ProgramSkillFrame f) => getCoreGameEngineShanheSourceChunlao3161Runtime().BeginShanheSourceChunlaoStorage(f);
}
