namespace CardGame.Core;

internal sealed record ProgramCardUseResultSink(long ParentFrameId, int InstructionIndex, string Bind,
    int DeclarerSeat, int ProviderSeat, IReadOnlyList<int> IssuedTargetSeats)
{
    private readonly IReadOnlyList<int> _issuedTargetSeats = Array.AsReadOnly(IssuedTargetSeats.ToArray());
    public IReadOnlyList<int> IssuedTargetSeats
    { get => _issuedTargetSeats; init => _issuedTargetSeats = Array.AsReadOnly(value.ToArray()); }
}

/// <summary>The completed native use result of one original program instruction.</summary>
public sealed record ProgramCardUseResultBinding(string Bind, int InstructionIndex, long NativeUseId,
    long? ActionId, int DeclarerSeat, int ActorSeat, int ProviderSeat, CardKind Kind,
    IReadOnlyList<int> IssuedTargetSeats, IReadOnlyList<int> TargetSeats)
{
    private readonly IReadOnlyList<int> _issuedTargetSeats = Array.AsReadOnly(IssuedTargetSeats.ToArray());
    private readonly IReadOnlyList<int> _targetSeats = Array.AsReadOnly(TargetSeats.ToArray());
    public IReadOnlyList<int> IssuedTargetSeats
    { get => _issuedTargetSeats; init => _issuedTargetSeats = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<int> TargetSeats
    { get => _targetSeats; init => _targetSeats = Array.AsReadOnly(value.ToArray()); }
}

public sealed partial record CardUseFrame
{
    internal ProgramCardUseResultSink? ProgramResultSink { get; init; }
}

public sealed partial record ProgramSkillFrame
{
    private readonly IReadOnlyList<ProgramCardUseResultBinding> _cardUseResults =
        Array.AsReadOnly(Array.Empty<ProgramCardUseResultBinding>());
    public IReadOnlyList<ProgramCardUseResultBinding> CardUseResults
    { get => _cardUseResults; init => _cardUseResults = Array.AsReadOnly(value.ToArray()); }
}

internal static class ProgramCardUseResultResources
{
    internal static IReadOnlyList<ProgramResourceOperation> Append(SkillProgramEffect effect,
        IReadOnlyList<ProgramResourceOperation> inputs) => effect.ResultBind is { } bind
        ? [.. inputs, new CreateUseResultBinding(bind)] : inputs;
}

internal interface IProgramCardUseResultCapture : IProgramRuntimeOperations
{
    void Capture(CardUseFrame use);
}

[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramCardUseResultCaptureRuntime(IProgramRuntimeFrames frames) : IProgramCardUseResultCapture
{
    public void Capture(CardUseFrame use)
    {
        if (use.ProgramResultSink is not { } sink)
            return;
        if (use.Step != ResolutionFrameStep.Completed)
            throw new InvalidOperationException("A program card-use result requires its native completion boundary.");
        if (frames.Find(sink.ParentFrameId) is not ProgramSkillFrame parent)
            throw new InvalidOperationException("A program card-use result lost its original parent frame.");
        // A repeated completion never replaces the originally delivered result,
        // even after the parent has advanced beyond this instruction.
        if (parent.CardUseResults.Any(previous => previous.NativeUseId == use.Id))
            return;
        if (parent.InstructionIndex != sink.InstructionIndex ||
            frames.ParentOf(use.Id)?.Id != parent.Id)
            throw new InvalidOperationException("A program card-use result requires its original parent and instruction cursor.");
        var result = new ProgramCardUseResultBinding(sink.Bind, sink.InstructionIndex, use.Id,
            use.Action?.ActionId, sink.DeclarerSeat, use.SourceSeat,
            use.Action?.ProviderSeat ?? sink.ProviderSeat, use.CardKind,
            sink.IssuedTargetSeats, use.TargetSeats);
        frames.Replace(parent.Id, parent with
        {
            CardUseResults = parent.CardUseResults.Where(previous => previous.Bind != sink.Bind)
                .Append(result).ToArray()
        });
    }
}
