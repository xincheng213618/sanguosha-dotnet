# Development gates

These are the detailed development, format, verification and artifact policies
referenced by [AGENTS.md](../AGENTS.md). Read the sections relevant to the task.
Setup and entry points are in [the developer quickstart](GETTING_STARTED.md).

## Formats and content

| Data | Version source | Change when |
| --- | --- | --- |
| Checkpoint | [GameCheckpoint.CurrentSchemaVersion](../src/CardGame.Core/Replay.cs) | The persisted structure changes in a way that needs versioned handling |
| Skill rules JSON | [SkillProgramCatalog.RulesSchemaVersion](../src/CardGame.Core/SkillPrograms.cs) | The rules JSON format changes incompatibly |
| Desktop save | [GameSaveFile.CurrentFormatVersion](../src/CardGame.Wpf/Persistence/GameSaveStore.cs) | The outer save format changes |
| Content package | The package's `CurrentVersion` | An intentional content release batch |
| AI decisions | [AiDecisionPolicy](../src/CardGame.Core/Replay.cs) | Replay needs to preserve decision-policy semantics |

- Do not add a global gameplay rules epoch or checkpoint `RulesVersion`. Replay saves under the current implementation with the existing schema, content and command validation; if the desktop cannot load or replay a save, initialize a fresh game without immediately overwriting the source save. Legacy skill JSON minimum-rules metadata is retained only for content fingerprint stability, never as a runtime gate. Raise `SkillProgramCatalog.RulesSchemaVersion` only for an incompatible rules JSON format; additive optional nodes can keep the existing schema. Change package versions for an intentional content release batch.

- Keep format version sources in `src/CardGame.Core/Replay.cs`, `src/CardGame.Core/SkillPrograms.cs`, and the relevant package's `CurrentVersion`. Do not copy current numbers into unrelated tests or development guides.

- Embedded rules omit the root rules version and load through `EmbeddedSkillProgramCatalog` with `SkillProgramCatalog.LoadCurrent`. External `Load` and Inspector validation still require an explicit supported rules version; do not add an optional-version fallback. Presentation and save formats keep their separate contracts.

- Test fixtures derive the current rules format from `SkillProgramCatalog.RulesSchemaVersion` or the shared fixture helper; keep numeric literals only when an intentionally incompatible format is the behavior under test. Batch incompatible rule-format retirements instead of bumping the format for each removed operation.

Checkpoints validate schema, package signatures, content hashes and individual
commands before replaying under the current implementation. On desktop load,
validation or replay failure, start a fresh game, stop automatic advancement and
protect the original save; see [save and resume](SAVE_AND_RESUME.md).

## Content development

Character files live in `src/Characters/<character>/<variant>/`:
`Content/*.character.json` declares identity and registration, `Skills/` holds
rules and presentation, and `Runtime/` holds independent rule capabilities.
Shared configuration lives in `_shared/Skills/`. Runtime modules compile into Core;
optional `Wpf/` code compiles only into WPF. See [character modules](CHARACTER_MODULES.md).

- Ordinary new generals and skills that use existing capabilities change their content and registration. Existing shared loader, registry, and mechanism checks cover pure configuration. Add a focused behavior check only for behavior that is not already covered or for a bug fix; do not require a duplicate definition snapshot for each character. Do not raise the global rules epoch, skill JSON schema, or content package version per character. The content hash already distinguishes content changes during development.

- Optimize expansion for complete generals delivered per wall-clock hour. When parallel work is authorized, assign independent generals to agents and serialize only conflicting shared integration and final verification; do not default to assigning all agents to one general. Prefer existing capabilities and make a minimal implementation compile and exercise its main flow early.

- Do not require a fixed number of checks per general or repeat shared privacy, forged-input, collection-freezing, and replay checks at every character prompt. Keep focused checks for new behavior and actual regressions, with representative recovery coverage for new pending state. Defer speculative cross-skill and boundary combinations to a separate mechanism batch; pure content reuse does not need a new character fixture.

For shared capability changes, cover node parameters, invalid contexts, resource
consumption and actual behavior, adding relevant privacy, zone conservation,
continuation and replay checks. Use [the capability method](content/skill-composition/CAPABILITY_MAP.md)
and the applicable [mechanism contracts](content/skill-composition/contracts/README.md).

## Verification workflow

[tools/Test-Changed.ps1](../tools/Test-Changed.ps1) needs Windows, PowerShell 7,
the SDK selected by [global.json](../global.json), and Python 3.10+ on PATH.
Python uses only the standard library to register temporary outputs.
The runner builds an isolated scope once per invocation and reuses its outputs.

- The sole routine gameplay acceptance runs one fixed-seed all-AI identity match through the public command API. It requires a normal victory and compares the winner and each seat's final snapshot after replaying the accepted commands from the same opening. A stall, exception or mismatch fails acceptance. Run it once when a gameplay batch is ready.

- Full verification is manual only: run `tools/Test-Changed.ps1 -Full` or any unfiltered test runner only when the user explicitly requests a manual full run. Do not automatically start full verification after a change, batch, development milestone, or routine-check failure. This restriction remains in force throughout the main development cycle and later follow-up cycles until the user explicitly cancels it; completing the main development cycle does not lift it automatically.

- For targeted diagnosis, use the existing Core and WPF checks with the relevant name filter. Add a focused check for uncovered behavior or a specific regression, using the same runner and filter syntax. Keep WPF checks specific to UI behavior. Fix a failing check or establish whether it already fails on the base commit.

Run from the repository root:

