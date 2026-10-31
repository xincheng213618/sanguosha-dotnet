#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class XuanGongZhuProgramCapability(Func<RuntimeEntries.CoreGameEngineQimeiProgramsEntries> getCoreGameEngineQimeiProgramsRuntime, Func<RuntimeEntries.CoreGameEngineZhuijiProgramsEntries> getCoreGameEngineZhuijiProgramsRuntime, Func<RuntimeEntries.CoreGameEngineIdentityConcealmentEntries> getCoreGameEngineIdentityConcealmentRuntime) : IXuanGongZhuProgramHost
{
    public SkillProgramStepOutcome ArmQimeiPair(ProgramSkillFrame f) => getCoreGameEngineQimeiProgramsRuntime().ArmQimeiPair(f);
    public SkillProgramStepOutcome SettleQimeiPairEquality(ProgramSkillFrame f) => getCoreGameEngineQimeiProgramsRuntime().SettleQimeiPairEquality(f);
    public SkillProgramStepOutcome OfferZhuijiActualPlayChoice(ProgramSkillFrame f) => getCoreGameEngineZhuijiProgramsRuntime().ArmZhuijiActualPlay(f);
    public SkillProgramStepOutcome SettleZhuijiActualPlayDebt(ProgramSkillFrame f) => getCoreGameEngineZhuijiProgramsRuntime().SettleZhuijiActualPlayDebt(f);
    public SkillProgramStepOutcome AfterActualIdentityAppearanceRecover(ProgramSkillFrame f) => getCoreGameEngineIdentityConcealmentRuntime().ArmGaolingRecovery(f);
}
