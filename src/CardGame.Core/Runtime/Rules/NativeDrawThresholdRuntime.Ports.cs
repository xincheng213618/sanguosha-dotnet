namespace CardGame.Core;

// Neighboring damage machinery and character-native paid receipts keep their
// existing owners; threshold qualification and stack authority live in the module.
internal interface INativeDrawThresholdNativeOperations : IProgramRuntimeOperations
{
    SkillProgramStepOutcome BeginDamage(ProgramSkillFrame frame, int target, int amount);
    bool CompletedNativeDrawMatches(ProgramSkillFrame producer, NativeDrawInvocationProof proof);
}
