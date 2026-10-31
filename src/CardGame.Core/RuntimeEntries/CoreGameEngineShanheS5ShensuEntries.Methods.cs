#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ShensuEntries
{
    internal bool CanOfferShanheS5Shensu(ProgramTriggerLegalityContext gate) => _resolve().CanOfferShanheS5Shensu(gate);
    internal bool ContinueShanheS5Shensu(ProgramSkillFrame f) => _resolve().ContinueShanheS5Shensu(f);
    internal bool IsPaidShanheS5TurnoverReturn(ProgramSkillFrame f) => _resolve().IsPaidShanheS5TurnoverReturn(f);
    internal bool IsShanheS5ShensuFrame(ProgramSkillFrame f) => _resolve().IsShanheS5ShensuFrame(f);
    internal void ObserveShanheS5ExtraDrawSkip(IGameEvent payload) => _resolve().ObserveShanheS5ExtraDrawSkip(payload);
    internal bool ObserveShanheS5NativePhaseSkip(ProgramActualPhaseSkipContext context) => _resolve().ObserveShanheS5NativePhaseSkip(context);
    internal void PrepareShanheS5Shensu(ProgramSkillFrame input) => _resolve().PrepareShanheS5Shensu(input);
    internal void RecordShanheS5ActualPhaseSkip(int owner, SkillProgramTurnPhase phase, long extraDrawFrameId = 0L, long? costProgramFrameId = default) => _resolve().RecordShanheS5ActualPhaseSkip(owner, phase, extraDrawFrameId, costProgramFrameId);
    internal void RecordShanheS5PhaseSkipCost(ProgramSkillFrame f, IReadOnlyList<SkillProgramTurnPhase> phases, ProgramLifecycleTriggerWindowFrame parent) => _resolve().RecordShanheS5PhaseSkipCost(f, phases, parent);
    internal int ShanheS5ShensuBaseDamageBonus(ProgramSkillFrame active) => _resolve().ShanheS5ShensuBaseDamageBonus(active);
    internal bool TracksShanheS5PhaseSkips { get => _resolve().TracksShanheS5PhaseSkips; }
}
