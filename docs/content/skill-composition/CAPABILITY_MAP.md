# 基础能力：方法与地图

先定位现有能力和实际消费者，再决定是否需要扩展。机制的归属、现行规则、例外和剩余缺口由对应契约维护。

## 干净的架构是什么样

判断架构是否干净，靠读代码回答下面这些问题，而不是看行数或指令数：

- **一个概念，一个归属**：每个规则概念（技能有效性、使用合法性、获得、交给、距离……）只有一个拥有者。“这个技能现在有效吗”“这张牌能用吗”这类问题，在引擎中只有一个地方回答。
- **依赖方向单一**：规则概念不知道具体武将；武将依赖概念，武将之间互不调用。共享查询里没有人物分支。
- **层次分明**：每条规则落在它该在的层——触发时机、效果步骤、持续规则（修正 / 查询）、流程（顺序、分支、逐个结算）。“此次伤害 +1”是修正，不是伤害步骤；“不能使用”是查询，不是执行动作。
- **技能读起来像规则原文**。简单技能用 JSON 组合已有能力，复杂技能可直接写成类型化 C#。在一处能读懂这个技能的完整规则。
- **改动是局部的**：改一个概念时不用碰技能；加一个技能时不用碰引擎或其他技能；新老技能的交互由概念保证，而不是靠彼此打补丁。
- **只有一条路径**：同一件事没有新旧两套实现并存，也没有为过渡留下的适配层。
- **信任边界清楚**：校验只放在边界（命令输入、加载、存档恢复、执行器），内部代码互相信任，不重复自证。

检查本批的依赖方向时，确认共享能力独立于具体人物，人物实现只依赖共享能力。Entries 和 DeferredRuntime 解包后仍按实际目标记依赖；把调用隐藏到上下文、服务定位器、通用查询接口或另一个人物包装器中不算消除。现有机制检查的存量基线只约束不新增及及时收缩，不能拿基线通过替代这项完成标准。反向依赖基本消失后，再把剩余注入按真实概念收成能力接口，不单独追求构造参数变短。

核对一个新武将需要增加哪些代码。独有规则写在武将实现中。每个武将都要重复添加的专属指令、适配层、转发和自证逻辑，应在本批涉及的路径中简化。

行数和指令数只是表现，不是目标。多数情况下，架构变干净后体积会变小；但如果增加代码能让结构更干净，例如补一个完整的概念、明确一个回答点、拆开一个焊在一起的指令，就应该增加。

## 发现缺口的信号

遇到以下情况时，先看本次规则能否复用已有能力，再判断是否需要补齐共享能力。

- 准备新增的指令名里含有武将名、技能名，或含 Then / And / Or / If / When。
- 同一个规则术语已经有两个以上专属指令或专属实现。
- 一个共享查询里出现了针对某个人物的分支。
- 回答同一个“能不能 / 是不是”的问题，需要查看多个地方。
- 为了让新技能和老技能正确交互，需要修改老技能的代码。
- 写 JSON 时必须借助一个只服务本技能的 C# 指令，否则无法表达。

## 判断：封装、组合还是保留

- **封装成基础能力**。当前改动体现了可复用的规则语义时，由对应模块提供统一操作或查询，例如获得、交给、限制使用。
- **用组合表达**。已有能力能直接表达的顺序、分支和条件，用 JSON 步骤序列和 `expression` 组合。
- **保留 C#**。复杂流程可由武将用类型化 C# 调用共享能力，技能的条件、顺序和分支留在同一处。

使用次数只用来发现问题（同一概念出现多种写法），不作为是否封装的依据。

## 补齐一个概念的步骤

1. **确认范围**。阅读本批涉及的规则和调用方，明确动作、对象、时机以及需要保留的交互。语义不清时再查其他用例。
2. **定义**：确定最小的参数集合、唯一的回答点，以及它分别落在哪一层：效果步骤、持续规则（modifier / 查询）还是触发窗口。参照已有的同类能力，复用现有存储和协议，不另建体系。
3. **实现并迁移**。接入本批涉及的调用方，移除由此失去用途的专属指令、分散判断和人物分支。其余等价调用方可分批迁移。
4. **锁定**：机制归属、采样时点、生命周期、例外或剩余缺口发生变化时，更新[对应契约](contracts/README.md)。本文随设计方法或主题入口变动更新。
5. **复核本批改动**。对照规则原文读一遍技能实现，确认共享规则有明确归属，旧的专属转发或重复判断已从迁移路径中移除。

