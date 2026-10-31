#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangZengouCapability(Func<RuntimeEntries.CoreGameEngineWushuangZengouEntries> getCoreGameEngineWushuangZengouRuntime) : IWushuangZengouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCoreGameEngineWushuangZengouRuntime().BeginWushuangZengou(f);
}
