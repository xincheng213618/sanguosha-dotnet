#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnqiFeirenProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineAnqiFeirenEntries> getCoreGameEngineAnqiFeirenRuntime) : IAnqiFeirenHost
{
    public SkillProgramStepOutcome ResolveAnqiFeiren(ProgramSkillFrame frame) => getCoreGameEngineAnqiFeirenRuntime().ResolveAnqiFeiren(frame);
}
