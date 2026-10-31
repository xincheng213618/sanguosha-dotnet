# 人物 Runtime 向共享动作与 JSON 迁移

每批按真实退役方法和 `src` C# 净变化计量，20 处作为收益参考。资格留在原阶段；规则差异登记，不合并成伪共享动作；不剪测试。

## 历史查询分类（本批）

基线 `7eb746654`；现有 Roslyn 工具 `--history` 模式解析人物 Runtime，排除 Contracts、DependencyAdapters、StateContracts、NativeOwners。共 4,856 个实际 accessor 调用（typed 4,294 / 聚合 562）、3,014 方法 / 34,743 行；原 3,604 是全 Runtime 的匹配物理行数。以下为保守自动分类，不把 Paid/Proof 方法名当证据。

| 分类 | 查询数 | 类内去重的方法行数 | 3 个例子（基线 src/Characters） |
| --- | ---: | ---: | --- |
| a 本帧自身步骤/支付证明 | 1,344（typed 1,249 / 聚合 95） | 12,110 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoObserversRuntime.OlMoRumoObservers.cs:18`；`a-hui-nan/ol/Runtime/CharacterAHuiNanOlRuntime.AHuiNan.cs:74`；`a-hui-nan/ol/Runtime/CharacterAHuiNanOlRuntime.AHuiNanInvariants.cs:11` |
| b 规则需要的时域/跨帧历史 | 702（typed 627 / 聚合 75） | 5,405 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:45`；`bei-mi-hu/ol/Runtime/CharacterBeiMiHuOlRuntime.BeiMiHuPrograms.cs:12`；`bei-mi-hu/ol/Runtime/CharacterBeiMiHuOlRuntime.BeiMiHuPrograms.cs:24` |
| c 其他及待判 | 2,810（typed 2,418 / 聚合 392） | 23,203 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoObserversRuntime.OlMoRumoObservers.cs:11`；`_shared/Runtime/CharacterSharedGameEngineOlMoRumoProofsRuntime.OlMoRumoProofs.cs:11`；`a-hui-nan/ol/Runtime/CharacterAHuiNanOlRuntime.AHuiNan.cs:18` |

各类方法行数会交叠，不能相加作删行预算；(a) 仅明确识别 27.68%，涉及 826 方法，只有 (a) 查询的方法为 636 个 / 8,165 行；并未证实其占多数。(c) 为明确其他 9、待判 2,801，含未追清的别名、原生父子身份和混合消费，不将待判项伪装成规则历史。用户据此明确继续两个已核实消费者；本帧回执查询也不一定重复。

## 步骤回执与原契约

签发：InstructionIndex 在执行前推进，不能充当付款。实际应用：复用原帧 `ProgramSelectedCardPayment.MovementCommitted`，在原生物理移动回调写实际牌、来源、终点、批次和序号；到 Processing、OutsideGame 或保留在原处不等于送达。子结算返回：复用 ActiveChildFrameId / LastCompletedChildFrameId 和 `ProgramSelectedCardPaymentResult`，不得重付；不与 PendingMovementContinuation 并存。
许可与事实分开：activation 的 `continueAfterSkillLossAfter` 显式引用送达步骤的 resultBind，只有本次实际送达后才能履约本 activation 的剩余步骤；默认不授权失技续行，也不授权 owner 死亡续行。子窗口再次移走牌，不撤销已提交的送达。安恤在全部赠牌子窗口结束后比较双方当前手牌并发布选择；回答时不重新比较。
必须保留原帧/owner/技能实例、玩法哈希、动态选牌与支付合法性、原生 Give 或装备置入语义、typed child 返回、私密集合冻结与冷恢复。可删除的是人物再发专属付款事件、扫历史并逐项对照同一移牌记录、游标和身份的重复证明；有外部规则消费者的事件不能删。
其他内建步骤复用已有承载：选择用 ChoiceBindings / CardSetBindings；摸牌用 NativeDraw、RecipientApplied 与 DrawCompleted；伤害用 AttackAttempt / AttackReturn；给予保留 native give / gain settlement。本批只接通安恤所需的选牌→移牌→原帧返回，不另铺通用账本。

## 本批范围与保留项

- `mobile:anxu` 与 `boundary:zhijian-mobile`：实际送达后失技履约，owner 死亡无额外许可；各自 Give / 装备置入身份不变。人物 Delivered / Paid / Compared 事件没有外部消费者，随重复证明和续接一起退役。
- `boundary:anxu-mobile`：原来源移出即可保留，Delivered 另决定奖励；其默认 movement return 还要求 donor 存活并检查 source。与本批不同，保持原行为。
- `mobile:youdi`、`boundary:jianchu-mobile`、`mou-mobile:liuli`：分别还依赖逻辑弃牌、当前 Slash Use/Action 身份、死亡后重定向许可，不用送达回执替换。
- (b) 规则历史读取留待下一批，本批不增加表达式历史读取。

本批完成：两个消费者共删 3 个自定义 Event、2 个 Paid 方法、2 个失技保留钩子、2 个移牌续接，安恤 Runtime 3 文件全部退役。人物 C# 删 89 行；共享增 85 / 删 23，净增 62；`src` C# 净减 27 至 552,492 行，含 JSON 净减 26 行。8 个代表场景、22 项相关 Core 和例行 4 项通过，未运行全量。
当前样本（C# 文件 / 行 / 字节）：`bao-xin` 13 / 656 / 55,859；`liao-hua` 3 / 159 / 13,767；`xun-yu` 19 / 1,133 / 127,949。分类、回执与许可的范围相互独立，不把查询归为 (a) 就视为可直接删除。

## 既有批次记录

首批迁屯田两版、勤国、秉壹共 4 技能，`src` C# 净减 114 行；另删 967 个未用 NativeOwners / 工厂净减 11,028 行，后者不计业务 JSON 迁移。此前“选择→摸牌”批完成 19 个执行方法，人物 C# 净减 369、共享净增 59，`src` C# 净减 310 至 552,519 行；详见下列基线排名与批次结果。

## 模式排名（上一批）

Roslyn 基线 `e27ab1a64`：20,564 方法、159,052 方法体行、2,626 个含方法文件。共享调用按实际 callee 绑定，变量/人物前缀归一；先保留控制骨架与精确变体，再按动作/查询族及注册边界合并。定义校验、来源元数据、桥接与 AI 策略另列，不当业务动作。
下列是按方法数排序的非等价候选范围，行数为方法体 span 合计，不能视作可删除量。A 表示已有基础操作，不承诺整个族自动迁移；例子为基线文件行号。工具：`tools/SkillMigrationAnalysis/`。

| # | 一句话模式 | 方法 / 行 | 现有能力或最小缺口 | 3 个例子（相对 src/Characters） |
| --- | --- | ---: | --- | --- |
| 1 | 资格：聚合已发生历史 | 583 / 670 | B：冻结历史范围/过滤读取 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:24`；`bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYunNative.cs:69`；`bu-lian-shi/mobile/Runtime/CharacterBuLianShiMobileRuntime.MobileBuLianShi.cs:15` |
| 2 | 资格：带守卫聚合历史 | 198 / 1590 | B：历史读取及原资格阶段 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoObserversRuntime.OlMoRumoObservers.cs:6`；`a-hui-nan/ol/Runtime/CharacterAHuiNanOlRuntime.AHuiNan.cs:69`；`bu-lian-shi/boundary-mobile/Runtime/CharacterBuLianShiBoundaryMobileRuntime.BoundaryBuLianShi.cs:47` |
| 3 | 等待原生子窗口返回 | 164 / 164 | B：接回同帧续接，保留已付状态 | `bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYun.cs:93`；`bao-san-niang/ol/Runtime/CharacterBaoSanNiangOlRuntime.Bao2025.cs:240`；`bao-xin/ol/Runtime/CharacterBaoXinOlRuntime.BaoXin.cs:118` |
| 4 | 带条件读取执行历史 | 149 / 662 | B：共享历史范围/过滤 | `cai-yang/anniversary/Runtime/CharacterCaiYangAnniversaryRuntime.AnniversaryCaiYangProofs.cs:14`；`cai-yang/anniversary/Runtime/CharacterCaiYangAnniversaryRuntime.AnniversaryCaiYangSource.cs:92`；`cai-yong/le/Runtime/CharacterCaiYongLeRuntime.LeCaiYongJiaowei.cs:12` |
| 5 | 移牌/伤害/摸牌混合查询 | 115 / 131 | B：混合族需拆分，不能当单动作 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoObserversRuntime.OlMoRumoObservers.cs:22`；`_shared/Runtime/CharacterSharedGameEngineOlMoRumoObserversRuntime.OlMoRumoObservers.cs:23`；`cai-yong/le/Runtime/CharacterCaiYongLeRuntime.LeCaiYongFeibai.cs:369` |
| 6 | 筛选候选后选择目标/牌 | 109 / 665 | A：基础选择已有，复杂候选保留 | `bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYun.cs:47`；`bu-lian-shi/boundary-mobile/Runtime/BoundaryBuLianShiPrograms.cs:48`；`cao-ang/mou/Runtime/MouCaoAngRuntime.cs:40` |
| 7 | 历史限定的选择资格 | 99 / 129 | B：历史限定候选读取 | `bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYunNative.cs:71`；`bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYunProofs.cs:33`；`bao-xin/ol/Runtime/CharacterBaoXinOlRuntime.MutaoQualifications.cs:6` |
| 8 | 写帧并签发带历史的事实 | 95 / 1079 | B：原返回时点的共享事实接点 | `bu-lian-shi/boundary-mobile/Runtime/CharacterBuLianShiBoundaryMobileRuntime.BoundaryBuLianShi.cs:29`；`cao-pi/anniversary-god/Runtime/CharacterCaoPiAnniversaryGodRuntime.ShenCaoPi.cs:167`；`cao-zhi/le/Runtime/CharacterCaoZhiLeRuntime.LeCaoZhiWenlanLegacyDuel.cs:93` |
| 9 | 签发结束事实并完成技能 | 95 / 509 | B：专属结束事实/清理语义 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:97`；`a-hui-nan/ol/Runtime/CharacterAHuiNanOlRuntime.AHuiNan.cs:170`；`bao-san-niang/ol/Runtime/CharacterBaoSanNiangOlRuntime.Bao2025.cs:400` |
| 10 | 带守卫筛选历史候选 | 93 / 750 | B：历史范围与候选读取 | `bai-ling-yun/anniversary/Runtime/CharacterBaiLingYunAnniversaryRuntime.AnniversaryBaiLingYunProofs.cs:6`；`cai-wen-ji/le/Runtime/CharacterCaiWenJiLeRuntime.LeCaiWenJiBeifen.cs:28`；`cai-wen-ji/le/Runtime/CharacterCaiWenJiLeRuntime.LeCaiWenJiBeifen.cs:123` |
| 11 | 注册钩子中的历史资格 | 85 / 88 | B：保持钩子阶段的事实读取 | `bu-lian-shi/boundary-mobile/Runtime/CharacterBuLianShiBoundaryMobileRuntime.BoundaryBuLianShi.cs:58`；`bu-lian-shi/mobile/Runtime/CharacterBuLianShiMobileRuntime.MobileBuLianShi.cs:16`；`cao-ang/mou/Runtime/MouCaoAngRuntime.cs:82` |
| 12 | 目标或牌选择资格 | 83 / 96 | A：已有目标类型；缺口逐实例核对 | `bu-zhi/ol/Runtime/CharacterBuZhiOlRuntime.Dingpan.cs:8`；`cao-cao/ol-magic/Runtime/CharacterCaoCaoOlMagicRuntime.MoCaoCaoEquipment.cs:41`；`cao-cao/ol-magic/Runtime/CharacterCaoCaoOlMagicRuntime.MoCaoCaoRuntimeServices.cs:109` |
| 13 | 移牌/健康后等待子窗口 | 77 / 79 | A：已有动作；自定义履约保留 | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:71`；`cao-ang/mou/Runtime/CharacterCaoAngMouRuntime.MouCaoAngServices.cs:31`；`cao-cao/ol-magic/Runtime/CharacterCaoCaoOlMagicRuntime.MoCaoCaoRuntimeServices.cs:115` |
| 14 | 历史查询与异常守卫 | 67 / 370 | B：共享冻结事实读取 | `cao-ang/mou/Runtime/MouCaoAngRuntime.cs:67`；`cao-ang/mou/Runtime/MouCaoAngRuntime.cs:74`；`cao-pi/ol-god/Runtime/OlShenCaoPiRuntime.cs:42` |
| 15 | 按条件构造选择项 | 64 / 115 | A：chooseOption；部分选项条件缺表达式 | `bao-xin/ol/Runtime/CharacterBaoXinOlRuntime.Yimou.cs:6`；`cao-cao/ol-magic/Runtime/MoCaoCaoBachaoRuntime.cs:45`；`cao-hong/mou/Runtime/MouCaoHongRuntime.cs:39` |
| 16 | 清理原帧并完成技能 | 63 / 253 | A：既有游标结束；专属状态仍需处理 | `bei-mi-hu/ol/Runtime/CharacterBeiMiHuOlRuntime.BeiMiHuPrograms.cs:209`；`bu-zhi/ol/Runtime/CharacterBuZhiOlRuntime.BuZhiPrograms.cs:241`；`cao-shuang/mobile-shi/Runtime/CharacterCaoShuangMobileShiRuntime.MobileShiCaoShuang.cs:448` |
| 17 | 推进/替换帧游标 | 61 / 252 | B：自定义阶段到原游标的连接 | `cao-pi/anniversary-god/Runtime/CharacterCaoPiAnniversaryGodRuntime.ShenCaoPi.cs:178`；`cao-zhen/mobile-shi/Runtime/CharacterCaoZhenMobileShiRuntime.MobileShiCaoZhen.cs:155`；`chen-qun/mobile-shi/Runtime/CharacterChenQunMobileShiRuntime.MobileShiChenQun.cs:117` |
| 18 | 历史筛选后选择 | 59 / 348 | B：历史候选绑定 | `cai-yang/anniversary/Runtime/CharacterCaiYangAnniversaryRuntime.AnniversaryCaiYangSource.cs:55`；`cao-xian/ol/Runtime/CaoXianRuntime.cs:47`；`cao-zhen/mobile-shi/Runtime/CharacterCaoZhenMobileShiRuntime.MobileShiCaoZhenLedger.cs:12` |
| 19 | 更新帧并签发规则事件 | 58 / 312 | B：共同事实的发布接点 | `bao-xin/ol/Runtime/CharacterBaoXinOlRuntime.BaoXin.cs:71`；`cai-yang/anniversary/Runtime/CharacterCaiYangAnniversaryRuntime.AnniversaryCaiYang.cs:116`；`cao-pi/anniversary-god/Runtime/CharacterCaoPiAnniversaryGodRuntime.KemingSkillEcho.cs:77` |
| 20 | 读取拥有区域牌作资格 | 57 / 77 | A：已有区域/种类过滤，逐处核对 | `bao-san-niang/ol/Runtime/CharacterBaoSanNiangOlRuntime.Bao2025Observers.cs:100`；`bu-zhi/ol/Runtime/ProgramBuZhiSelection.cs:14`；`cai-fu-ren/ol/Runtime/CharacterCaiFuRenOlRuntime.OrdinaryQieting.cs:10` |
| 21 | 构造固定选择项 | 50 / 61 | A：chooseOption | `bao-san-niang/ol/Runtime/CharacterBaoSanNiangOlRuntime.Bao2025.cs:52`；`cao-shuang/mobile-shi/Runtime/CharacterCaoShuangMobileShiRuntime.MobileShiCaoShuang.cs:192`；`cao-ying/ol/Runtime/CharacterCaoYingOlRuntime.CaoYingPrograms.cs:65` |
| 22 | 公开状态与历史资格 | 49 / 49 | B：事实时点读取，不可偷换live/frozen | `_shared/Runtime/CharacterSharedGameEngineOlMoRumoRuntime.OlMoRumo.cs:44`；`cai-wen-ji/le/Runtime/CharacterCaiWenJiLeRuntime.LeCaiWenJiBeifen.cs:40`；`cao-hong/mou/Runtime/CharacterCaoHongMouRuntime.MouCaoHongServices.cs:39` |
| 23 | 带条件读取事件历史 | 45 / 223 | B：共享历史范围/过滤 | `cao-cao/ol-magic/Runtime/MoCaoCaoFuzaiRuntime.cs:39`；`cao-cao/ol-magic/Runtime/MoCaoCaoFuzaiRuntime.cs:42`；`cao-yu/ol/Runtime/CharacterCaoYuOlRuntime.XiangxuLedger.cs:163` |
| 24 | 拥有技能关系资格 | 44 / 44 | B：同资格阶段的技能关系读取 | `cai-wen-ji/le/Runtime/CharacterCaiWenJiLeRuntime.LeCaiWenJiShuangjia.cs:11`；`cai-wen-ji/le/Runtime/ShuangjiaMarkRuntime.cs:20`；`cao-pi/mou/Runtime/CharacterCaoPiMouRuntime.MouXingshang.cs:6` |
| 25 | 移牌/摸牌/健康混合流程 | 43 / 530 | B：拆出明确动作链，不建大一统节点 | `cao-ang/mou/Runtime/CharacterCaoAngMouRuntime.MouCaoAngChildren.cs:7`；`cao-ang/mou/Runtime/CharacterCaoAngMouRuntime.MouCaoAngChildren.cs:33`；`cao-pi/mou/Runtime/CharacterCaoPiMouRuntime.MouCaoPiObservers.cs:9` |

