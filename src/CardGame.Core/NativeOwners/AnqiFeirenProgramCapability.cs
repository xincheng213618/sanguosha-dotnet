#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class AnqiFeirenProgramCapability(Func<RuntimeEntries.CoreGameEngineAnqiFeirenEntries> getCoreGameEngineAnqiFeirenRuntime) : IAnqiFeirenHost
{
    public SkillProgramStepOutcome ResolveAnqiFeiren(ProgramSkillFrame frame) => getCoreGameEngineAnqiFeirenRuntime().ResolveAnqiFeiren(frame);
}
