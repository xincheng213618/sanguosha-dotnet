namespace CardGame.Core;
public sealed record ProgramEndingHandCountHistory(int OwnerSeat, string SkillId, string SkillInstanceId, string StateId, int LastActualTurnNumber, int LastHandCount, IReadOnlyList<int> Counts);
public sealed record ProgramEndingHandCountRecordedEvent(int OwnerSeat, string SkillId, string SkillInstanceId, string StateId, int ActualTurnNumber, int HandCount, bool IsNew) : IGameEvent;
internal interface IEndingHandCountProgramHost
{
    void RecordEndHandCountAndGrantMarker(ProgramSkillFrame frame, string stateId, PlayerMarkerKind marker);
}

public sealed class RecordEndHandCountAndGrantMarkerHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.RecordEndHandCountAndGrantMarker;

    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    {
        host.GetCapability<IEndingHandCountProgramHost>().RecordEndHandCountAndGrantMarker(frame, effect.StateId!, effect.Marker!.Value);
        return SkillProgramStepOutcome.Continue;
    }
}

internal sealed class RecordEndHandCountAndGrantMarkerDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.RecordEndHandCountAndGrantMarker;
    public override ISkillProgramEffectHandler Handler { get; } = new RecordEndHandCountAndGrantMarkerHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.ChangeAttributedMarker, static (_, _) =>
    {
    });

    public override SkillProgramEffect Parse(ProgramOperationNodeReader r)
    {
        r.AllowOnly("op", "target", "stateId", "marker", "amount", "condition");
        if (r.RequiredInt("amount") != 1)
            throw new InvalidOperationException("An ending hand-count observation grants exactly one marker.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(r), 1, r.Condition(), stateId: r.RequiredIdentifier("stateId"), marker: r.RequiredEnum<PlayerMarkerKind>("marker"));
        RequireAlways(effect, r.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) => [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding), new RequireOwnTurnBoundary()];
}

public sealed partial class GameEngine
{
    private ref readonly global::System.Collections.Generic.Dictionary<(int Owner, string Skill, string Instance, string State), global::CardGame.Core.ProgramEndingHandCountHistory> _endingHandCountHistories => ref _endingHandCountPrograms.ReferenceToEndingHandCountHistories;

    /// <summary>Trusted rule diagnostics, not a player projection. Returned nested collections are frozen.</summary>
    public IReadOnlyList<ProgramEndingHandCountHistory> GetProgramEndingHandCountHistoryDiagnostics() => GetRuntimeModule<global::CardGame.Core.EngineOwners.CoreEndingHandCountProgramsRuntime>().GetProgramEndingHandCountHistoryDiagnostics();
}