本轮选“选择分支→摸牌”贯通：AST 另筛到 34 个回答后摸牌候选。先迁谋移动陈宫、快乐神司马懿、SP OL 孙尚香的公共选择/摸牌链，保留实际阶段、首次接受、已付代价和帮助事实等规则；卫兹公开提示不同，暂不混入。
共享契约：在原帧中签发选择结果并继续普通 Draw；完成事实发生于摸牌/替换返回之后、移牌子窗口之前，零实际牌同样完成。暂不引入循环或第二套 pending。

本批结果：19 个执行方法（原方法段 226 行）由 JSON/既有执行器替代：陈宫 5、司马懿 7、孙尚香 7；另删 12 个配套续接/校验方法与 1 个桥接。首用资格捕获改名保留，不计迁移；三人物均仍有规则 C#。
人物 C# 删 482 / 增 113，净减 369；共享 C# 增 110 / 删 51，净增 59；`src` C# 净减 310 至 552,519 行，含 JSON 的 `src` 净减 279 行。26 项相关 Core、3 项 WPF、例行 4 项通过，无全量；覆盖原付费失技、原生 HP/移牌子窗、选择与子窗冷恢复、真实禅润替换摸零张的帮助事实。
样本（C# 文件 / 行 / 字节）：`bao-xin` 13 / 656 / 55,859；`liao-hua` 3 / 159 / 13,767；`xun-yu` 19 / 1,133 / 127,949。跳过：卫兹需公开提示；历史资格族保留原冻结/资格阶段，不能按同族计作等价迁移。
