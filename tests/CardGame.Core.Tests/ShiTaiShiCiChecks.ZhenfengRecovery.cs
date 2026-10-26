using System.Text.Json;
using CardGame.Core;

internal static partial class ShiTaiShiCiChecks
{
    private const string RecoveryWatch = "fixture:shi-tai-shi-ci-recovery-watch";

    public static void ZhenfengRecoveryPaysOnceAndReturnsAfterHealthChildCold()
    {
        foreach (var initialHp in new[] { 2, 3 })
        {
            var s = Scene.Start("zhenfeng-recovery-" + initialHp, heroHp: initialHp, recoveryWatch: true);
            s.Play();
            s.Need(s.V(0).Hp == initialHp && s.V(0).MaxHp == 4,
                "The existing real mode starts the genuinely wounded four-maximum-HP owner.");
            var result = s.G.Submit(new UseProgramSkillCommand(0, Zhenfeng, "spearhead", [], [], s.G.Revision, s.P!.PromptId));
            s.Need(result.Accepted, result.Error?.Message ?? "The native Zhenfeng activation must be accepted.");
            s.Reach(() => s.P?.SkillPrompt?.SkillId == Zhenfeng &&
                s.P.Choices.Any(choice => choice.Parameters.GetValueOrDefault("branch") == "recover"));
            s.Cold();
            s.Answer(choice => choice.Parameters.GetValueOrDefault("branch") == "recover");
            s.Reach(() => s.P?.SkillPrompt?.SkillId == RecoveryWatch && s.P.Choices.Any());
            var owner = s.G.ResolutionStack.OfType<ProgramSkillFrame>().Single(frame => frame.SkillId == Zhenfeng);
            s.Need(owner.ShiTaiShiCi is { Operation: "zhenfeng", Stage: 12 } &&
                s.G.ResolutionStack.OfType<HpChangedTriggerWindowFrame>().Any(frame => frame.ResumeFrameId == owner.Id) &&
                s.V(0).Hp == 4 && s.E<RecoveryAppliedEvent>().Count(recovery => recovery.TargetSeat == 0 &&
                    recovery.Amount == 4 - initialHp) == 1 && s.G.ResolutionStack.All(frame => frame is not RecoveryFrame),
                "The shared host applies the clamped recovery once before the real HP-change child; the paid producer already holds its return stage.");
            s.Cold();
            s.Answer(_ => true);
            s.Play();
            s.Need(s.V(0).Hp == 4 && s.E<RecoveryAppliedEvent>().Count(recovery => recovery.TargetSeat == 0) == 1 &&
                s.G.ResolutionStack.All(frame => frame.Id != owner.Id && frame is not RecoveryFrame) &&
                !s.G.GetHumanLegalActions().Any(action => action.ProgramSkillId == Zhenfeng),
                "Returning from the actual health child completes the same paid skill without repeated recovery or an orphan primitive frame, and its limited use stays spent.");
            s.Cold(); s.Replay();
            s.End();
            s.Reach(() => s.G.State.CurrentSeat != 0);
            s.Need(s.G.ResolutionStack.All(frame => frame.Id != owner.Id && frame is not RecoveryFrame),
                "The real play phase can end through the unchanged clean native producer boundary after the paid recovery settles.");
        }
    }

    private sealed class ZhenfengRecoveryWatchPackage : IGameContentPackage
    {
        public PackageManifest Manifest { get; } = new("fixture:shi-tai-shi-ci-recovery-watch", new(1, 0, 0));
        public void Register(IContentRegistryBuilder builder)
        {
            var catalog = SkillProgramCatalog.Load($$"""
                {"schemaVersion":{{SkillProgramCatalog.RulesSchemaVersion}},"skills":[
                  {"id":"{{RecoveryWatch}}","revision":1,"triggers":[
                    {"id":"actual-recovery-child","window":"afterHealthChanged","subject":"owner",
                     "condition":{"kind":"healthChangeKindIs","hpChangeKind":"recovery"},"optional":false,
                     "effects":[{"op":"chooseOption","target":"owner","resultBind":"paid","options":[{"id":"continue"}]}]}
                  ]}
                ]}
                """, JsonSerializer.Serialize(new { schemaVersion = SkillProgramCatalog.PresentationSchemaVersion,
                    skills = new Dictionary<string, object> { [RecoveryWatch] = new { name = "回复已付子窗", description = "在实际回复后暂停一次。",
                        optionLabels = new Dictionary<string, string> { ["continue"] = "继续" } } } }));
            builder.AddSkill(new(RecoveryWatch, catalog.Presentations[RecoveryWatch].Name,
                catalog.Presentations[RecoveryWatch].Description) { Program = catalog.Programs[RecoveryWatch],
                ProgramPresentation = catalog.Presentations[RecoveryWatch] });
        }
    }
}
