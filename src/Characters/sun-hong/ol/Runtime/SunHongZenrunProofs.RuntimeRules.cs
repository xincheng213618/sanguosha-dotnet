namespace CardGame.Core.RuntimeSkills;

[Core.ProgramRuntimeModule]
internal static class RuntimeRules_sun_hong_ol_SunHongZenrunProofs
{
    internal static bool ZenrunChildren(ZenrunStage stage) => stage is ZenrunStage.TakeChildren or ZenrunStage.CompensationChildren;
}
