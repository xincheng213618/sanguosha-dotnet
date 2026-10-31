#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class WholeCardUseNullificationCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramCardUseNullificationEntries> getCoreGameEngineProgramCardUseNullificationRuntime) : IWholeCardUseNullificationHost
{
    public void NullifyCurrentCardUse(ProgramSkillFrame frame) => getCoreGameEngineProgramCardUseNullificationRuntime().NullifyCurrentProgramCardUse(frame);
}