## 选择下一个概念

优先处理离“干净”最远、交互面最大的概念：同一概念的写法越多，被其他规则查询得越频繁，就越优先。每完成一个概念后重新评估顺序。日常新增武将时发现的缺口，记入对应契约，按同样的标准排队。

## 概念总览

| 要找的能力 | 当前入口 |
| --- | --- |
| JSON 节点、表达式、绑定 | [规则配置](RUNTIME.md)、[表达式](RUNTIME_EXPRESSIONS.md) |
| 命令、隐私、帧续接、事实采样、目录 | [运行时边界](contracts/RUNTIME_BOUNDARIES.md) |
| 授技、失效、寿命、标记、手牌上限、装备 | [技能与已施加状态](contracts/SKILLS_AND_STATES.md) |
| 用牌和目标准入、范围、次数、原生杀请求 | [用牌合法性](contracts/USE_LEGALITY.md) |
| 实际用牌与阶段、原生返回、再次结算、回复 | [用牌历史与结算](contracts/USE_RESOLUTION.md) |
| 实体、材料领取与退休、获得和交给 | [材料与移动](contracts/MATERIALS_AND_MOVEMENT.md) |
| 响应、无效、游标暂停、手牌展示 | [响应与可见性](contracts/RESPONSES_AND_VISIBILITY.md) |
| 格式、指纹、验证 | [开发门禁](../../DEVELOPMENT_GATES.md) |

