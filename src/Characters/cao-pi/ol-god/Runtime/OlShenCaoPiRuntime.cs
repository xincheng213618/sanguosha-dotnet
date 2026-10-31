namespace CardGame.Core;

[ProgramRuntimeModule,ProgramCapability]
internal sealed class OlShenCaoPiRuntime(IProgramRuntimeFrames frames, IProgramRuntimeEvents events,
    IProgramRuntimePrompts prompts, IProgramOperationDraws draws, IOlShenCaoPiFacts facts,
    IOlShenCaoPiOperations operations) : IOlShenCaoPiHost
{
    private static CardConversionSource OlCaoPiSource(ProgramSkillFrame f) => new(f.SkillId, RuntimeSkills.RuntimeRules_Core_ProgramLifecycle.GetProgramBindingId(f), f.OwnerSeat, f.SkillInstanceId);
    private PublicPersistentPileSource? OlChuPile(int owner, string instance) => facts.ReferencedPiles(owner, "ol:chuyuan", instance)
        .OrderByDescending(p => facts.PileCards(p).Count).ThenBy(p => p.SkillInstanceId, StringComparer.Ordinal).FirstOrDefault();
    [ProgramTriggerLegality(ProgramTriggerLegalityPhase.BeforeGainQualification, 40550, SkillProgramEffectOp.StoreOlChuyuanAfterDamage)]
    private bool CanOfferOlChuyuan(ProgramTriggerLegalityContext g) => g.WindowContext is { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } target, Amount: > 0 } &&
        facts.Player(target).IsAlive && (OlChuPile(g.OwnerSeat, g.Candidate.SkillInstanceId) is not { } pile || facts.PileCards(pile).Count < facts.Player(g.OwnerSeat).MaxHp);
    public SkillProgramStepOutcome Resolve(ProgramSkillFrame frame, SkillProgramEffect effect) => BeginOlShenCaoPi(frame, effect);
    private SkillProgramStepOutcome BeginOlShenCaoPi(ProgramSkillFrame supplied, SkillProgramEffect e)
    {
        var f = frames.ActiveProgram(supplied.Id); var storage = e.Op == SkillProgramEffectOp.StoreOlChuyuanAfterDamage;
        if (f.OlShenCaoPi is not null || f.InstructionIndex != (storage ? 1 : 2) || !OlCaoPiParentMatches(f, storage))
            throw new InvalidOperationException("OL Shen Cao Pi lost his original unpaid damage or preparation binding.");
        var target = storage ? f.WindowContext!.TargetSeat!.Value : f.OwnerSeat;
        var pile = storage ? null : OlChuPile(f.OwnerSeat, f.SkillInstanceId);
        var cards = pile is null ? [] : facts.PileCards(pile).Select(c => c.Id).ToArray();
        var r = new OlShenCaoPiReceipt(OlCaoPiSource(f), f.GameplayHash, e.Op, f.WindowContext!.ParentFrameId, target,
            storage ? OlShenCaoPiStage.Drawing : OlShenCaoPiStage.AwakeningChildren, pile, facts.MovementSequence, facts.MovementSequence) { Cards = cards };
        frames.ReplaceTop(f = f with { OlShenCaoPi = r });
        if (storage) { draws.BeginOperationProgramDraw(f, target, 1, new("skill-program.ol:chuyuan.draw")); return SkillProgramStepOutcome.AwaitChild; }
        if (pile is not null && cards.Length > 0) operations.MoveCards(facts.PileCards(pile).ToArray(), pile.Location, CardLocation.Hand(f.OwnerSeat), new("skill-program." + f.SkillId + ".obtain-chu"));
        frames.ReplaceTop(f = frames.ActiveProgram(f.Id) with { OlShenCaoPi = r with { After = facts.MovementSequence } });
        events.AdvanceFact(new OlCaoPiPileObtainedEvent(f.Id, r.Source, cards.Length, r.Before, facts.MovementSequence));
        ResumeOlShenCaoPi(f.Id); return SkillProgramStepOutcome.AwaitChild;
    }
    private bool OlCaoPiParentMatches(ProgramSkillFrame f, bool storage)
    {
        if (f.WindowContext is not { } c || c.OwnerSeat != f.OwnerSeat || f.TriggerId is null) return false;
        if (!storage) return facts.ExactAwakeningParent(f);
        return c is { Window: SkillProgramTriggerWindow.AfterDamageApplied, TargetSeat: { } target, Amount: > 0 } &&
            frames.OfType<DamageTriggerWindowFrame>().SingleOrDefault(w => w.Id == c.ParentFrameId) is { } parent && parent.TargetSeat == target &&
            parent.CandidateIndex >= 0 && parent.CandidateIndex < parent.Candidates.Count && RuntimeSkills.RuntimeRules_Core_DamageTargetMountObserver.MountObserverCandidateMatches(f, parent.Candidates[parent.CandidateIndex].ToProgramCandidate()) &&
            parent.TriggerWindow == c.Window && c.DamageFrameId == parent.ParentFrameId;
    }
    private bool DrainOlShenCaoPi(long id) => operations.DrainChildWindows(id);
    [ProgramNativeDrawReturn(SkillProgramEffectOp.StoreOlChuyuanAfterDamage)] private void ReturnOlChuyuanDraw(ProgramSkillFrame f, NativeDrawOutcome o)
    {
        var r = f.OlShenCaoPi ?? throw new InvalidOperationException("Chuyuan's native draw lost its original frame receipt.");
        if (r.Stage != OlShenCaoPiStage.Drawing || o.RecipientSeat != r.TargetSeat || o.RequestedCount != 1 || o.SequenceAfter != facts.MovementSequence ||
            facts.History<OlChuyuanDrawReturnedEvent>().Any(e => e.FrameId == f.Id)) throw new InvalidOperationException("Chuyuan cannot repeat or redirect its real draw.");
        frames.ReplaceTop(f with { OlShenCaoPi = r with { Stage = OlShenCaoPiStage.DrawChildren, DrawOutcome = o } });
        events.AdvanceFact(new OlChuyuanDrawReturnedEvent(f.Id, r.TargetSeat, o.ActualDrawIds.Count, o.SequenceBefore, o.SequenceAfter)); ResumeOlShenCaoPi(f.Id);
    }
    private IReadOnlyList<PromptChoice> OlCaoPiChoices(ProgramSkillFrame f)
    {
        var r = f.OlShenCaoPi!;
        if (r.Stage == OlShenCaoPiStage.DepositChoice)
            return Array.AsReadOnly(facts.Hand(r.TargetSeat).Where(c => facts.StorageCard(r.TargetSeat, f.SkillId, f.SkillInstanceId, c, CardLocation.Hand(r.TargetSeat)))
                .Select(c => new PromptChoice(new($"ol-chuyuan:{f.Id}:{c.Id}"), "将【" + c.DisplayName + "】置为神曹丕的“储”。", [c.Id], [],
                    new Dictionary<string, string> { ["program-action"] = "ol-shen-cao-pi", ["value"] = c.Id.ToString(System.Globalization.CultureInfo.InvariantCulture) })).ToArray());
        if (r.Stage == OlShenCaoPiStage.SkillChoice)
            return Array.AsReadOnly(OlShenCaoPiComposition.TianxingReferences.Skip(1).Select(id => new PromptChoice(new($"ol-tianxing:{f.Id}:{id}"), "永久获得【" + facts.Skill(id).Name + "】。", [], [],
                new Dictionary<string, string> { ["program-action"] = "ol-shen-cao-pi", ["value"] = id })).ToArray());
        return [];
    }
    private void PublishOlCaoPiChoice(ProgramSkillFrame f)
    {
        var r = f.OlShenCaoPi!; var choices = OlCaoPiChoices(f);
        if (choices.Count == 0 || !facts.Player(f.OwnerSeat).IsAlive || !facts.Player(r.TargetSeat).IsAlive || facts.Winner != Winner.None) { FinishOlShenCaoPi(f); return; }
        var chooser = r.Stage == OlShenCaoPiStage.DepositChoice ? r.TargetSeat : f.OwnerSeat; var skill = facts.Skill(f.SkillId);
        frames.ReplaceTop(f with { Step = ResolutionFrameStep.AwaitingResponse });
        prompts.Set(new(DecisionKind.ProgramTrigger, chooser, r.Stage == OlShenCaoPiStage.DepositChoice ? "请选择一张手牌置为“储”。" : "请选择永久获得的技能。", choices.SelectMany(c => c.Cards).ToArray(), [], f.OwnerSeat)
        { PromptId = prompts.NextId(), IsPrivate = r.Stage == OlShenCaoPiStage.DepositChoice, Choices = choices, SkillPrompt = new(f.SkillId, skill.Name, skill.Name, skill.Description) }, facts.Player(chooser).IsHuman);
        operations.PublishState();
    }
    [ProgramAnswer("ol-shen-cao-pi")] private void AnswerOlCaoPi(PromptChoice choice)
    {
        if (frames.Top is not ProgramSkillFrame { OlShenCaoPi: { } r } f || !OlCaoPiChoices(f).Any(c => RuntimeSkills.RuntimeRules_Core_AssistedPhysicalCards.AssistedChoicesEqual([c], [choice])))
            throw new InvalidOperationException("OL Shen Cao Pi rejects a stale private deposit or permanent skill choice.");
prompts.Clear(); frames.ReplaceTop(f = f with { Step = ResolutionFrameStep.ResolvingEffect });
        if (r.Stage == OlShenCaoPiStage.DepositChoice)
        {
            var id = choice.Cards.Single(); var pile = operations.EnsurePileSource(f, int.MaxValue); var before = facts.MovementSequence;
            frames.ReplaceTop(f = f with { OlShenCaoPi = r with { Pile = pile, DepositedId = id, Before = before, After = before, Stage = OlShenCaoPiStage.DepositChildren } });
            operations.MoveCard(facts.Hand(r.TargetSeat).Single(c => c.Id == id), CardLocation.Hand(r.TargetSeat), pile.Location, new("skill-program.ol:chuyuan.deposit"), CardMovementDisclosure.FaceUp);
            frames.ReplaceTop(f = frames.ActiveProgram(f.Id) with { OlShenCaoPi = f.OlShenCaoPi! with { After = facts.MovementSequence } });
            events.AdvanceFact(new OlChuyuanDepositedEvent(f.Id, r.Source, r.TargetSeat, id, pile.Location, before, facts.MovementSequence));
        }
        else
        {
            var skill = choice.Parameters["value"];
            frames.ReplaceTop(f = f with { OlShenCaoPi = r with { AcquiredSkillId = skill, Stage = OlShenCaoPiStage.SkillChildren } });
            operations.GrantSkills(f, [skill]); events.AdvanceFact(new OlTianxingGrantedEvent(f.Id, f.OwnerSeat, skill));
        }
        ResumeOlShenCaoPi(f.Id);
    }
    [ProgramAiChoice(SkillProgramEffectOp.StoreOlChuyuanAfterDamage, SkillProgramEffectOp.AwakenOlTianxing)] private PromptChoice SelectOlCaoPiChoice(PendingDecision p, ProgramSkillFrame f) =>
        f.OlShenCaoPi!.Stage == OlShenCaoPiStage.DepositChoice ? p.Choices.OrderBy(c => facts.KeepValue(f.OlShenCaoPi.TargetSeat, facts.Hand(f.OlShenCaoPi.TargetSeat).Single(card => card.Id == c.Cards.Single()))).First() : p.Choices.First();
    [ProgramContinuation(ProgramContinuationKind.Resume, 40550)] private bool ResumeOlShenCaoPi(long id)
    {
        if (frames.Top is not ProgramSkillFrame { OlShenCaoPi: { } r } f || f.Id != id) return false;
        if (r.Stage == OlShenCaoPiStage.Drawing) throw new InvalidOperationException("Chuyuan must resume through its exact native draw return.");
        if (DrainOlShenCaoPi(id)) return true;
        f = frames.ActiveProgram(id); r = f.OlShenCaoPi!;
        if (r.Stage == OlShenCaoPiStage.DrawChildren)
            frames.ReplaceTop(f = f with { OlShenCaoPi = r = r with { Stage = OlShenCaoPiStage.DepositChoice } });
        if (r.Stage == OlShenCaoPiStage.AwakeningChildren && r.Operation == SkillProgramEffectOp.AwakenOlTianxing)
        {
            frames.ReplaceTop(f = f with { OlShenCaoPi = r = r with { Stage = OlShenCaoPiStage.SkillChoice } });
            operations.ObserveSkillRevisions();
            foreach (var grant in facts.Grants(f.OwnerSeat).Where(g => g.SkillId == "ol:chuyuan").ToArray()) operations.RemoveGrant(f.OwnerSeat, grant.GrantId);
            events.AdvanceFact(new CharacterSkillsLostEvent(f.OwnerSeat, f.OwnerSeat, Array.AsReadOnly(new[] { "ol:chuyuan" })));
            if (DrainOlShenCaoPi(id)) return true;
        }
        if (r.Stage is OlShenCaoPiStage.DepositChoice or OlShenCaoPiStage.SkillChoice) PublishOlCaoPiChoice(f); else FinishOlShenCaoPi(f);
        return true;
    }
    [ProgramContinuation(ProgramContinuationKind.Movement, 40550)] private bool ReturnOlCaoPiMovement(ProgramSkillFrame f) => f.OlShenCaoPi is not null && ResumeOlShenCaoPi(f.Id);
    private void FinishOlShenCaoPi(ProgramSkillFrame f) { frames.ReplaceTop(f with { OlShenCaoPi = null }); frames.AdvanceProgram(f.Id); }
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterOwnerDeath, 40550)]
    [ProgramRuntimeHook(ProgramRuntimePhase.AfterSkillLoss, 40550)] private bool RetainOlCaoPi(ProgramSkillFrame f) => f.OlShenCaoPi is not null;
}
