using System.Text.Json.Serialization;

namespace CardGame.Core;

public enum ProgramGiveActor { Owner, SelectedTarget, MaterialOwner }

public sealed partial class SkillProgramEffect
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ProgramGiveActor? GiveBy { get; internal init; }
}

internal static class ProgramCardGiving
{
    internal static ProgramGiveActor? Parse(ProgramOperationNodeReader reader, SkillProgramCardDestination destination)
    {
        if (!reader.Has("giveBy")) return null;
        if (destination is not (SkillProgramCardDestination.OwnerHand or SkillProgramCardDestination.SelectedTargetHand or
                SkillProgramCardDestination.PhaseOwnerHand))
            throw new InvalidOperationException($"Invalid skill program at {reader.Path}.giveBy: Give requires a hand destination.");
        return reader.RequiredEnum<ProgramGiveActor>("giveBy");
    }

    internal static int? Giver(ProgramSkillFrame frame, ProgramGiveActor? declaration,
        IReadOnlyList<CardLocation> sources) => declaration switch
    {
        null => null,
        ProgramGiveActor.Owner => frame.OwnerSeat,
        ProgramGiveActor.SelectedTarget => frame.SelectedTargetSeats.Single(),
        ProgramGiveActor.MaterialOwner => sources.Select(source => source.OwnerSeat).Distinct().ToArray() is [int owner]
            ? owner : throw new InvalidOperationException("A declared material giver requires one actual source owner."),
        _ => throw new InvalidOperationException("Unknown declared Give actor.")
    };
}
