using System.Text.Json.Serialization;

namespace CardGame.Core;

internal static class MobileYuJinJieyue
{
    internal const string Skill = "mobile:jieyue",Ending = "ending-discard-and-other-choice",Preparation = "preparation-retrieve-jieyue";

    [ProgramCatalogValidation(76300)]
    private static void Validate(string path, SkillProgram program)
    {
        if (!program.Triggers.SelectMany(t => t.Effects).Concat(program.Activations.SelectMany(a => a.Effects))
                .Any(e => e.Op is SkillProgramEffectOp.ResolveMobileYuJinJieyue or SkillProgramEffectOp.CollectMobileYuJinJieyue)) return;
        if (program.Id != Skill || program.Activations.Count != 0 || program.Triggers.Count != 2 ||
            program.Triggers.SingleOrDefault(t => t.Id == Ending) is not { Window: SkillProgramTriggerWindow.TurnEnding,
                Subject: SkillProgramTriggerSubject.Owner, TurnOwnerScope: SkillProgramTurnOwnerScope.Own, Optional: true, Effects:
                [{ Op: SkillProgramEffectOp.SelectTarget, TargetKind: SkillProgramTargetKind.OtherLiving },
                 { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.DiscardPile, ResultBind: "jieyue-cost", AwaitMovementTriggers: true },
                 { Op: SkillProgramEffectOp.ResolveMobileYuJinJieyue, ResultBind: "jieyue-answer" },
                 { Op: SkillProgramEffectOp.SelectAndMoveOwnedCard, Destination: SkillProgramCardDestination.DiscardPile, SkipIfNoCards: true, AwaitMovementTriggers: true }] } ending ||
            program.Triggers.SingleOrDefault(t => t.Id == Preparation) is not { Window: SkillProgramTriggerWindow.TurnStartBeforeNormalFlow,
                Subject: SkillProgramTriggerSubject.Owner, Optional: false, Effects: [{ Op: SkillProgramEffectOp.CollectMobileYuJinJieyue }] } ||
            !ending.Effects[1].Zones.SequenceEqual([CardZoneKind.Hand]) ||
            ending.Effects[1].ChooserRef?.Kind != ProgramParticipantRef.Owner || ending.Effects[1].CardOwnerRef?.Kind != ProgramParticipantRef.Owner ||
            ending.Effects[1].Condition.Kind != SkillProgramConditionKind.Always || ending.Effects[2].Condition.Kind != SkillProgramConditionKind.Always ||
            ending.Effects[3].Condition is not { Kind: SkillProgramConditionKind.ChoiceIs, SourceBind: "jieyue-answer", OptionId: "discard" } ||
            ending.Effects[3].ChooserRef?.Kind != ProgramParticipantRef.Owner || ending.Effects[3].CardOwnerRef?.Kind != ProgramParticipantRef.SelectedTarget ||
            !ending.Effects[3].Zones.SequenceEqual([CardZoneKind.Hand, CardZoneKind.Equipment, CardZoneKind.Judgment]))
            throw new InvalidOperationException(path + ": mobile Jieyue requires its paid hand discard, exact other-participant store/refuse choice, native foreign HEJ discard, and own preparation retrieval.");
    }
}

