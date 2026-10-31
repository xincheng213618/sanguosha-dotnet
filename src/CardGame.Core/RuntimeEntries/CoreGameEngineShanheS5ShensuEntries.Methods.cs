#nullable enable
namespace CardGame.Core.RuntimeEntries;
internal sealed partial class CoreGameEngineShanheS5ShensuEntries
{
    internal void AssertShanheS5Shensu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().AssertShanheS5Shensu(f);
    internal bool CanOfferShanheS5Shensu(global::CardGame.Core.ProgramTriggerLegalityContext gate) => _resolve().CanOfferShanheS5Shensu(gate);
    internal bool ContinueShanheS5Shensu(global::CardGame.Core.ProgramSkillFrame f) => _resolve().ContinueShanheS5Shensu(f);
    internal bool IsPaidShanheS5TurnoverReturn(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsPaidShanheS5TurnoverReturn(f);
    internal bool IsShanheS5ShensuFrame(global::CardGame.Core.ProgramSkillFrame f) => _resolve().IsShanheS5ShensuFrame(f);
    internal void ObserveShanheS5ExtraDrawSkip(global::CardGame.Core.IGameEvent payload) => _resolve().ObserveShanheS5ExtraDrawSkip(payload);
    internal bool ObserveShanheS5NativePhaseSkip(global::CardGame.Core.ProgramActualPhaseSkipContext context) => _resolve().ObserveShanheS5NativePhaseSkip(context);
    internal void PrepareShanheS5Shensu(global::CardGame.Core.ProgramSkillFrame input) => _resolve().PrepareShanheS5Shensu(input);
    internal void RecordShanheS5ActualPhaseSkip(int owner, global::CardGame.Core.SkillProgramTurnPhase phase, long extraDrawFrameId = 0L, long? costProgramFrameId = default) => _resolve().RecordShanheS5ActualPhaseSkip(owner, phase, extraDrawFrameId, costProgramFrameId);
    internal void RecordShanheS5PhaseSkipCost(global::CardGame.Core.ProgramSkillFrame f, global::System.Collections.Generic.IReadOnlyList<global::CardGame.Core.SkillProgramTurnPhase> phases, global::CardGame.Core.ProgramLifecycleTriggerWindowFrame parent) => _resolve().RecordShanheS5PhaseSkipCost(f, phases, parent);
    internal int ShanheS5ShensuBaseDamageBonus(global::CardGame.Core.ProgramSkillFrame active) => _resolve().ShanheS5ShensuBaseDamageBonus(active);
    internal bool TracksShanheS5PhaseSkips { get => _resolve().TracksShanheS5PhaseSkips; }
}
