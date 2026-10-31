#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WushuangZengouCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineWushuangZengouEntries> getCoreGameEngineWushuangZengouRuntime) : IWushuangZengouHost
{
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame f) => getCoreGameEngineWushuangZengouRuntime().BeginWushuangZengou(f);
}
