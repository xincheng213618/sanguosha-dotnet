using CardGame.Core.EngineOwners;
using CardGame.Core.EngineStates.Contracts;
using CardGame.Core.RuntimeEntries;

namespace CardGame.Core;

/// <summary>Shared physical equipment acquisition, private offers and native placement.</summary>
[ProgramRuntimeModule, ProgramCapability]
internal sealed class ProgramEquipmentRuntime(IProgramRuntimeFrames frames, IProgramRuntimePrompts prompts,
    IMatchParticipantStateAccess players, IProgramContentQueries content, ICardZoneQueries cards,
    IMatchRandomSource random, IKernelCardZoneStoreAccess store,
    ICoreGameEngineAdvancedWeaponsGeneratedPhysicalCardIdsAccess generated,
    DeferredRuntime<CoreTrustedCardZoneDiagnosticsRuntime> diagnostics,
    CoreGameEngineProgramLifecycleEntries bindings, CoreGameEngineAdvancedWeaponsEntries placement,
    CoreGameEngineBoundCardMovementContinuationEntries movements,
    IKernelCreateProgramExpressionWorldOperations expressions) : IProgramEquipmentHost
{
    public SkillProgramStepOutcome Offer(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        var count = effect.AmountExpression is { } amount
            ? ProgramExpressions.EvaluateInt(amount, expressions.CreateProgramExpressionWorld(frame)) ?? 0 : effect.Amount;
        var pool = effect.EquipmentTemplates!.Where(template => !diagnostics.Value.CreateCardZoneDiagnostics()
            .Any(entity => entity.CardKind == template.Kind && entity.Location != CardLocation.OutsideGame &&
                cards.CardsAt(entity.Location).Single(c => c.Id == entity.CardId).EquipmentOrigin?.SourceId == effect.EquipmentOriginSource)).ToList();
        random.Shuffle(pool);
        var offered = pool.Take(Math.Clamp(count, 0, pool.Count)).ToArray();
        if (offered.Length == 0)
        {
            bindings.SetProgramCardSet(frame.Id, effect.ResultBind!, [], SkillProgramCardSetVisibility.Public, []);
            return SkillProgramStepOutcome.Continue;
        }
        frames.ReplaceTop(frame = frame with { EquipmentOffer = new(effect.ResultBind!, effect.EquipmentOriginSource!, offered) });
        Present(frame, "equipment-offer", "选择一件装备。", offered.Select((t, i) =>
            new PromptChoice(new($"equipment-offer:{frame.Id}:{i}"), $"【{CardCatalog.Get(t.Kind).DisplayName}】：{EquipmentCatalog.Get(t.Kind).Description}", [], [],
                new Dictionary<string, string> { ["program-action"] = "equipment-offer", ["template-index"] = i.ToString() })).ToArray());
        return SkillProgramStepOutcome.AwaitChoice;
    }

    [ProgramAnswer("equipment-offer")]
    private void ChooseEquipment(PromptChoice choice)
    {
        var frame = (ProgramSkillFrame)frames.Top!;
        var offer = frame.EquipmentOffer!;
        var template = offer.Templates[int.Parse(choice.Parameters["template-index"])];
        var id = diagnostics.Value.CreateCardZoneDiagnostics().Select(entity => entity.CardId).DefaultIfEmpty().Max() + 1;
        var card = new Card(id, template.Kind, template.Suit, template.Rank)
        { EquipmentOrigin = new(id, offer.Source, frame.OwnerSeat) { IssuingSkillId = frame.SkillId } };
        store._cardZones.AddGeneratedCard(card);
        generated._generatedPhysicalCardIds.Add(id);
        prompts.Clear();
        frames.ReplaceTop(frame with { EquipmentOffer = null });
        bindings.SetProgramCardSet(frame.Id, offer.ResultBind, [id], SkillProgramCardSetVisibility.Public, [CardLocation.OutsideGame]);
        frames.AdvanceProgram(frame.Id);
    }

    public void Select(ProgramSkillFrame frame, SkillProgramEffect effect)
    {
        var pool = effect.Zones.SelectMany(zone => cards.CardsAt(new CardLocation(zone)))
            .Where(card => EquipmentCatalog.IsEquipment(card.Kind) &&
                (effect.EquipmentSlots.Count == 0 || effect.EquipmentSlots.Contains(EquipmentCatalog.Get(card.Kind).Slot))).ToList();
        random.Shuffle(pool);
        var selected = new List<Card>();
        foreach (var card in pool)
        {
            if (effect.DistinctEquipmentCategories == true && selected.Any(c => Category(c.Kind) == Category(card.Kind))) continue;
            selected.Add(card);
            if (selected.Count == effect.Amount) break;
        }
        bindings.SetProgramCardSet(frame.Id, effect.ResultBind!, Array.AsReadOnly(selected.Select(c => c.Id).ToArray()),
            SkillProgramCardSetVisibility.Private, Array.AsReadOnly(selected.Select(c => cards.GetLocation(c.Id)).ToArray()));
    }
    private static EquipmentSlot Category(CardKind kind) => EquipmentCatalog.Get(kind).Slot == EquipmentSlot.DefensiveHorse
        ? EquipmentSlot.OffensiveHorse : EquipmentCatalog.Get(kind).Slot;

    public SkillProgramStepOutcome Place(ProgramSkillFrame supplied, SkillProgramEffect effect)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        frames.ReplaceTop(frame = frame with { EquipmentPlacement = new(effect.SourceBind!, 0,
            effect.ChooseEquipmentRecipient == true ? null : frame.OwnerSeat, false) });
        return ContinuePlacement(frame, effect.ChooseEquipmentRecipient == true);
    }

    private SkillProgramStepOutcome ContinuePlacement(ProgramSkillFrame supplied, bool choose)
    {
        var frame = frames.ActiveProgram(supplied.Id);
        var state = frame.EquipmentPlacement!;
        var binding = bindings.GetProgramCardSet(frame, state.SourceBind);
        while (state.Index < binding.CardIds.Count)
        {
            var card = cards.CardsAt(cards.GetLocation(binding.CardIds[state.Index])).Single(c => c.Id == binding.CardIds[state.Index]);
            var eligible = players.Players.Where(p => p.IsAlive && p.EquipmentSlotCapacity(EquipmentCatalog.Get(card.Kind).Slot) > 0).ToArray();
            if (choose && state.RecipientSeat is null && eligible.Length > 0)
            {
                Present(frame, "equipment-recipient", $"将【{card.DisplayName}】置入一名角色的装备区（替换原装备）。",
                    eligible.Select(p => new PromptChoice(new($"equipment-recipient:{frame.Id}:{p.Seat}"), p.Name, [], [p.Seat],
                        new Dictionary<string, string> { ["program-action"] = "equipment-recipient" })).ToArray());
                return SkillProgramStepOutcome.AwaitChoice;
            }
            var recipient = state.RecipientSeat;
            var canPlace = recipient is { } seat && eligible.Any(p => p.Seat == seat);
            state = state with { Index = state.Index + 1, RecipientSeat = choose ? null : frame.OwnerSeat, AwaitingMovement = canPlace };
            frames.ReplaceTop(frame = frame with { EquipmentPlacement = state,
                PendingMovementContinuation = canPlace ? new(frame.OwnerSeat, 0, null) : null });
            if (!canPlace) continue;
            placement.PlaceAdvancedEquipment(card, recipient!.Value);
            var outcome = movements.AwaitProgramBoundCardMovements(frame.Id, frame.OwnerSeat);
            if (outcome != SkillProgramStepOutcome.Continue) return outcome;
            state = state with { AwaitingMovement = false };
            frames.ReplaceTop(frame = frames.ActiveProgram(frame.Id) with { EquipmentPlacement = state });
        }
        frames.ReplaceTop(frames.ActiveProgram(frame.Id) with { EquipmentPlacement = null });
        return SkillProgramStepOutcome.Continue;
    }

    [ProgramAnswer("equipment-recipient")]
    private void ChooseRecipient(PromptChoice choice)
    {
        var frame = (ProgramSkillFrame)frames.Top!;
        prompts.Clear();
        frames.ReplaceTop(frame = frame with { EquipmentPlacement = frame.EquipmentPlacement! with { RecipientSeat = choice.Targets.Single() } });
        if (ContinuePlacement(frame, true) == SkillProgramStepOutcome.Continue) frames.AdvanceProgram(frame.Id);
    }

    [ProgramContinuation(ProgramContinuationKind.Movement, 910001)]
    private bool PlacementReturned(ProgramSkillFrame frame)
    {
        if (frame.EquipmentPlacement is not { AwaitingMovement: true } state) return false;
        frames.ReplaceTop(frame = frame with { EquipmentPlacement = state with { AwaitingMovement = false }, PendingMovementContinuation = null });
        var effect = ProgramInstructionResolver.Default.Resolve(frame, content.Skill(frame.SkillId).Program!)
            .GetPausedInstruction(frame.InstructionIndex).Effect;
        if (ContinuePlacement(frame, effect.ChooseEquipmentRecipient == true) == SkillProgramStepOutcome.Continue) frames.AdvanceProgram(frame.Id);
        return true;
    }

    private void Present(ProgramSkillFrame frame, string action, string message, IReadOnlyList<PromptChoice> choices)
    {
        var skill = content.Skill(frame.SkillId);
        prompts.Set(new(DecisionKind.ProgramTrigger, frame.OwnerSeat, message, [], [], frame.OwnerSeat)
        { PromptId = prompts.NextId(), IsPrivate = true, SkillPrompt = new(frame.SkillId, skill.Name, skill.Name, skill.Description),
            Choices = Array.AsReadOnly(choices.ToArray()) }, players.At(frame.OwnerSeat).IsHuman);
    }
    [ProgramAiChoice(SkillProgramEffectOp.OfferGeneratedEquipment)]
    private PromptChoice SelectEquipmentForAi(PendingDecision decision) => decision.Choices
        .OrderByDescending(c => CardCatalog.Get(((ProgramSkillFrame)frames.Top!).EquipmentOffer!.Templates[int.Parse(c.Parameters["template-index"])].Kind).HandKeepValue).First();
    [ProgramAiChoice(SkillProgramEffectOp.PlaceBoundEquipment)]
    private PromptChoice SelectRecipientForAi(PendingDecision decision) => decision.Choices
        .OrderByDescending(c => c.Targets.Contains(decision.PlayerSeat)).First();
}
