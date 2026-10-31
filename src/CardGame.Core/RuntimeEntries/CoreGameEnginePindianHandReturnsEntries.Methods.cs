#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEnginePindianHandReturnsEntries
{
    internal void AssertPindianHandReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertPindianHandReturn(f);
    internal long? BeginPindianWholeHandAcquisition(global::CardGame.Core.ProgramSkillFrame f, string bind, global::CardGame.Core.SkillProgramCardDestination destination) => _resolve().BeginPindianWholeHandAcquisition(f, bind, destination);
    internal void CommitPindianWholeHandAcquisition(long frameId, string bind, long before) => _resolve().CommitPindianWholeHandAcquisition(frameId, bind, before);
    internal bool ContinuePaidPindianHandReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinuePaidPindianHandReturn(f);
    internal void FreezePindianHalfReturn(global::CardGame.Core.ProgramSkillFrame f, int count) => _resolve().FreezePindianHalfReturn(f, count);
    internal bool HasPaidPindianHandReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().HasPaidPindianHandReturn(f);
    internal bool IsPindianHandReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsPindianHandReturn(f);
    internal void RecordPindianHandContestReturn(global::CardGame.Core.ProgramSkillFrame f, global::CardGame.Core.PindianFrame native) => _resolve().RecordPindianHandContestReturn(f, native);
    internal global::CardGame.Core.ProgramPindianHandReturnReceipt RequirePindianHandReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().RequirePindianHandReturn(f);
    internal bool ResumeInvalidPindianHandReturn(long frameId) => _resolve().ResumeInvalidPindianHandReturn(frameId);
}
