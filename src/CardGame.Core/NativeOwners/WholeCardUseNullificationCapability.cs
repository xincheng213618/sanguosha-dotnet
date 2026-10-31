#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WholeCardUseNullificationCapability(Func<RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries> getCoreGameEngineProgramCardUseNullificationRuntime) : IWholeCardUseNullificationHost
{
    public void NullifyCurrentCardUse(ProgramSkillFrame frame) => getCoreGameEngineProgramCardUseNullificationRuntime().NullifyCurrentProgramCardUse(frame);
}
