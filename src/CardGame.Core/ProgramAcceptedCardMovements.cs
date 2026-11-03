using System.Text.Json;
using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ProgramAcceptedMovementSourceLoss { Stop, Retain }
public enum ProgramAcceptedMovementOwnerDeath { Stop, Drain }
public sealed record ProgramAcceptedMovementContinuation(
    ProgramAcceptedMovementSourceLoss SourceLoss = ProgramAcceptedMovementSourceLoss.Stop,
    ProgramAcceptedMovementOwnerDeath OwnerDeath = ProgramAcceptedMovementOwnerDeath.Stop);

/// <summary>Acceptance of the original materials, not a receipt that they have already moved.</summary>
public sealed record ProgramAcceptedCardMovement(int InstructionIndex, SkillProgramCardDestination Destination,
    ProgramAcceptedMovementContinuation Continuation);

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramAcceptedMovementContinuation? AcceptedMovementContinuation { get; internal init; }
}

internal static class ProgramAcceptedCardMovements
{
    internal static bool RetainsSourceLoss(ProgramSkillFrame frame) => frame.CardSetBindings.Any(binding =>
        binding.AcceptedMovement is { Continuation.SourceLoss: ProgramAcceptedMovementSourceLoss.Retain } accepted &&
        accepted.InstructionIndex <= frame.InstructionIndex);

    internal static bool DrainsOwnerDeath(ProgramSkillFrame frame) => frame.CardSetBindings.Any(binding =>
        binding.AcceptedMovement is { Continuation.OwnerDeath: ProgramAcceptedMovementOwnerDeath.Drain } accepted &&
        accepted.InstructionIndex <= frame.InstructionIndex);

    internal static bool MayExecuteWhileDraining(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        if (effect.Op == SkillProgramEffectOp.MoveBoundCards &&
            frame.CardSetBindings.SingleOrDefault(binding => binding.Name == effect.SourceBind)?.AcceptedMovement is
                { Continuation.OwnerDeath: ProgramAcceptedMovementOwnerDeath.Drain } accepted &&
            accepted.InstructionIndex < frame.InstructionIndex && accepted.Destination == effect.Destination)
            return true;
        // A delivered request may still drain its existing native children. This
        // does not authorize issuing another Draw from the remaining recipe.
        return effect.Op == SkillProgramEffectOp.Draw && frame.NativeDraw is
            { RecipientApplied: true } draw && draw.InstructionIndex == frame.InstructionIndex;
    }
}

internal sealed class AcceptBoundCardMovementDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.AcceptBoundCardMovement;
    public override ISkillProgramEffectHandler Handler { get; } = new AcceptHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.CaptureSelectedCards,
        static (effect, context) => context.AcceptBoundMovement(effect));

    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        reader.AllowOnly("op", "target", "sourceBind", "resultBind", "destination", "continuation", "condition");
        var source = reader.RequiredIdentifier("sourceBind");
        var result = reader.RequiredIdentifier("resultBind");
        var destination = reader.RequiredEnum<SkillProgramCardDestination>("destination");
        if (source == result || destination != SkillProgramCardDestination.DiscardPile)
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}: accepted movement requires distinct bindings and discardPile.");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0,
            reader.Condition(), sourceBind: source, resultBind: result, destination: destination)
            { AcceptedMovementContinuation = ParseContinuation(reader) };
        RequireAlways(effect, reader.Path);
        return effect;
    }

    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new AcceptMovement(effect.SourceBind!, effect.ResultBind!, effect.Destination!.Value)];

    private static ProgramAcceptedMovementContinuation ParseContinuation(ProgramOperationNodeReader reader)
    {
        if (!reader.Has("continuation")) return new();
        var node = reader.Node.GetProperty("continuation");
        if (node.ValueKind != JsonValueKind.Object || node.EnumerateObject().Any(property => property.Name is not ("sourceLoss" or "ownerDeath")))
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}.continuation: requires sourceLoss and ownerDeath policies.");
        string? Value(string name) => !node.TryGetProperty(name, out var property) ? null :
            property.ValueKind == JsonValueKind.String ? property.GetString() : "invalid";
        var sourceLoss = Value("sourceLoss") switch
        {
            null or "stop" => ProgramAcceptedMovementSourceLoss.Stop,
            "retain" => ProgramAcceptedMovementSourceLoss.Retain,
            _ => throw new InvalidOperationException($"Invalid skill program at {reader.Path}.continuation.sourceLoss: requires stop or retain.")
        };
        var ownerDeath = Value("ownerDeath") switch
        {
            null or "stop" => ProgramAcceptedMovementOwnerDeath.Stop,
            "drain" => ProgramAcceptedMovementOwnerDeath.Drain,
            _ => throw new InvalidOperationException($"Invalid skill program at {reader.Path}.continuation.ownerDeath: requires stop or drain.")
        };
        return new(sourceLoss, ownerDeath);
    }

    private sealed class AcceptHandler : ISkillProgramEffectHandler
    {
        public SkillProgramEffectOp Op => SkillProgramEffectOp.AcceptBoundCardMovement;
        public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
        {
            host.GetCapability<IProgramAcceptedCardMovementHost>().Accept(frame, effect);
            return SkillProgramStepOutcome.Continue;
        }
    }
}
