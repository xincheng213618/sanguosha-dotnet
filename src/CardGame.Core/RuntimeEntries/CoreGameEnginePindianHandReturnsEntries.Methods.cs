#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianHandReturnsEntries
{
    internal long? BeginPindianWholeHandAcquisition(ProgramSkillFrame f, string bind, SkillProgramCardDestination destination) => _resolve().BeginPindianWholeHandAcquisition(f, bind, destination);
    internal void CommitPindianWholeHandAcquisition(long frameId, string bind, long before) => _resolve().CommitPindianWholeHandAcquisition(frameId, bind, before);
    internal bool ContinuePaidPindianHandReturn(ProgramSkillFrame f) => _resolve().ContinuePaidPindianHandReturn(f);
    internal void FreezePindianHalfReturn(ProgramSkillFrame f, int count) => _resolve().FreezePindianHalfReturn(f, count);
    internal bool HasPaidPindianHandReturn(ProgramSkillFrame f) => _resolve().HasPaidPindianHandReturn(f);
    internal bool IsPindianHandReturn(ProgramSkillFrame f) => _resolve().IsPindianHandReturn(f);
    internal void RecordPindianHandContestReturn(ProgramSkillFrame f, PindianFrame native) => _resolve().RecordPindianHandContestReturn(f, native);
    internal ProgramPindianHandReturnReceipt RequirePindianHandReturn(ProgramSkillFrame f) => _resolve().RequirePindianHandReturn(f);
    internal bool ResumeInvalidPindianHandReturn(long frameId) => _resolve().ResumeInvalidPindianHandReturn(frameId);
}
