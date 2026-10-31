#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class ConversionPolarityProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineProgramConversionPolarityEntries> getCoreGameEngineProgramConversionPolarityRuntime) : IConversionPolarityProgramHost
{
    public void CommitConversionPolarity(ProgramSkillFrame frame) => getCoreGameEngineProgramConversionPolarityRuntime().CommitProgramConversionPolarity(frame);
}
