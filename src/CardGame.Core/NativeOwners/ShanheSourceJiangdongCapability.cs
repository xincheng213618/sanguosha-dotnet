#nullable enable

namespace CardGame.Core.NativeOwners;
internal sealed class ShanheSourceJiangdongCapability(Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceLihuo3160Runtime>> getCoreGameEngineShanheSourceLihuo3160Runtime, Func<RuntimeEntries.DeferredRuntime<EngineOwners.CoreGameEngineShanheSourceChunlao3161Runtime>> getCoreGameEngineShanheSourceChunlao3161Runtime) : IShanheSourceJiangdongTrioHost
{
    public SkillProgramStepOutcome LihuoFirstDamage(ProgramSkillFrame f) => getCoreGameEngineShanheSourceLihuo3160Runtime().Value.BeginShanheSourceLihuoCost(f);
    public SkillProgramStepOutcome ChunlaoStorage(ProgramSkillFrame f) => getCoreGameEngineShanheSourceChunlao3161Runtime().Value.BeginShanheSourceChunlaoStorage(f);
}