```powershell
# Run once after integrating a gameplay batch.
pwsh -File .\tools\Test-Changed.ps1 -CoreFilter 'Fixed-seed all-AI identity match and command replay'
# Examples of filters for targeted diagnosis.
pwsh -File .\tools\Test-Changed.ps1 -CoreFilter 'program interaction '
pwsh -File .\tools\Test-Changed.ps1 -WpfFilter 'selection, target toggle'
git diff --check
```

For documentation-only changes, check content, links and the intended diff; a
game build or test run is unnecessary. Do not turn a documentation edit into a
verification run against unrelated concurrent source changes.

Running the script without filters selects its historical diagnostic scope from
[VerificationScopes.psd1](../tools/VerificationScopes.psd1). It covers victory
rules, separation of rules and presentation, invalid definitions, and registration
IDs, references and fingerprints. When changing those smoke checks, keep their
test-process time near one second and measure build and runner overhead separately.

Multiple filters in one group run their union, with each check executed once.
Explicit empty filters and zero matches fail. Preserve the real failure count,
process exit code and build result when reporting failures.
[Inspect-SkillProgram.ps1](../tools/Inspect-SkillProgram.ps1) can check content
loading first; successful loading establishes only the checks it actually performs.
Before committing, run `git diff --check` and inspect the staged scope.

## Test maintenance

- Preserve intentional test pruning when merging older branches or worktrees. Carry over only new checks for uncovered behavior or regressions; do not restore an entire historical test suite to match an old test count or branch convention. Resolve test conflicts against the current retained coverage and remove obsolete registrations, methods, and unused fixtures together. Do not prune checks in the same batch as a large refactor.

- Preserve checkpoint, replay, content fingerprint, input validation, privacy, and card movement boundaries. Do not delete historical compatibility code or change stored data solely to reduce test count.

When removing obsolete helpers, inspect attribute discovery, reflection, extension
methods and cross-file partial references as well as ordinary calls.

## Temporary files and cleanup

The default verification directory is stable per repository:
`%TEMP%\CardGame-TestChanged-<repository-hash>`. Runs from the same repository
coordinate with a lock. The runner records bulky outputs in
[cleanup_queue.json](../tools/cleanup_queue.json), while `summary.json` and logs
stay in that output directory. A registration failure reports its path and fails
the run.

### Outputs and authorization

- Benchmark logs, verification summaries, screenshots, source snapshots, and recovery archives are temporary outputs. Use the ignored `.artifacts/benchmarks/` directory when they need a repository-local location; do not add them to Git or maintain references to them in documentation or source manifests. Keep lasting rule explanations and source citations in the relevant documentation instead.

- Generate release/publish outputs, installers, and distribution packages only when the user explicitly requests publishing or packaging. Ordinary development and verification must not run release/publish workflows. This restriction does not prohibit temporary archives used to complete the task.

- Temporary archives, repository/asset copies, worktrees, and isolated build/test outputs are allowed when useful; no separate permission is needed just to use them. Track exact paths and ownership. Reuse compatible checkouts and build outputs, respecting locks and concurrent work, instead of creating a new timestamp/GUID directory for each feature.

### Ownership and cleanup

- Automatically clean up this task's disposable temporary files and directories as soon as their purpose is complete, including `%TEMP%` diagnostics, isolated `.artifacts` build/test outputs, temporary archives, and small test fixtures. This standing instruction authorizes ordinary task-owned cleanup without another confirmation. Preserve deliverables, necessary diagnostic evidence, recovery backups, and outputs the user explicitly requested to keep before cleanup.

- Before recursive deletion, resolve and verify the exact absolute target is strictly inside the intended temporary/output root, check ownership, reject linked paths, and stop or wait for this task's own consumers. Respect verification locks and concurrent work. Use PowerShell cmdlets with `-LiteralPath`; never clear all of `%TEMP%`, delete by a broad `Card-*` wildcard, or delete another task's outputs/shared caches.

- Put automatic cleanup in `try/finally` where practical, covering successful and failed runs. Before the final response, verify that disposable outputs were removed. When resuming an interrupted task, inspect its known temporary paths and finish any permitted cleanup. Registering a path does not mean space has been freed.

### Retained outputs and the queue

- If cleanup is rejected as `blocked by policy`, do not retry the same deletion through a script, another tool, or a later task stage to bypass the rejection. If policy, permissions, locks, or necessary diagnostic retention prevent cleanup, register the exact retained directory with `python -B tools/cleanup_temp.py --add <absolute-directory> --root <verified-temporary-or-output-root> --reason <reason>`; include `--lock-root <artifacts-root>` for verification outputs. Report its path and reason in the final response.

- `tools/cleanup_queue.json` is the fallback UTF-8 text database for retained directories. Inspect it with `--list`/`--dry-run`; the user can double-click root `clean.cmd` or run `python tools/cleanup_temp.py` to retry cleanup. Agents must not run the entire historical queue automatically or use it to retry policy-blocked deletion. Remove only the matching records after successful deletion or verified absence, preserve unrelated entries, and retain failures for retry.

- Prefer the stable default directory of `tools/Test-Changed.ps1` and reuse its outputs during the task. The runner currently registers its `bin`, `obj`, and `renders` and retains them; this does not discharge the agent's cleanup responsibility. Once verification and inspection are complete, automatically remove this task's disposable outputs when permitted, preserving necessary summary/logs. Explicit `-ArtifactsPath` follows the same lifecycle; honor explicit user requests to retain outputs.

- Cleanup tests may mock deletion and must not remove real historical queued directories. Automatically clean up their own disposable fixture directories after testing when permitted; register them if cleanup is blocked.

Inspect or preview the queue without executing historical cleanup:

```powershell
python -B tools/cleanup_temp.py --list
python -B tools/cleanup_temp.py --dry-run
```

The user can double-click [clean.cmd](../clean.cmd) to retry the queue.