internal interface IMobileYuJinHost
{
    SkillProgramStepOutcome Transfer(ProgramSkillFrame frame, SkillProgramEffect effect);
    bool MatchesChoiceResult(ProgramSkillFrame frame, ProgramChoiceResultBinding binding);
}
internal abstract class MobileYuJinDescriptor : ProgramOperationDescriptorBase, IProgramChoiceResultProof
{
    public override ISkillProgramEffectHandler Handler => new MobileYuJinHandler(Op);
    public override ProgramOperationInteraction Interaction => Op == SkillProgramEffectOp.ResolveMobileYuJinJieyue ? ProgramOperationInteraction.Choice : ProgramOperationInteraction.Automatic;
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Move, static (_, c) => c.PublicControlValue(1));
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader)
    {
        if (Op == SkillProgramEffectOp.CollectMobileYuJinJieyue) return ParseOwnerAlways(reader);
        reader.AllowOnly("op", "target", "resultBind", "condition");
        var effect = new SkillProgramEffect(Op, FilterBoundCardsProgramOperationDescriptor.Owner(reader), 0, reader.Condition(), resultBind: reader.RequiredIdentifier("resultBind"));
        RequireAlways(effect, reader.Path); return effect;
    }
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        Op == SkillProgramEffectOp.CollectMobileYuJinJieyue
            ? [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnStartBeforeNormalFlow)]
            : [new RequireTriggerWindow(SkillProgramTriggerWindow.TurnEnding), new ReadSelectedTarget(),
                new CreateChoiceResult(effect.ResultBind!, ["store", "discard"])];
    public bool MatchesChoiceResult(SkillProgramEffect effect, ProgramSkillFrame frame,
        ProgramChoiceResultBinding binding, ISkillProgramEffectHost host) =>
        Op == SkillProgramEffectOp.ResolveMobileYuJinJieyue && effect.Op == Op && effect.ResultBind == binding.Name &&
        host.GetCapability<IMobileYuJinHost>().MatchesChoiceResult(frame, binding);
}
internal sealed class ResolveMobileYuJinJieyueDescriptor : MobileYuJinDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.ResolveMobileYuJinJieyue; }
internal sealed class CollectMobileYuJinJieyueDescriptor : MobileYuJinDescriptor { public override SkillProgramEffectOp Op => SkillProgramEffectOp.CollectMobileYuJinJieyue; }
internal sealed class MobileYuJinHandler(SkillProgramEffectOp op) : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => op;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host) =>
        host.GetCapability<IMobileYuJinHost>().Transfer(frame, effect);
}

public sealed record MobileYuJinTransferReceipt
{
    public required SkillProgramEffectOp Operation { get; init; }
    public required CardConversionSource Source { get; init; }
    public required string GameplayHash { get; init; }
    public required PublicPersistentPileSource Pile { get; init; }
    public int ActualTurn { get; init; }
    public long ParentId { get; init; }
    public int InstructionIndex { get; init; }
    public int Before { get; init; }
    public int After { get; init; }
    public CardLocation Destination { get; init; }
    private readonly IReadOnlyList<int> _cards = Array.AsReadOnly(Array.Empty<int>());
    private readonly IReadOnlyList<CardLocation> _from = Array.AsReadOnly(Array.Empty<CardLocation>());
    public IReadOnlyList<int> Cards { get => _cards; init => _cards = Array.AsReadOnly(value.ToArray()); }
    public IReadOnlyList<CardLocation> From { get => _from; init => _from = Array.AsReadOnly(value.ToArray()); }
}
public sealed partial record ProgramSkillFrame
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileYuJinTransferReceipt? MobileYuJinTransfer { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] public MobileYuJinChoiceDraft? MobileYuJinChoice { get; init; }
}
public sealed record MobileYuJinTransferStartedEvent(long FrameId, SkillProgramEffectOp Operation, CardConversionSource Source,
    string GameplayHash, int ActualTurn, long ParentId, int InstructionIndex, PublicPersistentPileSource Pile, int Count, int Before) : IGameEvent;
public sealed record MobileYuJinCardTransferredEvent(long FrameId, int CardId, CardLocation From, CardLocation To, int Sequence) : IGameEvent;
public sealed record MobileYuJinTransferReturnedEvent(long FrameId, int Count, int After) : IGameEvent;

public enum MobileYuJinChoiceStage { Branch, Store }
public sealed record MobileYuJinChoiceDraft(int TargetSeat, MobileYuJinChoiceStage Stage, CardConversionSource Source, string GameplayHash, int ActualTurn, long ParentId);
public sealed record MobileYuJinChoiceOpenedEvent(long FrameId, int TargetSeat, CardConversionSource Source, string GameplayHash, int ActualTurn, long ParentId) : IGameEvent;
public sealed record MobileYuJinBranchChosenEvent(long FrameId, int TargetSeat, bool Store) : IGameEvent;
