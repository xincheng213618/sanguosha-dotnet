#nullable enable
namespace CardGame.Core.NativeOwners;
internal sealed class NearestLegalSlashProgramCapability(global::System.Func<global::CardGame.Core.RuntimeEntries.CoreGameEngineNearestLegalSlashRequestsEntries> getCoreGameEngineNearestLegalSlashRequestsRuntime) : INearestLegalSlashProgramHost
{
    public SkillProgramStepOutcome RequestLegalNearestSlashes(SkillProgramEffect effect, ProgramSkillFrame frame) => getCoreGameEngineNearestLegalSlashRequestsRuntime().RequestLegalNearestSlashes(effect, frame);
    public SkillProgramStepOutcome OfferUnlimitedVirtualSlash(ProgramSkillFrame frame) => getCoreGameEngineNearestLegalSlashRequestsRuntime().OfferUnlimitedVirtualSlash(frame);
}
