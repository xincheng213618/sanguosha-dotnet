using System.Text.Json.Serialization;

namespace CardGame.Core;

public sealed record WuLuKangPindianSelectionAdmission(long FrameId, CardConversionSource ProgramSource,
    string ProgramHash, CardConversionSource CapabilitySource, string CapabilityHash,
    ActualDiscardRecoveryPhaseKey Phase);
public sealed record WuLuKangPindianSelectionAdmittedEvent(WuLuKangPindianSelectionAdmission Admission) : IGameEvent;
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public WuLuKangPindianSelectionAdmission? WuLuKangPindianSelection { get; init; }
}
