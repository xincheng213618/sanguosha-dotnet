namespace CardGame.Core;

public sealed record MobileMaZhongFumanPreparedEvent(long FrameId, CardConversionSource Source, string Hash,
    int RecipientSeat, int CardId, CardKind CardKind, int TurnNumber, long Before) : IGameEvent;
public sealed record MobileMaZhongFumanUseEntitledEvent(long GiftFrameId, long UseFrameId, long ActionId,
    CardConversionSource Source, string Hash, int RecipientSeat, int CardId, long PaymentSequence) : IGameEvent;

internal interface IMobileMaZhongHost { void Prepare(ProgramSkillFrame frame); }

internal sealed class PrepareMobileMaZhongFumanDescriptor : ProgramOperationDescriptorBase
{
    public override SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileMaZhongFuman;
    public override SkillProgramEffect Parse(ProgramOperationNodeReader reader) => ParseOwnerAlways(reader);
    public override ISkillProgramEffectHandler Handler { get; } = new PrepareMobileMaZhongFumanHandler();
    public override ProgramOperationAiPolicy AiPolicy { get; } = new(ProgramOperationAiSemantic.Gift, static (_, c) => c.PublicControlValue(1));
    public override IReadOnlyList<ProgramResourceOperation> Resources(SkillProgramEffect effect) =>
        [new ReadSelectedTarget(), new RequireOwnedCardSet("fuman-gift", SkillProgramEffectTarget.Owner, 1, [CardZoneKind.Hand]), new ReadSingleCardSet("fuman-gift")];
}
internal sealed class PrepareMobileMaZhongFumanHandler : ISkillProgramEffectHandler
{
    public SkillProgramEffectOp Op => SkillProgramEffectOp.PrepareMobileMaZhongFuman;
    public SkillProgramStepOutcome Execute(SkillProgramEffect effect, ProgramSkillFrame frame, int targetSeat, ISkillProgramEffectHost host)
    { host.GetCapability<IMobileMaZhongHost>().Prepare(frame); return SkillProgramStepOutcome.Continue; }
}

internal static class MobileMaZhongComposition
{
    [ProgramCatalogValidation(78800)]
    private static void Validate(string path, SkillProgram program)
    {
        if (program.Id != "mobile:fuman" && !program.Activations.SelectMany(a => a.Effects).Any(e => e.Op == SkillProgramEffectOp.PrepareMobileMaZhongFuman)) return;
        if (program.Id != "mobile:fuman" || program.Activations is not [{ Id: "give-physical-slash", MinCards: 0, MaxCards: 0,
                MinTargets: 1, MaxTargets: 1, TargetKind: SkillProgramTargetKind.OtherLiving, TargetPhaseLedgerId: "give-physical-slash",
                UsesPerTurn: null, UsesPerPhase: null, Effects:
                [{ Op: SkillProgramEffectOp.ConsumeTargetPhaseLedger, StateId: "give-physical-slash" },
                 { Op: SkillProgramEffectOp.SelectOwnedCards, Target: SkillProgramEffectTarget.Owner, MinimumCards: 1, MaximumCards: 1, ResultBind: "fuman-gift" } select,
                 { Op: SkillProgramEffectOp.PrepareMobileMaZhongFuman, Target: SkillProgramEffectTarget.Owner },
                 { Op: SkillProgramEffectOp.MoveBoundCards, Target: SkillProgramEffectTarget.Owner, SourceBind: "fuman-gift", Destination: SkillProgramCardDestination.SelectedTargetHand, AwaitMovementTriggers: false }] } activation] ||
            !activation.SourceZones.SequenceEqual([CardZoneKind.Hand]) || !select.Zones.SequenceEqual([CardZoneKind.Hand]) ||
            !select.CardKinds.SequenceEqual([CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash]) || program.Triggers is not
                [{ Id: "gifted-slash-used", Window: SkillProgramTriggerWindow.CardUseCommitted,
                   OwnerRelation: SkillProgramCardActionOwnerRelation.Observer, Optional: false, IncludeResponseUses: false,
                   Effects: [{ Op: SkillProgramEffectOp.Draw, Target: SkillProgramEffectTarget.Owner, Amount: 1, Condition.Kind: SkillProgramConditionKind.Always }] } trigger] ||
            !trigger.CardKinds.SequenceEqual([CardKind.Slash, CardKind.FireSlash, CardKind.ThunderSlash]))
            throw new InvalidOperationException(path + ": mobile Fuman requires one physical Hand Slash, its per-target Play quota, native paid gift, and exactly one draw at real Use commitment.");
    }
}