过去的迁移过程和逐批消费者核对见 [Git 历史中的能力地图](https://github.com/xincheng213618/sanguosha-dotnet/blob/77ab25557053b2e6c1033af6c6f392484c5b71dd/docs/content/skill-composition/CAPABILITY_MAP.md)。

## 小核心的职责与接管顺序

小核心用一组与武将无关的规则运行真实牌局。武将提供发动条件、选择和效果组合，模式提供开局、额外规则与胜负条件。核心按本局实际生效的技能工作，包括副将、装备授技和中途获得或失去的技能。普通技能用 JSON 组合已有动作，特殊技能可在一处用类型化 C# 调用同一批动作。

下表列出当前生产入口和待迁移的职责。每行是一项规则职责，可以由多个现有模块共同承担。

| 职责 | 可直接沿用的生产入口 | 仍分散的职责或上层边界 |
| --- | --- | --- |
| 对局状态、命令与观察 | [GameEngine.Submit](../../../src/CardGame.Core/GameEngine.cs)、[按座位投影视图](../../ARCHITECTURE.md)、[确定性随机源](../../../src/CardGame.Core/DeterministicRandom.cs)、[GameReplay.Restore](../../../src/CardGame.Core/Replay.cs)。[MatchModeRuntime](../../../src/CardGame.Core/Modes/MatchModeRuntime.cs) 装配本局模式。 | 座位、体力、牌区与结算进度有统一状态。身份局的胜负条件属于模式。AI、WPF 共用命令与可见视图，继续以这个入口检验迁移。 |
| 实体牌、牌区与移动 | [AtomicCardMovementRuntime](../../../src/CardGame.Core/Runtime/Rules/AtomicCardMovementRuntime.cs) 执行单张及批量移动，记录实际去向，[摸牌](../../../src/CardGame.Core/NativeDrawProducerRuntime.cs)和[逻辑弃置](../../../src/CardGame.Core/Runtime/Rules/NativeLogicalDiscardRuntime.cs)已有独立语义。杀、群体牌和普通用牌共用终弃记录。[移动界固政](../../../src/Characters/zhang-zhao-zhang-hong/boundary-mobile/Runtime/CharacterZhangZhaoZhangHongBoundaryMobileRuntime.BoundaryZhangZhaoMobile.cs)使用[实际阶段弃牌查询](../../../src/CardGame.Core/Runtime/Rules/ActualDiscardRecoveryPhaseRuntime.cs)和原帧上的实际归还结果。 | 移动返回记录实际去向，后续领取检查原入堆记录。各技能分别确定自己要等待的子窗口顺序。 |
| 数值、限额与规则修正 | [MatchTurnNumericQueries](../../../src/CardGame.Core/Runtime/Rules/MatchTurnNumericQueries.cs)计算实际摸牌数和出杀次数，[MatchHandLimitQueries](../../../src/CardGame.Core/Runtime/Rules/MatchHandLimitQueries.cs)计算手牌上限。[界英姿](../../../src/Characters/zhou-yu/boundary/Skills/boundary-zhou-yu.rules.json)和[界将驰](../../../src/Characters/cao-zhang/boundary/Skills/boundary-cao-zhang.rules.json)声明加值。[曹羲刚述](../../../src/Characters/cao-xi/ol/Runtime/CharacterCaoXiOlRuntime.CaoXiLedger.cs)与[修耕](../../../src/Characters/guo-yuan/mobile-shi/Runtime/CharacterGuoYuanMobileShiRuntime.MobileShiGuoYuan.cs)通过同一数值入口注册。[厉兵](../../../src/Characters/xiahou-ba/mobile-shi/Runtime/CharacterXiahouBaMobileShiRuntime.MobileShiXiahouBa.cs)注册到[攻击范围查询](../../../src/CardGame.Core/Runtime/Rules/MatchDistanceQueries.cs)的有限加值阶段。摸牌的装备项、席距、攻击范围和在场装备资格共用[场上装备的有效牌面读取](../../../src/CardGame.Core/EngineOwners/CoreGameEngineFieldEquipmentRuntime.FieldEquipment.cs)。 | 其他具名加值可以分批迁入。攻击范围数值、距离、指定目标免限和手牌免计保留各自语义。 |
| 技能装配、触发、选择与续接 | [MatchSkillBindingIndex](../../../src/CardGame.Core/MatchSkillBindingIndex.cs) 和 [ProgramTriggerRuntime](../../../src/CardGame.Core/Runtime/Rules/ProgramTriggerRuntime.cs)按席位及时机取有效技能。[SkillProgramExecutor](../../../src/CardGame.Core/SkillProgramExecutor.cs)运行步骤、处理选择和等待。[ProgramStageRuntime](../../../src/CardGame.Core/ProgramStageRuntime.cs)接管移动界固政的移动续接。[程序执行入口](../../../src/CardGame.Core/EngineOwners/CoreNativeFrameRoutingRuntimeRuntime.NativeFrameRoutingRuntime.cs)在继续或执行前让已注册的观察者从原帧发出发动子窗。 | 移动界固政保留选择及子窗口顺序。周年神华佗的发动资格与调用指纹仍归原规则。 |
| 用牌、打出、转化、目标、响应与判定 | [原生用牌](../../../src/CardGame.Core/EngineOwners/CoreNativeCardUseRuntime.Main.cs)、[目标准入](../../../src/CardGame.Core/EngineOwners/CoreNativeTargetLegalityRuntime.Main.cs)、[闪响应](../../../src/CardGame.Core/EngineOwners/CoreNativeDodgeRuntime.Main.cs)、[原生判定](../../../src/CardGame.Core/EngineOwners/CoreNativeJudgmentRuntime.Main.cs)已有实际调用。[共用改判模块](../../../src/CardGame.Core/EngineOwners/CoreGameEngineJudgmentReplacementProgramsRuntime.JudgmentReplacementPrograms.cs)处理候选、盲顶提示及其原判定付款。[武圣](../../../src/Characters/guan-yu/classic/Skills/classic-wusheng-skills.rules.json)直接声明红牌视为杀并可用于主动用牌和响应。 | [合法动作枚举](../../../src/CardGame.Core/EngineOwners/CoreNativeLegalActionsRuntime.Main.cs)仍有具名角色分支。[移动王异](../../../src/Characters/wang-yi/mobile/Runtime/CharacterWangYiMobileRuntime.MobileWangYi.cs)按公开判定提供秘计的盲顶 AI 选择。[严畯](../../../src/Characters/yan-jun/ol/Runtime/CharacterYanJunOlRuntime.YanJunPrograms.cs)直接调用赵俨记录同协。用牌与单纯打出需保留各自的时机、次数与发起者。 |
| 伤害、失去体力、回复、濒死与死亡 | [伤害落定](../../../src/CardGame.Core/EngineOwners/CoreNativeDamageApplicationRuntime.Main.cs)在原伤害帧记录实际体力，[煞决](../../../src/Characters/xu-rong/ol/Runtime/CharacterXuRongOlRuntime.XuRongObservers.cs)从原濒死父帧读取。[濒死救援](../../../src/CardGame.Core/EngineOwners/CoreNativeDyingRuntime.Main.cs)和[死亡帧](../../../src/CardGame.Core/EngineOwners/CoreNativeDeathFramesRuntime.Main.cs)各有入口。普通桃和技能 `Recover` 共用[原回复帧的体力写入](../../../src/CardGame.Core/EngineOwners/CoreNativeRecoveryFramesRuntime.Main.cs)。[苦肉](../../../src/Characters/huang-gai/classic/Skills/classic-kujin-skills.rules.json)用“失去体力、摸牌”两个步骤表达。 | 其他具名伤害观察仍可分批迁入。受伤、失去体力、回复替换和成功救援各有独立时机。 |
| 回合、阶段和持续状态 | [阶段身份](../../../src/CardGame.Core/Runtime/Rules/ActualDiscardRecoveryPhaseRuntime.cs)区分本次实际阶段。[弃牌阶段入口](../../../src/CardGame.Core/EngineOwners/CoreActualDiscardPhaseRuntime.Main.cs)接管自动、手动收尾及已受理的同步跳过，再归回合结束。[弃牌前政策](../../../src/CardGame.Core/EngineOwners/CoreActualDiscardPhaseRuntime.NativeDiscardEntry.cs)在原入口帧保留选择和发动子窗，[移动克己](../../../src/Characters/lu-meng/mobile/Skills/mobile-lu-meng.rules.json)直接声明这个时点。[技能变更](../../../src/CardGame.Core/Runtime/Rules/ProgramSkillChangeRuntime.cs)按声明期限处理[孔武](../../../src/Characters/hu-che-er/mou/Runtime/MouHuCheErRuntime.cs)的装备失效，使用受影响者下一次实际回合结束边界。 | [肆恣](../../../src/Characters/zhong-hui/mobile-shi/Runtime/CharacterZhongHuiMobileShiRuntime.MobileShiZhongHuiLedger.cs)仍自己处理每次付款的剩余结算次数和到期。阶段核心负责开始、跳过和结束，技能保留发动条件及期限。 |

### 从真实对局迁移

现有命令、阶段身份、技能索引和步骤执行器从第一批起就直接使用。以下顺序针对尚需接管的职责。

1. **实体牌与实际移动结果。** 统一用牌终弃，让定西和界严颜“乘势”读原用牌交回的结果。用决斗、南蛮入侵、万箭齐发及杀覆盖不同结算入口。随后核对经典与移动界固政的弃牌范围、归还改向及余牌领取，收掉本批重复的阶段记录。牌从未离开和离开后重入应得出不同结果。
2. **摸牌数与出杀次数。** 从 OL 曹羲刚述的一项具名加成起步，武将保留加成与期限，现有查询统一计算，删掉此次迁移的中央人物分支。用界英姿或界将驰作对照。正常摸牌数、额外执行一次摸牌、对指定目标免限，分别按各自规则验证。手牌上限和距离随后按同样方法接入。
3. **触发、选择和步骤续接。** 从已接入移动能力的技能选一条“选牌 → 移动 → 等子结算 → 继续”的路径。把通用等待交给现有运行器，武将保留自己的选择和先后顺序。移动界固政与通用流程的子窗口次序有差异，先确认差异再迁移。
4. **用牌、响应与判定。** 从武圣的普通转化和八卦判定切入。让公布候选、提交动作和支付沿用相同的资格与实体，实际用牌、单纯打出各按自己的时机和次数处理。每个观察者只读取原动作，逐步去掉人物间的调用。
5. **伤害与体力。** 先让普通桃和一个技能回复共享实际增加量与写入，再迁一条伤害落定观察。用苦肉检查失去体力的触发，用濒死救援检查子结算返回后是否只继续一次。
6. **阶段结果与持续状态。** 先统一弃牌阶段正常完成和跳过时共有的结束处理，再迁一个按阶段或回合到期的效果。以额外出牌阶段、同一角色再次发动、不同来源授予同一技能区分各次生效范围。少见模式和武将独有效果可以继续走当前生产路径。

一人负责当批共享入口，其余人并行接入或验证独立消费者。每批说明哪段旧判断已删、下一个同类武将能直接使用什么。沿用[现有游戏验收](../../../AGENTS.md#verification)，从公开命令完成固定种子全 AI 身份局，并从相同开局重放已接受的命令，比对胜者和各席可见终局。
